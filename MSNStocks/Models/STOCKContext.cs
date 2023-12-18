using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MSNStocks.Models
{
    public partial class STOCKContext : DbContext
    {
        public STOCKContext()
        {
        }

        public STOCKContext(DbContextOptions<STOCKContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bustockback> Bustockbacks { get; set; } = null!;
        public virtual DbSet<BuyStock> BuyStocks { get; set; } = null!;
       // public virtual DbSet<Equity> Equities { get; set; } = null!;
        public virtual DbSet<Equity> Equitys { get; set; } = null!;
        public virtual DbSet<LiveStock> LiveStocks { get; set; } = null!;
        public virtual DbSet<TempPivot> TempPivots { get; set; } = null!;
        public virtual DbSet<TempPivotResult> TempPivotResults { get; set; } = null!;
        public virtual DbSet<TickerStock> TickerStocks { get; set; } = null!;
        public virtual DbSet<TickerStocksDay> TickerStocksDays { get; set; } = null!;
        public virtual DbSet<TickerStocksHistry> TickerStocksHistries { get; set; } = null!;
        public virtual DbSet<TickerStocksYesterday> TickerStocksYesterdays { get; set; } = null!;
        public virtual DbSet<TodaysRatio> TodaysRatios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=HAADVISRI\\AGS;Initial Catalog=STOCK;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bustockback>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bustockback");

                entity.Property(e => e.AvgPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("avgPrice");

                entity.Property(e => e.BPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bPrice");

                entity.Property(e => e.Change)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("change");

                entity.Property(e => e.Close)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("close");

                entity.Property(e => e.High)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("high");

                entity.Property(e => e.Last)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("last");

                entity.Property(e => e.Low)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("low");

                entity.Property(e => e.Ltt)
                    .HasColumnType("datetime")
                    .HasColumnName("ltt");

                entity.Property(e => e.Open)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("open");

                entity.Property(e => e.Prev)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("prev");

                entity.Property(e => e.Ratio).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sPrice");

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("stock_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e.Ttv)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ttv");

                entity.Property(e => e.VolumeC)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("volumeC");
            });

            modelBuilder.Entity<BuyStock>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BuyStock");

                entity.HasIndex(e => e.Ltt, "ClusteredIndex-20230829-213820")
                    .IsClustered();

                entity.Property(e => e.AvgPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("avgPrice");

                entity.Property(e => e.BPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bPrice");

                entity.Property(e => e.Change)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("change");

                entity.Property(e => e.Close)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("close");

                entity.Property(e => e.High)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("high");

                entity.Property(e => e.Last)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("last");

                entity.Property(e => e.Low)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("low");

                entity.Property(e => e.Ltt)
                    .HasColumnType("datetime")
                    .HasColumnName("ltt");

                entity.Property(e => e.Open)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("open");

                entity.Property(e => e.Prev)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("prev");

                entity.Property(e => e.Ratio).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sPrice");

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("stock_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e.Ttv)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ttv");

                entity.Property(e => e.VolumeC)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("volumeC");
            });

           

            modelBuilder.Entity<Equity>(entity =>
            {
                entity.Property(e => e.Group).HasMaxLength(255);

                entity.Property(e => e.IgroupName).HasMaxLength(255);

                entity.Property(e => e.Industry).HasMaxLength(255);

                entity.Property(e => e.IndustryNewName).HasMaxLength(255);

                entity.Property(e => e.Instrument).HasMaxLength(255);

                entity.Property(e => e.Isinno)
                    .HasMaxLength(255)
                    .HasColumnName("ISINNo");

                entity.Property(e => e.IssuerName).HasMaxLength(255);

                entity.Property(e => e.IsubgroupName)
                    .HasMaxLength(255)
                    .HasColumnName("ISubgroupName");

                entity.Property(e => e.Recommondations)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("recommondations");

                entity.Property(e => e.SectorName).HasMaxLength(255);

                entity.Property(e => e.SecurityId).HasMaxLength(255);

                entity.Property(e => e.SecurityName).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(255);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LiveStock>(entity =>
            {
                entity.ToTable("Live_Stocks");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AvgPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("avgPrice");

                entity.Property(e => e.BPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bPrice");

                entity.Property(e => e.BQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bQty");

                entity.Property(e => e.Change)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("change");

                entity.Property(e => e.Close)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("close");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("exchange");

                entity.Property(e => e.High)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("high");

                entity.Property(e => e.Last)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("last");

                entity.Property(e => e.Low)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("low");

                entity.Property(e => e.LowerCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("lowerCktLm");

                entity.Property(e => e.Ltq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ltq");

                entity.Property(e => e.Ltt)
                    .HasColumnType("datetime")
                    .HasColumnName("ltt");

                entity.Property(e => e.MonthMax)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Month_Max");

                entity.Property(e => e.MonthMin)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Month_min");

                entity.Property(e => e.Open)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("open");

                entity.Property(e => e.Quotes)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("quotes");

                entity.Property(e => e.SPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sPrice");

                entity.Property(e => e.SQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sQty");

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("stock_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e.ThreeMonthMax)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Three_Month_max");

                entity.Property(e => e.ThreeMonthMin)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Three_Month_min");

                entity.Property(e => e.TotalBuyQt).HasColumnName("totalBuyQt");

                entity.Property(e => e.TotalSellQ).HasColumnName("totalSellQ");

                entity.Property(e => e.Trend)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("trend");

                entity.Property(e => e.Ttq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ttq");

                entity.Property(e => e.Ttv)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("ttv");

                entity.Property(e => e.TwoWeeksMax)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("TwoWeeks_Max");

                entity.Property(e => e.TwoWeeksMin)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("TwoWeeks_Min");

                entity.Property(e => e.UpperCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("upperCktLm");

                entity.Property(e => e.WeekMax)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Week_Max");

                entity.Property(e => e.WeekMin)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Week_Min");
            });

            modelBuilder.Entity<TempPivot>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TempPivot");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("STOCK_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e.Value).HasColumnType("decimal(25, 2)");
            });

            modelBuilder.Entity<TempPivotResult>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("STOCK_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e._20230901)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("2023-09-01");

                entity.Property(e => e._20230904)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("2023-09-04");

                entity.Property(e => e._20230905)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("2023-09-05");
            });

            modelBuilder.Entity<TickerStock>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Ticker_Stocks");

                entity.HasIndex(e => e.Ltt, "NonClusteredIndex-20230829-213857");

                entity.Property(e => e.AvgPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("avgPrice");

                entity.Property(e => e.BPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bPrice");

                entity.Property(e => e.BQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bQty");

                entity.Property(e => e.Change)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("change");

                entity.Property(e => e.Close)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("close");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("exchange");

                entity.Property(e => e.High)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("high");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Last)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("last");

                entity.Property(e => e.Low)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("low");

                entity.Property(e => e.LowerCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("lowerCktLm");

                entity.Property(e => e.Ltq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ltq");

                entity.Property(e => e.Ltt)
                    .HasColumnType("datetime")
                    .HasColumnName("ltt");

                entity.Property(e => e.Open)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("open");

                entity.Property(e => e.Quotes)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("quotes");

                entity.Property(e => e.SPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sPrice");

                entity.Property(e => e.SQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sQty");

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("stock_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e.TotalBuyQt).HasColumnName("totalBuyQt");

                entity.Property(e => e.TotalSellQ).HasColumnName("totalSellQ");

                entity.Property(e => e.Trend)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("trend");

                entity.Property(e => e.Ttq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ttq");

                entity.Property(e => e.Ttv)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ttv");

                entity.Property(e => e.UpperCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("upperCktLm");

                entity.Property(e => e.VolumeC)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TickerStocksDay>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Ticker_Stocks_Days");

                entity.Property(e => e.AvgPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("avgPrice");

                entity.Property(e => e.BPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bPrice");

                entity.Property(e => e.BQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bQty");

                entity.Property(e => e.Change)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("change");

                entity.Property(e => e.Close)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("close");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("exchange");

                entity.Property(e => e.High)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("high");

                entity.Property(e => e.Last)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("last");

                entity.Property(e => e.Low)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("low");

                entity.Property(e => e.LowerCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("lowerCktLm");

                entity.Property(e => e.Ltq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ltq");

                entity.Property(e => e.Ltt)
                    .HasColumnType("datetime")
                    .HasColumnName("ltt");

                entity.Property(e => e.Open)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("open");

                entity.Property(e => e.Quotes)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("quotes");

                entity.Property(e => e.SPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sPrice");

                entity.Property(e => e.SQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sQty");

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("stock_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e.TotalBuyQt).HasColumnName("totalBuyQt");

                entity.Property(e => e.TotalSellQ).HasColumnName("totalSellQ");

                entity.Property(e => e.Trend)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("trend");

                entity.Property(e => e.Ttq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ttq");

                entity.Property(e => e.Ttv)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ttv");

                entity.Property(e => e.UpperCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("upperCktLm");
            });

            modelBuilder.Entity<TickerStocksHistry>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Ticker_Stocks_Histry");

                entity.HasIndex(e => e.Ltt, "ClusteredIndex-20230829-213926")
                    .IsClustered();

                entity.Property(e => e.AvgPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("avgPrice");

                entity.Property(e => e.BPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bPrice");

                entity.Property(e => e.BQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bQty");

                entity.Property(e => e.Change)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("change");

                entity.Property(e => e.Close)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("close");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("exchange");

                entity.Property(e => e.High)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("high");

                entity.Property(e => e.Last)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("last");

                entity.Property(e => e.Low)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("low");

                entity.Property(e => e.LowerCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("lowerCktLm");

                entity.Property(e => e.Ltq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ltq");

                entity.Property(e => e.Ltt)
                    .HasColumnType("datetime")
                    .HasColumnName("ltt");

                entity.Property(e => e.Open)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("open");

                entity.Property(e => e.Quotes)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("quotes");

                entity.Property(e => e.SPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sPrice");

                entity.Property(e => e.SQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sQty");

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("stock_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e.TotalBuyQt).HasColumnName("totalBuyQt");

                entity.Property(e => e.TotalSellQ).HasColumnName("totalSellQ");

                entity.Property(e => e.Trend)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("trend");

                entity.Property(e => e.Ttq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ttq");

                entity.Property(e => e.Ttv)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ttv");

                entity.Property(e => e.UpperCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("upperCktLm");

                entity.Property(e => e.VolumeC)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TickerStocksYesterday>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Ticker_Stocks_Yesterday");

                entity.Property(e => e.AvgPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("avgPrice");

                entity.Property(e => e.BPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bPrice");

                entity.Property(e => e.BQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("bQty");

                entity.Property(e => e.Change)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("change");

                entity.Property(e => e.Close)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("close");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("exchange");

                entity.Property(e => e.High)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("high");

                entity.Property(e => e.Last)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("last");

                entity.Property(e => e.Low)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("low");

                entity.Property(e => e.LowerCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("lowerCktLm");

                entity.Property(e => e.Ltq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ltq");

                entity.Property(e => e.Ltt)
                    .HasColumnType("datetime")
                    .HasColumnName("ltt");

                entity.Property(e => e.Open)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("open");

                entity.Property(e => e.Quotes)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("quotes");

                entity.Property(e => e.SPrice)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sPrice");

                entity.Property(e => e.SQty)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("sQty");

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("stock_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e.TotalBuyQt).HasColumnName("totalBuyQt");

                entity.Property(e => e.TotalSellQ).HasColumnName("totalSellQ");

                entity.Property(e => e.Trend)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("trend");

                entity.Property(e => e.Ttq)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("ttq");

                entity.Property(e => e.Ttv)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ttv");

                entity.Property(e => e.UpperCktLm)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("upperCktLm");

                entity.Property(e => e.VolumeC)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TodaysRatio>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Close)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("close");

                entity.Property(e => e.High)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("high");

                entity.Property(e => e.Last)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("last");

                entity.Property(e => e.Ltt)
                    .HasColumnType("datetime")
                    .HasColumnName("ltt");

                entity.Property(e => e.Open)
                    .HasColumnType("decimal(20, 4)")
                    .HasColumnName("open");

                entity.Property(e => e.Prev)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("prev");

                entity.Property(e => e.Ratio).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.StockName)
                    .IsUnicode(false)
                    .HasColumnName("stock_name");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("symbol");

                entity.Property(e => e.Ttv)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ttv");

                entity.Property(e => e.VolumeC)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("volumeC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
