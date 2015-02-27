
DROP TABLE [dbo].[TransactionFlatTable]
IF OBJECT_ID(N'[dbo].[TransactionFlatTable]') IS NULL
  BEGIN
    CREATE TABLE [dbo].[TransactionFlatTable]
	(
		[Id] [nvarchar](128) NOT NULL PRIMARY KEY CLUSTERED,
		[ReturnRate_1] float,
		[ReturnRate_5] float,
		[ReturnRate_10] float,
		[Delta_High_Open] float,
		[Delta_Low_Open] float,
		[Delta_High_Close] float,
		[Delta_Low_Close] float,
		[Delta_Volume] float,
		[Delta_Volume_5] float,
		[Delta_Volume_10] float,
		[Delta_Volume_20] float,
		[Delta_Position] float,
		[Delta_Position_5] float,
		[Delta_Position_10] float,
		[Delta_Position_20] float,
		[Delta_ClosePrice] float,
		[Delta_ClosePrice_5] float,
		[Delta_ClosePrice_10] float,
		[Delta_ClosePrice_20] float,
		[Delta_SettlePrice] float,
		[Delta_SettlePrice_5] float,
		[Delta_SettlePrice_10] float,
		[Delta_SettlePrice_20] float,
		[Delta_CloseSettlePrice_5] float,
		[Delta_CloseSettlePrice_10] float,
		[Delta_CloseSettlePrice_20] float
	)

  END;

GO

INSERT INTO [dbo].[TransactionFlatTable]

	SELECT CONCAT('_N_', CAST(a.[QuoteDate] AS Varchar(12)), '_', a.[Commodity]) AS Id, 
		a.[ReturnRate_1],
		a.[ReturnRate_5],
		a.[ReturnRate_10],
		(a.[HighPrice]-a.[OpenPrice])/a.[OpenPrice] AS Delta_High_Open,
		(a.[LowPrice]-a.[OpenPrice])/a.[OpenPrice] AS Delta_Low_Open,
		(a.[HighPrice]-a.[ClosePrice])/a.[ClosePrice] AS Delta_High_Close,
		(a.[LowPrice]-a.[ClosePrice])/a.[ClosePrice] AS Delta_Low_Close,
		(a.[Volume]-a.[LastVolume])/a.[LastVolume] AS Delta_Volume,
		(a.[Volume]-a.[LastVolume_5])/a.[LastVolume_5] AS Delta_Volume_5,
		(a.[Volume]-a.[LastVolume_10])/a.[LastVolume_10] AS Delta_Volume_10,
		(a.[Volume]-a.[LastVolume_20])/a.[LastVolume_20] AS Delta_Volume_20,
		(a.[Position]-a.[LastPosition])/a.[LastPosition] AS Delta_Position,
		(a.[Position]-a.[LastPosition_5])/a.[LastPosition_5] AS Delta_Position_5,
		(a.[Position]-a.[LastPosition_10])/a.[LastPosition_10] AS Delta_Position_10,
		(a.[Position]-a.[LastPosition_20])/a.[LastPosition_20] AS Delta_Position_20,
		(a.[ClosePrice]-a.[LastClosePrice])/a.[LastClosePrice] AS Delta_ClosePrice,
		(a.[ClosePrice]-a.[AvgClosePrice_5])/a.[AvgClosePrice_5] AS Delta_ClosePrice_5,
		(a.[ClosePrice]-a.[AvgClosePrice_10])/a.[AvgClosePrice_10] AS Delta_ClosePrice_10,
		(a.[ClosePrice]-a.[AvgClosePrice_20])/a.[AvgClosePrice_20] AS Delta_ClosePrice_20,
		(a.[SettlePrice]-a.[LastSettlePrice])/a.[LastSettlePrice] AS Delta_SettlePrice,
		(a.[SettlePrice]-a.[AvgSettlePrice_5])/a.[AvgSettlePrice_5] AS Delta_SettlePrice_5,
		(a.[SettlePrice]-a.[AvgSettlePrice_10])/a.[AvgSettlePrice_10] AS Delta_SettlePrice_10,
		(a.[SettlePrice]-a.[AvgSettlePrice_20])/a.[AvgSettlePrice_20] AS Delta_SettlePrice_20,
		(a.[ClosePrice]-a.[AvgSettlePrice_5])/a.[AvgSettlePrice_5] AS Delta_CloseSettlePrice_5,
		(a.[ClosePrice]-a.[AvgSettlePrice_10])/a.[AvgSettlePrice_10] AS Delta_CloseSettlePrice_10,
		(a.[ClosePrice]-a.[AvgSettlePrice_20])/a.[AvgSettlePrice_20] AS Delta_CloseSettlePrice_20
	FROM [dbo].[TmpTransactionInfo] a
	INNER JOIN
	  (SELECT QuoteDate, Commodity, Max(Volume) AS dominate_volume FROM [dbo].[TmpTransactionInfo]
	   GROUP BY QuoteDate, Commodity) b
	ON a.QuoteDate = b.QuoteDate and a.Commodity=b.Commodity and a.Volume = b.dominate_volume
	WHERE b.dominate_volume > 15000



