USE [STOCK]
GO
/****** Object:  UserDefinedFunction [dbo].[ufnSplit]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufnSplit] (@string NVARCHAR(MAX))
RETURNS @parsedString TABLE (id NVARCHAR(MAX))
AS 
BEGIN
   DECLARE @separator NCHAR(1)
   SET @separator=','
   DECLARE @position int
   SET @position = 1
   SET @string = @string + @separator
   WHILE charindex(TRIM(@separator),TRIM(@string),@position) <> 0
      BEGIN
         INSERT into @parsedString
         SELECT substring(TRIM(@string), @position, charindex(TRIM(@separator),TRIM(@string),@position) - @position)
         SET @position = charindex(TRIM(@separator),TRIM(@string),@position) + 1
      END
     RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[UnwrapJson]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   FUNCTION [dbo].[UnwrapJson]
/**
summary:   >
  This multi-statement table-valued function talkes a JSON string and
  unwraps it into a relational hierarchy table that also retains
  the path to each element in the JSON document, and calculates the
  best-fit sql datatype fpr every simple value
Author: Phil Factor
Revision: 1.0
date: 1 Nov 2020
example:
  - SELECT * FROM UnwrapJson (N'[  
    {"name":"Phil", "email":"PhilipFactor@gmail.com"},  
    {"name":"Bob", "email":"bob32@gmail.com"}  
    ]')
returns:   >
  id, level, [key], Value, type, SQLDatatype, parent, path
 
**/    
(
    @JSON NVARCHAR(MAX)
)
RETURNS @Unwrapped TABLE 
  (
  [id] INT IDENTITY, --just used to get a unique reference to each json item
  [level] INT, --the hierarchy level
  [key] NVARCHAR(100), --the key or name of the item
  [Value] NVARCHAR(MAX),--the value, if it is a null, int,binary,numeric or string
  type INT, --0 TO 5, the JSON type, null, numeric, string, binary, array or object
  SQLDatatype sysname, --whatever the datatype can be parsed to
  parent INT, --the ID of the parent
  [path] NVARCHAR(4000) --the path as used by OpenJSON
  )
AS begin
INSERT INTO @Unwrapped ([level], [key], Value, type, SQLDatatype, parent,
[path])
VALUES
  (0, --the level
   NULL, --the key,
   @json, --the value,
   CASE WHEN Left(ltrim(@json),1)='[' THEN 4 ELSE 5 END, --the type
   'json', --SQLDataType,
   0 , --no parent
   '$' --base path
  );
DECLARE @ii INT = 0,--the level
@Rowcount INT = -1; --the number of rows from the previous iteration
WHILE @Rowcount <> 0 --while we are still finding levels
  BEGIN
    INSERT INTO @Unwrapped ([level], [key], Value, type, SQLDatatype, parent,
    [path])
      SELECT [level] + 1 AS [level], new.[Key] AS [key],
        new.[Value] AS [value], new.[Type] AS [type],
-- SQL Prompt formatting off
/* in order to determine the datatype of a json value, the best approach is to a determine
the datatype that can be parsed. It JSON, an array of objects can contain attributes that arent
consistent either in their name or value. */
       CASE 
        WHEN new.Type = 0 THEN 'bit null'
		WHEN new.[type] IN (1,2)  then COALESCE(
  		  CASE WHEN TRY_CONVERT(INT,new.[value]) IS NOT NULL THEN 'int' END, 
  		  CASE WHEN TRY_CONVERT(NUMERIC(14,4),new.[value]) IS NOT NULL THEN 'numeric' END,
  		  CASE WHEN TRY_CONVERT(FLOAT,new.[value]) IS NOT NULL THEN 'float' END,
		  CASE WHEN TRY_CONVERT(MONEY,new.[value]) IS NOT NULL THEN 'money' END,
  		  CASE WHEN TRY_CONVERT(DateTime,new.[value],126) IS NOT NULL THEN 'Datetime2' END,
		  CASE WHEN TRY_CONVERT(Datetime,new.[value],127) IS NOT NULL THEN 'Datetime2' END,
		  'nvarchar')
	   WHEN new.Type = 3 THEN 'bit'
	   WHEN new.Type = 5 THEN 'object' ELSE 'array' END AS SQLDatatype,
        old.[id],
        old.[path] + CASE WHEN old.type = 5 THEN '.' + new.[Key] 
					   ELSE '[' + new.[Key] COLLATE DATABASE_DEFAULT + ']' END AS path
-- SQL Prompt formatting on
      FROM @Unwrapped old
        CROSS APPLY OpenJson(old.[Value]) new
          WHERE old.[level] = @ii AND old.type IN (4, 5);
    SELECT @Rowcount = @@RowCount;
    SELECT @ii = @ii + 1;
  END;
  return
END
GO
/****** Object:  UserDefinedFunction [dbo].[OpenJSONExpressions]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   FUNCTION [dbo].[OpenJSONExpressions]
/**
summary:   >
  This inline table-valued function talkes a JSON string and
  locates every table structure. Then it creates an OpenJSON
  Statement that can then be executed to create that table
  from the original JSON.
Author: Phil Factor
Revision: 1.0
date: 1 Nov 2020
example:
  - SELECT * FROM OpenJSONExpressions (N'[  
    {"name":"Phil", "email":"PhilipFactor@gmail.com"},  
    {"name":"Bob", "email":"bob32@gmail.com"}  
    ]')
returns:   >
  expression
 
**/    
(
   @JSON NVARCHAR(MAX)
    
)
RETURNS TABLE AS RETURN
(
WITH UnwrappedJSON (id, [level], [key], [Value], [type], SQLDatatype, parent,
                   [path]
                   )
AS (SELECT id, [level], [key], [Value], [type], SQLDatatype, parent, [path]
      FROM dbo.UnwrapJson(@json) )
  SELECT 'Select * from openjson(@json,''' + path + ''')
WITH ('  + String_Agg(
                       [name] + ' ' + datatype + ' ' --the WITH statement
-- SQL Prompt formatting off
   + case when datatype='nvarchar' then '('+length+')' 
     WHEN datatype='numeric' then  '(14,4)' ELSE '' end,', ')
   WITHIN GROUP ( ORDER BY  TheOrder  ASC  )    +')' as expression
-- SQL Prompt formatting on
    FROM
      (
      SELECT Parent.path, GrandChild.[key] AS [name], Min(GrandChild.id) AS TheOrder,
	    Max(GrandChild.SQLDatatype) AS datatype,
        Convert(NVARCHAR(100), Max(Len(GrandChild.Value))) AS length
        FROM
          (SELECT path, id FROM UnwrappedJSON WHERE type = 4) Parent
          INNER JOIN UnwrappedJSON Child
            ON Child.parent = Parent.id AND child.type IN (4, 5)
          INNER JOIN UnwrappedJSON GrandChild
            ON GrandChild.parent = Child.id AND GrandChild.type NOT IN (4, 5)
        GROUP BY Parent.path, GrandChild.[key]
      ) TheFields
    GROUP BY path
	)
GO
/****** Object:  Table [dbo].[bustockback]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bustockback](
	[symbol] [varchar](50) NULL,
	[ltt] [datetime] NULL,
	[stock_name] [varchar](max) NULL,
	[last] [decimal](20, 4) NULL,
	[open] [decimal](20, 4) NULL,
	[close] [decimal](20, 4) NULL,
	[high] [decimal](20, 4) NULL,
	[ttv] [decimal](20, 4) NULL,
	[low] [decimal](20, 4) NULL,
	[change] [decimal](20, 4) NULL,
	[avgPrice] [decimal](20, 4) NULL,
	[bPrice] [decimal](20, 4) NULL,
	[sPrice] [decimal](20, 4) NULL,
	[prev] [decimal](20, 4) NULL,
	[Ratio] [decimal](10, 2) NULL,
	[volumeC] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BuyStock]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuyStock](
	[symbol] [varchar](50) NULL,
	[ltt] [datetime] NULL,
	[stock_name] [varchar](max) NULL,
	[last] [decimal](20, 4) NULL,
	[open] [decimal](20, 4) NULL,
	[close] [decimal](20, 4) NULL,
	[high] [decimal](20, 4) NULL,
	[ttv] [decimal](20, 4) NULL,
	[low] [decimal](20, 4) NULL,
	[change] [decimal](20, 4) NULL,
	[avgPrice] [decimal](20, 4) NULL,
	[bPrice] [decimal](20, 4) NULL,
	[sPrice] [decimal](20, 4) NULL,
	[prev] [decimal](20, 4) NULL,
	[Ratio] [decimal](10, 2) NULL,
	[volumeC] [varchar](25) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquitiesFromMSN]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquitiesFromMSN](
	[Symbol] [varchar](150) NOT NULL,
	[MSN_SEC] [varchar](50) NULL,
	[jsonstring] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Symbol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equity$]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equity$](
	[Security Code] [float] NULL,
	[Issuer Name] [nvarchar](255) NULL,
	[Security Id] [nvarchar](255) NULL,
	[Security Name] [nvarchar](255) NULL,
	[Status] [nvarchar](255) NULL,
	[Group] [nvarchar](255) NULL,
	[Face Value] [float] NULL,
	[ISIN No] [nvarchar](255) NULL,
	[Industry] [nvarchar](255) NULL,
	[Instrument] [nvarchar](255) NULL,
	[Sector Name] [nvarchar](255) NULL,
	[Industry New Name] [nvarchar](255) NULL,
	[Igroup Name] [nvarchar](255) NULL,
	[ISubgroup Name] [nvarchar](255) NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equitys]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equitys](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SecurityCode] [float] NULL,
	[IssuerName] [nvarchar](255) NULL,
	[SecurityId] [nvarchar](255) NULL,
	[SecurityName] [nvarchar](255) NULL,
	[Status] [nvarchar](255) NULL,
	[Group] [nvarchar](255) NULL,
	[FaceValue] [float] NULL,
	[ISINNo] [nvarchar](255) NULL,
	[Industry] [nvarchar](255) NULL,
	[Instrument] [nvarchar](255) NULL,
	[SectorName] [nvarchar](255) NULL,
	[IndustryNewName] [nvarchar](255) NULL,
	[IgroupName] [nvarchar](255) NULL,
	[ISubgroupName] [nvarchar](255) NULL,
	[Symbol] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[Rating] [int] NULL,
	[recommondations] [varchar](500) NULL,
	[JsonData] [varchar](max) NULL,
	[MSN_SECID] [varchar](25) NULL,
	[UpdatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equitys_Histry]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equitys_Histry](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SecurityCode] [float] NULL,
	[IssuerName] [nvarchar](255) NULL,
	[SecurityId] [nvarchar](255) NULL,
	[SecurityName] [nvarchar](255) NULL,
	[Status] [nvarchar](255) NULL,
	[Group] [nvarchar](255) NULL,
	[FaceValue] [float] NULL,
	[ISINNo] [nvarchar](255) NULL,
	[Industry] [nvarchar](255) NULL,
	[Instrument] [nvarchar](255) NULL,
	[SectorName] [nvarchar](255) NULL,
	[IndustryNewName] [nvarchar](255) NULL,
	[IgroupName] [nvarchar](255) NULL,
	[ISubgroupName] [nvarchar](255) NULL,
	[Symbol] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[Rating] [int] NULL,
	[recommondations] [varchar](500) NULL,
	[JsonData] [varchar](max) NULL,
	[MSN_SECID] [varchar](25) NULL,
	[Created] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Live_Stocks]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Live_Stocks](
	[symbol] [varchar](50) NULL,
	[open] [decimal](20, 4) NULL,
	[last] [decimal](20, 4) NULL,
	[high] [decimal](20, 4) NULL,
	[low] [decimal](20, 4) NULL,
	[change] [decimal](20, 4) NULL,
	[bPrice] [decimal](20, 4) NULL,
	[bQty] [decimal](20, 4) NULL,
	[sPrice] [decimal](20, 4) NULL,
	[sQty] [decimal](20, 4) NULL,
	[ltq] [decimal](20, 4) NULL,
	[avgPrice] [decimal](20, 4) NULL,
	[quotes] [varchar](50) NULL,
	[ttq] [decimal](20, 4) NULL,
	[totalBuyQt] [bigint] NULL,
	[totalSellQ] [bigint] NULL,
	[ttv] [varchar](1000) NULL,
	[trend] [varchar](50) NULL,
	[lowerCktLm] [decimal](20, 4) NULL,
	[upperCktLm] [decimal](20, 4) NULL,
	[ltt] [datetime] NULL,
	[close] [decimal](20, 4) NULL,
	[exchange] [varchar](50) NULL,
	[stock_name] [varchar](max) NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Week_Min] [decimal](8, 2) NULL,
	[Week_Max] [decimal](8, 2) NULL,
	[TwoWeeks_Min] [decimal](8, 2) NULL,
	[TwoWeeks_Max] [decimal](8, 2) NULL,
	[Month_min] [decimal](8, 2) NULL,
	[Month_Max] [decimal](8, 2) NULL,
	[Three_Month_min] [decimal](8, 2) NULL,
	[Three_Month_max] [decimal](8, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MSNSTCOKS]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MSNSTCOKS](
	[symbol] [varchar](50) NULL,
	[SecurityName] [nvarchar](255) NULL,
	[beta] [nvarchar](4000) NULL,
	[recommendation] [nvarchar](4000) NULL,
	[dateLastUpdated] [date] NULL,
	[price] [nvarchar](4000) NULL,
	[priceChange] [nvarchar](4000) NULL,
	[priceDayHigh] [nvarchar](4000) NULL,
	[priceDayLow] [nvarchar](4000) NULL,
	[yieldPercent] [nvarchar](4000) NULL,
	[return1Week] [nvarchar](4000) NULL,
	[return1Month] [nvarchar](4000) NULL,
	[return3Month] [nvarchar](4000) NULL,
	[return6Month] [nvarchar](4000) NULL,
	[peRatio] [nvarchar](4000) NULL,
	[numberOfAnalysts] [nvarchar](4000) NULL,
	[recommendationRate] [nvarchar](4000) NULL,
	[meanPriceTarget] [nvarchar](4000) NULL,
	[highPriceTarget] [nvarchar](4000) NULL,
	[lowPriceTarget] [nvarchar](4000) NULL,
	[medianPriceTarget] [nvarchar](4000) NULL,
	[medianEpsTarget] [nvarchar](4000) NULL,
	[strongBuy] [nvarchar](4000) NULL,
	[buy] [nvarchar](4000) NULL,
	[underperform] [nvarchar](4000) NULL,
	[hold] [nvarchar](4000) NULL,
	[sell] [nvarchar](4000) NULL,
	[eps] [nvarchar](4000) NULL,
	[forwardPriceToEPS] [nvarchar](4000) NULL,
	[payoutRatio] [nvarchar](4000) NULL,
	[priceToBookRatio] [nvarchar](4000) NULL,
	[profitability] [nvarchar](4000) NULL,
	[stockGrowth] [nvarchar](4000) NULL,
	[latestNetProfitMargin] [nvarchar](4000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPivot]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPivot](
	[symbol] [varchar](50) NULL,
	[STOCK_name] [varchar](max) NULL,
	[Value] [decimal](25, 2) NOT NULL,
	[Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPivotResults]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPivotResults](
	[symbol] [varchar](50) NULL,
	[STOCK_name] [varchar](max) NULL,
	[2023-09-06] [decimal](38, 2) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks](
	[symbol] [varchar](50) NULL,
	[open] [decimal](20, 4) NULL,
	[last] [decimal](20, 4) NULL,
	[high] [decimal](20, 4) NULL,
	[low] [decimal](20, 4) NULL,
	[change] [decimal](20, 4) NULL,
	[bPrice] [decimal](20, 4) NULL,
	[bQty] [decimal](20, 4) NULL,
	[sPrice] [decimal](20, 4) NULL,
	[sQty] [decimal](20, 4) NULL,
	[ltq] [decimal](20, 4) NULL,
	[avgPrice] [decimal](20, 4) NULL,
	[quotes] [varchar](50) NULL,
	[ttq] [decimal](20, 4) NULL,
	[totalBuyQt] [bigint] NULL,
	[totalSellQ] [bigint] NULL,
	[ttv] [varchar](50) NULL,
	[trend] [varchar](50) NULL,
	[lowerCktLm] [decimal](20, 4) NULL,
	[upperCktLm] [decimal](20, 4) NULL,
	[ltt] [datetime] NULL,
	[close] [decimal](20, 4) NULL,
	[exchange] [varchar](50) NULL,
	[stock_name] [varchar](max) NULL,
	[VolumeC] [varchar](20) NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks_Days]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_Days](
	[symbol] [varchar](50) NULL,
	[open] [decimal](20, 4) NULL,
	[last] [decimal](20, 4) NULL,
	[high] [decimal](20, 4) NULL,
	[low] [decimal](20, 4) NULL,
	[change] [decimal](20, 4) NULL,
	[bPrice] [decimal](20, 4) NULL,
	[bQty] [decimal](20, 4) NULL,
	[sPrice] [decimal](20, 4) NULL,
	[sQty] [decimal](20, 4) NULL,
	[ltq] [decimal](20, 4) NULL,
	[avgPrice] [decimal](20, 4) NULL,
	[quotes] [varchar](50) NULL,
	[ttq] [decimal](20, 4) NULL,
	[totalBuyQt] [bigint] NULL,
	[totalSellQ] [bigint] NULL,
	[ttv] [varchar](50) NULL,
	[trend] [varchar](50) NULL,
	[lowerCktLm] [decimal](20, 4) NULL,
	[upperCktLm] [decimal](20, 4) NULL,
	[ltt] [datetime] NULL,
	[close] [decimal](20, 4) NULL,
	[exchange] [varchar](50) NULL,
	[stock_name] [varchar](max) NULL,
	[RnAsc] [bigint] NULL,
	[RnDesc] [bigint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks_Histry]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_Histry](
	[symbol] [varchar](50) NULL,
	[open] [decimal](20, 4) NULL,
	[last] [decimal](20, 4) NULL,
	[high] [decimal](20, 4) NULL,
	[low] [decimal](20, 4) NULL,
	[change] [decimal](20, 4) NULL,
	[bPrice] [decimal](20, 4) NULL,
	[bQty] [decimal](20, 4) NULL,
	[sPrice] [decimal](20, 4) NULL,
	[sQty] [decimal](20, 4) NULL,
	[ltq] [decimal](20, 4) NULL,
	[avgPrice] [decimal](20, 4) NULL,
	[quotes] [varchar](50) NULL,
	[ttq] [decimal](20, 4) NULL,
	[totalBuyQt] [bigint] NULL,
	[totalSellQ] [bigint] NULL,
	[ttv] [varchar](50) NULL,
	[trend] [varchar](50) NULL,
	[lowerCktLm] [decimal](20, 4) NULL,
	[upperCktLm] [decimal](20, 4) NULL,
	[ltt] [datetime] NULL,
	[close] [decimal](20, 4) NULL,
	[exchange] [varchar](50) NULL,
	[stock_name] [varchar](max) NULL,
	[VolumeC] [varchar](25) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks_Yesterday]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_Yesterday](
	[symbol] [varchar](50) NULL,
	[open] [decimal](20, 4) NULL,
	[last] [decimal](20, 4) NULL,
	[high] [decimal](20, 4) NULL,
	[low] [decimal](20, 4) NULL,
	[change] [decimal](20, 4) NULL,
	[bPrice] [decimal](20, 4) NULL,
	[bQty] [decimal](20, 4) NULL,
	[sPrice] [decimal](20, 4) NULL,
	[sQty] [decimal](20, 4) NULL,
	[ltq] [decimal](20, 4) NULL,
	[avgPrice] [decimal](20, 4) NULL,
	[quotes] [varchar](50) NULL,
	[ttq] [decimal](20, 4) NULL,
	[totalBuyQt] [bigint] NULL,
	[totalSellQ] [bigint] NULL,
	[ttv] [varchar](50) NULL,
	[trend] [varchar](50) NULL,
	[lowerCktLm] [decimal](20, 4) NULL,
	[upperCktLm] [decimal](20, 4) NULL,
	[ltt] [datetime] NULL,
	[close] [decimal](20, 4) NULL,
	[exchange] [varchar](50) NULL,
	[stock_name] [varchar](max) NULL,
	[RnAsc] [bigint] NULL,
	[RnDesc] [bigint] NULL,
	[VolumeC] [varchar](25) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TodaysRatios]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TodaysRatios](
	[symbol] [varchar](50) NULL,
	[ltt] [datetime] NULL,
	[stock_name] [varchar](max) NULL,
	[last] [decimal](20, 4) NULL,
	[open] [decimal](20, 4) NULL,
	[close] [decimal](20, 4) NULL,
	[high] [decimal](20, 4) NULL,
	[ttv] [varchar](50) NULL,
	[prev] [varchar](50) NULL,
	[Ratio] [decimal](10, 2) NULL,
	[volumeC] [varchar](25) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[BatchExecute]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[BatchExecute] 
as begin 
print '1'
--exec Import_Histry
--exec Ticker_yesterday
--exec Import_Last_Stock
end
GO
/****** Object:  StoredProcedure [dbo].[BatchExecute_copy]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[BatchExecute_copy] 
as begin 
print '1'
exec Import_Histry
exec Ticker_yesterday
exec Import_Last_Stock
end
GO
/****** Object:  StoredProcedure [dbo].[Get_MSN_Stocks]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Get_MSN_Stocks]
as
begin

truncate table  MSNSTCOKS

Insert into MSNSTCOKS

sELECT symbol,SecurityName, 
JSON_VALUE(JsonData, '$.equity.beta')  as beta,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.recommendation')  as recommendation,
cast(JSON_VALUE(JsonData, '$.equity.analysis.estimate.dateLastUpdated') as Date)  as dateLastUpdated,

JSON_VALUE(JsonData, '$.quote.price')  as price,
JSON_VALUE(JsonData, '$.quote.priceChange')  as priceChange,

JSON_VALUE(JsonData, '$.quote.priceDayHigh')  as priceDayHigh,
JSON_VALUE(JsonData, '$.quote.priceDayLow')  as priceDayLow,
JSON_VALUE(JsonData, '$.quote.yieldPercent')  as yieldPercent,
JSON_VALUE(JsonData, '$.quote.return1Week')  as return1Week,
JSON_VALUE(JsonData, '$.quote.return1Month')  as return1Month,
JSON_VALUE(JsonData, '$.quote.return3Month')  as return3Month,
JSON_VALUE(JsonData, '$.quote.return6Month')  as return6Month,
JSON_VALUE(JsonData, '$.quote.peRatio')  as peRatio,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.numberOfAnalysts')  as numberOfAnalysts,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.recommendationRate')  as recommendationRate,

JSON_VALUE(JsonData, '$.equity.analysis.estimate.meanPriceTarget')  as meanPriceTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.highPriceTarget')  as highPriceTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.lowPriceTarget')  as lowPriceTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.medianPriceTarget')  as medianPriceTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.medianEpsTarget')  as medianEpsTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.analystRecommendation.strongBuy')  as strongBuy,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.analystRecommendation.buy')  as buy,

JSON_VALUE(JsonData, '$.equity.analysis.estimate.analystRecommendation.underperform')  as underperform,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.analystRecommendation.hold')  as hold,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.analystRecommendation.hold')  as sell,



JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.eps')  as eps,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.forwardPriceToEPS')  as forwardPriceToEPS,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.payoutRatio')  as payoutRatio,

JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.priceToBookRatio')  as priceToBookRatio,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.profitability')  as profitability,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.stockGrowth')  as stockGrowth,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.latestNetProfitMargin')  as latestNetProfitMargin







FROM Equitys WHERE recommondations in ('STRONGBUY','buy') 

Select * from MSNSTCOKS where  recommendation in ('STRONGBUY','buy') and cast(dateLastUpdated as Date)=CAST(GETDATE() as Date)   order by dateLastUpdated desc
End







GO
/****** Object:  StoredProcedure [dbo].[GetBuysStocks]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/

CREATE proc [dbo].[GetBuysStocks](@top int ,@Date DateTime)
as
begin
SELECT *,cast([Last]-[open] as Decimal(10,2)) as INC
  FROM [STOCK].[dbo].[BuyStock] where ratio >=@top and CAST(ltt as Date)=cast(@Date as Date)  order by ratio desc
  end
GO
/****** Object:  StoredProcedure [dbo].[GetPivotData]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--GetPivotData '2023-09-14','high-low'
--GetPivotData '2023-08-15','open'
--GetPivotData '2023-08-15','change'



CREATE Proc [dbo].[GetPivotData](@Date Date, @column varchar(20), @groupName varchar(max)='', @subGroup varchar(max)='')
as
begin 

if not exists (select * from Ticker_Stocks_Histry where cast(ltt as Date)=CAST(@Date as Date))
begin
set @Date= (select MAX(CAST(ltt as Date)) from Ticker_Stocks_Histry) 
end 
Declare @previousDate DateTime = @Date
truncate table TempPivot;
drop table if exists TempPivotResults

--Declare @previousDate Date = (SELECT DATEADD(
--    day,
--    IIF(DATENAME(weekday,@Date) = 'Monday', -3, -1),
--    CAST(@Date AS DATE)
--));
drop table if exists #tempdata;
DECLARE @sql_select  nvarchar(max);

    SELECT  e.symbol,STOCK_name,ltt,[close],
	
	case when @column ='high-low' then [high]-[low] end as DayChange,
	[open],[last],ttv,change,low,high,
        RnAsc = ROW_NUMBER() OVER(PARTITION BY E.symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc = ROW_NUMBER() OVER(PARTITION BY E.symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    into #tempCTE  FROM Ticker_Stocks_Histry   H
	left join Equitys E on h.symbol=E.Symbol
	where  CAST(ltt as Date) >=cast(@previousDate as Date)
	and ( @groupName is null or  @groupName=''  or E.IgroupName    in  (SELECT id FROM [dbo].[ufnSplit](@groupName)))
	and ( @subGroup is null or   @subGroup='' or  E.ISubgroupName    in  (SELECT id FROM [dbo].[ufnSplit](@subGroup)))

	if(@column='high-low')
	 set @column='DayChange'
	--#select distinct symbol,STOCK_name, 0 as Value,cast(ltt as Date) as Date  into TempPivot   from #tempCTE where RnDesc=1 order by symbol
SET @sql_select='Insert into  TempPivot select distinct symbol,STOCK_name,['+@column+'] as value,cast(ltt as Date) as Date   from #tempCTE where RnDesc=1  order by symbol'


EXECUTE sp_executesql @sql_select
--Select * from TempPivot


DECLARE @sql  nvarchar(max);
DECLARE @columnname nvarchar(max);


select @columnname = STUFF((SELECT ',' + QUOTENAME(convert(char(10), date, 120)) 
                    from TempPivot
                    group by date
                    order by date asc
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

SET @sql='
SELECT * into TempPivotResults 
FROM  TempPivot t
PIVOT
(SUM([value])
FOR [Date] IN('+@columnname+')) AS p '

EXECUTE sp_executesql @sql


Select [open],[last],[last]-[open] as Diff , P.* from TempPivotResults P
left join Live_Stocks E  on E.Symbol =P.Symbol
left JOIN MSNSTCOKS  eq ON eq.Symbol= P.Symbol
--WHERE EQ.recommondations='strongBuy' 

--Select P.* from TempPivotResults P
--left join Live_Stocks E  on E.Symbol =P.Symbol
End
GO
/****** Object:  StoredProcedure [dbo].[Import_Histry]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Import_Histry]
as
begin

Delete from [Ticker_Stocks_Histry] where  CAST(ltt as Date )=  DATEADD(hour, -1, GETDATE())
Declare @Date DateTime =(select isnull(MAX(cast(ltt as Date)),GETDATE()-30) from Ticker_Stocks_Histry);

WITH Cte AS(
SELECT *,
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM [Ticker_Stocks] with (nolock)   where (ltt ) >=  DATEADD(hour, -1, GETDATE()) and [open] >0 )

	INSERT INTO [Ticker_Stocks_Histry]
           ([symbol]
           ,[open]
           ,[last]
           ,[high]
           ,[low]
           ,[change]
           ,[bPrice]
           ,[bQty]
           ,[sPrice]
           ,[sQty]
           ,[ltq]
           ,[avgPrice]
           ,[quotes]
           ,[ttq]
           ,[totalBuyQt]
           ,[totalSellQ]
           ,[ttv]
           ,[trend]
           ,[lowerCktLm]
           ,[upperCktLm]
           ,[ltt]
           ,[close]
           ,[exchange]
           ,[stock_name],volumeC)
	SELECT distinct  [symbol]
           ,[open]
           ,[last]
           ,[high]
           ,[low]
           ,[change]
           ,[bPrice]
           ,[bQty]
           ,[sPrice]
           ,[sQty]
           ,[ltq]
           ,[avgPrice]
           ,[quotes]
           ,[ttq]
           ,[totalBuyQt]
           ,[totalSellQ]
           ,[ttv]
           ,[trend]
           ,[lowerCktLm]
           ,[upperCktLm]
           ,[ltt]
           ,[close]
           ,[exchange]
           ,[stock_name],volumeC
FROM Cte
WHERE
    RnDesc_C= 1
ORDER BY symbol, ltt;
End
GO
/****** Object:  StoredProcedure [dbo].[Import_Last_Stock]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  proc [dbo].[Import_Last_Stock]
as 
begin
truncate table Live_Stocks
Declare @previousDate Date = (SELECT DATEADD(
    day,
    IIF(DATENAME(weekday,getdate()) = 'Monday', -3, -1),
    CAST(getdate() as DATE)
));
Declare @Date DateTime =(select isnull(MAX(ltt),GETDATE()-30) from Ticker_Stocks_Histry);
WITH Cte AS(
SELECT *,
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM Ticker_Stocks_Histry  where CAST(ltt as Date) >= cast(@Date as date))

	INSERT INTO Live_Stocks
           ([symbol]
           ,[open]
           ,[last]
           ,[high]
           ,[low]
           ,[change]
           ,[bPrice]
           ,[bQty]
           ,[sPrice]
           ,[sQty]
           ,[ltq]
           ,[avgPrice]
           ,[quotes]
           ,[ttq]
           ,[totalBuyQt]
           ,[totalSellQ]
           ,[ttv]
           ,[trend]
           ,[lowerCktLm]
           ,[upperCktLm]
           ,[ltt]
           ,[close]
           ,[exchange]
           ,[stock_name])
	SELECT distinct  [symbol]
           ,[open]
           ,[last]
           ,[high]
           ,[low]
           ,[change]
           ,[bPrice]
           ,[bQty]
           ,[sPrice]
           ,[sQty]
           ,[ltq]
           ,[avgPrice]
           ,[quotes]
           ,[ttq]
           ,[totalBuyQt]
           ,[totalSellQ]
           ,[ttv]
           ,[trend]
           ,[lowerCktLm]
           ,[upperCktLm]
           ,[ltt]
           ,[close]
           ,[exchange]
           ,[stock_name]
		  
FROM Cte
WHERE
    RnDesc_C= 1
ORDER BY symbol, ltt;

End
GO
/****** Object:  StoredProcedure [dbo].[insert_MSN_Equities]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create proc [dbo].[insert_MSN_Equities]
as
begin
insert into [Equitys_Histry]
Select [SecurityCode]
      ,[IssuerName]
      ,[SecurityId]
      ,[SecurityName]
      ,[Status]
      ,[Group]
      ,[FaceValue]
      ,[ISINNo]
      ,[Industry]
      ,[Instrument]
      ,[SectorName]
      ,[IndustryNewName]
      ,[IgroupName]
      ,[ISubgroupName]
      ,[Symbol]
      ,[IsActive]
      ,[Rating]
      ,[recommondations]
      ,[JsonData]
      ,[MSN_SECID],GETDATE() from  Equitys
	  End
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_CHART_STOCKS_BY_STOCK]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--SP_GET_CHART_STOCKS_BY_STOCK '1.1!539992'

CREATE Procedure [dbo].[SP_GET_CHART_STOCKS_BY_STOCK](@Code varchar(50))
as
begin 


--Select  *  from [dbo].Ticker_Stocks with (Nolock) where (symbol=@Code or  @Code='' or @Code is null )and CAST(ltt as Date)=cast(GETDATE() as date)order by ltt desc 

Select  *  from [dbo].Ticker_Stocks with (Nolock) where symbol=@Code and CAST(ltt as Date)=cast(GETDATE() as date)order by ltt asc 
end  
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_groupName_By]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 Create  Procedure [dbo].[SP_GET_groupName_By](@IndustryNewName varchar(100))
  as begin 
  Select distinct  [IgroupName] as Text  from  [STOCK].[dbo].[Equitys] where  [IndustryNewName]  in  (SELECT id FROM [dbo].[ufnSplit](@IndustryNewName))
  End 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_IndustryNewName_By]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE Procedure [dbo].[SP_GET_IndustryNewName_By](@SectorName varchar(100))
  as begin 
  Select distinct  [IndustryNewName] as Text  from  [STOCK].[dbo].[Equitys] where  SectorName  in  (SELECT id FROM [dbo].[ufnSplit](@SectorName))
  End 
 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK](@Code varchar(50)='')
as
begin 
if(@Code is null or @Code='' )
begin 
Select  *  from [dbo].[Live_Stocks] with (Nolock) where  
--CAST(ltt as Date)=cast(GETDATE() as date) and 
[open] < 150 order by ltt desc 

end
else
Select top 1 *  from [dbo].[Live_Stocks] with (Nolock) where symbol=@Code and CAST(ltt as Date)=cast(GETDATE() as date) and [open] < 150 order by ltt desc 
end 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_LOAD]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_LOAD](@Code varchar(50)='')
as
begin 
if(@Code is null or @Code='' )
begin 
SELECT 
       E.symbol,
	   isnull([open],0) [open]
      ,isnull([last],0) [last]
      ,isnull([high],0) [high]
      ,isnull([low],0) [low]
      ,isnull([change],0) [change]
      ,isnull([bPrice],0) [bPrice]
      ,isnull([bQty],0) [bQty]
      ,isnull([sPrice],0) [sPrice]
      ,isnull([sQty],0) [sQty]
      ,isnull([ltq],0) [ltq]
      ,isnull([avgPrice],0) [avgPrice]
      ,isnull([quotes],'') [quotes]
      ,isnull([ttq],0) [ttq]
      ,isnull([totalBuyQt],0) [totalBuyQt]
      ,isnull([totalSellQ],0) [totalSellQ]
      ,isnull([ttv],0) [ttv]
      ,isnull([trend],'') [trend]
      ,isnull([lowerCktLm],0) [lowerCktLm]
      ,isnull([upperCktLm],0) [upperCktLm]
      ,isnull([ltt],0) [ltt]
      ,isnull([close],0) [close]
      ,isnull([exchange],'') [exchange]
      ,isnull([SecurityName],'') [stock_name]
      ,isnull(L.[ID],0) Id
      ,isnull([Week_Min],0) [Week_Min]
      ,isnull([Week_Max],0) [Week_Max]
      ,isnull([TwoWeeks_Min],0) [TwoWeeks_Min]
      ,isnull([TwoWeeks_Max],0) [TwoWeeks_Max]
      ,isnull([Month_min],0) [Month_min]
      ,isnull([Month_Max],0) [Month_Max]
      ,isnull([Three_Month_min],0) [Three_Month_min]
      ,isnull([Three_Month_max],0) [Three_Month_max]
      
  FROM [STOCK].[dbo].[Equitys] E

 left join  [dbo].[Live_Stocks] L  on  E.symbol=l.symbol
 where e.isactive=1


--[dbo].[Live_Stocks] with (Nolock) where  
--[open] < 250 order by ltt desc 

end
else
Select top 1 *  from [dbo].[Live_Stocks] with (Nolock) where symbol=@Code and CAST(ltt as Date)=cast(GETDATE() as date) and [open] < 150 order by ltt desc 
end 


GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_Test]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_Test](@Code varchar(50)='')
as
begin 
if(@Code is null or @Code='' )
begin 
Select  *  from [dbo].[Live_Stocks] with (Nolock) where  
--CAST(ltt as Date)=cast(GETDATE() as date) and 
[open] < 100 order by ltt desc 

end
else
Select top 1 *  from [dbo].[Live_Stocks] with (Nolock) where symbol=@Code and CAST(ltt as Date)=cast(GETDATE() as date) and [open] < 100 order by ltt desc 
end 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_SectorName]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE Procedure [dbo].[SP_GET_SectorName]
  as begin 
  Select distinct  [SectorName] as Text from  [STOCK].[dbo].[Equitys] where SectorName <> '-'
  End 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_SubgroupName_By]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  Procedure [dbo].[SP_GET_SubgroupName_By](@groupName varchar(100))
  as begin 
  Select distinct  [ISubgroupName] as Text  from  [STOCK].[dbo].[Equitys] where  IgroupName  in  (SELECT id FROM [dbo].[ufnSplit](@groupName))
  End 
 
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTopPerformer]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--SP_GetTopPerformer 100,'2023-08-18'


CREATE Proc [dbo].[SP_GetTopPerformer](@top int ,@Date DateTime)
as
Begin
Select distinct top (@top)  stock_name,max(cast(ttv as decimal)) as Volume,MIN(last) as min_last,MAX(last) as max_last,AVG(last) as avg,MAX([open]) as [Open],MIN(cast(change as decimal(10,2))) as min_change,MAX(cast(change as decimal(10,2))) as max_change,

MIN(cast(bPrice as decimal(10,2))) as min_bPrice,MAX(cast(bPrice as decimal(10,2))) as max_bPrice,MIN(cast(sPrice as decimal(10,2))) as min_sPrice,MAX(cast(sPrice as decimal(10,2))) as max_sPrice,symbol,

AVG(cast(bPrice as decimal(10,2))) as bPrice,AVG(cast(sPrice as decimal(10,2))) as sPrice


from dbo.Ticker_Stocks where CAST(ltt as Date)=cast(@Date as Date) 
group by stock_name,symbol
order by max(cast(ttv as decimal)) desc
End



					
GO
/****** Object:  StoredProcedure [dbo].[TablesFromJSON]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TablesFromJSON] @TheJSON NVARCHAR(MAX)
    
AS
DECLARE @expressions TABLE (id INT IDENTITY, TheExpression NVARCHAR(MAX));
INSERT INTO @expressions (TheExpression)
  SELECT expression FROM OpenJSONExpressions(@TheJSON);
DECLARE @RowCount INT = -1, @ii INT = 1, @expressionToExcecute NVARCHAR(MAX);
WHILE @RowCount <> 0
  BEGIN
    SELECT @expressionToExcecute = TheExpression FROM @expressions WHERE id = @ii;
    SELECT @RowCount = @@RowCount;
    SELECT @ii = @ii + 1;
    IF @RowCount > 0
      EXECUTE sp_executesql @expressionToExcecute, N'@JSON NVARCHAR(MAX)',
        @JSON = @TheJSON;
  END;
GO
/****** Object:  StoredProcedure [dbo].[Ticker_Current]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Ticker_Current](@Date DateTime)
as
begin

Declare @previousDate Date = (SELECT DATEADD(
    day,
    IIF(DATENAME(weekday,@Date) = 'Monday', -3, -1),
    CAST(@Date AS DATE)
));
truncate table [Ticker_Stocks_Days];
WITH Cte AS(
    SELECT *,
        RnAsc = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM Ticker_Stocks  where CAST(ltt as Date) >=@previousDate
)
INSERT INTO [dbo].[Ticker_Stocks_Days]
           ([symbol]
           ,[open]
           ,[last]
           ,[high]
           ,[low]
           ,[change]
           ,[bPrice]
           ,[bQty]
           ,[sPrice]
           ,[sQty]
           ,[ltq]
           ,[avgPrice]
           ,[quotes]
           ,[ttq]
           ,[totalBuyQt]
           ,[totalSellQ]
           ,[ttv]
           ,[trend]
           ,[lowerCktLm]
           ,[upperCktLm]
           ,[ltt]
           ,[close]
           ,[exchange]
           ,[stock_name]
           ,[RnAsc]
           ,[RnDesc])
SELECT distinct  [symbol]
      ,[open]
      ,[last]
      ,[high]
      ,[low]
      ,[change]
      ,[bPrice]
      ,[bQty]
      ,[sPrice]
      ,[sQty]
      ,[ltq]
      ,[avgPrice]
      ,[quotes]
      ,[ttq]
      ,[totalBuyQt]
      ,[totalSellQ]
      ,[ttv]
      ,[trend]
      ,[lowerCktLm]
      ,[upperCktLm]
      ,[ltt]
      ,[close]
      ,[exchange]
      ,[stock_name],
	  RnAsc,
	  RnDesc

	
FROM Cte
WHERE
    RnAsc = 1 OR RnDesc = 1 
ORDER BY symbol, ltt
END
GO
/****** Object:  StoredProcedure [dbo].[Ticker_today]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--[Ticker_today] '2023-08-25'

CREATE proc [dbo].[Ticker_today](@Date DateTime)
as 
begin

drop table if exists #tempData
Declare @previousDate Date = (SELECT DATEADD(
    day,
    IIF(DATENAME(weekday,@Date) = 'Monday', -3, -1),
    CAST(@Date AS DATE)
));

exec Ticker_Current @Date;
truncate table [Ticker_Stocks_Yesterday];
truncate table TodaysRatios;

--Declare @previousDate Date = (SELECT DATEADD(
--    day,
--    IIF(DATENAME(weekday, GETDATE()) = 'Monday', -3, -1),
--    CAST(GETDATE() AS DATE)
--));


WITH Cte AS(
SELECT *,
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM [Ticker_Stocks_Days]  where CAST(ltt as Date) >= cast(@previousDate as Date)  ) 

	INSERT INTO [Ticker_Stocks_Yesterday]
           ([symbol]
           ,[open]
           ,[last]
           ,[high]
           ,[low]
           ,[change]
           ,[bPrice]
           ,[bQty]
           ,[sPrice]
           ,[sQty]
           ,[ltq]
           ,[avgPrice]
           ,[quotes]
           ,[ttq]
           ,[totalBuyQt]
           ,[totalSellQ]
           ,[ttv]
           ,[trend]
           ,[lowerCktLm]
           ,[upperCktLm]
           ,[ltt]
           ,[close]
           ,[exchange]
           ,[stock_name]
           ,[RnAsc]
           ,[RnDesc])
	SELECT distinct  [symbol]
           ,[open]
           ,[last]
           ,[high]
           ,[low]
           ,[change]
           ,[bPrice]
           ,[bQty]
           ,[sPrice]
           ,[sQty]
           ,[ltq]
           ,[avgPrice]
           ,[quotes]
           ,[ttq]
           ,[totalBuyQt]
           ,[totalSellQ]
           ,[ttv]
           ,[trend]
           ,[lowerCktLm]
           ,[upperCktLm]
           ,[ltt]
           ,[close]
           ,[exchange]
           ,[stock_name]
           ,[RnAsc]
           ,[RnDesc]
FROM Cte
WHERE
    RnAsc_C= 2 
ORDER BY symbol, ltt;

	

	
	with CTE AS(
	SELECT symbol, 
       ltt, stock_name,[last] as last,
      [open],[close],[high],ttv,
	 
       LAG(ttv, 1, null) OVER(PARTITION BY symbol
       ORDER BY ltt 
                 ASC) AS prev
FROM dbo.[Ticker_Stocks_Yesterday])



insert into TodaysRatios
Select * ,
cast((cast(ttv as decimal(25,2))-cast(prev as decimal(25,2)))/cast(prev as decimal(25,2))*100 as decimal(10,2)) as Ratio  from CTE where prev is not null 

order by ltt desc

Select * from TodaysRatios where Ratio > 100 order by Ratio desc;
End
GO
/****** Object:  StoredProcedure [dbo].[Ticker_yesterday]    Script Date: 06-09-2023 11.15.55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Ticker_yesterday]
as 
begin
drop table if exists #temp_Ratio
--truncate table [Ticker_Stocks_Yesterday];
truncate table BuyStock


Declare @previousDate Date = (SELECT DATEADD(
    day,
    IIF(DATENAME(weekday,getdate()) = 'Monday', -3, -1),
    CAST(getdate() as DATE)
));

print @previousDate;


Delete from [Ticker_Stocks_Yesterday]  where  CAST(ltt as Date )= CAST(GETDATE() as Date )
Declare @Date DateTime =(select isnull(MAX(cast(ltt as Date)),@previousDate) from [Ticker_Stocks_Yesterday]);

print @Date;
WITH Cte AS(
SELECT *,
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM Ticker_Stocks_Histry   where CAST(ltt as Date)  >cast( @Date as Date)) -- >= cast(@previousDate as date))

	INSERT INTO [Ticker_Stocks_Yesterday]
           ([symbol]
           ,[open]
           ,[last]
           ,[high]
           ,[low]
           ,[change]
           ,[bPrice]
           ,[bQty]
           ,[sPrice]
           ,[sQty]
           ,[ltq]
           ,[avgPrice]
           ,[quotes]
           ,[ttq]
           ,[totalBuyQt]
           ,[totalSellQ]
           ,[ttv]
           ,[trend]
           ,[lowerCktLm]
           ,[upperCktLm]
           ,[ltt]
           ,[close]
           ,[exchange]
           ,[stock_name],volumeC)
	SELECT distinct  [symbol]
           ,[open]
           ,[last]
           ,[high]
           ,[low]
           ,[change]
           ,[bPrice]
           ,[bQty]
           ,[sPrice]
           ,[sQty]
           ,[ltq]
           ,[avgPrice]
           ,[quotes]
           ,[ttq]
           ,[totalBuyQt]
           ,[totalSellQ]
           ,[ttv]
           ,[trend]
           ,[lowerCktLm]
           ,[upperCktLm]
           ,[ltt]
           ,[close]
           ,[exchange]
           ,[stock_name],volumeC
		  
FROM Cte
WHERE
    RnDesc_C= 1
ORDER BY symbol, ltt;

	

	
	with CTE AS(
	SELECT symbol, 
       ltt, stock_name,[last] as last,volumeC,
      [open],[close],[high],cast(ttv as decimal(25,2)) as ttv,low,change,avgPrice,bPrice,sPrice,
	 
       LAG(cast(ttv as decimal(25,2)), 1, null) OVER(PARTITION BY symbol
       ORDER BY ltt 
                 ASC) AS prev
FROM dbo.[Ticker_Stocks_Yesterday])

Select  [symbol]
      ,[ltt]
      ,[stock_name]
      ,[last]
      ,[open]
      ,[close]
      ,[high]
      ,[ttv]
      ,[low]
      ,[change]
      ,[avgPrice]
      ,[bPrice]
      ,[sPrice]
      ,[prev],
case when cast(prev as decimal(25,2)) > 0 then  cast((cast(ttv as decimal(25,2))-cast(prev as decimal(25,2)))/(cast(prev as decimal(25,2))) as decimal(10,2)) *100 else 0 end as Ratio,volumeC   into #temp_Ratio from CTE  --where prev is not null  and prev > 0
Insert into BuyStock

Select * from #temp_Ratio 

End
GO
