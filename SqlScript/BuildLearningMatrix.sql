select CONCAT('_N_', CAST(a.[QuoteDate] AS Varchar(12)), '_', a.[Commodity]) AS Id, 
	a.[ReturnRate_1],
	a.[ReturnRate_5],
	a.[ReturnRate_10],
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
	(a.[SettlePrice]-a.[AvgSettlePrice_20])/a.[AvgSettlePrice_20] AS Delta_SettlePrice_20

from [dbo].[TmpTransactionInfo] a
inner join
  (select QuoteDate, Commodity, Max(Volume) as dominate_volume from [dbo].[TmpTransactionInfo]
   group by QuoteDate, Commodity) b
   on a.QuoteDate = b.QuoteDate and a.Commodity=b.Commodity and a.Volume = b.dominate_volume
where b.dominate_volume > 15000
order by  a.Commodity, a.QuoteDate


