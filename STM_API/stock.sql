USE [STOCK]
GO
/****** Object:  User [HAADVISRI\ganga]    Script Date: 05-02-2024 21:23:23 ******/
CREATE USER [HAADVISRI\ganga] FOR LOGIN [HAADVISRI\ganga] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[GetPerCentageOfTotalValue]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetPerCentageOfTotalValue](
    @percenate int,@Sum decimal(10,2)
   
)
RETURNS decimal(10,2)
AS 
BEGIN
Declare @Value decimal(10,2)
set @Value=  @percenate % (@Sum)/100*@Sum+(@Sum)

--(select top 1 [low] from Ticker_Stocks_Histry where symbol=@Symbol order by change desc)
    RETURN @Value
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GETStockPreviousData]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GETStockPreviousData] (@string NVARCHAR(MAX))
RETURNS @parsedString TABLE (Symbol NVARCHAR(MAX),change NVARCHAR(MAX),VolumeC NVARCHAR(MAX),[open] NVARCHAR(MAX),[close] NVARCHAR(MAX),ltt NVARCHAR(MAX))
AS 
BEGIN
    
	 INSERT into @parsedString
  
	Select top 7   @string as Symbol,
	STRING_AGG(cast([change] as decimal(10,2)), ', ') change,
	STRING_AGG(VolumeC, ', ') VolumeC,
	STRING_AGG(cast([open] as decimal(10,2)), ', ') [open],
	STRING_AGG(cast([close] as decimal(10,2)), ', ') [close],
	STRING_AGG(cast(ltt as Date), ', ') ltt  from Ticker_Stocks_Histry where symbol=@string order by ltt desc 
     RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetThreshold_Min]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetThreshold_Min](
    @Symbol varchar(100)
   
)
RETURNS decimal(10,2)
AS 
BEGIN
Declare @Value decimal(10,2)
set @Value= (select top 1 [low] from Ticker_Stocks_Histry where symbol=@Symbol order by change desc)
    RETURN @Value
END;
GO
/****** Object:  UserDefinedFunction [dbo].[split_string]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[split_string](
          @delimited NVARCHAR(MAX),
          @delimiter NVARCHAR(100)
        ) RETURNS @t TABLE (id INT IDENTITY(1,1), val NVARCHAR(MAX))
AS
BEGIN
  DECLARE @xml XML
  SET @xml = N'<t>' + REPLACE(@delimited,@delimiter,'</t><t>') + '</t>'

  INSERT INTO @t(val)
  SELECT  r.value('.','varchar(MAX)') as item
  FROM  @xml.nodes('/t') as records(r)
  RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ufnSplit]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  UserDefinedFunction [dbo].[UnwrapJson]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  UserDefinedFunction [dbo].[OpenJSONExpressions]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[AUTO_BUY_EQUTIES]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AUTO_BUY_EQUTIES](
	[Symbol] [varchar](50) NULL,
	[Quanity] [bigint] NULL,
	[RID] [varchar](50) NULL,
	[EX] [varchar](50) NULL,
	[ORDERType] [varchar](50) NULL,
	[BUY_PRICE] [decimal](18, 2) NULL,
	[STOPLoss] [decimal](18, 2) NULL,
	[stock_code] [varchar](50) NULL,
	[DATE] [datetime] NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](10) NULL,
	[InvestedPrice] [decimal](10, 3) NULL,
	[Resposne] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AUTO_BUY_EQUTIES_OLD]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AUTO_BUY_EQUTIES_OLD](
	[Symbol] [varchar](50) NULL,
	[Quanity] [bigint] NULL,
	[RID] [varchar](50) NULL,
	[EX] [varchar](50) NULL,
	[ORDERType] [varchar](50) NULL,
	[BUY_PRICE] [decimal](18, 2) NULL,
	[STOPLoss] [decimal](18, 2) NULL,
	[stock_code] [varchar](50) NULL,
	[DATE] [datetime] NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](10) NULL,
	[InvestedPrice] [decimal](10, 3) NULL,
	[Resposne] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AUTO_BUY_EQUTIES_Old2]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AUTO_BUY_EQUTIES_Old2](
	[Symbol] [varchar](50) NULL,
	[Quanity] [bigint] NULL,
	[RID] [varchar](50) NULL,
	[EX] [varchar](50) NULL,
	[ORDERType] [varchar](50) NULL,
	[BUY_PRICE] [decimal](18, 2) NULL,
	[STOPLoss] [decimal](18, 2) NULL,
	[stock_code] [varchar](50) NULL,
	[DATE] [datetime] NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](10) NULL,
	[InvestedPrice] [decimal](10, 3) NULL,
	[Resposne] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AUTO_BUY_EQUTIES_oldnew]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AUTO_BUY_EQUTIES_oldnew](
	[Symbol] [varchar](50) NULL,
	[Quanity] [bigint] NULL,
	[RID] [varchar](50) NULL,
	[EX] [varchar](50) NULL,
	[ORDERType] [varchar](50) NULL,
	[BUY_PRICE] [decimal](18, 2) NULL,
	[STOPLoss] [decimal](18, 2) NULL,
	[stock_code] [varchar](50) NULL,
	[DATE] [datetime] NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](10) NULL,
	[InvestedPrice] [decimal](10, 3) NULL,
	[Resposne] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AUTO_BUY_EQUTIES2]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AUTO_BUY_EQUTIES2](
	[Symbol] [varchar](50) NULL,
	[Quanity] [bigint] NULL,
	[RID] [varchar](50) NULL,
	[EX] [varchar](50) NULL,
	[ORDERType] [varchar](50) NULL,
	[BUY_PRICE] [decimal](18, 2) NULL,
	[STOPLoss] [decimal](18, 2) NULL,
	[stock_code] [varchar](50) NULL,
	[DATE] [datetime] NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](10) NULL,
	[InvestedPrice] [decimal](10, 3) NULL,
	[Resposne] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BSE_NEWS]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BSE_NEWS](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NEWSID] [varchar](100) NULL,
	[SCRIP_CD] [int] NULL,
	[XML_NAME] [varchar](1000) NULL,
	[NEWSSUB] [varchar](500) NULL,
	[DT_TM] [varchar](500) NULL,
	[NEWS_DT] [varchar](250) NULL,
	[CRITICALNEWS] [bit] NULL,
	[ANNOUNCEMENT_TYPE] [varchar](250) NULL,
	[QUARTER_ID] [bit] NULL,
	[FILESTATUS] [varchar](550) NULL,
	[ATTACHMENTNAME] [varchar](550) NULL,
	[MORE] [varchar](800) NULL,
	[HEADLINE] [varchar](max) NULL,
	[CATEGORYNAME] [varchar](550) NULL,
	[OLD] [bit] NULL,
	[RN] [bit] NULL,
	[PDFFLAG] [bit] NULL,
	[NSURL] [varchar](1000) NULL,
	[SLONGNAME] [varchar](100) NULL,
	[AGENDA_ID] [int] NULL,
	[TotalPageCnt] [int] NULL,
	[News_submission_dt] [varchar](550) NULL,
	[DissemDT] [varchar](550) NULL,
	[TimeDiff] [varchar](580) NULL,
	[Fld_Attachsize] [int] NULL,
	[SUBCATNAME] [varchar](1000) NULL,
	[AUDIO_VIDEO_FILE] [varchar](1000) NULL,
	[BASEURL] [varchar](500) NULL,
	[CreatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[bustockback]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[BuyStock]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[Company_Ratings_Only_DIVI]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company_Ratings_Only_DIVI](
	[ID] [bigint] IDENTITY(10,2) NOT NULL,
	[Symbol] [varchar](50) NULL,
	[Rating] [varchar](50) NULL,
	[Rating_NUM] [decimal](10, 2) NULL,
	[THRS_MIN] [decimal](10, 2) NULL,
	[THRS_MAX] [decimal](10, 2) NULL,
	[ROUND] [decimal](10, 2) NULL,
	[LAST] [decimal](10, 2) NULL,
	[CAN_BUY] [bit] NULL,
	[Dividends] [decimal](10, 2) NULL,
	[BUY_CODE] [varchar](50) NULL,
	[PERAtio] [decimal](10, 2) NULL,
	[AverageVolume] [decimal](20, 2) NULL,
	[EPS] [decimal](20, 2) NULL,
	[IsRisky] [bit] NULL,
	[IsExclude] [bit] NULL,
	[IsGoodStock] [bit] NULL,
	[IsMicro] [bit] NULL,
	[Comments] [varchar](1000) NULL,
	[Divident_Date] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[MSNRating] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equity$]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[Equitys]    Script Date: 05-02-2024 21:23:23 ******/
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
	[ISPSU] [bit] NULL,
	[IsNonPSU] [bit] NULL,
	[IsPrime] [bit] NULL,
	[IsEnabledForAutoTrade] [bit] NULL,
	[Tdays] [varchar](10) NULL,
	[WatchList] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equitys_Histry]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[ipo_current_issue]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ipo_current_issue](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[symbol] [varchar](20) NOT NULL,
	[companyName] [varchar](500) NOT NULL,
	[series] [varchar](500) NOT NULL,
	[issueStartDate] [varchar](11) NOT NULL,
	[issueEndDate] [varchar](11) NOT NULL,
	[status] [varchar](56) NOT NULL,
	[issueSize] [int] NULL,
	[issuePrice] [varchar](57) NULL,
	[srNo] [varchar](150) NULL,
	[category] [varchar](250) NULL,
	[noOfSharesOffered] [int] NOT NULL,
	[noOfsharesBid] [int] NOT NULL,
	[noOfTime] [numeric](5, 2) NOT NULL,
	[isBse] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ipo_Upcomming]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ipo_Upcomming](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[symbol] [varchar](10) NOT NULL,
	[companyName] [varchar](500) NOT NULL,
	[series] [varchar](500) NULL,
	[issueStartDate] [varchar](11) NOT NULL,
	[issueEndDate] [varchar](11) NOT NULL,
	[status] [varchar](11) NOT NULL,
	[issueSize] [int] NULL,
	[issuePrice] [varchar](7) NULL,
	[sr_no] [int] NULL,
	[isBse] [bit] NULL,
	[lotSize] [int] NULL,
	[priceBand] [varchar](250) NULL,
	[filename] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Live_Stocks]    Script Date: 05-02-2024 21:23:23 ******/
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
	[VolumeC] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MMSN_Companies]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MMSN_Companies](
	[MSN_SECID] [nvarchar](4000) NULL,
	[estimate_numberOfAnalysts] [nvarchar](4000) NULL,
	[estimate_recommendationRate] [nvarchar](4000) NULL,
	[estimate_recommendation] [nvarchar](4000) NULL,
	[estimate_currency] [nvarchar](4000) NULL,
	[estimate_numberOfPriceTargets] [nvarchar](4000) NULL,
	[estimate_meanPriceTarget] [nvarchar](4000) NULL,
	[estimate_highPriceTarget] [nvarchar](4000) NULL,
	[estimate_lowPriceTarget] [nvarchar](4000) NULL,
	[estimate_medianPriceTarget] [nvarchar](4000) NULL,
	[estimate_medianEpsTarget] [nvarchar](4000) NULL,
	[estimate_analystRecommendation] [nvarchar](4000) NULL,
	[estimate_dateLastUpdated] [nvarchar](4000) NULL,
	[beta] [nvarchar](4000) NULL,
	[CompanyMetrics_pE5YearHighRatio] [nvarchar](4000) NULL,
	[CompanyMetrics_pE5YearLowRatio] [nvarchar](4000) NULL,
	[CompanyMetrics_revenueYTDYTD] [nvarchar](4000) NULL,
	[CompanyMetrics_revenueQQLastYearGrowthRate] [nvarchar](4000) NULL,
	[CompanyMetrics_netIncomeYTDYTDGrowthRate] [nvarchar](4000) NULL,
	[CompanyMetrics_netIncomeQQLastYearGrowthRate] [nvarchar](4000) NULL,
	[CompanyMetrics_revenue5YearAverageGrowthRate] [nvarchar](4000) NULL,
	[CompanyMetrics_interestCoverage] [nvarchar](4000) NULL,
	[CompanyMetrics_priceCashFlowRatio] [nvarchar](4000) NULL,
	[CompanyMetrics_revenue3YearAverage] [nvarchar](4000) NULL,
	[CompanyMetrics_trailingAnnualDividendYield] [nvarchar](4000) NULL,
	[CompanyMetrics_priceBookRatio] [nvarchar](4000) NULL,
	[CompanyMetrics_priceSalesRatio] [nvarchar](4000) NULL,
	[CompanyMetrics_bookValueShareRatio] [nvarchar](4000) NULL,
	[CompanyMetrics_ratingCashFlow] [nvarchar](4000) NULL,
	[CompanyMetrics_payoutRatio] [nvarchar](4000) NULL,
	[CompanyMetrics_quickRatio] [nvarchar](4000) NULL,
	[CompanyMetrics_current] [nvarchar](4000) NULL,
	[CompanyMetrics_debtEquityRatio] [nvarchar](4000) NULL,
	[CompanyMetrics_grossMargin] [nvarchar](4000) NULL,
	[CompanyMetrics_preTaxMargin] [nvarchar](4000) NULL,
	[CompanyMetrics_netProfitMargin] [nvarchar](4000) NULL,
	[CompanyMetrics_averageGrossMargin5Year] [nvarchar](4000) NULL,
	[CompanyMetrics_averagePreTaxMargin5Year] [nvarchar](4000) NULL,
	[CompanyMetrics_averageNetProfitMargin5Year] [nvarchar](4000) NULL,
	[CompanyMetrics_operatingMargin] [nvarchar](4000) NULL,
	[CompanyMetrics_netMarginPercent] [nvarchar](4000) NULL,
	[CompanyMetrics_returnOnAssetCurrent] [nvarchar](4000) NULL,
	[CompanyMetrics_returnOnAsset5YearAverage] [nvarchar](4000) NULL,
	[CompanyMetrics_returnOnCapitalCurrent] [nvarchar](4000) NULL,
	[CompanyMetrics_assetTurnover] [nvarchar](4000) NULL,
	[CompanyMetrics_inventoryTurnover] [nvarchar](4000) NULL,
	[CompanyMetrics_receivableTurnover] [nvarchar](4000) NULL,
	[shareStatistics_lastSplitFactor] [nvarchar](4000) NULL,
	[shareStatistics_lastSplitDate] [nvarchar](4000) NULL,
	[shareStatistics_dividendYield] [nvarchar](4000) NULL,
	[shareStatistics_exDividendAmount] [nvarchar](4000) NULL,
	[shareStatistics_sharesOutstanding] [nvarchar](4000) NULL,
	[shareStatistics_enterpriseValue] [nvarchar](4000) NULL,
	[industryMetrics_assetTurnover] [nvarchar](4000) NULL,
	[industryMetrics_averageGrossMargin5Year] [nvarchar](4000) NULL,
	[industryMetrics_averageNetProfitMargin5Year] [nvarchar](4000) NULL,
	[industryMetrics_averagePreTaxMargin5Year] [nvarchar](4000) NULL,
	[industryMetrics_bookValueShareRatio] [nvarchar](4000) NULL,
	[industryMetrics_currentRatio] [nvarchar](4000) NULL,
	[industryMetrics_debtEquityRatio] [nvarchar](4000) NULL,
	[industryMetrics_dividendYield] [nvarchar](4000) NULL,
	[industryMetrics_dividendYield5YearAverage] [nvarchar](4000) NULL,
	[industryMetrics_grossMargin] [nvarchar](4000) NULL,
	[industryMetrics_incomeEmployee] [nvarchar](4000) NULL,
	[industryMetrics_interestCoverage] [nvarchar](4000) NULL,
	[industryMetrics_inventoryTurnover] [nvarchar](4000) NULL,
	[industryMetrics_netIncomeQQLastYearGrowthRate] [nvarchar](4000) NULL,
	[industryMetrics_netIncomeYTDYTDGrowthRate] [nvarchar](4000) NULL,
	[industryMetrics_netProfitMargin] [nvarchar](4000) NULL,
	[industryMetrics_pEGrowthRatio] [nvarchar](4000) NULL,
	[industryMetrics_preTaxMargin] [nvarchar](4000) NULL,
	[industryMetrics_priceBookRatio] [nvarchar](4000) NULL,
	[industryMetrics_priceCashFlowRatio] [nvarchar](4000) NULL,
	[industryMetrics_priceSalesRatio] [nvarchar](4000) NULL,
	[industryMetrics_quickRatio] [nvarchar](4000) NULL,
	[industryMetrics_returnOnAsset5YearAverage] [nvarchar](4000) NULL,
	[industryMetrics_returnOnAssetCurrent] [nvarchar](4000) NULL,
	[industryMetrics_returnOnCapital5YearAverage] [nvarchar](4000) NULL,
	[industryMetrics_returnOnCapitalCurrent] [nvarchar](4000) NULL,
	[industryMetrics_returnOnEquity5YearAverage] [nvarchar](4000) NULL,
	[industryMetrics_returnOnEquityCurrent] [nvarchar](4000) NULL,
	[industryMetrics_revenueEmployee] [nvarchar](4000) NULL,
	[industryMetrics_revenueQQLastYearGrowthRate] [nvarchar](4000) NULL,
	[industryMetrics_revenueYTDYTD] [nvarchar](4000) NULL,
	[industryMetrics_receivableTurnover] [nvarchar](4000) NULL,
	[industryMetrics_leverageRatio] [nvarchar](4000) NULL,
	[keyMetrics_debtToEquityRatio] [nvarchar](4000) NULL,
	[keyMetrics_eps] [nvarchar](4000) NULL,
	[keyMetrics_forwardPriceToEPS] [nvarchar](4000) NULL,
	[keyMetrics_payoutRatio] [nvarchar](4000) NULL,
	[keyMetrics_priceToBookRatio] [nvarchar](4000) NULL,
	[keyMetrics_profitability] [nvarchar](4000) NULL,
	[keyMetrics_stockGrowth] [nvarchar](4000) NULL,
	[keyMetrics_latestRevenue] [nvarchar](4000) NULL,
	[keyMetrics_latestIncome] [nvarchar](4000) NULL,
	[keyMetrics_latestNetProfitMargin] [nvarchar](4000) NULL,
	[keyMetrics_latestRevenuePerShare] [nvarchar](4000) NULL,
	[quote_price] [nvarchar](4000) NULL,
	[quote_priceChange] [nvarchar](4000) NULL,
	[quote_priceDayHigh] [nvarchar](4000) NULL,
	[quote_priceDayLow] [nvarchar](4000) NULL,
	[quote_timeLastTraded] [nvarchar](4000) NULL,
	[quote_priceDayOpen] [nvarchar](4000) NULL,
	[quote_pricePreviousClose] [nvarchar](4000) NULL,
	[quote_datePreviousClose] [nvarchar](4000) NULL,
	[quote_priceAsk] [nvarchar](4000) NULL,
	[quote_askSize] [nvarchar](4000) NULL,
	[quote_priceBid] [nvarchar](4000) NULL,
	[quote_bidSize] [nvarchar](4000) NULL,
	[quote_accumulatedVolume] [nvarchar](4000) NULL,
	[quote_averageVolume] [nvarchar](4000) NULL,
	[quote_peRatio] [nvarchar](4000) NULL,
	[quote_priceChangePercent] [nvarchar](4000) NULL,
	[quote_price52wHigh] [nvarchar](4000) NULL,
	[quote_price52wLow] [nvarchar](4000) NULL,
	[quote_priceClose] [nvarchar](4000) NULL,
	[quote_yieldPercent] [nvarchar](4000) NULL,
	[quote_priceChange1Week] [nvarchar](4000) NULL,
	[quote_priceChange1Month] [nvarchar](4000) NULL,
	[quote_priceChange3Month] [nvarchar](4000) NULL,
	[quote_priceChange6Month] [nvarchar](4000) NULL,
	[quote_priceChangeYTD] [nvarchar](4000) NULL,
	[quote_priceChange1Year] [nvarchar](4000) NULL,
	[quote_return1Week] [nvarchar](4000) NULL,
	[quote_return1Month] [nvarchar](4000) NULL,
	[quote_return3Month] [nvarchar](4000) NULL,
	[quote_return6Month] [nvarchar](4000) NULL,
	[quote_returnYTD] [nvarchar](4000) NULL,
	[quote_return1Year] [nvarchar](4000) NULL,
	[quote_sourceExchangeCode] [nvarchar](4000) NULL,
	[quote_sourceExchangeName] [nvarchar](4000) NULL,
	[quote_marketCap] [nvarchar](4000) NULL,
	[quote_marketCapCurrency] [nvarchar](4000) NULL,
	[quote_exchangeId] [nvarchar](4000) NULL,
	[quote_exchangeCode] [nvarchar](4000) NULL,
	[quote_exchangeName] [nvarchar](4000) NULL,
	[quote_offeringStatus] [nvarchar](4000) NULL,
	[quote_displayName] [nvarchar](4000) NULL,
	[quote_shortName] [nvarchar](4000) NULL,
	[quote_securityType] [nvarchar](4000) NULL,
	[quote_instrumentId] [nvarchar](4000) NULL,
	[quote_symbol] [nvarchar](4000) NULL,
	[quote_country] [nvarchar](4000) NULL,
	[quote_market] [nvarchar](4000) NULL,
	[quote_timeLastUpdated] [nvarchar](4000) NULL,
	[quote_currency] [nvarchar](4000) NULL,
	[quote__p] [nvarchar](4000) NULL,
	[quote_id] [nvarchar](4000) NULL,
	[quote__t] [nvarchar](4000) NULL,
	[address_street] [nvarchar](4000) NULL,
	[address_city] [nvarchar](4000) NULL,
	[address_state] [nvarchar](4000) NULL,
	[address_zip] [nvarchar](4000) NULL,
	[address_country] [nvarchar](4000) NULL,
	[address_countryCode] [nvarchar](4000) NULL,
	[address_phone] [nvarchar](4000) NULL,
	[address_fax] [nvarchar](4000) NULL,
	[Symbol] [varchar](50) NULL,
	[WebSite] [varchar](500) NULL,
	[Created_On] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MSN_Equities_Notification]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MSN_Equities_Notification](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MSN_SECID] [varchar](50) NULL,
	[Description] [varchar](500) NULL,
	[IsFavoriteAdded] [bit] NULL,
	[Status] [varchar](50) NULL,
	[Url] [varchar](500) NULL,
	[Created_On] [datetime] NULL,
	[Updated_On] [datetime] NULL,
	[IsNotified] [bit] NULL,
	[IsFavoriteRemoved] [bit] NULL,
 CONSTRAINT [PK_MSN_Equities_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MSNDownStocks]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MSNDownStocks](
	[keyMetrics_eps] [nvarchar](4000) NULL,
	[industryMetrics_dividendYield] [nvarchar](4000) NULL,
	[shareStatistics_dividendYield] [nvarchar](4000) NULL,
	[Created_On] [datetime] NULL,
	[Symbol] [varchar](50) NULL,
	[quote_displayName] [nvarchar](4000) NULL,
	[quote_return1Week] [nvarchar](4000) NULL,
	[quote_return1Month] [nvarchar](4000) NULL,
	[quote_return3Month] [nvarchar](4000) NULL,
	[quote_return6Month] [nvarchar](4000) NULL,
	[quote_priceChange1Month] [nvarchar](4000) NULL,
	[quote_priceChange3Month] [nvarchar](4000) NULL,
	[quote_priceChange6Month] [nvarchar](4000) NULL,
	[estimate_recommendation] [nvarchar](4000) NULL,
	[quote_priceDayOpen] [nvarchar](4000) NULL,
	[quote_priceChange1Week] [decimal](18, 0) NULL,
	[previous_priceChange1Week] [decimal](18, 0) NULL,
	[quote_accumulatedVolume] [nvarchar](4000) NULL,
	[quote_averageVolume] [nvarchar](4000) NULL,
	[quote_timeLastUpdated] [datetime] NULL,
	[estimate_dateLastUpdated] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MSNSTCOKS]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MSNSTCOKS](
	[symbol] [varchar](50) NULL,
	[JsonData] [varchar](max) NULL,
	[MSN_SECID] [nvarchar](4000) NULL,
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
	[latestNetProfitMargin] [nvarchar](4000) NULL,
	[latestIncome] [nvarchar](4000) NULL,
	[latestRevenuePerShare] [nvarchar](4000) NULL,
	[debtToEquityRatio] [nvarchar](4000) NULL,
	[exDividendAmount] [nvarchar](4000) NULL,
	[sharesOutstanding] [nvarchar](4000) NULL,
	[enterpriseValue] [nvarchar](4000) NULL,
	[dividendYield] [nvarchar](4000) NULL,
	[bookValueShareRatio] [nvarchar](4000) NULL,
	[trailingAnnualDividendYield] [nvarchar](4000) NULL,
	[website] [nvarchar](4000) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MSNUPStocks]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MSNUPStocks](
	[keyMetrics_eps] [nvarchar](4000) NULL,
	[industryMetrics_dividendYield] [nvarchar](4000) NULL,
	[shareStatistics_dividendYield] [nvarchar](4000) NULL,
	[Created_On] [datetime] NULL,
	[Symbol] [varchar](50) NULL,
	[quote_displayName] [nvarchar](4000) NULL,
	[quote_return1Week] [nvarchar](4000) NULL,
	[quote_return1Month] [nvarchar](4000) NULL,
	[quote_return3Month] [nvarchar](4000) NULL,
	[quote_return6Month] [nvarchar](4000) NULL,
	[quote_priceChange1Month] [nvarchar](4000) NULL,
	[quote_priceChange3Month] [nvarchar](4000) NULL,
	[quote_priceChange6Month] [nvarchar](4000) NULL,
	[estimate_recommendation] [nvarchar](4000) NULL,
	[quote_priceDayOpen] [nvarchar](4000) NULL,
	[quote_priceChange1Week] [decimal](18, 0) NULL,
	[previous_priceChange1Week] [decimal](18, 0) NULL,
	[quote_accumulatedVolume] [nvarchar](4000) NULL,
	[quote_averageVolume] [nvarchar](4000) NULL,
	[quote_timeLastUpdated] [datetime] NULL,
	[estimate_dateLastUpdated] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NSEAnnouncement]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NSEAnnouncement](
	[Symbol] [varchar](100) NULL,
	[CompanyName] [varchar](max) NULL,
	[Subject] [varchar](max) NULL,
	[Details] [varchar](max) NULL,
	[BroadcastDateTime] [datetime] NULL,
	[Receipt] [varchar](max) NULL,
	[DISSEMINATION] [varchar](500) NULL,
	[Difference] [varchar](max) NULL,
	[Attachement] [varchar](max) NULL,
	[CreatedOn] [date] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PO_CONFIG]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PO_CONFIG](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PO_KEY_NAME] [varchar](100) NULL,
	[PO_KEY_TOKEN] [varchar](100) NULL,
	[PO_DESCRIPTION] [varchar](150) NULL,
	[user] [varchar](200) NULL,
	[title] [varchar](500) NULL,
	[retry] [int] NULL,
	[expire] [int] NULL,
	[sound] [varchar](50) NULL,
 CONSTRAINT [PK_PO_CONFIG_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PO_CONFIG_2]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PO_CONFIG_2](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PO_KEY_NAME] [varchar](100) NULL,
	[PO_KEY_TOKEN] [varchar](100) NULL,
	[PO_DESCRIPTION] [varchar](150) NULL,
	[user] [varchar](200) NULL,
	[title] [varchar](500) NULL,
	[retry] [int] NULL,
	[expire] [int] NULL,
	[sound] [varchar](50) NULL,
 CONSTRAINT [PK_PO_CONFIG] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PO_CONFIG_olds]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PO_CONFIG_olds](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PO_KEY_NAME] [varchar](100) NULL,
	[PO_KEY_TOKEN] [varchar](100) NULL,
	[PO_DESCRIPTION] [varchar](150) NULL,
	[user] [varchar](200) NULL,
	[priority] [int] NULL,
	[title] [varchar](500) NULL,
	[retry] [int] NULL,
	[expire] [int] NULL,
	[sound] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Portfolio_Holdings_Details]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Portfolio_Holdings_Details](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[stock_code] [varchar](60) NOT NULL,
	[exchange_code] [varchar](30) NOT NULL,
	[quantity] [int] NOT NULL,
	[average_price] [decimal](7, 2) NOT NULL,
	[booked_profit_loss] [decimal](9, 2) NOT NULL,
	[current_market_price] [decimal](7, 2) NOT NULL,
	[change_percentage] [decimal](18, 16) NOT NULL,
	[answer_flag] [varchar](10) NOT NULL,
	[product_type] [varchar](30) NULL,
	[expiry_date] [varchar](50) NULL,
	[strike_price] [varchar](50) NULL,
	[right] [varchar](50) NULL,
	[category_index_per_stock] [varchar](50) NULL,
	[action] [varchar](50) NULL,
	[realized_profit] [varchar](50) NULL,
	[unrealized_profit] [varchar](50) NULL,
	[open_position_value] [varchar](50) NULL,
	[portfolio_charges] [varchar](50) NULL,
	[Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Portfolio_Holdings_Details_old]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Portfolio_Holdings_Details_old](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[stock_code] [varchar](60) NOT NULL,
	[exchange_code] [varchar](30) NOT NULL,
	[quantity] [int] NOT NULL,
	[average_price] [decimal](7, 2) NOT NULL,
	[booked_profit_loss] [decimal](9, 2) NOT NULL,
	[current_market_price] [decimal](7, 2) NOT NULL,
	[change_percentage] [decimal](18, 16) NOT NULL,
	[answer_flag] [varchar](10) NOT NULL,
	[product_type] [varchar](30) NULL,
	[expiry_date] [varchar](50) NULL,
	[strike_price] [varchar](50) NULL,
	[right] [varchar](50) NULL,
	[category_index_per_stock] [varchar](50) NULL,
	[action] [varchar](50) NULL,
	[realized_profit] [varchar](50) NULL,
	[unrealized_profit] [varchar](50) NULL,
	[open_position_value] [varchar](50) NULL,
	[portfolio_charges] [varchar](50) NULL,
	[Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Portfolio_Positions_Details]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Portfolio_Positions_Details](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[segment] [varchar](60) NOT NULL,
	[product_type] [varchar](40) NOT NULL,
	[exchange_code] [varchar](30) NOT NULL,
	[stock_code] [varchar](60) NOT NULL,
	[expiry_date] [varchar](100) NULL,
	[strike_price] [varchar](100) NULL,
	[right] [varchar](100) NULL,
	[action] [varchar](40) NOT NULL,
	[quantity] [int] NOT NULL,
	[average_price] [decimal](10, 2) NOT NULL,
	[settlement_id] [int] NOT NULL,
	[margin_amount] [bit] NOT NULL,
	[ltp] [decimal](6, 2) NOT NULL,
	[price] [bit] NOT NULL,
	[stock_index_indicator] [varchar](30) NULL,
	[cover_quantity] [bit] NOT NULL,
	[stoploss_trigger] [varchar](30) NULL,
	[stoploss] [bit] NOT NULL,
	[take_profit] [bit] NOT NULL,
	[available_margin] [bit] NOT NULL,
	[squareoff_mode] [varchar](50) NOT NULL,
	[mtf_sell_quantity] [bit] NOT NULL,
	[mtf_net_amount_payable] [bit] NOT NULL,
	[mtf_expiry_date] [varchar](10) NOT NULL,
	[order_id] [varchar](10) NOT NULL,
	[cover_order_flow] [varchar](10) NOT NULL,
	[cover_order_executed_quantity] [bit] NOT NULL,
	[pledge_status] [varchar](30) NULL,
	[pnl] [bit] NOT NULL,
	[underlying] [varchar](30) NULL,
	[order_segment_code] [varchar](10) NOT NULL,
	[Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stock_Award]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock_Award](
	[Code] [varchar](50) NULL,
	[Symbol] [varchar](50) NULL,
	[SecurityName] [nvarchar](255) NULL,
	[OrderCount] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stock_Days]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock_Days](
	[Symbol] [varchar](50) NULL,
	[Days] [bigint] NULL,
	[BearishCount] [bigint] NULL,
	[BullishCount] [bigint] NULL,
	[CUNT] [bigint] NULL,
	[Updated_on] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STOCK_NTFCTN]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STOCK_NTFCTN](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[symbol] [varchar](50) NULL,
	[Date] [datetime] NULL,
	[STOCKName] [varchar](max) NULL,
	[IsNotified] [bit] NULL,
	[IsUppCKT] [bit] NULL,
	[ISSell] [bit] NULL,
	[ISPrict] [bit] NULL,
	[Change] [decimal](10, 2) NULL,
	[PO_KEY] [varchar](500) NULL,
	[last] [decimal](10, 2) NULL,
	[Priority] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STOCK_NTFCTN_OLD]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STOCK_NTFCTN_OLD](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[symbol] [varchar](50) NULL,
	[Date] [date] NULL,
	[STOCKName] [varchar](max) NULL,
	[IsNotified] [bit] NULL,
	[IsUppCKT] [bit] NULL,
	[ISSell] [bit] NULL,
	[ISPrict] [bit] NULL,
	[Change] [decimal](5, 2) NULL,
	[PO_KEY] [varchar](50) NULL,
	[last] [decimal](10, 2) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockAlerts_Automatic]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockAlerts_Automatic](
	[Symbol] [varchar](50) NULL,
	[Quanity] [bigint] NULL,
	[RID] [varchar](50) NULL,
	[EX] [varchar](50) NULL,
	[ORDERType] [varchar](50) NULL,
	[BUY_PRICE] [decimal](18, 2) NULL,
	[STOPLoss] [decimal](18, 2) NULL,
	[stock_code] [varchar](50) NULL,
	[DATE] [datetime] NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](10) NULL,
	[InvestedPrice] [decimal](10, 3) NULL,
	[Resposne] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockPriceConfig]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockPriceConfig](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](100) NOT NULL,
	[Price] [decimal](10, 2) NULL,
	[Created_On] [datetime] NULL,
	[Updated_On] [datetime] NULL,
	[change] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Symbol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STOCKPriceLock]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STOCKPriceLock](
	[Symbol] [varchar](50) NULL,
	[PriceLock] [nchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPivot]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[TempPivotCompany]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPivotCompany](
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
	[latestNetProfitMargin] [nvarchar](4000) NULL,
	[latestIncome] [nvarchar](4000) NULL,
	[latestRevenuePerShare] [nvarchar](4000) NULL,
	[debtToEquityRatio] [nvarchar](4000) NULL,
	[exDividendAmount] [nvarchar](4000) NULL,
	[sharesOutstanding] [nvarchar](4000) NULL,
	[enterpriseValue] [nvarchar](4000) NULL,
	[dividendYield] [nvarchar](4000) NULL,
	[bookValueShareRatio] [nvarchar](4000) NULL,
	[trailingAnnualDividendYield] [nvarchar](4000) NULL,
	[website] [nvarchar](4000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPivotResults]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPivotResults](
	[symbol] [varchar](50) NULL,
	[STOCK_name] [varchar](max) NULL,
	[12-12-2023] [decimal](38, 2) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[Ticker_Stocks_01_02_24]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_01_02_24](
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
/****** Object:  Table [dbo].[Ticker_Stocks_02]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_02](
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
/****** Object:  Table [dbo].[Ticker_Stocks_18_01_24]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_18_01_24](
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
/****** Object:  Table [dbo].[Ticker_Stocks_19]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_19](
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
/****** Object:  Table [dbo].[Ticker_Stocks_20]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_20](
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
/****** Object:  Table [dbo].[Ticker_Stocks_23]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_23](
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
/****** Object:  Table [dbo].[Ticker_Stocks_24]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_24](
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
/****** Object:  Table [dbo].[Ticker_Stocks_30_01_24]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_30_01_24](
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
/****** Object:  Table [dbo].[Ticker_Stocks_31_01_24]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_31_01_24](
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
/****** Object:  Table [dbo].[Ticker_Stocks_Days]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[Ticker_Stocks_Histry]    Script Date: 05-02-2024 21:23:23 ******/
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
	[VolumeC] [varchar](25) NULL,
	[id] [bigint] IDENTITY(1,1) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks_Histry_Extended]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_Histry_Extended](
	[symbol] [varchar](50) NOT NULL,
	[BearishCount] [int] NULL,
	[BulishCount] [int] NULL,
	[Ltt] [datetime] NULL,
	[Match] [varchar](50) NULL,
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks_Histry_Extended_Ticks]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_Histry_Extended_Ticks](
	[symbol] [varchar](50) NOT NULL,
	[BearishCount] [int] NULL,
	[BulishCount] [int] NULL,
	[Ltt] [datetime] NULL,
	[Match] [varchar](50) NULL,
	[Data] [varchar](max) NULL,
	[Stock_Name] [varchar](500) NULL,
	[StockCode] [varchar](100) NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks_Histry_Extended_Ticks_OLD]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_Histry_Extended_Ticks_OLD](
	[symbol] [varchar](50) NOT NULL,
	[BearishCount] [int] NULL,
	[BulishCount] [int] NULL,
	[Ltt] [datetime] NULL,
	[Match] [varchar](50) NULL,
	[Data] [varchar](max) NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks_Histry_Extended_Ticks_old2]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_Histry_Extended_Ticks_old2](
	[symbol] [varchar](50) NOT NULL,
	[BearishCount] [int] NULL,
	[BulishCount] [int] NULL,
	[Ltt] [datetime] NULL,
	[Match] [varchar](50) NULL,
	[Data] [varchar](max) NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticker_Stocks_temp]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_temp](
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
/****** Object:  Table [dbo].[Ticker_Stocks_Yesterday]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[Ticker_Stocks_Yesterday_old]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticker_Stocks_Yesterday_old](
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
/****** Object:  Table [dbo].[TodaysRatios]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  Table [dbo].[WatchList]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WatchList](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Symbol] [varchar](50) NOT NULL,
	[Date] [datetime] NULL,
	[Comments] [varchar](500) NULL,
	[Created_On] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WatchList_NEW]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WatchList_NEW](
	[SYmbole] [varchar](50) NULL,
	[Comments] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[yourTable]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[yourTable](
	[color] [varchar](5) NULL,
	[Paul] [int] NULL,
	[John] [int] NULL,
	[Tim] [int] NULL,
	[Eric] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Company_Ratings_Only_DIVI] ADD  DEFAULT (getdate()) FOR [UpdatedOn]
GO
ALTER TABLE [dbo].[Equitys] ADD  DEFAULT ((0)) FOR [IsEnabledForAutoTrade]
GO
ALTER TABLE [dbo].[Equitys] ADD  DEFAULT (NULL) FOR [Tdays]
GO
ALTER TABLE [dbo].[NSEAnnouncement] ADD  CONSTRAINT [DF_NSEAnnouncement_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Stock_Days] ADD  DEFAULT (getdate()) FOR [Updated_on]
GO
ALTER TABLE [dbo].[STOCK_NTFCTN] ADD  DEFAULT ((0)) FOR [Priority]
GO
/****** Object:  StoredProcedure [dbo].[AddOrUpdateAutoTrade]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[AddOrUpdateAutoTrade](@msnId varchar(500),@isAutoTrade bit)
as
begin
Update Equitys set IsEnabledForAutoTrade=@isAutoTrade where MSN_SECID=@msnId
if(@isAutoTrade=0)
begin
Delete from StockPriceConfig where Symbol=(select Symbol from Equitys where MSN_SECID= @msnId)
end
End
GO
/****** Object:  StoredProcedure [dbo].[AddOrUpdateFavorites]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[AddOrUpdateFavorites](@msnId varchar(500),@ifavorite bit)
as
begin
Update Equitys set isprime=@ifavorite where MSN_SECID=@msnId
End
GO
/****** Object:  StoredProcedure [dbo].[AddOrUpdateForT3]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[AddOrUpdateForT3](@symbol varchar(500),@Value varchar(10) )
as
begin

Update Equitys set TDays=@Value where Symbol=@symbol
--if(@column='T1')
--Update Equitys set IsT1=@Value where Symbol=@symbol
--if(@column='T2')
--Update Equitys set IsT2=@Value where Symbol=@symbol
--if(@column='T3')
--Update Equitys set IsT3=@Value where Symbol=@symbol

End
GO
/****** Object:  StoredProcedure [dbo].[AddOrUpdateForWatchList]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[AddOrUpdateForWatchList](@symbol varchar(500),@Value varchar(10) )
as
begin

Update Equitys set watchList=@Value where Symbol=@symbol
--if(@column='T1')
--Update Equitys set IsT1=@Value where Symbol=@symbol
--if(@column='T2')
--Update Equitys set IsT2=@Value where Symbol=@symbol
--if(@column='T3')
--Update Equitys set IsT3=@Value where Symbol=@symbol

End
GO
/****** Object:  StoredProcedure [dbo].[AddOrUpdateStockChangeConfig]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[AddOrUpdateStockChangeConfig](@Symbol varchar(100),@Change varchar(10))
as
begin
if exists (select 1 from StockPriceConfig where Symbol= @Symbol)
begin
Update StockPriceConfig set [Change]=@Change,Updated_On=GETDATE() where Symbol=@Symbol
end
else
begin 
INSERT INTO [dbo].[StockPriceConfig]
           ([Symbol]
           ,[Change]
           ,[Created_On]
           ,[Updated_On])
     VALUES
           (@Symbol
           ,@Change
           ,GETDATE()
           ,null)
end
end
GO
/****** Object:  StoredProcedure [dbo].[AddOrUpdateStockPriceConfig]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create Procedure [dbo].[AddOrUpdateStockPriceConfig](@Symbol varchar(100),@Price decimal(10,2))
as
begin
if exists (select 1 from StockPriceConfig where Symbol= @Symbol)
begin
Update StockPriceConfig set Price=@Price,Updated_On=GETDATE() where Symbol=@Symbol
end
else
begin 
INSERT INTO [dbo].[StockPriceConfig]
           ([Symbol]
           ,[Price]
           ,[Created_On]
           ,[Updated_On])
     VALUES
           (@Symbol
           ,@Price
           ,GETDATE()
           ,null)
end
end
GO
/****** Object:  StoredProcedure [dbo].[AddOrUpdateWatchList]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[AddOrUpdateWatchList](@Symbol varchar(500),@Date DateTime)
as
begin
INSERT INTO [dbo].[WatchList]
           ([Symbol]
           ,[Date],Created_On)
     VALUES
           (@Symbol
           ,@Date,
		   GETDATE())
End
GO
/****** Object:  StoredProcedure [dbo].[AUTOSELLHolds]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[AUTOSELLHolds]
as
begin

print 1
--Declare @Date  DateTime = (select MAX(Date) from Portfolio_Holdings_Details )

--INSERT INTO [dbo].[AUTO_BUY_EQUTIES]
--           ([Symbol]
--           ,[Quanity]
--           ,[RID]
--           ,[EX]
--           ,[ORDERType]
--           ,[BUY_PRICE]
--           ,[STOPLoss]
--           ,[stock_code]
--           ,[DATE]
--           ,[Action],InvestedPrice)
--SELECT  
--      [stock_code]
--      ,quantity
--      ,NULL
--      ,exchange_code
--      ,'limit'
--	  ,cast( ([dbo].GetPerCentageOfTotalValue(10,quantity*average_price)/quantity) as decimal(10,0))
--	  ,null
--	  ,[stock_code]
--	  ,GETDATE()
--	  ,'Sell'
--	  , average_price
--  FROM [STOCK].[dbo].[Portfolio_Holdings_Details] where 
--  CAST(Date AS date)= CAST(@Date AS date) and stock_code not in  ('JIOFIN','ELEIN')
--  and not  exists (select * from  [AUTO_BUY_EQUTIES]  EQ where Eq.stock_code=[stock_code] and action ='sell' and cast(Date as date) =cast(GETDATE() as date))


--  INSERT INTO [dbo].[AUTO_BUY_EQUTIES]
--           ([Symbol]
--           ,[Quanity]
--           ,[RID]
--           ,[EX]
--           ,[ORDERType]
--           ,[BUY_PRICE]
--           ,[STOPLoss]
--           ,[stock_code]
--           ,[DATE]
--           ,[Action],InvestedPrice)
--SELECT  
--      [stock_code]
--      ,quantity
--      ,NULL
--      ,exchange_code
--      ,'limit'
--	  ,cast( ([dbo].GetPerCentageOfTotalValue(6,quantity*ltp)/quantity) as decimal(10,0))
--	  ,null
--	  ,[stock_code]
--	  ,GETDATE()
--	  ,'Sell'
--	  , average_price
--  FROM [STOCK].[dbo].[Portfolio_Positions_Details] where 
--  CAST(Date AS date)= CAST(@Date AS date) and stock_code not in  ('JIOFIN','ELEIN')
--  and not  exists (select * from  [AUTO_BUY_EQUTIES]  EQ where Eq.stock_code=[stock_code] and action ='sell' and cast(Date as date) =cast(GETDATE() as date))


END
GO
/****** Object:  StoredProcedure [dbo].[BatchExecute]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  StoredProcedure [dbo].[BatchExecute_copy]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[BatchExecute_copy] 
as begin 
print '1'
exec Import_Histry
--exec Ticker_yesterday
--exec Import_Last_Stock
end
GO
/****** Object:  StoredProcedure [dbo].[CLEAR_beRISH_bULLISH]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[CLEAR_beRISH_bULLISH]
AS
BEGIN

--;WITH Cte AS(
--SELECT *,
--        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
--        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
--    FROM Ticker_Stocks_Histry_Extended_Ticks with (nolock)    where CAST(ltt as Date )=  CAST(GETDATE()-1 as Date) )

--	Delete  from  Cte where RnDesc_C > 1 

--	sELECT L.stock_name ,* FROM Ticker_Stocks_Histry_Extended_Ticks  t
--LEFT JOIN Live_Stocks L ON L.symbol =t.symbol ORDER BY t.BulishCount DESC
print  1
eND
GO
/****** Object:  StoredProcedure [dbo].[DynamicPivotTableInSql]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC dbo.DynamicPivotTableInSql
--  N'Subject'
--  ,N'[Mathematics],[Science],[Geography]'

CREATE PROCEDURE [dbo].[DynamicPivotTableInSql]
  @ColumnToPivot  NVARCHAR(255),
  @ListToPivot    NVARCHAR(255)
AS
BEGIN
 
  DECLARE @SqlStatement NVARCHAR(MAX)
  SET @SqlStatement = N'
    SELECT * FROM (
      SELECT
        [symbol],
        [dateLastUpdated],
		[recommendation],
		[beta]
        
      FROM [MSNSTCOKS]
    ) StudentResults
    PIVOT (
      SUM([beta])
      FOR ['+@ColumnToPivot+']
      IN (
        '+@ListToPivot+'
      )
    ) AS PivotTable
  ';
 
  EXEC(@SqlStatement)
 
END
GO
/****** Object:  StoredProcedure [dbo].[DynamicPivotTableInSqlForCompanies]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DynamicPivotTableInSqlForCompanies]
  @ColumnToPivot  NVARCHAR(255),
  @ListToPivot    NVARCHAR(255)
AS
--EXEC dbo.DynamicPivotTableInSql
--  N'Subject'
--  ,N'[Mathematics],[Science],[Geography]'
BEGIN
 
  DECLARE @SqlStatement NVARCHAR(MAX)
  SET @SqlStatement = N'
    SELECT * FROM (
      SELECT
        [Student],
        [Subject],
        [Marks]
      FROM Grades
    ) StudentResults
    PIVOT (
      SUM([Marks])
      FOR ['+@ColumnToPivot+']
      IN (
        '+@ListToPivot+'
      )
    ) AS PivotTable
  ';
 
  EXEC(@SqlStatement)
 
END
GO
/****** Object:  StoredProcedure [dbo].[ExportJsonData]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/


CREATE procedure [dbo].[ExportJsonData]
as
begin
--exec INSERT_MMSN_Companies
 drop table if exists  #temp1json
SELECT   h.[symbol]
      ,[ltt],
	  [stock_name]
      ,[last]
      ,[open]
      ,[close]
      ,[high]
	  ,[ttv],
	  [low]
      ,[change]
	  ,[avgPrice]
      ,[bPrice]
	    ,[sPrice]
      ,[bQty]
    
      ,[sQty]
      ,[ltq]
      
      ,[ttq]
      ,[totalBuyQt]
      ,[totalSellQ]
      
      
      ,[lowerCktLm]
      ,[upperCktLm]
     
      --,[close]
      --,[exchange]
      
      ,[VolumeC] ,
	  eq.MSN_SECID as msn_secid
  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] h
  left join Equitys eq on eq.Symbol=h.symbol --order by symbol,ltt desc
  where  CAST(ltt as Date)= CAST(GETDATE() as Date)
  
--  where  CAST(ltt as Date) >= '2023-09-01'
  End 
GO
/****** Object:  StoredProcedure [dbo].[GET_CKT]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--[GET_CKT] 10,'2023-09-08','upperCktLm'
CREATE PROC [dbo].[GET_CKT](@top int ,@Date DateTime,@CKTName varchar(25)=null)
as begin 
WITH Cte AS(
SELECT [symbol]
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
      ,[bQty]
      ,[sQty]
      ,[ltq]
      ,[ttq]
      ,[totalBuyQt]
      ,[totalSellQ]
      ,[lowerCktLm]
      ,[upperCktLm]
      ,[VolumeC],
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM [Ticker_Stocks] with (nolock)   where cast(ltt as date ) >   DATEADD(HOUR, -5,@Date)  and  [open]> 0  and 
	(@CKTName ='' or  @CKTName is null or [last]= CASE
         WHEN @CKTName ='upperCktLm' THEN upperCktLm
         WHEN @CKTName ='lowerCktLm' THEN lowerCktLm
		 end))
	Select TK.*,EQ.MSN_SECID from Cte TK
	left join Equitys Eq on Eq.Symbol=TK.symbol
	WHERE
    RnDesc_C= 1 and [last] < 1000
	order by bPrice desc
	END
GO
/****** Object:  StoredProcedure [dbo].[Get_MSN_Stocks]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Get_MSN_Stocks]
as
begin

drop table if exists MSNSTCOKS






sELECT symbol,
JsonData,
JSON_VALUE(JsonData, '$."instrumentId"')  as MSN_SECID,
SecurityName, 
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
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.latestNetProfitMargin')  as latestNetProfitMargin,

JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.latestIncome')  as latestIncome,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.latestRevenuePerShare')  as latestRevenuePerShare,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.debtToEquityRatio')  as debtToEquityRatio,	

JSON_VALUE(JsonData, '$.equity.analysis.shareStatistics.exDividendAmount')  as exDividendAmount,
JSON_VALUE(JsonData, '$.equity.analysis.shareStatistics.sharesOutstanding')  as sharesOutstanding,
JSON_VALUE(JsonData, '$.equity.analysis.shareStatistics.enterpriseValue')  as enterpriseValue,


JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.dividendYield')  as dividendYield,
JSON_VALUE(JsonData, '$.equity.analysis.bookValueShareRatio.bookValueShareRatio')  as bookValueShareRatio,

JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.trailingAnnualDividendYield')  as trailingAnnualDividendYield,


JSON_VALUE(JsonData, '$.equity.company.website')  as website


into MSNSTCOKS


FROM Equitys --WHERE MSN_SECID in ('ahhnr7') 

Select * from MSNSTCOKS order by dateLastUpdated--where  recommendation in ('STRONGBUY','buy') and cast(dateLastUpdated as Date)=CAST(GETDATE() as Date)   order by dateLastUpdated desc
End







GO
/****** Object:  StoredProcedure [dbo].[Get_STock_Days]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_STock_Days]
as
begin

dECLARE @dATE DATE=(SELECT MAX(CREATED_ON) FROM MMSN_Companies)
SELECT  isnull(stock_name,isnull(c.quote_displayName,d.symbol)) as stock_name,isnull([open],0) [open],isnull([close],0) [close],isnull(change,0) change 
      ,[Days]
      ,isnull([BearishCount],0) as [BearishCount]
      ,isnull([BullishCount],0) as [BullishCount] ,
	  isnull(c.estimate_recommendation,'') as estimate_recommendation , isnull(volumeC,'')as volumeC,
	  isnull(c.MSN_SECID,'') as msn_secid
       from [STOCK].[dbo].[Stock_Days] D
	  left join Live_Stocks l on l.symbol=d.symbol
	  left join MMSN_Companies c on c.Symbol=d.symbol and cast(c.Created_On as Date)= CAST(@dATE as Date)
	  order by Days desc
	  
End



GO
/****** Object:  StoredProcedure [dbo].[Get_StockticksbySymbol]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Get_StockticksbySymbol '1.1!541956'
CREATE Procedure [dbo].[Get_StockticksbySymbol](@symbol varchar(20))
as
begin
SELECT  [symbol]
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
      ,[VolumeC]
      ,[Id]
  FROM [STOCK].[dbo].Ticker_Stocks_20 where symbol=@symbol and CAST(ltt as date) > cast('2023-10-01' as date)

  union all
  
  SELECT [symbol]
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
      ,[VolumeC]
      ,[Id]
  FROM [STOCK].[dbo].[Ticker_Stocks_19]  where symbol=@symbol
  end
GO
/****** Object:  StoredProcedure [dbo].[Get_Todays_Stock_PRED]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[Get_Todays_Stock_PRED]
as
begin
drop table if exists #temp


SELECT  
      [open]
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
      ,[VolumeC],
	   cast((cast([totalBuyQt]  as decimal(10,2) )-cast(totalSellQ as decimal(10,2) ))/(cast(totalSellQ as int )) as decimal(10,2)) *100  as Diff into #temp

  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
left JOIN Equitys  eq ON eq.Symbol= P.Symbol
--where E.symbol ='1.1!516092'
WHERE EQ.recommondations in ('strongBuy','buy') 
  
  and CAST(ltt as Date)=CAST(GETDATE()-1 as Date)  and totalBuyQt> 0 and totalSellQ > 0

  Select * from #temp where Diff > 800 order by totalSellQ desc
  End

GO
/****** Object:  StoredProcedure [dbo].[Get_Todays_Stock_PRED_Upper_CKT]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_Todays_Stock_PRED_Upper_CKT]
as
begin
Declare @Date DateTime  = getdate();
 with CTE as (

select distinct symbol,cast([open]  as decimal(15,2)) as [open] ,cast([last]  as decimal(15,2)) as Last,stock_name,VolumeC,cast(ttv  as decimal(15,2)) as ttv,change, 'Increament of 2 and Volumeof 2 ' as Type  from  Ticker_Stocks_Histry where CAST(ltt as date)=CAST(@Date as Date) 
and  last > cast([open]  as decimal(10,2))+ (cast([open]  as decimal(10,2))/100)* 2  and cast([open]  as decimal(10,2))>0 and cast(ttv as decimal(15,2)) > (cast(ttv as decimal(15,2))/100)* 2 and cast (ttv as decimal)  > 500000

union all
select distinct symbol,[open],[last],stock_name,VolumeC,ttv,change ,'Increament of 4 and Volumeof 5 ' as Type from  Ticker_Stocks_Histry where CAST(ltt as date)=CAST(@Date as Date) 
and  last > [open] + ([open]/100)* 4  and [open]>0 and cast(ttv as decimal(15,2)) > (cast(ttv as decimal(15,2))/100)* 5 and cast (ttv as decimal)  > 1000000


union all

select distinct symbol,[open],[last],stock_name,VolumeC,ttv,change ,'Increament of 6 and Volumeof 8 ' as Type from  Ticker_Stocks_Histry where CAST(ltt as date)=CAST(@Date as Date) 
and  last > [open] + ([open]/100)* 6  and [open]>0 and cast(ttv as decimal(15,2)) > (cast(ttv as decimal(15,2))/100)* 8 and cast (ttv as decimal)  > 1000000


union all

select distinct symbol,[open],[last],stock_name,VolumeC,ttv,change ,'Increament of 10 and Volumeof 10 ' as Type from  Ticker_Stocks_Histry where CAST(ltt as date)=CAST(@Date as Date) 
and  last > [open] + ([open]/100)* 10  and [open]>0 and cast(ttv as decimal(15,2)) > (cast(ttv as decimal(15,2))/100)* 10 and cast (ttv as decimal)  > 1000000

)


--Insert into STOCK_NTFCTN(
--[symbol]
--      ,[Date]
--	  ,Last
--      ,[STOCKName]
--      ,[IsNotified]
--      ,[IsUppCKT]
--      ,[ISSell]
--      ,[ISPrict]
--      ,[Change]
--      ,[PO_KEY])
--select p.symbol, cast(@Date as Date) as Date, p.[last], 
--( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Type : '+p.Type +' VOlume :'+ P.VolumeC   ) as STOCKName, 
--0,0,0,0,cast(p.change as decimal(15,2)),'TODAY_ROSEUP' as GroupName 
--from CTE p 
--where not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2))  and CAST(Date as Date)=CAST(GETDATE() as Date) and PO_KEY='TODAY_ROSEUP' )
--order by cast(ttv as decimal) desc


--Insert into STOCK_NTFCTN([symbol]
--      ,[Date]
--	  ,Last
--      ,[STOCKName]
--      ,[IsNotified]
--      ,[IsUppCKT]
--      ,[ISSell]
--      ,[ISPrict]
--      ,[Change]
--      ,[PO_KEY])


--SELECT distinct   p.symbol, cast(ltt as Date) as Date, p.[last],
--( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Rating: '+ cast(c.Rating as varchar(100))+'; Min:'+ cast(THRS_MIN as varchar(100))+'; Max: '+ cast(THRS_MAX as varchar(100))+' ; VOlume :'+ P.VolumeC) as STOCKName 
--,
--0,0,0,0,cast(p.change as decimal(5,2)),

--case when eq.SectorName='Diversified' Then 'Common_LOW_IP'  
--     when eq.SectorName='Telecommunication' Then 'Common_LOW_IP' else 
--     SUBSTRING (eq.SectorName, 1, 13) + '_LOW_IP' End as  GroupName 


--FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
--inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol 
--inner  join Company_Ratings_Only_DIVI C on C.Symbol=p.symbol
--WHERE  cast(change as decimal(5,2)) < -3 and cast(change as decimal(5,2)) < 0 
----and cast(p.change as  decimal(5,2))%0.25=0.0    
--and cast(P.ttv  as decimal)> 500000
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,0)) = cast(p.change as  decimal(5,0))  
--and CAST(Date as Date)=CAST(ltt as Date) )
--GROUP BY SectorName, p.symbol,change,p.[last],c.Rating,THRS_MIN,THRS_MAX,cast(ltt as Date),[stock_name],VolumeC;







Insert into STOCK_NTFCTN([symbol]
      ,[Date]
	  ,Last
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY])

(
SELECT distinct   p.symbol, cast(ltt as Date) as Date, p.[last],
( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Rating: '+ cast(c.Rating as varchar(100))+'; Min:'+ cast(THRS_MIN as varchar(100))+'; Max: '+ cast(THRS_MAX as varchar(100))+' ; VOlume :'+ P.VolumeC) as STOCKName 
,
0,0,0,0,cast(p.change as decimal(5,2)),

case when eq.SectorName='Diversified' Then 'Common_LOW'  
     when eq.SectorName='Telecommunication' Then 'Common_LOW' else 
     SUBSTRING (eq.SectorName, 1, 13) + '_LOW' End as  GroupName 


FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol 
inner  join Company_Ratings_Only_DIVI C on C.Symbol=p.symbol
WHERE  cast(change as decimal(5,2)) < -1 and cast(change as decimal(5,2)) < 0 and cast(p.change as  decimal(5,2))%0.25=0.0    and cast(P.ttv  as decimal)> 500000
and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as  decimal(5,2))  
and CAST(Date as Date)=CAST(ltt as Date) )
GROUP BY SectorName, p.symbol,change,p.[last],c.Rating,THRS_MIN,THRS_MAX,cast(ltt as Date),[stock_name],VolumeC


UNION ALL

SELECT distinct    p.symbol, cast(ltt as Date) as Date, p.[last],
( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Rating: '+ cast(c.Rating as varchar(100))+'; Min:'+ cast(THRS_MIN as varchar(100))+'; Max: '+ cast(THRS_MAX as varchar(100))+' ; VOlume :'+ P.VolumeC) as STOCKName 
,
0,0,0,0,cast(p.change as decimal(5,2)),

case when eq.SectorName='Diversified' Then 'Common_HIGH'  
     when eq.SectorName='Telecommunication' Then 'Common_HIGH' 
	 else 
     SUBSTRING (eq.SectorName, 1, 13) + '_HIGH' End as  GroupName 
FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol 
inner  join Company_Ratings_Only_DIVI C on C.Symbol=p.symbol
WHERE  cast(change as decimal(5,2))  > 1 and cast(change as decimal(5,2)) < 3 and cast(p.change as  decimal(5,2))%0.25=0.0  and   cast(P.ttv  as decimal) > 500000
and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2))  and CAST(Date as Date)=CAST(ltt as Date))
GROUP BY SectorName, p.symbol,change,p.[last],c.Rating,THRS_MIN,THRS_MAX,cast(ltt as Date),[stock_name],VolumeC

UNION ALL

SELECT distinct   p.symbol, cast(ltt as Date) as Date, p.[last],
( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Rating: '+ cast(c.Rating as varchar(100))+'; Min:'+ cast(THRS_MIN as varchar(100))+'; Max: '+ cast(THRS_MAX as varchar(100))+' ; VOlume :'+ P.VolumeC ) as STOCKName 
,
0,0,0,0,cast(p.change as decimal(5,2)),

case when eq.SectorName='Diversified' Then 'Common_ROSEUP'  
     when eq.SectorName='Telecommunication' Then 'Common_ROSEUP' 
	  when eq.SectorName='Utilities' Then 'Common_ROSEUP' 
	 when eq.SectorName='Commodities' Then 'Commodities_HIGH' 
	  when eq.SectorName='Consumer Discretionary' Then 'Consumer Disc_HIGH' 
	   when eq.SectorName='Fast Moving Consumer Goods' Then 'Fast Moving C_HIGH' 
	 when eq.SectorName='Energy' Then 'Energy_HIGH' 
	  when eq.SectorName=' Information Technology' Then 'Common_ROSEUP' 
	
	 else 
SUBSTRING (SectorName, 1, 13) + '_ROSEUP'  End as GroupName 
FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol 
inner  join Company_Ratings_Only_DIVI C on C.Symbol=p.symbol
WHERE  cast(change as decimal(5,2)) > 4  and cast(p.change as  decimal(5,2))%0.25=0.0   and cast(P.ttv  as decimal) > 500000
and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol
and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2))  and CAST(Date as Date)=CAST(ltt as Date)    )
GROUP BY (SectorName), p.symbol,change,p.[last],c.Rating,THRS_MIN,THRS_MAX,cast(ltt as Date),[stock_name],VolumeC

union all

SELECT distinct   p.symbol, cast(ltt as Date) as Date, p.[last],
( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + ' Total Sell QTY :  '  + cast (totalSellQ  as  varchar(100) )+  ' ; Sellprice:  ' + cast( sPrice as   varchar(100)  ) + '; SellQTY:' + cast( sQty as   varchar(100) ) +' ; VOlume :'+ P.VolumeC   ) as STOCKName 
,
0,0,0,0,cast(p.change as decimal(5,2)),'SELL_STOCK_OWN' as GroupName 
FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
WHERE symbol in ('1.1!526608') and CAST(ltt as Date)=CAST(@Date as Date) and  totalSellQ >=  (totalBuyQt/100)* 5 
 and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2))  and PO_KEY='SELL_STOCK_OWN' )


  union all
 
SELECT distinct   p.symbol, cast(ltt as Date) as Date, p.[last],
( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + ' Total Sell QTY :  '  + cast (totalSellQ  as  varchar(100) )+  ' ; Sellprice:  ' + cast( sPrice as   varchar(100)  ) + '; SellQTY:' + cast( sQty as   varchar(100) )  +' ; VOlume :'+ P.VolumeC   ) as STOCKName 
,
0,0,0,0,cast(p.change as decimal(5,2)),'Upper_CKT' as GroupName 
FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
WHERE  CAST(ltt as Date)=CAST(@Date as Date) and  P.upperCktLm =P.last and  cast (ttv as decimal)  > 1000000
 and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2))  and PO_KEY='Upper_CKT' )
 )



--UNION ALL

--SELECT distinct   p.symbol, cast(ltt as Date) as Date, 
--( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Rating: '+ cast(c.Rating as varchar(100))+'; Min:'+ cast(THRS_MIN as varchar(100))+'; Max: '+ cast(THRS_MAX as varchar(100))+'  ' ) as STOCKName 
--,
--0,0,0,0,cast(p.change as decimal(5,2)),Eq.IgroupName + '_ROSEUP4' as GroupName 
--FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
--inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol 
--inner  join Company_Ratings_Only_DIVI C on C.Symbol=p.symbol
--WHERE eq.ISPSU=1 and cast(change as decimal(5,2)) > 4  AND cast(change as decimal(5,2)) < 5
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )
--GROUP BY eq.IgroupName, p.symbol,change,p.[last],c.Rating,THRS_MIN,THRS_MAX,cast(ltt as Date),[stock_name]

--UNION ALL

--SELECT distinct   p.symbol, cast(ltt as Date) as Date, 
--( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Rating: '+ cast(c.Rating as varchar(100))+'; Min:'+ cast(THRS_MIN as varchar(100))+'; Max: '+ cast(THRS_MAX as varchar(100))+'  ' ) as STOCKName 
--,
--0,0,0,0,cast(p.change as decimal(5,2)),Eq.IgroupName + '_ROSEUP5' as GroupName 
--FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
--inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol 
--inner  join Company_Ratings_Only_DIVI C on C.Symbol=p.symbol
--WHERE eq.ISPSU=1 and cast(change as decimal(5,2)) > 5 AND cast(change as decimal(5,2)) < 6
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )
--GROUP BY eq.IgroupName, p.symbol,change,p.[last],c.Rating,THRS_MIN,THRS_MAX,cast(ltt as Date),[stock_name]

--UNION ALL

--SELECT distinct   p.symbol, cast(ltt as Date) as Date, 
--( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Rating: '+ cast(c.Rating as varchar(100))+'; Min:'+ cast(THRS_MIN as varchar(100))+'; Max: '+ cast(THRS_MAX as varchar(100))+'  ' ) as STOCKName 
--,
--0,0,0,0,cast(p.change as decimal(5,2)),Eq.IgroupName + '_ROSEUP6' as GroupName 
--FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
--inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol 
--inner  join Company_Ratings_Only_DIVI C on C.Symbol=p.symbol
--WHERE eq.ISPSU=1 and cast(change as decimal(5,2)) > 6 AND cast(change as decimal(5,2)) < 7
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )
--GROUP BY eq.IgroupName, p.symbol,change,p.[last],c.Rating,THRS_MIN,THRS_MAX,cast(ltt as Date),[stock_name]

--UNION ALL

--SELECT distinct   p.symbol, cast(ltt as Date) as Date, 
--( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Rating: '+ cast(c.Rating as varchar(100))+'; Min:'+ cast(THRS_MIN as varchar(100))+'; Max: '+ cast(THRS_MAX as varchar(100))+'  ' ) as STOCKName 
--,
--0,0,0,0,cast(p.change as decimal(5,2)),Eq.IgroupName + '_ROSEUP7' as GroupName 
--FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
--inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol 
--inner  join Company_Ratings_Only_DIVI C on C.Symbol=p.symbol
--WHERE eq.ISPSU=1 and cast(change as decimal(5,2)) > 7 AND cast(change as decimal(5,2)) < 8
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )
--GROUP BY eq.IgroupName, p.symbol,change,p.[last],c.Rating,THRS_MIN,THRS_MAX,cast(ltt as Date),[stock_name]

--UNION ALL

--SELECT distinct   p.symbol, cast(ltt as Date) as Date, 
--( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast (p.[last] as varchar(100)) + 'Rating: '+ cast(c.Rating as varchar(100))+'; Min:'+ cast(THRS_MIN as varchar(100))+'; Max: '+ cast(THRS_MAX as varchar(100))+'  ' ) as STOCKName 
--,
--0,0,0,0,cast(p.change as decimal(5,2)),Eq.IgroupName + '_ROSEUP8' as GroupName 
--FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
--inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol 
--inner  join Company_Ratings_Only_DIVI C on C.Symbol=p.symbol
--WHERE eq.ISPSU=1 and cast(change as decimal(5,2)) > 8  
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )
--GROUP BY eq.IgroupName, p.symbol,change,p.[last],c.Rating,THRS_MIN,THRS_MAX,cast(ltt as Date),[stock_name]

--SELECT distinct   p.symbol, cast(ltt as Date) as Date, ( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast ([last] as varchar(100)) ) as STOCKName,0,0,0,0,cast(p.change as decimal(5,2)),'BANKS_LOW' as POKEY
      
--  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
--left JOIN Equitys  eq ON eq.Symbol= P.Symbol

--WHERE eq.ISPSU=1 and cast(change as decimal(5,2)) < -1 and cast(change as decimal(5,2)) < 0
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )



--union all
--SELECT distinct   p.symbol, cast(ltt as Date) as Date, ( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast ([last] as varchar(100)) ) as STOCKName ,0,0,0,0,cast(p.change as decimal(5,2)),'BANKS_HIGH'
      
--  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
--left JOIN Equitys  eq ON eq.Symbol= P.Symbol

--WHERE  eq.ISPSU=1 and change > 1 and change < 2
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists  (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )



--union all


--SELECT distinct   p.symbol, cast(ltt as Date) as Date, ( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast ([last] as varchar(100)) ) as STOCKName ,0,0,0,0,cast(p.change as decimal(5,2)),'NONPSU_ROSEUP'
      
--  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
--left JOIN Equitys  eq ON eq.Symbol= P.Symbol

--WHERE   eq.ISPSU=1 and change > 3
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )

--union all

--SELECT distinct   p.symbol, cast(ltt as Date) as Date, ( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast ([last] as varchar(100)) ) as STOCKName,0,0,0,0,cast(p.change as decimal(5,2)),'NONPSU_LOW' as POKEY
      
--  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
--left JOIN Equitys  eq ON eq.Symbol= P.Symbol

--WHERE eq.ISNonPSU=1 and cast(change as decimal(5,2)) < -1 and cast(change as decimal(5,2)) < 0
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )



--union all
--SELECT distinct   p.symbol, cast(ltt as Date) as Date, ( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast ([last] as varchar(100)) ) as STOCKName ,0,0,0,0,cast(p.change as decimal(5,2)),'NONPSU_HIGH'
      
--  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
--left JOIN Equitys  eq ON eq.Symbol= P.Symbol

--WHERE  eq.ISNonPSU=1 and change > 1 and change < 2
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists  (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )



--union all


--SELECT distinct   p.symbol, cast(ltt as Date) as Date, ( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(100)) + '; Last : ' + cast ([last] as varchar(100)) ) as STOCKName ,0,0,0,0,cast(p.change as decimal(5,2)),'BANK_ROSEUP'
      
--  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
--left JOIN Equitys  eq ON eq.Symbol= P.Symbol

--WHERE   eq.ISNonPSU=1 and change > 3
--and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2)) )



  End

GO
/****** Object:  StoredProcedure [dbo].[Get_Todays_Stock_PRED_Upper_CKT_OLD]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_Todays_Stock_PRED_Upper_CKT_OLD]
as
begin
Declare @Date DateTime  = getdate()
Insert into STOCK_NTFCTN([symbol]
      ,[Date]
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY])
SELECT distinct   p.symbol, cast(ltt as Date) as Date, ( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(50)) + '; Last : ' + cast ([last] as varchar(100)) ) as STOCKName,0,0,0,0,cast(p.change as decimal(5,2)),'BANKS_LOW' as POKEY
      
  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
left JOIN Equitys  eq ON eq.Symbol= P.Symbol

WHERE   EQ.recommondations in ('strongBuy','buy')  and (eq.IgroupName='BANKS' or eq.ISPSU=1 ) and cast(change as int) < -1 and cast(change as int) < 0
and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as int) = cast(p.change as int) )



union all
SELECT distinct   p.symbol, cast(ltt as Date) as Date, ( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(50)) + '; Last : ' + cast ([last] as varchar(100)) ) as STOCKName ,0,0,0,0,cast(p.change as Int),'BANKS_HIGH'
      
  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
left JOIN Equitys  eq ON eq.Symbol= P.Symbol

WHERE   EQ.recommondations in ('strongBuy','buy')   and (eq.IgroupName='BANKS' or eq.ISPSU=1 ) and change > 1 and change < 2
and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists  (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as int) = cast(p.change as int) )



union all


SELECT distinct   p.symbol, cast(ltt as Date) as Date, ( [stock_name] + '; change' + ' : ' +  cast( change  as varchar(50)) + '; Last : ' + cast ([last] as varchar(100)) ) as STOCKName ,0,0,0,0,cast(p.change as Int),'BANK_ROSEUP'
      
  FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
 
left JOIN Equitys  eq ON eq.Symbol= P.Symbol

WHERE   EQ.recommondations in ('strongBuy','buy')  and  (eq.IgroupName='BANKS' or eq.ISPSU=1 ) and change > 3
and  CAST(ltt as Date)=CAST(@Date as Date)   and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as int) = cast(p.change as int) )


  End

GO
/****** Object:  StoredProcedure [dbo].[GetBuysStocks]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  StoredProcedure [dbo].[GetBuyStockTriggers]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure  [dbo].[GetBuyStockTriggers]
as begin

SELECT eq.SecurityId as StockCode,  eq.Symbol as symbol,eq.SecurityName as StockName,
isnull(T.Price,0) as PointerPrice,cast(((isnull(T.Price,0)) + (0.002 * isnull(T .Price,0))) as decimal(10,2)) as buyATPrice, cast(((isnull(T.Price,0)) + ((0.1)*isnull(T.Price,0))) as decimal(10,2)) as sellAtPrice,

(CASE WHEN T.change='' THEN -9999
     WHEN T.change is null then -9999
	 else cast(ISNULL(T.Change,-9999) as decimal(10,2)) end) as buyATChange
--cast(ISNULL(T.Change,-9999) as decimal(10,2)) as  buyATChange
  FROM Equitys eq
  left join [STOCK].[dbo].[StockPriceConfig] T  on eq.Symbol=T.Symbol
  where eq.IsEnabledForAutoTrade=1  order by eq.SecurityName
  End


GO
/****** Object:  StoredProcedure [dbo].[GetDummyTestBySymbol]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


  CREATE Procedure [dbo].[GetDummyTestBySymbol](@Code varchar(20))
  as
  begin

  Select * from Ticker_Stocks_01_02_24 where  ltt < ='2024-02-01 09:20:06.000' 
  --where symbol='1.1!543720' and  [open] > 0 order by ltt desc
  end
GO
/****** Object:  StoredProcedure [dbo].[GetPivotData]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--GetPivotData '2023-11-01','ttv'
--GetPivotData '2023-11-14','ttv' ,'','','','>','10','100'
--GetPivotData '2023-08-15','change'



CREATE Proc [dbo].[GetPivotData](@Date Date, @column varchar(20), @groupName varchar(max)='', @subGroup varchar(max)='',



@CKTName varchar(25)=null,
@ConditionOperator varchar(10)=null,
@dynamicminValue varchar(100) =null,
@dynamicmaxValue  varchar(100)=null,
@isWatchList bit= 0

)
as
begin 

Declare @previousDate DateTime = @Date


declare @DynamicWhere  varchar(max)='';

if(@ConditionOperator is not null and @ConditionOperator<>'' and @column <> 'ttv'  )
begin
print @dynamicminValue
set @DynamicWhere=case when @ConditionOperator = '>' Then  '['+@column+'] '+@ConditionOperator+' cast ('+ @dynamicminValue+' as bigint) '  
                       when @ConditionOperator = '<' Then  '['+@column+']'+@ConditionOperator+' cast ('+ @dynamicminValue+' as bigint) '  
					   when @ConditionOperator  like '%range%' Then  '['+@column+'] between '+' cast ('+ @dynamicminValue+' as bigint) and  cast('  + @dynamicmaxValue + ' as bigint)'
					   End


 print @DynamicWhere
End
else 
begin
set @DynamicWhere='1=1'
end 

if not exists (select * from Ticker_Stocks_Histry where cast(ltt as Date)=CAST(@Date as Date))
begin
set @Date= (select MAX(CAST(ltt as Date)) from Ticker_Stocks_Histry) 
end 

if not exists (select * from Ticker_Stocks_Histry where cast(ltt as Date)=CAST(@Date as Date))
begin
set @Date= (select MAX(CAST(ltt as Date)) from Ticker_Stocks_Histry) 
end 

truncate table TempPivot;
drop table if exists TempPivotResults;

--Declare @previousDate Date = (SELECT DATEADD(
--    day,
--    IIF(DATENAME(weekday,@Date) = 'Monday', -3, -1),
--    CAST(@Date AS DATE)
--));
drop table if exists #tempdata;
DECLARE @sql_select  nvarchar(max);

    SELECT  e.symbol,STOCK_name + ' ' +e.symbol as STOCK_name  ,ltt,[close],
	
	case when @column ='high-low' then [high]-[low] end as DayChange,
	[open],[last],ttv,change,low,high,
        RnAsc = ROW_NUMBER() OVER(PARTITION BY E.symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc = ROW_NUMBER() OVER(PARTITION BY E.symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    into #tempCTE  FROM Ticker_Stocks_Histry   H
	left join Equitys E on h.symbol=E.Symbol
	where  CAST(ltt as Date) >=cast(@previousDate as Date) --and cast (ttv as decimal) > 5000000 
	and ( @groupName is null or  @groupName=''  or E.IgroupName    in  (SELECT id FROM [dbo].[ufnSplit](@groupName)))
	and ( @subGroup is null or   @subGroup='' or  E.ISubgroupName    in  (SELECT id FROM [dbo].[ufnSplit](@subGroup)))
	and (@CKTName ='' or  @CKTName is null or 
	    [last]= CASE
         WHEN @CKTName ='upperCktLm' THEN upperCktLm
         WHEN @CKTName ='lowerCktLm' THEN lowerCktLm
		 end
		 
		 or 
		  [open]= CASE
         WHEN @CKTName ='upperCktLm' THEN upperCktLm
         WHEN @CKTName ='lowerCktLm' THEN lowerCktLm
		 end
		 or 
		  [close]= CASE
         WHEN @CKTName ='upperCktLm' THEN upperCktLm
         WHEN @CKTName ='lowerCktLm' THEN lowerCktLm
		 end
		 )

		
		-- and [open]=upperCktLm
	--and (@lastcolumnwhere ='' or  @lastcolumnwhere is null or last=@lastcolumnwhere)


	if(@column='high-low')
	 set @column='DayChange'
	--#select distinct symbol,STOCK_name, 0 as Value,cast(ltt as Date) as Date  into TempPivot   from #tempCTE where RnDesc=1 order by symbol
--SET @sql_select='Insert into  TempPivot select distinct symbol,STOCK_name,['+@column+'] as value,cast(ltt as Date) as Date   from #tempCTE where RnDesc=1   order by symbol'

SET @sql_select='Insert into  TempPivot select distinct symbol,STOCK_name,['+@column+'] as value,cast(ltt as Date) as Date   from #tempCTE where RnDesc=1 and '+@DynamicWhere+'   order by symbol'
print @sql_select
EXECUTE sp_executesql @sql_select
--Select * from TempPivot


DECLARE @sql  nvarchar(max);
DECLARE @columnname nvarchar(max);


--select @columnname = STUFF((SELECT ',' + QUOTENAME(convert(char(10), date, 120)) 
select @columnname = STUFF((SELECT ',' + QUOTENAME(convert(char(10), date, 110)) 
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


Select Eq.MSN_SECID as M, [open],[last],[last]-[open] as Diff , P.* from TempPivotResults P
left join Live_Stocks E  on E.Symbol =P.Symbol
inner  JOIN Equitys  eq ON eq.Symbol= P.Symbol --and ISPSU=1
left join (SELECT  [Symbol],CAN_BUY,MSNRating  FROM Company_Ratings_Only_DIVI )A  on A.Symbol= P.Symbol  

where (ISPSU=1 or  CAN_BUY=1 or MSNRating in ('strongBuy','Buy') )and 
@isWatchList=0 or eq.MSN_SECID in (select symbol from WatchList where symbol=eq.MSN_SECID and cast(date  as date)> =cast(GETDATE()-1 as date))
--where eq.Symbol in (select Symbol from MSNDownStocks where estimate_recommendation in ('buy','strongbuy') and cast(Created_On as date)=cast(GETDATE() as Date))
--where eq.Symbol in (select Symbol from MSNupStocks where estimate_recommendation in ('buy','strongbuy'))
--where eq.symbol in ('1.1!530355','1.1!532960','1.1!532430','1.1!504286',
--'1.1!543284','1.1!532708','1.1!507878','1.1!531205',
--'1.1!532444',' 1.1!511700','1.1!511571','1.1!526335','1.1!530525','1.1!533287','1.1!526861',
--'1.1!522257','1.1!526662','1.1!536659','1.1!517556','1.1!500540','1.1!521149','1.1!532933','1.1!511116','1.1!506605','1.1!543540','1.1!531859'
--,'1.1!521246','1.1!519528','1.1!535136','1.1!531092','1.1!511535','1.1!511535','1.1!530557','1.1!508922','1.1!533152','1.1!526869','1.1!532925',
--'1.1!513250','1.1!532799','1.1!532624','1.1!524314','1.1!523836','1.1!517288','1.1!506858','1.1!501848',' 1.1!538568','1.1!514010','1.1!533217','1.1!540361')
--where E.symbol ='1.1!516092'
--WHERE EQ.recommondations in ('strongBuy','buy') 

--Select P.* from TempPivotResults P
--left join Live_Stocks E  on E.Symbol =P.Symbol
End
GO
/****** Object:  StoredProcedure [dbo].[GetPivotData_Company]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--GetPivotData '2023-09-01','ttv'
--GetPivotData '2023-09-08','last' ,'upperCktLm'
--GetPivotData '2023-08-15','change'


--[GetPivotData_Company] 'test'

CREATE Proc [dbo].[GetPivotData_Company]( @column varchar(20))
as
begin 


drop table if exists  TempPivotCompany;
drop table if exists TempPivotResultsCompnay;

--Declare @previousDate Date = (SELECT DATEADD(
--    day,
--    IIF(DATENAME(weekday,@Date) = 'Monday', -3, -1),
--    CAST(@Date AS DATE)
--));
drop table if exists #tempdata;
DECLARE @sql_select  nvarchar(max);

    SELECT MSN.[symbol]
      ,MSN.[SecurityName]
      ,[beta]
      ,[recommendation]
      ,[dateLastUpdated]
      ,[price]
      ,[priceChange]
      ,[priceDayHigh]
      ,[priceDayLow]
      ,[yieldPercent]
      ,[return1Week]
      ,[return1Month]
      ,[return3Month]
      ,[return6Month]
      ,[peRatio]
      ,[numberOfAnalysts]
      ,[recommendationRate]
      ,[meanPriceTarget]
      ,[highPriceTarget]
      ,[lowPriceTarget]
      ,[medianPriceTarget]
      ,[medianEpsTarget]
      ,[strongBuy]
      ,[buy]
      ,[underperform]
      ,[hold]
      ,[sell]
      ,[eps]
      ,[forwardPriceToEPS]
      ,[payoutRatio]
      ,[priceToBookRatio]
      ,[profitability]
      ,[stockGrowth]
      ,[latestNetProfitMargin]
      ,[latestIncome]
      ,[latestRevenuePerShare]
      ,[debtToEquityRatio]
      ,[exDividendAmount]
      ,[sharesOutstanding]
      ,[enterpriseValue]
      ,[dividendYield]
      ,[bookValueShareRatio]
      ,[trailingAnnualDividendYield]
      ,[website]
    into #tempCTE  FROM MSNSTCOKS  MSN
	left join Equitys E on MSN.symbol=E.Symbol
	
		-- and [open]=upperCktLm
	--and (@lastcolumnwhere ='' or  @lastcolumnwhere is null or last=@lastcolumnwhere)

--SET @sql_select='Insert into  TempPivotCompany select *  from #tempCTE where RnDesc=1  order by symbol'

SET @sql_select='select * into TempPivotCompany from #tempCTE '


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
SELECT * into TempPivotResultsCompnay 
FROM  TempPivotCompany t
PIVOT
(SUM([value])
FOR [Date] IN('+@columnname+')) AS p '

EXECUTE sp_executesql @sql


Select * from TempPivotResultsCompnay
--where eq.symbol in ('1.1!530355','1.1!532960','1.1!532430','1.1!504286',
--'1.1!543284','1.1!532708','1.1!507878','1.1!531205',
--'1.1!532444',' 1.1!511700','1.1!511571','1.1!526335','1.1!530525','1.1!533287','1.1!526861',
--'1.1!522257','1.1!526662','1.1!536659','1.1!517556','1.1!500540','1.1!521149','1.1!532933','1.1!511116','1.1!506605','1.1!543540','1.1!531859'
--,'1.1!521246','1.1!519528','1.1!535136','1.1!531092','1.1!511535','1.1!511535','1.1!530557','1.1!508922','1.1!533152','1.1!526869','1.1!532925',
--'1.1!513250','1.1!532799','1.1!532624','1.1!524314','1.1!523836','1.1!517288','1.1!506858','1.1!501848',' 1.1!538568','1.1!514010','1.1!533217','1.1!540361')
--where E.symbol ='1.1!516092'
--WHERE EQ.recommondations in ('strongBuy','buy') 

--Select P.* from TempPivotResults P
--left join Live_Stocks E  on E.Symbol =P.Symbol
End
GO
/****** Object:  StoredProcedure [dbo].[getStockDetails]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--getStockDetails 0
CREATE procedure [dbo].[getStockDetails](@symbol varchar(1000))
as
begin
if(@symbol <> '0')
begin
select * from (
select N.Subject, '<a target=''_blank''  href='+N.Attachement+ '>'+N.Attachement+'</a>' as files,N.Details,N.BroadcastDateTime,E.Symbol,'NSE' as EXC from Equitys  E
inner join [STOCK].[dbo].NSEAnnouncement N on N.symbol=E.SecurityId
where E.MSN_SECID =@symbol 
union all  
select N.NEWSSUB as Subject, '<a target=''_blank'' href='+N.ATTACHMENTNAME+ '>'+N.ATTACHMENTNAME+'</a>' as files ,N.HEADLINE as Details,cast (N.CreatedOn as datetime) as BroadcastDateTime,Symbol,'BSE' as EXC from Equitys  E
inner join [STOCK].[dbo].BSE_NEWS N on '1.1!' + cast (N.SCRIP_CD as varchar(15))=E.Symbol
where E.MSN_SECID =@symbol  )A
order by cast(BroadcastDateTime as datetime) desc
End

else 
select * from (
select N.Subject, '<a target=''_blank''  href='+N.Attachement+ '>'+N.Attachement+'</a>' as files,N.Details,N.BroadcastDateTime,E.Symbol,'NSE' as EXC from Equitys  E
inner join [STOCK].[dbo].NSEAnnouncement N on N.symbol=E.SecurityId
where cast(BroadcastDateTime as date) > GETDATE()-7
union all  
select N.NEWSSUB as Subject, '<a target=''_blank'' href='+N.ATTACHMENTNAME+ '>'+N.ATTACHMENTNAME+'</a>' as files ,N.HEADLINE as Details,cast (N.CreatedOn as datetime) as BroadcastDateTime,Symbol,'BSE' as EXC from Equitys  E
inner join [STOCK].[dbo].BSE_NEWS N on '1.1!' + cast (N.SCRIP_CD as varchar(15))=E.Symbol
where cast(CreatedOn as date) > GETDATE()-7  )A
order by cast(BroadcastDateTime as datetime) desc

end
  
GO
/****** Object:  StoredProcedure [dbo].[GetTopStockforBuyAutomation]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[GetTopStockforBuyAutomation] '0'
	CREATE Procedure [dbo].[GetTopStockforBuyAutomation](@isorderbysize bit =1)
	as
	begin
	exec CLEAR_beRISH_bULLISH;
	Declare @Date DateTime =(select max(ltt) from [Ticker_Stocks_Histry_Extended_Ticks] with (nolock) )
	if(@isorderbysize=1)
	begin 
	   
	
	

	 WITH Cte AS(
	select T.Symbol,  
	isnull(T.BulishCount,0) as BulishCount,
	isnull(T.BearishCount,0) as BearishCount,
	isnull(T.Stock_Name,'') as Stock_Name,
	isnull(T.StockCode,'') as StockCode,
	T.ltt,
	isnull(JSON_VALUE(Data, '$.candleResult.Price'),0) as  candleResult_Price,
	isnull(JSON_VALUE(Data, '$.candleResult.Match'),0) as  candleResult_Match ,
	cast(isnull(JSON_VALUE(Data, '$.candleResult.Candle.Size'),0) as decimal(10,3)) as  candleResult_Size,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.Body'),0) as  candleResult_Body,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.UpperWick'),0) as  candleResult_UpperWick,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.LowerWick'),0) as  candleResult_LowerWick,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.BodyPct'),0) as  candleResult_BodyPct,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.UpperWickPct'),0) as  candleResult_UpperWickPct,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.LowerWickPct'),0) as  candleResult_LowerWickPct,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.IsBullish'),0) as  candleResult_IsBullish,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.IsBearish'),0) as  candleResult_IsBearish,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.Volume'),0) as  candleResult_Volume,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.macdresult.Macd') , 1, 10) as Decimal(10,3)),0)   as macdresult_Macd,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.macdresult.Signal') , 1, 10) as Decimal(10,3)),0)   as macdresult_Signal,

	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.macdresult.FastEma') , 1, 10) as Decimal(10,3)),0)   as macdresult_FastEma,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.macdresult.SlowEma') , 1, 10) as Decimal(10,3)),0)   as macdresult_SlowEma,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.rsiResults.Rsi') , 1, 10) as Decimal(10,3)),0)   as macdresult_Rsi,

	
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.Volatilityresults.Sar') , 1, 4) as Decimal(10,3)),0)   as Volatilityresults_Sar,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.Volatilityresults.UpperBand') , 1, 10) as Decimal(10,3)),0)   as Volatilityresults_UpperBand,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.Volatilityresults.LowerBand') , 1, 10) as Decimal(10,3)),0)  as Volatilityresults_LowerBand,
	(JSON_VALUE(Data, '$.Volatilityresults.Candle.IsStop')) as  Volatilityresults_IsStop, 
	RnAsc_C = ROW_NUMBER() OVER(PARTITION BY t.symbol, CAST(t.ltt as Date) ORDER BY t.ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  t.symbol,CAST(t.ltt as Date) ORDER BY t.ltt DESC)

	
	from [Ticker_Stocks_Histry_Extended_Ticks] T with (nolock) 
	where CAST( ltt as Date )=  cast(getdate() as Date)) -- CAST(@Date as Date))

	
		Select top 25 *   from  Cte where RnDesc_C = 1 order by   candleResult_Size   desc

	End
	--where symbol='1.1!532814' order by Ltt desc
	else 
	begin 
	     
	
	--exec CLEAR_beRISH_bULLISH;

	 WITH Cte AS(
	select T.Symbol,  
	isnull(T.BulishCount,0) as BulishCount,
	isnull(T.BearishCount,0) as BearishCount,
	isnull(T.Stock_Name,'') as Stock_Name,
	isnull(T.StockCode,'') as StockCode,
	T.ltt,
	isnull(JSON_VALUE(Data, '$.candleResult.Price'),0) as  candleResult_Price,
	isnull(JSON_VALUE(Data, '$.candleResult.Match'),0) as  candleResult_Match ,
	cast(isnull(JSON_VALUE(Data, '$.candleResult.Candle.Size'),0) as decimal(10,3)) as  candleResult_Size,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.Body'),0) as  candleResult_Body,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.UpperWick'),0) as  candleResult_UpperWick,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.LowerWick'),0) as  candleResult_LowerWick,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.BodyPct'),0) as  candleResult_BodyPct,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.UpperWickPct'),0) as  candleResult_UpperWickPct,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.LowerWickPct'),0) as  candleResult_LowerWickPct,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.IsBullish'),0) as  candleResult_IsBullish,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.IsBearish'),0) as  candleResult_IsBearish,
	isnull(JSON_VALUE(Data, '$.candleResult.Candle.Volume'),0) as  candleResult_Volume,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.macdresult.Macd') , 1, 10) as Decimal(10,3)),0)   as macdresult_Macd,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.macdresult.Signal') , 1, 10) as Decimal(10,3)),0)   as macdresult_Signal,

	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.macdresult.FastEma') , 1, 10) as Decimal(10,3)),0)   as macdresult_FastEma,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.macdresult.SlowEma') , 1, 10) as Decimal(10,3)),0)   as macdresult_SlowEma,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.rsiResults.Rsi') , 1, 10) as Decimal(10,3)),0)   as macdresult_Rsi,

	
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.Volatilityresults.Sar') , 1, 4) as Decimal(10,3)),0)   as Volatilityresults_Sar,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.Volatilityresults.UpperBand') , 1, 10) as Decimal(10,3)),0)   as Volatilityresults_UpperBand,
	isnull(cast(SUBSTRING(JSON_VALUE(Data, '$.Volatilityresults.LowerBand') , 1, 10) as Decimal(10,3)),0)  as Volatilityresults_LowerBand,
	(JSON_VALUE(Data, '$.Volatilityresults.Candle.IsStop')) as  Volatilityresults_IsStop, 
	RnAsc_C = ROW_NUMBER() OVER(PARTITION BY t.symbol, CAST(t.ltt as Date) ORDER BY t.ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  t.symbol,CAST(t.ltt as Date) ORDER BY t.ltt DESC)

	
	from [Ticker_Stocks_Histry_Extended_Ticks] T with (nolock) 
	where CAST( ltt as Date )=  cast(getdate() as Date)) -- CAST(@Date as Date))

	
		Select top 25 *   from  Cte where RnDesc_C = 1 order by   BulishCount   desc

	End
	--where symb
	End
GO
/****** Object:  StoredProcedure [dbo].[Import_Histry]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Import_Histry]
as
begin

Declare @Date DateTime =DATEADD(HOUR, -2, GETDATE());

--Delete from [Ticker_Stocks_Histry] where  CAST(ltt as Date )=  CAST(GETDATE() as Date)

 -- =(select isnull(MAX(cast(ltt as Date)),GETDATE()) from Ticker_Stocks_Histry);

WITH Cte AS(
SELECT *,
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM [Ticker_Stocks] with (nolock)   where cast(ltt as datetime  ) >=  @Date  and [open] >0 )

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

WITH Cte AS(
SELECT *,
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM [Ticker_Stocks_Histry] with (nolock)    where CAST(ltt as Date )=  CAST(GETDATE() as Date) )

	Delete  from  Cte where RnDesc_C > 1 

--if((select COUNT(*) from Ticker_Stocks) > 200000000)
--begin
--truncate table Ticker_Stocks
--end
End
GO
/****** Object:  StoredProcedure [dbo].[Import_Last_Stock]    Script Date: 05-02-2024 21:23:23 ******/
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
    IIF(DATENAME(weekday,getdate()) = 'Monday', -3, -2),
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
           ,[stock_name],VolumeC)
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
           ,[stock_name],VolumeC
		  
FROM Cte
WHERE
    RnDesc_C= 1
ORDER BY symbol, ltt;

End
GO
/****** Object:  StoredProcedure [dbo].[Insert_BSE_NEWS]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[Insert_BSE_NEWS]( 
 @NEWSID nvarchar(max)
,@SCRIP_CD int
,@XML_NAME nvarchar(max)
,@NEWSSUB nvarchar(max)
,@DT_TM nvarchar(max)
,@NEWS_DT  nvarchar(max)
,@CRITICALNEWS  bit
,@ANNOUNCEMENT_TYPE nvarchar(max)
,@QUARTER_ID  bit
,@FILESTATUS  varchar(50)
,@ATTACHMENTNAME  nvarchar(max)
,@MORE  varchar(600)
,@HEADLINE  nvarchar(max)
,@CATEGORYNAME  nvarchar(max)
,@OLD  bit
,@RN  bit
,@PDFFLAG bit
,@NSURL nvarchar(max)
,@SLONGNAME nvarchar(max)
,@AGENDA_ID  int
,@TotalPageCnt  int
,@News_submission_dt nvarchar(max)
,@DissemDT nvarchar(max)
,@TimeDiff varchar(50)
,@Fld_Attachsize int
,@SUBCATNAME nvarchar(max)
,@AUDIOFILE nvarchar(max)
,@BASEURL nvarchar(max)
,@CreatedOn datetime)
as
begin
if(@SUBCATNAME<>'Reg. 39 (3) - Details of Loss of Certificate / Duplicate Certificate')
begin
if not exists (select * from [dbo].[BSE_NEWS] where  NEWSSUB =@NEWSSUB and NEWS_DT=@NEWS_DT and SLONGNAME=@SLONGNAME)
begin
INSERT INTO [dbo].[BSE_NEWS]
([NEWSID]
,[SCRIP_CD]
,[XML_NAME]
,[NEWSSUB]
,[DT_TM]
,[NEWS_DT]
,[CRITICALNEWS]
,[ANNOUNCEMENT_TYPE]
,[QUARTER_ID]
,[FILESTATUS]
,[ATTACHMENTNAME]
,[MORE]
,[HEADLINE]
,[CATEGORYNAME]
,[OLD]
,[RN]
,[PDFFLAG]
,[NSURL]
,[SLONGNAME]
,[AGENDA_ID]
,[TotalPageCnt]
,[News_submission_dt]
,[DissemDT]
,[TimeDiff]
,[Fld_Attachsize]
,[SUBCATNAME]
,[AUDIO_VIDEO_FILE]
,[BASEURL]
,[CreatedOn])
VALUES
(
@NEWSID
,@SCRIP_CD
,@XML_NAME
,@NEWSSUB
,@DT_TM
,@NEWS_DT
,@CRITICALNEWS
,@ANNOUNCEMENT_TYPE
,@QUARTER_ID
,@FILESTATUS
,@ATTACHMENTNAME
,@MORE
,@HEADLINE
,@CATEGORYNAME
,@OLD
,@RN
,@PDFFLAG
,@NSURL
,@SLONGNAME
,@AGENDA_ID
,@TotalPageCnt
,@News_submission_dt
,@DissemDT
,@TimeDiff
,@Fld_Attachsize
,@SUBCATNAME
,@AUDIOFILE
,@BASEURL
,@CreatedOn
)


 Insert into STOCK_NTFCTN(
[symbol]
      ,[Date]
	  ,Last
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY],[Priority])
select @SCRIP_CD, cast(@NEWS_DT as datetime) as Date, 0, '<br>'+
@SLONGNAME + ' ' +  @NEWSSUB + '   ' +  ' ' +  @ATTACHMENTNAME + '' + ' ' ,
0,0,0,0,0,

case when @SUBCATNAME='Board Meeting' then 'BSE_Board Meeting' 
     when @SUBCATNAME='AGM/EGM' then 'BSE_Board Meeting' else 'BSE_NEWS' END
as GroupName ,case when @NEWSSUB like '%Order%' Then 2 
when  @SUBCATNAME='Board Meeting' then 0 else 1 End


Insert into STOCK_NTFCTN(
[symbol]
      ,[Date]
	  ,Last
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY],[Priority])
select @SCRIP_CD, cast(@NEWS_DT as datetime) as Date, 0, '<br>'+
@SLONGNAME + ' ' +  @NEWSSUB + '   ' +  ' ' +  @ATTACHMENTNAME + '' + ' ' ,
0,0,0,0,0,

case when @SUBCATNAME='Board Meeting' then 'BSE_Board Meeting_IP' 
     when @SUBCATNAME='AGM/EGM' then 'BSE_Board Meeting_IP' else 'BSE_NEW_IP' END
as GroupName ,case when @NEWSSUB like '%Order%' Then 2 
when  @SUBCATNAME='Board Meeting' then 0 else 1 End
where @NEWSSUB like '%Order%' and @SUBCATNAME <>'Board Meeting' and @SUBCATNAME <>'AGM/EGM'

End
end
END
GO
/****** Object:  StoredProcedure [dbo].[INSERT_MMSN_Companies]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  Procedure [dbo].[INSERT_MMSN_Companies]
as
begin
delete  from MMSN_Companies where cast(Created_On as date)= cast (GETDATE() as Date)
INSERT INTO [dbo].[MMSN_Companies]
           (
		   [MSN_SECID]
           ,[estimate_numberOfAnalysts]
           ,[estimate_recommendationRate]
           ,[estimate_recommendation]
           ,[estimate_currency]
           ,[estimate_numberOfPriceTargets]
           ,[estimate_meanPriceTarget]
           ,[estimate_highPriceTarget]
           ,[estimate_lowPriceTarget]
           ,[estimate_medianPriceTarget]
           ,[estimate_medianEpsTarget]
           ,[estimate_analystRecommendation]
           ,[estimate_dateLastUpdated]
           ,[beta]
           ,[CompanyMetrics_pE5YearHighRatio]
           ,[CompanyMetrics_pE5YearLowRatio]
           ,[CompanyMetrics_revenueYTDYTD]
           ,[CompanyMetrics_revenueQQLastYearGrowthRate]
           ,[CompanyMetrics_netIncomeYTDYTDGrowthRate]
           ,[CompanyMetrics_netIncomeQQLastYearGrowthRate]
           ,[CompanyMetrics_revenue5YearAverageGrowthRate]
           ,[CompanyMetrics_interestCoverage]
           ,[CompanyMetrics_priceCashFlowRatio]
           ,[CompanyMetrics_revenue3YearAverage]
           ,[CompanyMetrics_trailingAnnualDividendYield]
           ,[CompanyMetrics_priceBookRatio]
           ,[CompanyMetrics_priceSalesRatio]
           ,[CompanyMetrics_bookValueShareRatio]
           ,[CompanyMetrics_ratingCashFlow]
           ,[CompanyMetrics_payoutRatio]
           ,[CompanyMetrics_quickRatio]
           ,[CompanyMetrics_current]
           ,[CompanyMetrics_debtEquityRatio]
           ,[CompanyMetrics_grossMargin]
           ,[CompanyMetrics_preTaxMargin]
           ,[CompanyMetrics_netProfitMargin]
           ,[CompanyMetrics_averageGrossMargin5Year]
           ,[CompanyMetrics_averagePreTaxMargin5Year]
           ,[CompanyMetrics_averageNetProfitMargin5Year]
           ,[CompanyMetrics_operatingMargin]
           ,[CompanyMetrics_netMarginPercent]
           ,[CompanyMetrics_returnOnAssetCurrent]
           ,[CompanyMetrics_returnOnAsset5YearAverage]
           ,[CompanyMetrics_returnOnCapitalCurrent]
           ,[CompanyMetrics_assetTurnover]
           ,[CompanyMetrics_inventoryTurnover]
           ,[CompanyMetrics_receivableTurnover]
           ,[shareStatistics_lastSplitFactor]
           ,[shareStatistics_lastSplitDate]
           ,[shareStatistics_dividendYield]
           ,[shareStatistics_exDividendAmount]
           ,[shareStatistics_sharesOutstanding]
           ,[shareStatistics_enterpriseValue]
           ,[industryMetrics_assetTurnover]
           ,[industryMetrics_averageGrossMargin5Year]
           ,[industryMetrics_averageNetProfitMargin5Year]
           ,[industryMetrics_averagePreTaxMargin5Year]
           ,[industryMetrics_bookValueShareRatio]
           ,[industryMetrics_currentRatio]
           ,[industryMetrics_debtEquityRatio]
           ,[industryMetrics_dividendYield]
           ,[industryMetrics_dividendYield5YearAverage]
           ,[industryMetrics_grossMargin]
           ,[industryMetrics_incomeEmployee]
           ,[industryMetrics_interestCoverage]
           ,[industryMetrics_inventoryTurnover]
           ,[industryMetrics_netIncomeQQLastYearGrowthRate]
           ,[industryMetrics_netIncomeYTDYTDGrowthRate]
           ,[industryMetrics_netProfitMargin]
           ,[industryMetrics_pEGrowthRatio]
           ,[industryMetrics_preTaxMargin]
           ,[industryMetrics_priceBookRatio]
           ,[industryMetrics_priceCashFlowRatio]
           ,[industryMetrics_priceSalesRatio]
           ,[industryMetrics_quickRatio]
           ,[industryMetrics_returnOnAsset5YearAverage]
           ,[industryMetrics_returnOnAssetCurrent]
           ,[industryMetrics_returnOnCapital5YearAverage]
           ,[industryMetrics_returnOnCapitalCurrent]
           ,[industryMetrics_returnOnEquity5YearAverage]
           ,[industryMetrics_returnOnEquityCurrent]
           ,[industryMetrics_revenueEmployee]
           ,[industryMetrics_revenueQQLastYearGrowthRate]
           ,[industryMetrics_revenueYTDYTD]
           ,[industryMetrics_receivableTurnover]
           ,[industryMetrics_leverageRatio]
           ,[keyMetrics_debtToEquityRatio]
           ,[keyMetrics_eps]
           ,[keyMetrics_forwardPriceToEPS]
           ,[keyMetrics_payoutRatio]
           ,[keyMetrics_priceToBookRatio]
           ,[keyMetrics_profitability]
           ,[keyMetrics_stockGrowth]
           ,[keyMetrics_latestRevenue]
           ,[keyMetrics_latestIncome]
           ,[keyMetrics_latestNetProfitMargin]
           ,[keyMetrics_latestRevenuePerShare]
           ,[quote_price]
           ,[quote_priceChange]
           ,[quote_priceDayHigh]
           ,[quote_priceDayLow]
           ,[quote_timeLastTraded]
           ,[quote_priceDayOpen]
           ,[quote_pricePreviousClose]
           ,[quote_datePreviousClose]
           ,[quote_priceAsk]
           ,[quote_askSize]
           ,[quote_priceBid]
           ,[quote_bidSize]
           ,[quote_accumulatedVolume]
           ,[quote_averageVolume]
           ,[quote_peRatio]
           ,[quote_priceChangePercent]
           ,[quote_price52wHigh]
           ,[quote_price52wLow]
           ,[quote_priceClose]
           ,[quote_yieldPercent]
           ,[quote_priceChange1Week]
           ,[quote_priceChange1Month]
           ,[quote_priceChange3Month]
           ,[quote_priceChange6Month]
           ,[quote_priceChangeYTD]
           ,[quote_priceChange1Year]
           ,[quote_return1Week]
           ,[quote_return1Month]
           ,[quote_return3Month]
           ,[quote_return6Month]
           ,[quote_returnYTD]
           ,[quote_return1Year]
           ,[quote_sourceExchangeCode]
           ,[quote_sourceExchangeName]
           ,[quote_marketCap]
           ,[quote_marketCapCurrency]
           ,[quote_exchangeId]
           ,[quote_exchangeCode]
           ,[quote_exchangeName]
           ,[quote_offeringStatus]
           ,[quote_displayName]
           ,[quote_shortName]
           ,[quote_securityType]
           ,[quote_instrumentId]
           ,[quote_symbol]
           ,[quote_country]
           ,[quote_market]
           ,[quote_timeLastUpdated]
           ,[quote_currency]
           ,[quote__p]
           ,[quote_id]
           ,[quote__t]
           ,[address_street]
           ,[address_city]
           ,[address_state]
           ,[address_zip]
           ,[address_country]
           ,[address_countryCode]
           ,[address_phone]
           ,[address_fax],Symbol,website,Created_On)
		   select
JSON_VALUE(JsonData, '$."instrumentId"')  as MSN_SECID,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.numberOfAnalysts')  as estimate_numberOfAnalysts,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.recommendationRate')  as estimate_recommendationRate,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.recommendation')  as estimate_recommendation,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.currency')  as estimate_currency,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.numberOfPriceTargets')  as estimate_numberOfPriceTargets,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.meanPriceTarget')  as estimate_meanPriceTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.highPriceTarget')  as estimate_highPriceTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.lowPriceTarget')  as estimate_lowPriceTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.medianPriceTarget')  as estimate_medianPriceTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.medianEpsTarget')  as estimate_medianEpsTarget,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.analystRecommendation')  as estimate_analystRecommendation,
JSON_VALUE(JsonData, '$.equity.analysis.estimate.dateLastUpdated')  as estimate_dateLastUpdated,

JSON_VALUE(JsonData, '$.equity.beta')  as beta,


JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.pE5YearHighRatio')  as CompanyMetrics_pE5YearHighRatio,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.pE5YearLowRatio')  as CompanyMetrics_pE5YearLowRatio,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.revenueYTDYTD')  as CompanyMetrics_revenueYTDYTD,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.revenueQQLastYearGrowthRate')  as CompanyMetrics_revenueQQLastYearGrowthRate,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.netIncomeYTDYTDGrowthRate')  as CompanyMetrics_netIncomeYTDYTDGrowthRate,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.netIncomeQQLastYearGrowthRate')  as CompanyMetrics_netIncomeQQLastYearGrowthRate,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.revenue5YearAverageGrowthRate')  as CompanyMetrics_revenue5YearAverageGrowthRate,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.interestCoverage')  as CompanyMetrics_interestCoverage,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.priceCashFlowRatio')  as CompanyMetrics_priceCashFlowRatio,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.revenue3YearAverage')  as CompanyMetrics_revenue3YearAverage,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.trailingAnnualDividendYield')  as CompanyMetrics_trailingAnnualDividendYield,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.priceBookRatio')  as CompanyMetrics_priceBookRatio,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.priceSalesRatio')  as CompanyMetrics_priceSalesRatio,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.bookValueShareRatio')  as CompanyMetrics_bookValueShareRatio,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.ratingCashFlow')  as CompanyMetrics_ratingCashFlow,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.payoutRatio')  as CompanyMetrics_payoutRatio,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.quickRatio')  as CompanyMetrics_quickRatio,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.current')  as CompanyMetrics_current,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.debtEquityRatio')  as CompanyMetrics_debtEquityRatio,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.grossMargin')  as CompanyMetrics_grossMargin,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.preTaxMargin')  as CompanyMetrics_preTaxMargin,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.netProfitMargin')  as CompanyMetrics_netProfitMargin,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.averageGrossMargin5Year')  as CompanyMetrics_averageGrossMargin5Year,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.averagePreTaxMargin5Year')  as CompanyMetrics_averagePreTaxMargin5Year,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.averageNetProfitMargin5Year')  as CompanyMetrics_averageNetProfitMargin5Year,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.operatingMargin')  as CompanyMetrics_operatingMargin,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.netMarginPercent')  as CompanyMetrics_netMarginPercent,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.returnOnAssetCurrent')  as CompanyMetrics_returnOnAssetCurrent,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.returnOnAsset5YearAverage')  as CompanyMetrics_returnOnAsset5YearAverage,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.returnOnCapitalCurrent')  as CompanyMetrics_returnOnCapitalCurrent,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.assetTurnover')  as CompanyMetrics_assetTurnover,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.inventoryTurnover')  as CompanyMetrics_inventoryTurnover,
JSON_VALUE(JsonData, '$.equity.analysis.companyMetrics.receivableTurnover')  as CompanyMetrics_receivableTurnover,




JSON_VALUE(JsonData, '$.equity.analysis.shareStatistics.lastSplitFactor')  as shareStatistics_lastSplitFactor,
JSON_VALUE(JsonData, '$.equity.analysis.shareStatistics.lastSplitDate')  as shareStatistics_lastSplitDate,
JSON_VALUE(JsonData, '$.equity.analysis.shareStatistics.dividendYield')  as shareStatistics_dividendYield,
JSON_VALUE(JsonData, '$.equity.analysis.shareStatistics.exDividendAmount')  as shareStatistics_exDividendAmount,
JSON_VALUE(JsonData, '$.equity.analysis.shareStatistics.sharesOutstanding')  as shareStatistics_sharesOutstanding,
JSON_VALUE(JsonData, '$.equity.analysis.shareStatistics.enterpriseValue')  as shareStatistics_enterpriseValue,

JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.assetTurnover')  as industryMetrics_assetTurnover,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.averageGrossMargin5Year')  as industryMetrics_averageGrossMargin5Year,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.averageNetProfitMargin5Year')  as industryMetrics_averageNetProfitMargin5Year,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.averagePreTaxMargin5Year')  as industryMetrics_averagePreTaxMargin5Year,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.bookValueShareRatio')  as industryMetrics_bookValueShareRatio,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.currentRatio')  as industryMetrics_currentRatio,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.debtEquityRatio')  as industryMetrics_debtEquityRatio,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.dividendYield')  as industryMetrics_dividendYield,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.dividendYield5YearAverage')  as industryMetrics_dividendYield5YearAverage,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.grossMargin')  as industryMetrics_grossMargin,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.incomeEmployee')  as industryMetrics_incomeEmployee,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.interestCoverage')  as industryMetrics_interestCoverage,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.inventoryTurnover')  as industryMetrics_inventoryTurnover,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.netIncomeQQLastYearGrowthRate')  as industryMetrics_netIncomeQQLastYearGrowthRate,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.netIncomeYTDYTDGrowthRate')  as industryMetrics_netIncomeYTDYTDGrowthRate,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.netProfitMargin')  as industryMetrics_netProfitMargin,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.pEGrowthRatio')  as industryMetrics_pEGrowthRatio,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.preTaxMargin')  as industryMetrics_preTaxMargin,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.priceBookRatio')  as industryMetrics_priceBookRatio,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.priceCashFlowRatio')  as industryMetrics_priceCashFlowRatio,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.priceSalesRatio')  as industryMetrics_priceSalesRatio,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.quickRatio')  as industryMetrics_quickRatio,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.returnOnAsset5YearAverage')  as industryMetrics_returnOnAsset5YearAverage,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.returnOnAssetCurrent')  as industryMetrics_returnOnAssetCurrent,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.returnOnCapital5YearAverage')  as industryMetrics_returnOnCapital5YearAverage,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.returnOnCapitalCurrent')  as industryMetrics_returnOnCapitalCurrent,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.returnOnEquity5YearAverage')  as industryMetrics_returnOnEquity5YearAverage,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.returnOnEquityCurrent')  as industryMetrics_returnOnEquityCurrent,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.revenueEmployee')  as industryMetrics_revenueEmployee,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.revenueQQLastYearGrowthRate')  as industryMetrics_revenueQQLastYearGrowthRate,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.revenueYTDYTD')  as industryMetrics_revenueYTDYTD,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.receivableTurnover')  as industryMetrics_receivableTurnover,
JSON_VALUE(JsonData, '$.equity.analysis.industryMetrics.leverageRatio')  as industryMetrics_leverageRatio,




JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.debtToEquityRatio')  as keyMetrics_debtToEquityRatio,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.eps')  as keyMetrics_eps,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.forwardPriceToEPS')  as keyMetrics_forwardPriceToEPS,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.payoutRatio')  as keyMetrics_payoutRatio,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.priceToBookRatio')  as keyMetrics_priceToBookRatio,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.profitability')  as keyMetrics_profitability,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.stockGrowth')  as keyMetrics_stockGrowth,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.latestRevenue')  as keyMetrics_latestRevenue,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.latestIncome')  as keyMetrics_latestIncome,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.latestNetProfitMargin')  as keyMetrics_latestNetProfitMargin,
JSON_VALUE(JsonData, '$.equity.analysis.keyMetrics.latestRevenuePerShare')  as keyMetrics_latestRevenuePerShare,



JSON_VALUE(JsonData, '$.quote.price')  as quote_price,
JSON_VALUE(JsonData, '$.quote.priceChange')  as quote_priceChange,
JSON_VALUE(JsonData, '$.quote.priceDayHigh')  as quote_priceDayHigh,
JSON_VALUE(JsonData, '$.quote.priceDayLow')  as quote_priceDayLow,
JSON_VALUE(JsonData, '$.quote.timeLastTraded')  as quote_timeLastTraded,
JSON_VALUE(JsonData, '$.quote.priceDayOpen')  as quote_priceDayOpen,
JSON_VALUE(JsonData, '$.quote.pricePreviousClose')  as quote_pricePreviousClose,
JSON_VALUE(JsonData, '$.quote.datePreviousClose')  as quote_datePreviousClose,
JSON_VALUE(JsonData, '$.quote.priceAsk')  as quote_priceAsk,
JSON_VALUE(JsonData, '$.quote.askSize')  as quote_askSize,
JSON_VALUE(JsonData, '$.quote.priceBid')  as quote_priceBid,
JSON_VALUE(JsonData, '$.quote.bidSize')  as quote_bidSize,
JSON_VALUE(JsonData, '$.quote.accumulatedVolume')  as quote_accumulatedVolume,
JSON_VALUE(JsonData, '$.quote.averageVolume')  as quote_averageVolume,
JSON_VALUE(JsonData, '$.quote.peRatio')  as quote_peRatio,
JSON_VALUE(JsonData, '$.quote.priceChangePercent')  as quote_priceChangePercent,
JSON_VALUE(JsonData, '$.quote.price52wHigh')  as quote_price52wHigh,
JSON_VALUE(JsonData, '$.quote.price52wLow')  as quote_price52wLow,
JSON_VALUE(JsonData, '$.quote.priceClose')  as quote_priceClose,
JSON_VALUE(JsonData, '$.quote.yieldPercent')  as quote_yieldPercent,
JSON_VALUE(JsonData, '$.quote.priceChange1Week')  as quote_priceChange1Week,
JSON_VALUE(JsonData, '$.quote.priceChange1Month')  as quote_priceChange1Month,
JSON_VALUE(JsonData, '$.quote.priceChange3Month')  as quote_priceChange3Month,
JSON_VALUE(JsonData, '$.quote.priceChange6Month')  as quote_priceChange6Month,
JSON_VALUE(JsonData, '$.quote.priceChangeYTD')  as quote_priceChangeYTD,
JSON_VALUE(JsonData, '$.quote.priceChange1Year')  as quote_priceChange1Year,
JSON_VALUE(JsonData, '$.quote.return1Week')  as quote_return1Week,
JSON_VALUE(JsonData, '$.quote.return1Month')  as quote_return1Month,
JSON_VALUE(JsonData, '$.quote.return3Month')  as quote_return3Month,
JSON_VALUE(JsonData, '$.quote.return6Month')  as quote_return6Month,
JSON_VALUE(JsonData, '$.quote.returnYTD')  as quote_returnYTD,
JSON_VALUE(JsonData, '$.quote.return1Year')  as quote_return1Year,
JSON_VALUE(JsonData, '$.quote.sourceExchangeCode')  as quote_sourceExchangeCode,
JSON_VALUE(JsonData, '$.quote.sourceExchangeName')  as quote_sourceExchangeName,
JSON_VALUE(JsonData, '$.quote.marketCap')  as quote_marketCap,
JSON_VALUE(JsonData, '$.quote.marketCapCurrency')  as quote_marketCapCurrency,
JSON_VALUE(JsonData, '$.quote.exchangeId')  as quote_exchangeId,
JSON_VALUE(JsonData, '$.quote.exchangeCode')  as quote_exchangeCode,
JSON_VALUE(JsonData, '$.quote.exchangeName')  as quote_exchangeName,
JSON_VALUE(JsonData, '$.quote.offeringStatus')  as quote_offeringStatus,
JSON_VALUE(JsonData, '$.quote.displayName')  as quote_displayName,
JSON_VALUE(JsonData, '$.quote.shortName')  as quote_shortName,
JSON_VALUE(JsonData, '$.quote.securityType')  as quote_securityType,
JSON_VALUE(JsonData, '$.quote.instrumentId')  as quote_instrumentId,
JSON_VALUE(JsonData, '$.quote.symbol')  as quote_symbol,
JSON_VALUE(JsonData, '$.quote.country')  as quote_country,
JSON_VALUE(JsonData, '$.quote.market')  as quote_market,
JSON_VALUE(JsonData, '$.quote.timeLastUpdated')  as quote_timeLastUpdated,
JSON_VALUE(JsonData, '$.quote.currency')  as quote_currency,
JSON_VALUE(JsonData, '$.quote._p')  as quote__p,
JSON_VALUE(JsonData, '$.quote.id')  as quote_id,
JSON_VALUE(JsonData, '$.quote._t')  as quote__t,
JSON_VALUE(JsonData, '$.equity.company.address.street')  as address_street,
JSON_VALUE(JsonData, '$.equity.company.address.city')  as address_city,
JSON_VALUE(JsonData, '$.equity.company.address.state')  as address_state,
JSON_VALUE(JsonData, '$.equity.company.address.zip')  as address_zip,
JSON_VALUE(JsonData, '$.equity.company.address.country')  as address_country,
JSON_VALUE(JsonData, '$.equity.company.address.countryCode')  as address_countryCode,
JSON_VALUE(JsonData, '$.equity.company.address.phone')  as  address_phone,
JSON_VALUE(JsonData, '$.equity.company.address.fax')  as  address_fax, Symbol,
JSON_VALUE(JsonData, '$.equity.company.website')  as website,GETDATE()


FROM  Equitys where JSON_VALUE(JsonData, '$.equity.company.address.countryCode')='IN'
End
GO
/****** Object:  StoredProcedure [dbo].[InsertAUTO_BUY_EQUTIES]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[InsertAUTO_BUY_EQUTIES] ( @Symbol varchar(50),@Quanity int ,@exchange varchar(30),@marketor_Limit varchar(50),@buyprice decimal(10,2),@stoploss decimal(10,2)
,@BuyorSell varchar(100),@StockCode varchar(100))
as
begin
if not exists ( select 1 from AUTO_BUY_EQUTIES where Symbol=@Symbol and cast(DATE as date)=CAST(GETDATE() as Date))
begin 
INSERT INTO [dbo].[AUTO_BUY_EQUTIES]
           ([Symbol]
           ,[Quanity]
           
           ,[EX]
           ,[ORDERType]
           ,[BUY_PRICE]
           ,[STOPLoss]
           ,[stock_code]
           ,[DATE]
           ,[Action])
		   Select @Symbol,@Quanity,@exchange,@marketor_Limit,@buyprice,@stoploss,@StockCode,GETDATE(),@BuyorSell
End
--Declare @Date  DateTime = (select MAX(Date) from Portfolio_Holdings_Details )

--INSERT INTO [dbo].[AUTO_BUY_EQUTIES]
--           ([Symbol]
--           ,[Quanity]
--           ,[RID]
--           ,[EX]
--           ,[ORDERType]
--           ,[BUY_PRICE]
--           ,[STOPLoss]
--           ,[stock_code]
--           ,[DATE]
--           ,[Action],InvestedPrice)
--SELECT  
--      [stock_code]
--      ,quantity
--      ,NULL
--      ,exchange_code
--      ,'limit'
--	  ,cast( ([dbo].GetPerCentageOfTotalValue(10,quantity*average_price)/quantity) as decimal(10,0))
--	  ,null
--	  ,[stock_code]
--	  ,GETDATE()
--	  ,'Sell'
--	  , average_price
--  FROM [STOCK].[dbo].[Portfolio_Holdings_Details] where 
--  CAST(Date AS date)= CAST(@Date AS date) and stock_code not in  ('JIOFIN','ELEIN')
--  and not  exists (select * from  [AUTO_BUY_EQUTIES]  EQ where Eq.stock_code=[stock_code] and action ='sell' and cast(Date as date) =cast(GETDATE() as date))


--  INSERT INTO [dbo].[AUTO_BUY_EQUTIES]
--           ([Symbol]
--           ,[Quanity]
--           ,[RID]
--           ,[EX]
--           ,[ORDERType]
--           ,[BUY_PRICE]
--           ,[STOPLoss]
--           ,[stock_code]
--           ,[DATE]
--           ,[Action],InvestedPrice)
--SELECT  
--      [stock_code]
--      ,quantity
--      ,NULL
--      ,exchange_code
--      ,'limit'
--	  ,cast( ([dbo].GetPerCentageOfTotalValue(6,quantity*ltp)/quantity) as decimal(10,0))
--	  ,null
--	  ,[stock_code]
--	  ,GETDATE()
--	  ,'Sell'
--	  , average_price
--  FROM [STOCK].[dbo].[Portfolio_Positions_Details] where 
--  CAST(Date AS date)= CAST(@Date AS date) and stock_code not in  ('JIOFIN','ELEIN')
--  and not  exists (select * from  [AUTO_BUY_EQUTIES]  EQ where Eq.stock_code=[stock_code] and action ='sell' and cast(Date as date) =cast(GETDATE() as date))
END

GO
/****** Object:  StoredProcedure [dbo].[InsertBuyStocks]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   Procedure [dbo].[InsertBuyStocks](@STOCKCode varchar(100),@Change int,@ltp decimal(10,2) )  
as
begin
 
 Insert into STOCK_NTFCTN([symbol]
      ,[Date]
	  ,Last
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY])
 SELECT distinct  @STOCKCode, cast(GETDATE() as Date) as Date, @ltp,

 case when @Change > 4.0 then '<b><ul>'+  @STOCKCode +  ' </b></ul> has increased proift the percentage of ' + cast( @Change as varchar(20))  + ' Current Holdings'
   when @Change < 0.0 then  '<b><ul>'+  @STOCKCode +  ' </b></ul> has decreased loss  percentage of ' + cast( @Change as varchar(20)) + ' Current Holdings' end  as STOCKName 
,
0,0,0,0,cast(@Change as decimal(5,2)),'Common_Low' as GroupName 
WHERE  

(@Change > 4.0 or  @Change < 0.0) and 
not  exists (select 1 from STOCK_NTFCTN where symbol=@STOCKCode and cast(change as decimal(5,2)) = cast(@Change as decimal(5,2))  and CAST(Date as Date)=cast(GETDATE() as Date)    and PO_KEY='Common_Low' )



end 
GO
/****** Object:  StoredProcedure [dbo].[InsertCurrentHoldings]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[InsertCurrentHoldings](@STOCKCode varchar(100),@Change int,@ltp decimal(10,2) )  
as
begin
 
 Insert into STOCK_NTFCTN([symbol]
      ,[Date]
	  ,Last
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY])
 SELECT distinct  @STOCKCode, cast(GETDATE() as datetime) as Date, @ltp,

 case when @Change > 4.0 then '<b><ul>'+  @STOCKCode +  ' </b></ul> has increased proift the percentage of ' + cast( @Change as varchar(20))  + ' Current Holdings'
   when @Change < 0.0 then  '<b><ul>'+  @STOCKCode +  ' </b></ul> has decreased loss  percentage of ' + cast( @Change as varchar(20)) + ' Current Holdings' end  as STOCKName 
,
0,0,0,0,cast(@Change as decimal(5,2)),'SELL_STOCK_OWN' as GroupName 
WHERE  

(@Change > 4.0 or  @Change < 0.0) and 
not  exists (select 1 from STOCK_NTFCTN where symbol=@STOCKCode and cast(change as decimal(5,2)) = cast(@Change as decimal(5,2))  and CAST(Date as Date)=cast(GETDATE() as Date)    and PO_KEY='SELL_STOCK_OWN' )



end 
GO
/****** Object:  StoredProcedure [dbo].[InsertCurrentPosition]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[InsertCurrentPosition](@STOCKCode varchar(100),@Change int,@ltp decimal(10,2) )  
as
begin
 

 Insert into STOCK_NTFCTN([symbol]
      ,[Date]
	  ,Last
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY])
 SELECT distinct  @STOCKCode, cast(GETDATE() as DateTime) as Date, @ltp,

 case when @Change > 4.0 then '<b><ul>'+  @STOCKCode +  ' </b></ul> has increased proift the percentage of ' + cast( @Change as varchar(20))  + ' Current Position'
   when @Change < 0.0 then  '<b><ul>'+  @STOCKCode +  ' </b></ul> has decreased loss  percentage of ' + cast( @Change as varchar(20)) + ' Current Position' end  as STOCKName 
,
0,0,0,0,cast(@Change as decimal(5,2)),'SELL_STOCK_OWN' as GroupName 
WHERE 

(@Change > 4.0 or  @Change < 0.0) and 
-- CAST(ltt as Date)=cast(GETDATE() as Date) --and  totalSellQ >=  (totalBuyQt/100)* 80
  not  exists (select 1 from STOCK_NTFCTN where symbol=@STOCKCode and cast(change as decimal(5,2)) = cast(@Change as decimal(5,2))  and CAST(Date as Date)=cast(GETDATE() as Date)    and PO_KEY='SELL_STOCK_OWN' )




-- SELECT distinct  @STOCKCode, cast(GETDATE() as Date) as Date, @ltp,
--@Message + ' Current Position' as STOCKName 
--,
--0,0,0,0,cast(p.change as decimal(5,2)),'SELL_STOCK_OWN' as GroupName 
--FROM [STOCK].[dbo].[Ticker_Stocks_Histry] P
--WHERE  CAST(ltt as Date)=cast(GETDATE() as Date) and  totalSellQ >=  (totalBuyQt/100)* 80
-- and not  exists (select 1 from STOCK_NTFCTN where symbol=p.symbol and cast(change as decimal(5,2)) = cast(p.change as decimal(5,2))  and PO_KEY='SELL_STOCK_OWN' )



end 
GO
/****** Object:  StoredProcedure [dbo].[InsertFromMicrosoft]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/
CREATE Proc [dbo].[InsertFromMicrosoft]
as
begin 
Delete from Equitys_Histry where CAST(Created as Date)=CAST(GETDATE() as date)
Insert into Equitys_Histry
SELECT  
      [SecurityCode]
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
      ,[MSN_SECID]
      ,UpdatedOn
  FROM [STOCK].[dbo].[Equitys]
  End
GO
/****** Object:  StoredProcedure [dbo].[Insertipo_Current]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Insertipo_Current]
(@symbol varchar(10),
@companyName varchar(500),
@series varchar(500),
@issueStartDate varchar(11),
@issueEndDate varchar(11),
@status varchar(11),
@issueSize int,
@issuePrice varchar(7),
@sr_no int,
@isBse bit,
@lotSize int,
@priceBand varchar(250),
@filename varchar(500))
as
begin

if not  exists (select * from  ipo_current_issue where symbol=@symbol and issueStartDate = @issueStartDate ) 
begin
INSERT INTO [dbo].[ipo_current_issue]
           ([symbol]
           ,[companyName]
           ,[series]
           ,[issueStartDate]
           ,[issueEndDate]
           ,[status]
           ,[issueSize]
           ,[issuePrice]
           ,[srNo]
           ,[isBse],noOfSharesOffered,noOfsharesBid,noOfTime)
     VALUES
           (@symbol ,
@companyName ,
@series ,
@issueStartDate ,
@issueEndDate ,
@status,
@issueSize,
@issuePrice,
@sr_no,
@isBse,1,1,1)



Insert into STOCK_NTFCTN(
[symbol]
      ,[Date]
	  ,Last
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY],[Priority])
select @symbol, GETDATE() as Date, 0, '<br>'+
@companyName + ' IPO Current ' + CAST( @issueStartDate as varchar) + '   ' +  ' ' + CAST( @issueEndDate as varchar)  + '' + ' ' ,
0,0,0,0,0,
'IPO_Current'
as GroupName ,
0

End



End




GO
/****** Object:  StoredProcedure [dbo].[Insertipo_Upcomming]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Insertipo_Upcomming]
(@symbol varchar(10),
@companyName varchar(500),
@series varchar(500),
@issueStartDate varchar(11),
@issueEndDate varchar(11),
@status varchar(11),
@issueSize int,
@issuePrice varchar(7),
@sr_no int,
@isBse bit,
@lotSize int,
@priceBand varchar(250),
@filename varchar(500))
as
begin

if not  exists (select * from  [ipo_Upcomming] where symbol=@symbol and issueStartDate = @issueStartDate ) 
begin
INSERT INTO [dbo].[ipo_Upcomming]
           ([symbol]
           ,[companyName]
           ,[series]
           ,[issueStartDate]
           ,[issueEndDate]
           ,[status]
           ,[issueSize]
           ,[issuePrice]
           ,[sr_no]
           ,[isBse]
           ,[lotSize]
           ,[priceBand]
           ,[filename])
     VALUES
           (@symbol ,
@companyName ,
@series ,
@issueStartDate ,
@issueEndDate ,
@status,
@issueSize,
@issuePrice,
@sr_no,
@isBse ,
@lotSize ,
@priceBand ,
@filename)



Insert into STOCK_NTFCTN(
[symbol]
      ,[Date]
	  ,Last
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY],[Priority])
select @symbol, GETDATE() as Date, 0, '<br>'+
@companyName + ' IPO UpComming ' +  @priceBand + '   '  + @issueStartDate + +' '+@issueEndDate+  '  ' + @filename + '' + ' ' ,
0,0,0,0,0,
'IPO_UpComming'
as GroupName ,
0

End



End




GO
/****** Object:  StoredProcedure [dbo].[InsertNSENews]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[InsertNSENews](@Symbol varchar(100)
           ,@CompanyName varchar(max)
           ,@Subject varchar(max)
           ,@Details varchar(max)
           ,@BroadcastDateTime datetime
           ,@Receipt varchar(max)
           ,@DISSEMINATION varchar(500)
           ,@Difference varchar(max)
           ,@Attachement varchar(max)
           )
as
begin 

if (@Subject <>'Loss of Share Certificates')
begin

if not  exists (select top 1 Symbol from [dbo].[NSEAnnouncement] where  Symbol=@Symbol and Receipt=@Receipt and Subject=@Subject and DISSEMINATION=@DISSEMINATION )
begin 
INSERT INTO [dbo].[NSEAnnouncement]
           ([Symbol]
           ,[CompanyName]
           ,[Subject]
           ,[Details]
           ,[BroadcastDateTime]
           ,[Receipt]
           ,[DISSEMINATION]
           ,[Difference]
           ,[Attachement])
     VALUES
           (@Symbol
           ,@CompanyName
           ,@Subject
           ,@Details
           ,@BroadcastDateTime
           ,@Receipt
           ,@DISSEMINATION
           ,@Difference
           ,@Attachement
           )

 Insert into STOCK_NTFCTN(
[symbol]
      ,[Date]
	  ,Last
      ,[STOCKName]
      ,[IsNotified]
      ,[IsUppCKT]
      ,[ISSell]
      ,[ISPrict]
      ,[Change]
      ,[PO_KEY],[Priority])
select @Symbol, cast(@BroadcastDateTime as DateTime) as Date, 0, 
@CompanyName + ' ' +  @Subject + '  ' + ' ' + @Attachement + '  ' + ' <br>' ,
0,0,0,0,0,'NSE_NEWS' as GroupName , case when @Subject like '%Order%' Then 2 else 1 End

End
END


End
GO
/****** Object:  StoredProcedure [dbo].[InsertOrUpdateMSN_Notification]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[InsertOrUpdateMSN_Notification](@MSN_SECID varchar(50), @IsFavoriteAdded bit,@IsFavoriteRemoved bit)
as
begin

if NOt exists (select 1 from MSN_Equities_Notification where MSN_SECID=@MSN_SECID)
begin
INSERT INTO [dbo].[MSN_Equities_Notification]
           ([MSN_SECID]
            
           ,[IsFavoriteAdded]
          
           
           ,[Created_On]
          
          
           ,[IsFavoriteRemoved])
     VALUES
           (@MSN_SECID, @IsFavoriteAdded,GETDATE(),@IsFavoriteRemoved)
end

Else

Update [dbo].[MSN_Equities_Notification] set IsFavoriteAdded=@IsFavoriteAdded,IsFavoriteRemoved=@IsFavoriteRemoved,Updated_On=GETDATE() where MSN_SECID=@MSN_SECID

End 

GO
/****** Object:  StoredProcedure [dbo].[MSNDownStocksSP]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  procedure [dbo].[MSNDownStocksSP]
as
begin

drop table if exists #6MonthsData;
drop table if exists #3MonthsData;
drop table if exists #1MonthsData;
drop table if exists #1weekData;
with CTE3 as(
SELECT keyMetrics_eps,industryMetrics_dividendYield,shareStatistics_dividendYield,Symbol, quote_displayName, quote_return1Month,quote_return1Week,quote_return3Month,quote_return6Month, Created_On, cast(quote_priceChange6Month as decimal) as quote_priceChange6Month,estimate_recommendation,quote_priceDayOpen,
  LAG(cast(quote_priceChange6Month as decimal)) OVER(ORDER BY created_on) as previous_sale_value,quote_accumulatedVolume,quote_averageVolume,
cast (CONVERT(DATE, CONVERT(DATETIMEOFFSET,[quote_timeLastUpdated])) as  DATE) [quote_timeLastUpdated],
	cast (estimate_dateLastUpdated as date) as estimate_dateLastUpdated from MMSN_Companies msn)
select * into  #6MonthsData from CTE3  where cast(quote_priceChange6Month as decimal) < cast(previous_sale_value as decimal) and cast(Created_On as date)=CAST(GETDATE() as Date)  order by quote_displayName, Created_On ;


with CTE2 as(
SELECT keyMetrics_eps,industryMetrics_dividendYield,shareStatistics_dividendYield,Symbol, quote_displayName, quote_return1Month,quote_return1Week,quote_return3Month,quote_return6Month, Created_On, cast(quote_priceChange3Month as decimal) as quote_priceChange3Month,estimate_recommendation,quote_priceDayOpen,
  LAG(cast(quote_priceChange3Month as decimal)) OVER(ORDER BY created_on) as previous_sale_value,quote_accumulatedVolume,quote_averageVolume,
cast (CONVERT(DATE, CONVERT(DATETIMEOFFSET,[quote_timeLastUpdated])) as  DATE) [quote_timeLastUpdated],
	cast (estimate_dateLastUpdated as date) as estimate_dateLastUpdated from  MMSN_Companies msn)
select * into #3MonthsData from CTE2  where cast(quote_priceChange3Month as decimal) < cast(previous_sale_value as decimal) and cast(Created_On as date)=CAST(GETDATE() as Date)  order by quote_displayName, Created_On; 

with CTE1 as(
SELECT keyMetrics_eps,industryMetrics_dividendYield,shareStatistics_dividendYield,Symbol, quote_return1Month,quote_return1Week,quote_return3Month,quote_return6Month, Created_On, quote_displayName, cast(quote_priceChange1Month as decimal) as quote_priceChange1Month,estimate_recommendation,quote_priceDayOpen,
  LAG(cast(quote_priceChange1Month as decimal)) OVER(ORDER BY created_on) as previous_sale_value,quote_accumulatedVolume,quote_averageVolume,
cast (CONVERT(DATE, CONVERT(DATETIMEOFFSET,[quote_timeLastUpdated])) as  DATE) [quote_timeLastUpdated],
	cast (estimate_dateLastUpdated as date) as estimate_dateLastUpdated from  MMSN_Companies msn)
select *  into #1MonthsData from CTE1  where cast(quote_priceChange1Month as decimal) < cast(previous_sale_value as decimal) and cast(Created_On as date)=CAST(GETDATE() as Date)  order by quote_displayName, Created_On;



with CTE as(
SELECT keyMetrics_eps,industryMetrics_dividendYield,shareStatistics_dividendYield,Created_On,Symbol, quote_displayName,quote_return1Week, quote_return1Month,quote_return3Month,quote_return6Month,quote_priceChange1Month,quote_priceChange3Month,quote_priceChange6Month,estimate_recommendation,quote_priceDayOpen,cast(quote_priceChange1Week as decimal) as quote_priceChange1Week,
  LAG(cast(quote_priceChange1Week as decimal)) OVER(ORDER BY created_on) as previous_priceChange1Week,quote_accumulatedVolume,quote_averageVolume,
cast (CONVERT(DATE, CONVERT(DATETIMEOFFSET,[quote_timeLastUpdated])) as  DATE) [quote_timeLastUpdated],
	cast (estimate_dateLastUpdated as date) as estimate_dateLastUpdated from  MMSN_Companies msn)
select * into #1weekData  from CTE  where cast(quote_priceChange1Week as decimal) < cast(previous_priceChange1Week as decimal) and cast(Created_On as date)=CAST(GETDATE() as Date)  order by quote_displayName, Created_On ;

Update #1weekData set [estimate_dateLastUpdated]=null where estimate_dateLastUpdated='0001-01-01';
delete from MSNDownStocks where cast(Created_On as date)=CAST(GETDATE() as Date)
--Select W.* into MSNDownStocks from #1weekData W
--inner join #1MonthsData oneM on oneM.Symbol<>w.Symbol
--inner join #3MonthsData ThreM on ThreM.Symbol<>w.Symbol
--inner join #6MonthsData SixM on SixM.Symbol<>w.Symbol
Insert into MSNDownStocks([keyMetrics_eps]
           ,[industryMetrics_dividendYield]
           ,[shareStatistics_dividendYield]
           ,[Created_On]
           ,[Symbol]
           ,[quote_displayName]
           ,[quote_return1Week]
           ,[quote_return1Month]
           ,[quote_return3Month]
           ,[quote_return6Month]
           ,[quote_priceChange1Month]
           ,[quote_priceChange3Month]
           ,[quote_priceChange6Month]
           ,[estimate_recommendation]
           ,[quote_priceDayOpen]
           ,[quote_priceChange1Week]
           ,[previous_priceChange1Week]
           ,[quote_accumulatedVolume]
           ,[quote_averageVolume]
            ,quote_timeLastUpdated
           ,[estimate_dateLastUpdated])
select [keyMetrics_eps]
           ,[industryMetrics_dividendYield]
           ,[shareStatistics_dividendYield]
           ,[Created_On]
           ,[Symbol]
           ,[quote_displayName]
           ,[quote_return1Week]
           ,[quote_return1Month]
           ,[quote_return3Month]
           ,[quote_return6Month]
           ,[quote_priceChange1Month]
           ,[quote_priceChange3Month]
           ,[quote_priceChange6Month]
           ,[estimate_recommendation]
           ,[quote_priceDayOpen]
           ,[quote_priceChange1Week]
           ,[previous_priceChange1Week]
           ,[quote_accumulatedVolume]
           ,[quote_averageVolume], quote_timeLastUpdated,cast ([estimate_dateLastUpdated] As date)
      from #1weekData where Symbol not in (
Select Symbol from #1MonthsData where Symbol not in (
Select Symbol from #3MonthsData where Symbol not in  (
Select Symbol from #6MonthsData)))

Select * from MSNDownStocks

End

GO
/****** Object:  StoredProcedure [dbo].[MSNUpStocksSP]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  procedure [dbo].[MSNUpStocksSP]
as
begin

drop table if exists #6MonthsData;
drop table if exists #3MonthsData;
drop table if exists #1MonthsData;
drop table if exists #1weekData;
with CTE3 as(
SELECT keyMetrics_eps,industryMetrics_dividendYield,shareStatistics_dividendYield,Symbol, quote_displayName, quote_return1Month,quote_return1Week,quote_return3Month,quote_return6Month, Created_On, cast(quote_priceChange6Month as decimal) as quote_priceChange6Month,estimate_recommendation,quote_priceDayOpen,
  LAG(cast(quote_priceChange6Month as decimal)) OVER(ORDER BY created_on) as previous_sale_value,quote_accumulatedVolume,quote_averageVolume
,cast (CONVERT(DATE, CONVERT(DATETIMEOFFSET,[quote_timeLastUpdated])) as  DATE) [quote_timeLastUpdated],
	cast (estimate_dateLastUpdated as date) as estimate_dateLastUpdated  from MMSN_Companies msn)
select * into  #6MonthsData from CTE3  where cast(quote_priceChange6Month as decimal) > cast(previous_sale_value as decimal) and cast(Created_On as date)=CAST(GETDATE() as Date)  order by quote_displayName, Created_On ;


with CTE2 as(
SELECT keyMetrics_eps,industryMetrics_dividendYield,shareStatistics_dividendYield,Symbol, quote_displayName, quote_return1Month,quote_return1Week,quote_return3Month,quote_return6Month, Created_On, cast(quote_priceChange3Month as decimal) as quote_priceChange3Month,estimate_recommendation,quote_priceDayOpen,
  LAG(cast(quote_priceChange3Month as decimal)) OVER(ORDER BY created_on) as previous_sale_value,quote_accumulatedVolume,quote_averageVolume
,cast (CONVERT(DATE, CONVERT(DATETIMEOFFSET,[quote_timeLastUpdated])) as  DATE) [quote_timeLastUpdated],
	cast (estimate_dateLastUpdated as date) as estimate_dateLastUpdated from MMSN_Companies msn)
select * into #3MonthsData from CTE2  where cast(quote_priceChange3Month as decimal) > cast(previous_sale_value as decimal) and cast(Created_On as date)=CAST(GETDATE() as Date)  order by quote_displayName, Created_On; 

with CTE1 as(
SELECT keyMetrics_eps,industryMetrics_dividendYield,shareStatistics_dividendYield,Symbol, quote_return1Month,quote_return1Week,quote_return3Month,quote_return6Month, Created_On, quote_displayName, cast(quote_priceChange1Month as decimal) as quote_priceChange1Month,estimate_recommendation,quote_priceDayOpen,
  LAG(cast(quote_priceChange1Month as decimal)) OVER(ORDER BY created_on) as previous_sale_value,quote_accumulatedVolume,quote_averageVolume
,cast (CONVERT(DATE, CONVERT(DATETIMEOFFSET,[quote_timeLastUpdated])) as  DATE) [quote_timeLastUpdated],
	cast (estimate_dateLastUpdated as date) as estimate_dateLastUpdated from MMSN_Companies msn)
select *  into #1MonthsData from CTE1  where cast(quote_priceChange1Month as decimal) > cast(previous_sale_value as decimal) and cast(Created_On as date)=CAST(GETDATE() as Date)  order by quote_displayName, Created_On;



with CTE as(
SELECT keyMetrics_eps,industryMetrics_dividendYield,shareStatistics_dividendYield,Created_On,Symbol, quote_displayName,quote_return1Week, quote_return1Month,quote_return3Month,quote_return6Month,quote_priceChange1Month,quote_priceChange3Month,quote_priceChange6Month,estimate_recommendation,quote_priceDayOpen,cast(quote_priceChange1Week as decimal) as quote_priceChange1Week,
  LAG(cast(quote_priceChange1Week as decimal)) OVER(ORDER BY created_on) as previous_priceChange1Week,quote_accumulatedVolume,quote_averageVolume
,cast (CONVERT(DATE, CONVERT(DATETIMEOFFSET,[quote_timeLastUpdated])) as  DATE) [quote_timeLastUpdated],
	cast (estimate_dateLastUpdated as date) as estimate_dateLastUpdated from MMSN_Companies msn)
select * into #1weekData  from CTE  where cast(quote_priceChange1Week as decimal) > cast(previous_priceChange1Week as decimal) and cast(Created_On as date)=CAST(GETDATE() as Date)  order by quote_displayName, Created_On ;

Update #1weekData set [estimate_dateLastUpdated]=null where estimate_dateLastUpdated='0001-01-01';
delete from MSNUPStocks where cast(Created_On as date)=CAST(GETDATE() as Date)
--Select W.* into MSNDownStocks from #1weekData W
--inner join #1MonthsData oneM on oneM.Symbol<>w.Symbol
--inner join #3MonthsData ThreM on ThreM.Symbol<>w.Symbol
--inner join #6MonthsData SixM on SixM.Symbol<>w.Symbol
Insert into MSNUPStocks
select *  from #1weekData where Symbol  in (
Select Symbol from #1MonthsData where Symbol  in (
Select Symbol from #3MonthsData where Symbol  in  (
Select Symbol from #6MonthsData)))

Select * from MSNDownStocks

End

GO
/****** Object:  StoredProcedure [dbo].[RESET_DB]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RESET_DB]
as
begin
-- Truncate the log by changing the database recovery model to SIMPLE.
ALTER DATABASE STOCK
SET RECOVERY SIMPLE;

-- Shrink the truncated log file to 1 MB.
DBCC SHRINKFILE (STOCK_Log, 1);

-- Reset the database recovery model.
ALTER DATABASE STOCK
SET RECOVERY FULL;


END
GO
/****** Object:  StoredProcedure [dbo].[ResetBuyStockMarket]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[ResetBuyStockMarket]
as
Begin

Update AUTO_BUY_EQUTIES set ORDERType='limit' where RID is null and cast(DATE as Date)= cast( GETDATE() as Date)
Declare @DATE DateTime =(select MAX(cast(ltt as Date)) from Ticker_Stocks_Histry)

Select distinct  SB.stock_code into #tempdata from AUTO_BUY_EQUTIES  SB
inner join MMSN_Companies  E on SB.Symbol=E.quote_symbol and CAST(Created_On as date)=@DATE
inner join Ticker_Stocks_Histry s on E.Symbol=s.symbol   and CAST(ltt as date)=@DATE
where CAST(ltt as date)=@DATE  and cast(SB.DATE as Date)= cast( GETDATE() as Date)
and SB.BUY_PRICE+1 >= s.last  --and  [action]='buy'

Update AUTO_BUY_EQUTIES set ORDERType='limit' where stock_code in ( 
select * from #tempdata)

--Update AUTO_BUY_EQUTIES set ORDERType='market' where stock_code in ( 
--select * from #tempdata )
End


GO
/****** Object:  StoredProcedure [dbo].[Run_AwardCount]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  Procedure [dbo].[Run_AwardCount]
as 
begin
drop table if exists Stock_Award
Select n.symbol as Code ,eq.Symbol, Eq.SecurityName, COUNT(*) as OrderCount 
Into Stock_Award from STOCK_NTFCTN  N
left join Equitys Eq on replace(Eq.Symbol,'1.1!','')=N.symbol
where stockname like '%Order%'
group by n.symbol,eq.Symbol,Eq.SecurityName
order by COUNT(*) desc
End
GO
/****** Object:  StoredProcedure [dbo].[Run_MIN_MAX_Threshold]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Run_MIN_MAX_Threshold]
as
begin
--	with CTE as (
--Select keyMetrics_eps,quote_peRatio,quote_averageVolume, quote_price,isnull(MSN.shareStatistics_dividendYield,-1) as dividendYield, H.symbol, MIN (last) as  min,MAX(last) as max  from Ticker_Stocks_Histry  H
--left join MMSN_Companies MSN on MSN.Symbol= H.symbol and CAST(Created_On as Date)=CAST(GETDATE() as Date)
--where CAST(ltt as Date)>= CAST(GETDATE()-20 as Date)

	with CTE as (
Select keyMetrics_eps,quote_peRatio,quote_averageVolume, quote_price,isnull(MSN.shareStatistics_dividendYield,-1) as dividendYield, H.symbol, MIN ([open]) as  min,MAX([open]) as max ,MSN.estimate_recommendation from Ticker_Stocks_Histry  H
left join MMSN_Companies MSN on MSN.Symbol= H.symbol and CAST(Created_On as Date)=CAST(GETDATE() as Date)
where CAST(ltt as Date)>= CAST(GETDATE()-20 as Date)



group by H.symbol, MSN.shareStatistics_dividendYield,keyMetrics_eps,quote_peRatio,quote_averageVolume,quote_price,MSN.estimate_recommendation)

UPDATE
    Table_A
SET
    Table_A.THRS_MIN = Table_B.min,--(SELECT [dbo].[GetThreshold_Min] (Table_A.Symbol)),
    Table_A.THRS_MAX = Table_B.max,
	Rating_NUM=dividendYield,
	AverageVolume=quote_averageVolume,
	PERAtio=quote_peRatio,
	EPS=keyMetrics_eps,
	LAST=quote_price,
	UpdatedOn=GETDATE(),
	MSNRating=estimate_recommendation

FROM
    Company_Ratings_Only_DIVI AS Table_A
    INNER JOIN CTE AS Table_B
        ON Table_A.Symbol = Table_B.Symbol

		End



GO
/****** Object:  StoredProcedure [dbo].[SEND_NOTIFICATION]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--SEND_NOTIFICATION 0,0

CREATE procedure [dbo].[SEND_NOTIFICATION](@IsUpper bit,@IsSell bit)
as
begin

exec [dbo].[Get_Todays_Stock_PRED_Upper_CKT]

Delete from STOCK_NTFCTN where STOCKName is null;
drop table if exists  #temp1

Select * into #temp1 from 

( select N.* 
from  STOCK_NTFCTN N

left  join (SELECT  [Symbol],CAN_BUY,MSNRating  FROM Company_Ratings_Only_DIVI )A  on A.Symbol= N.Symbol  
left   JOIN Equitys  eq ON eq.Symbol= N.Symbol
where isNotified=0 and PO_KEY not  in ('BSE_NEWS','NSE_NEWS','BSE_Board Meeting')
 --and ( ISPSU=1 or  CAN_BUY=1 or MSNRating in ('strongBuy','Buy'))  

 union all 
 Select N.* from  STOCK_NTFCTN N

left  join (SELECT  [Symbol],CAN_BUY,MSNRating  FROM Company_Ratings_Only_DIVI )A  on A.Symbol= N.Symbol  
left   JOIN Equitys  eq ON eq.Symbol= N.Symbol
where isNotified=0 and PO_KEY in ('BSE_NEWS','NSE_NEWS','BSE_Board Meeting') )A

--and  - and ( @IsUpper =null or @IsUpper=IsUppCKT)  and (@IsSell=null or  @IsSell=ISSell)
--Update STOCK_NTFCTN set isNotified=1 where Id in (Select ID #temp1)
Select  top 100 * from #temp1 T
left  join PO_CONFIG  N on T.PO_KEY=n.PO_KEY_NAME 
where N.[user]='uh61jjrcvyy1tebgv184u67jr2r36x'
End  








GO
/****** Object:  StoredProcedure [dbo].[SP_GET_CHART_STOCKS_BY_STOCK]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GET_groupName_By]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 Create  Procedure [dbo].[SP_GET_groupName_By](@IndustryNewName varchar(100))
  as begin 
  Select distinct  [IgroupName] as Text  from  [STOCK].[dbo].[Equitys] where  [IndustryNewName]  in  (SELECT id FROM [dbo].[ufnSplit](@IndustryNewName))
  End 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_IndustryNewName_By]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE Procedure [dbo].[SP_GET_IndustryNewName_By](@SectorName varchar(100))
  as begin 
  Select distinct  [IndustryNewName] as Text  from  [STOCK].[dbo].[Equitys] where  SectorName  in  (SELECT id FROM [dbo].[ufnSplit](@SectorName))
  End 
 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK](@Code varchar(50)='',@isfavorite bit =0,@isAutoTrade bit =0

,@ShowNotification bit=0,
@minvalue int=0,
@maxvalue int =0,
@TDays varchar(10)=null,
@WatchList varchar(10)=null
)
as
begin 

drop table if exists #previousData;



WITH TOPTEN AS (
    SELECT *, ROW_NUMBER() 
    over (
        PARTITION BY symbol 
        order by ltt desc
    ) AS RowNo 
    FROM Ticker_Stocks_Histry
)
SELECT  Symbol,
	STRING_AGG(cast([change] as decimal(10,2)), ', ') change,
	STRING_AGG(VolumeC, ', ') VolumeC,
	STRING_AGG(cast([open] as decimal(10,2)), ', ') [open],
	STRING_AGG(cast([close] as decimal(10,2)), ', ') [close],
	STRING_AGG(convert(varchar, ltt, 5), ', ') ltt  into  #previousData FROM TOPTEN WHERE RowNo <= 17  group by symbol;


if(@Code is null or @Code='' )
begin 
Declare @Date DateTime, @DatefromEx DateTime;
Drop table if exists #tempresuls
select @Date=MAX(Created_On) from MMSN_Companies;

select @DatefromEx=MAX(ltt) from Ticker_Stocks_Histry_Extended;

WITH Cte AS(
SELECT *,
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM [Ticker_Stocks_Histry_Extended] with (nolock)    where CAST(ltt as Date )=  CAST(@DatefromEx as Date) )
Delete  from  Cte where RnDesc_C > 1 

Select  	L.* ,eq.MSN_SECID,isnull(c.quote_return1Week,0 ) as quote_return1Week,isnull(c.quote_return1Month,0) as quote_return1Month,isnull(c.quote_return3Month,0) as quote_return3Month,c.estimate_recommendation,isnull(c.estimate_numberOfAnalysts,0) as estimate_numberOfAnalysts,c.beta,c.keyMetrics_eps,c.estimate_meanPriceTarget,c.estimate_highPriceTarget,
eq.isprime as isfavorite,c.quote_return6Month,c.quote_return1Year,c.quote_returnYTD,

isnull(c.quote_priceChange,0 ) quote_priceChange,
isnull(c.quote_priceChange1Week,0 ) quote_priceChange1Week,
isnull(c.quote_priceChange1Month,0 ) quote_priceChange1Month,
isnull(c.quote_priceChange3Month,0 ) quote_priceChange3Month,
isnull(c.quote_priceChange6Month,0 ) quote_priceChange6Month,
isnull(c.quote_priceChange1Year,0 ) quote_priceChange1Year,
isnull(c.quote_priceChangeYTD,0 ) quote_priceChangeYTD,
isnull(c.quote_price52wHigh,0 ) quote_price52wHigh,
isnull(c.quote_price52wLow,0) quote_price52wLow,
isnull(eq.IsEnabledForAutoTrade,0) as isAutoTrad,
ISNULL(N.IsFavoriteAdded,0) as IsFavoriteAdded,
ISNULL(N.IsFavoriteRemoved,0) as IsFavoriteRemoved,
ISNULL(cn.Price,-1)as buyAt,
ISNULL(eq.SecurityId,l.stock_name) as securityId,
ISNULL(cn.[Change],'')as buyAtChange,

ISNULL(eq.TDays,'') as TDay,

ISNULL(eq.watchlist,'') as watchlist,
PR.[change] as pr_change,
PR.[close] as  pr_close,
PR.[open] as pr_open,
PR.[VolumeC] as  pr_volume,
 pr.ltt as pr_date,isnull(ex.BearishCount,0) BearishCount, isnull(ex.BulishCount,0) BulishCount,ex.match,   
 isnull(aw.OrderCount,0) as AwardCount
into #tempresuls

from [dbo].[Live_Stocks] l with (Nolock) 
left JOIN Equitys  eq ON eq.Symbol= l.Symbol
left join MMSN_Companies c on c.MSN_SECID=eq.MSN_SECID
left join StockPriceConfig cn on cn.Symbol=l.symbol
left join MSN_Equities_Notification N on N.MSN_SECID=eq.MSN_SECID and CAST(N.Created_On as date)=CAST(@Date as Date)
left join #previousData PR on PR.symbol=eq.Symbol
left join Ticker_Stocks_Histry_Extended ex on ex.symbol=L.symbol and  CAST(ex.Ltt as Date)=cast(@DatefromEx as date) 
left join Stock_Award AW on  AW.symbol=eq.Symbol
where -- l.symbol='1.1!544026' and --l. symbol='1.1!500009' and -- EQ.recommondations in ('strongBuy','buy')  and 
cast(last as decimal(10,2) )< 2000 and CAST(c.Created_On as Date)=cast(@Date as date)  
--and ex.BulishCount > 0

 and (@TDays is null  or @TDays ='' or @TDays =eq.TDays) 

and (@WatchList is null  or @WatchList ='' or @WatchList =eq.WatchList)

-- and cn.[Change] is not null



and (@isfavorite=0 or eq.isprime=@isfavorite)
and (@isAutoTrade=0 or eq.IsEnabledForAutoTrade=@isAutoTrade)
order by change desc
if(@ShowNotification=1)
begin
select * from #tempresuls where IsFavoriteAdded=1 or IsFavoriteRemoved=1 order by change desc
End

if(@minvalue > 0 and @maxvalue > 0)
select * from #tempresuls where [open] between @minvalue and @maxvalue  order by change desc

if(@minvalue = 0 and @maxvalue > 0)
select * from #tempresuls where [open] <= @maxvalue order by change desc

if(@minvalue > 0 and @maxvalue = 0)
select * from #tempresuls where [open] >= @minvalue order by change desc
else
Select * from #tempresuls order by change desc
end
else
Select top 1 L.*,eq.MSN_SECID from [dbo].[Live_Stocks] L with (Nolock) 
left JOIN Equitys  eq ON eq.Symbol= l.Symbol

where l.symbol=@Code and CAST(l.ltt as Date)=cast(GETDATE() as date) 

--and eq.ISPSU=1 order by change desc--and EQ.recommondations in ('strongBuy','buy') and [open] < 150
--where E.symbol ='1.1!516092'
--WHERE EQ.recommondations in ('strongBuy')--,'buy')  --and [last] < 500
end 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_CHART]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_CHART](@Code varchar(50)='')
as
begin 
if(@Code is null or @Code='' )
begin 
Select  L.* ,eq.MSN_SECID,eq.Symbol  from Equitys eq  with (Nolock) 
left JOIN [dbo].Ticker_Stocks_Histry  l  ON eq.Symbol= l.Symbol and CAST(ltt as Date)=cast(GETDATE() as date) 
--where  EQ.recommondations in ('strongBuy','buy') --and last < 10000

end
else
Select top 1 L.*,eq.MSN_SECID   from  [dbo].[Live_Stocks] L with (Nolock) 
left JOIN Equitys  eq ON eq.Symbol= l.Symbol
where l.symbol=@Code and CAST(ltt as Date)=cast(GETDATE() as date) 
and eq.ISPSU=1--and EQ.recommondations in ('strongBuy','buy') and [open] < 150
--where E.symbol ='1.1!516092'
--WHERE EQ.recommondations in ('strongBuy')--,'buy')  --and [last] < 500
end 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_LOAD]    Script Date: 05-02-2024 21:23:23 ******/
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
 where e.isactive=1  -- and E.Symbol='1.1!544026'


--[dbo].[Live_Stocks] with (Nolock) where  
--[open] < 250 order by ltt desc 

end
else
Select top 1 *  from [dbo].[Live_Stocks] with (Nolock) where symbol=@Code and CAST(ltt as Date)=cast(GETDATE() as date) and [open] < 150 order by ltt desc 
end 


GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_New]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create  Procedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_New](@Code varchar(50)='',@isfavorite bit =0)
as
begin 
if(@Code is null or @Code='' )
begin 
Declare @Date DateTime;

select @Date=MAX(Created_On) from MMSN_Companies;
Select L.* ,eq.MSN_SECID,isnull(c.quote_return1Week,0 ) as quote_return1Week,isnull(c.quote_return1Month,0) as quote_return1Month,isnull(c.quote_return3Month,0) as quote_return3Month,c.estimate_recommendation,isnull(c.estimate_numberOfAnalysts,0) as estimate_numberOfAnalysts,c.beta,c.keyMetrics_eps,c.estimate_meanPriceTarget,c.estimate_highPriceTarget,
eq.isprime as isfavorite from [dbo].[Live_Stocks] l with (Nolock) 
left JOIN Equitys  eq ON eq.Symbol= l.Symbol
left join MMSN_Companies c on c.MSN_SECID=eq.MSN_SECID 

where -- EQ.recommondations in ('strongBuy','buy')  and 
last < 2000 and CAST(c.Created_On as Date)=cast(@Date as date) 

and (@isfavorite=0 or eq.isprime=@isfavorite)
order by change desc
end
else
Select top 1 L.*,eq.MSN_SECID   from [dbo].[Live_Stocks] L with (Nolock) 
left JOIN Equitys  eq ON eq.Symbol= l.Symbol
where l.symbol=@Code and CAST(ltt as Date)=cast(GETDATE() as date) 
and eq.ISPSU=1--and EQ.recommondations in ('strongBuy','buy') and [open] < 150
--where E.symbol ='1.1!516092'
--WHERE EQ.recommondations in ('strongBuy')--,'buy')  --and [last] < 500
end 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_LIVE_STOCKS_BY_STOCK_Test]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GET_SectorName]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE Procedure [dbo].[SP_GET_SectorName]
  as begin 
  Select distinct  [SectorName] as Text from  [STOCK].[dbo].[Equitys] where SectorName <> '-'
  End 
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_SubgroupName_By]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  Procedure [dbo].[SP_GET_SubgroupName_By](@groupName varchar(100))
  as begin 
  Select distinct  [ISubgroupName] as Text  from  [STOCK].[dbo].[Equitys] where  IgroupName  in  (SELECT id FROM [dbo].[ufnSplit](@groupName))
  End 
 
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTopPerformer]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--SP_GetTopPerformer 10,'2023-08-18'


CREATE Proc [dbo].[SP_GetTopPerformer](@top int ,@Date DateTime)
as
Begin
Select distinct top (@top)  stock_name,max(cast(ttv as decimal)) as Volume,MIN(last) as min_last,MAX(last) as max_last,AVG(last) as avg,MAX([open]) as [Open],MIN(cast(change as decimal(10,2))) as min_change,MAX(cast(change as decimal(10,2))) as max_change,

MIN(cast(bPrice as decimal(10,2))) as min_bPrice,MAX(cast(bPrice as decimal(10,2))) as max_bPrice,MIN(cast(sPrice as decimal(10,2))) as min_sPrice,MAX(cast(sPrice as decimal(10,2))) as max_sPrice,symbol,

AVG(cast(bPrice as decimal(10,2))) as bPrice,AVG(cast(sPrice as decimal(10,2))) as sPrice


from dbo.Ticker_Stocks nolock where CAST(ltt as Date)=cast(@Date as Date) 
group by stock_name,symbol
order by max(cast(ttv as decimal)) desc
End



					
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTopPerformer_new]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--SP_GetTopPerformer 100,'2023-09-06'


CREATE   Proc [dbo].[SP_GetTopPerformer_new](@top int ,@Date DateTime)
as
Begin

WITH Cte AS(
SELECT  *,
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM [Ticker_Stocks] with (nolock)  where CAST(ltt as Date)=cast(@Date as Date)  and [open] >0 )

	SELECT  top (@top)  [symbol]
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
	order by (cast(ttv as decimal)) desc

--Select distinct top (@top)  stock_name,max(cast(ttv as decimal)) as Volume,MIN(last) as min_last,MAX(last) as max_last,AVG(last) as avg,MAX([open]) as [Open],MIN(cast(change as decimal(10,2))) as min_change,MAX(cast(change as decimal(10,2))) as max_change,

--MIN(cast(bPrice as decimal(10,2))) as min_bPrice,MAX(cast(bPrice as decimal(10,2))) as max_bPrice,MIN(cast(sPrice as decimal(10,2))) as min_sPrice,MAX(cast(sPrice as decimal(10,2))) as max_sPrice,symbol,

--AVG(cast(bPrice as decimal(10,2))) as bPrice,AVG(cast(sPrice as decimal(10,2))) as sPrice


--from dbo.Ticker_Stocks nolock where CAST(ltt as Date)=cast(@Date as Date) 
--group by stock_name,symbol
--order by max(cast(ttv as decimal)) desc
End



					
GO
/****** Object:  StoredProcedure [dbo].[TablesFromJSON]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  StoredProcedure [dbo].[Ticker_Current]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  StoredProcedure [dbo].[Ticker_today]    Script Date: 05-02-2024 21:23:23 ******/
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
/****** Object:  StoredProcedure [dbo].[Ticker_yesterday]    Script Date: 05-02-2024 21:23:23 ******/
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
    FROM Ticker_Stocks_Histry   where CAST(ltt as Date)  >=cast( @Date as Date)) -- >= cast(@previousDate as date))

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
/****** Object:  StoredProcedure [dbo].[TruncatloadExtended]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[TruncatloadExtended] 
as
begin

truncate table  [STOCK].[dbo].[Ticker_Stocks_Histry_Extended]


;WITH Cte AS(
SELECT *,
        RnAsc_C = ROW_NUMBER() OVER(PARTITION BY symbol, CAST(ltt as Date) ORDER BY ltt),
        RnDesc_C = ROW_NUMBER() OVER(PARTITION BY  symbol,CAST(ltt as Date) ORDER BY ltt DESC)
    FROM Ticker_Stocks_Histry_Extended_Ticks with (nolock)    where CAST(ltt as Date )=  CAST(GETDATE() as Date) )
	insert into [STOCK].[dbo].[Ticker_Stocks_Histry_Extended]
	Select symbol,BearishCount,BulishCount,Ltt,Match  from  Cte where RnDesc_C = 1 
End
--	sELECT L.stock_name ,* FROM Ticker_Stocks_Histry_Extended_Ticks  t
--LEFT JOIN Live_Stocks L ON L.symbol =t.symbol ORDER BY t.BulishCount DESC
GO
/****** Object:  StoredProcedure [dbo].[UpdateNotiifcation]    Script Date: 05-02-2024 21:23:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[UpdateNotiifcation](@ids varchar(50))
as

begin
Update STOCK_NTFCTN set isNotified=1 where Id in ( (SELECT id FROM [dbo].[ufnSplit](@ids))) 
end
GO
