namespace DataType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DealerPositionFeature
    {
        public string Id { get; set; }

        public int? Volume { get; set; }

        public double? TopBuy5Delta1 { get; set; }

        public double? TopBuy5Delta3 { get; set; }

        public double? TopBuy5Delta10 { get; set; }

        public double? TopBuy20Delta1 { get; set; }

        public double? TopBuy20Delta3 { get; set; }

        public double? TopBuy20Delta10 { get; set; }
        public double? TopSell5Delta1 { get; set; }

        public double? TopSell5Delta3 { get; set; }

        public double? TopSell5Delta10 { get; set; }

        public double? TopSell20Delta1 { get; set; }

        public double? TopSell20Delta3 { get; set; }

        public double? TopSell20Delta10 { get; set; }
    }
}
