namespace DataType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContractTransactionFeature
    {
        public string Id { get; set; }

        public int Volume { get; set; }
        public double? ReturnRate1 { get; set; }

        public double? ReturnRate5 { get; set; }

        public double? ReturnRate10 { get; set; }

        public double? DeltaHighOpen { get; set; }

        public double? DeltaLowOpen { get; set; }

        public double? DeltaHighClose { get; set; }

        public double? DeltaLowClose { get; set; }

        public double? DeltaVolume { get; set; }

        public double? DeltaVolume5 { get; set; }

        public double? DeltaVolume10 { get; set; }

        public double? DeltaVolume20 { get; set; }

        public double? DeltaPosition { get; set; }

        public double? DeltaPosition5 { get; set; }

        public double? DeltaPosition10 { get; set; }

        public double? DeltaPosition20 { get; set; }

        public double? DeltaClosePrice { get; set; }

        public double? DeltaClosePrice5 { get; set; }

        public double? DeltaClosePrice10 { get; set; }

        public double? DeltaClosePrice20 { get; set; }

        public double? DeltaSettlePrice { get; set; }

        public double? DeltaSettlePrice5 { get; set; }

        public double? DeltaSettlePrice10 { get; set; }

        public double? DeltaSettlePrice20 { get; set; }

        public double? DeltaCloseSettlePrice5 { get; set; }

        public double? DeltaCloseSettlePrice10 { get; set; }

        public double? DeltaCloseSettlePrice20 { get; set; }
    }
}
