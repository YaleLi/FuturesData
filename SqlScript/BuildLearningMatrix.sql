
--DROP TABLE [dbo].[ContractTransactionFeatures]
IF OBJECT_ID(N'[dbo].[ContractTransactionFeatures]') IS NULL
  BEGIN
    CREATE TABLE [dbo].[ContractTransactionFeatures]
	(
		[Id] [nvarchar](128) NOT NULL PRIMARY KEY CLUSTERED,
		[Volume] int,
		[ReturnRate1] float,
		[ReturnRate5] float,
		[ReturnRate10] float,
		[DeltaHighOpen] float,
		[DeltaLowOpen] float,
		[DeltaHighClose] float,
		[DeltaLowClose] float,
		[DeltaVolume] float,
		[DeltaVolume5] float,
		[DeltaVolume10] float,
		[DeltaVolume20] float,
		[DeltaPosition] float,
		[DeltaPosition5] float,
		[DeltaPosition10] float,
		[DeltaPosition20] float,
		[DeltaClosePrice] float,
		[DeltaClosePrice5] float,
		[DeltaClosePrice10] float,
		[DeltaClosePrice20] float,
		[DeltaSettlePrice] float,
		[DeltaSettlePrice5] float,
		[DeltaSettlePrice10] float,
		[DeltaSettlePrice20] float,
		[DeltaCloseSettlePrice5] float,
		[DeltaCloseSettlePrice10] float,
		[DeltaCloseSettlePrice20] float
	)
  END;


IF OBJECT_ID(N'[dbo].[DealerPositionFeatures]') IS NULL
  BEGIN
  	CREATE TABLE [dbo].[DealerPositionFeatures]
	(
		[Id] [nvarchar](128) NOT NULL PRIMARY KEY CLUSTERED,
		[Volume] int,
		[TopBuy5Delta1] float,
		[TopBuy5Delta3] float,
		[TopBuy5Delta10] float,
		[TopBuy20Delta1] float,
		[TopBuy20Delta3] float,
		[TopBuy20Delta10] float,
		[TopSell5Delta1] float,
		[TopSell5Delta3] float,
		[TopSell5Delta10] float,
		[TopSell20Delta1] float,
		[TopSell20Delta3] float,
		[TopSell20Delta10] float
	)

  END;

GO

INSERT INTO [dbo].[ContractTransactionFeatures]

	SELECT CONCAT('_N_', CAST(a.[QuoteDate] AS Varchar(12)), '_', a.[Commodity]) AS Id, 
		a.[ReturnRate_1],
		a.[ReturnRate_5],
		a.[ReturnRate_10],
		(a.[HighPrice]-a.[OpenPrice])/a.[OpenPrice] AS [DeltaHighOpen],
		(a.[LowPrice]-a.[OpenPrice])/a.[OpenPrice] AS DeltaLowOpen,
		(a.[HighPrice]-a.[ClosePrice])/a.[ClosePrice] AS DeltaHighClose,
		(a.[LowPrice]-a.[ClosePrice])/a.[ClosePrice] AS DeltaLowClose,
		(a.[Volume]-a.[LastVolume])/a.[LastVolume] AS DeltaVolume,
		(a.[Volume]-a.[LastVolume_5])/a.[LastVolume_5] AS DeltaVolume5,
		(a.[Volume]-a.[LastVolume_10])/a.[LastVolume_10] AS DeltaVolume10,
		(a.[Volume]-a.[LastVolume_20])/a.[LastVolume_20] AS DeltaVolume20,
		(a.[Position]-a.[LastPosition])/a.[LastPosition] AS DeltaPosition,
		(a.[Position]-a.[LastPosition_5])/a.[LastPosition_5] AS DeltaPosition5,
		(a.[Position]-a.[LastPosition_10])/a.[LastPosition_10] AS DeltaPosition10,
		(a.[Position]-a.[LastPosition_20])/a.[LastPosition_20] AS DeltaPosition20,
		(a.[ClosePrice]-a.[LastClosePrice])/a.[LastClosePrice] AS DeltaClosePrice,
		(a.[ClosePrice]-a.[AvgClosePrice_5])/a.[AvgClosePrice_5] AS DeltaClosePrice5,
		(a.[ClosePrice]-a.[AvgClosePrice_10])/a.[AvgClosePrice_10] AS DeltaClosePrice10,
		(a.[ClosePrice]-a.[AvgClosePrice_20])/a.[AvgClosePrice_20] AS DeltaClosePrice20,
		(a.[SettlePrice]-a.[LastSettlePrice])/a.[LastSettlePrice] AS DeltaSettlePrice,
		(a.[SettlePrice]-a.[AvgSettlePrice_5])/a.[AvgSettlePrice_5] AS DeltaSettlePrice5,
		(a.[SettlePrice]-a.[AvgSettlePrice_10])/a.[AvgSettlePrice_10] AS DeltaSettlePrice10,
		(a.[SettlePrice]-a.[AvgSettlePrice_20])/a.[AvgSettlePrice_20] AS DeltaSettlePrice20,
		(a.[ClosePrice]-a.[AvgSettlePrice_5])/a.[AvgSettlePrice_5] AS DeltaCloseSettlePrice5,
		(a.[ClosePrice]-a.[AvgSettlePrice_10])/a.[AvgSettlePrice_10] AS DeltaCloseSettlePrice10,
		(a.[ClosePrice]-a.[AvgSettlePrice_20])/a.[AvgSettlePrice_20] AS DeltaCloseSettlePrice20
	FROM [dbo].[TmpTransactionInfo] a
	INNER JOIN
	  (SELECT QuoteDate, Commodity, Max(Volume) AS dominate_volume FROM [dbo].[TmpTransactionInfo]
	   GROUP BY QuoteDate, Commodity) b
	ON a.QuoteDate = b.QuoteDate and a.Commodity=b.Commodity and a.Volume = b.dominate_volume
	WHERE b.dominate_volume > 15000



SELECT * FROM [dbo].[ContractTransactionFeatures]
  WHERE [ReturnRate1] IS NOT NULL
  and [ReturnRate5] IS NOT NULL
  and [ReturnRate10] IS NOT NULL
  and [DeltaVolume] IS NOT NULL
  and [DeltaVolume5]  IS NOT NULL
  and [DeltaVolume10]  IS NOT NULL
  and [DeltaVolume20]  IS NOT NULL
  and [DeltaPosition]  IS NOT NULL
  and [DeltaPosition5]  IS NOT NULL
  and [DeltaPosition10]  IS NOT NULL
  and [DeltaPosition20]  IS NOT NULL
  and [DeltaClosePrice]  IS NOT NULL
  and [DeltaClosePrice5]  IS NOT NULL
  and [DeltaClosePrice10]  IS NOT NULL
  and [DeltaClosePrice20]  IS NOT NULL
  and [DeltaSettlePrice]  IS NOT NULL
  and [DeltaSettlePrice5]  IS NOT NULL
  and [DeltaSettlePrice10]  IS NOT NULL
  and [DeltaSettlePrice20]  IS NOT NULL
  and [DeltaCloseSettlePrice5]  IS NOT NULL
  and [DeltaCloseSettlePrice10]  IS NOT NULL
  and [DeltaCloseSettlePrice20] IS NOT NULL
