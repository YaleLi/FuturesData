using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public class DceDealerPositionCrawler :DceDataCrawler
    {
        public string Commodity { get; private set; }
        public string Month { get; private set; }

        public DceDealerPositionCrawler(string commdity, string month) : base()
        {
            Commodity = string.IsNullOrEmpty(commdity) ? "" : commdity.Trim();
            Month = string.IsNullOrEmpty(month) ? "" : month.Trim();
        }
        protected override string BuildUrl(DateTime date)
        {
            string url = string.IsNullOrEmpty(Month) ? url = ConfigurationManager.AppSettings["Dce.Dealer.Position.Commodity.Url"] 
                                                     : ConfigurationManager.AppSettings["Dce.Dealer.Position.Contract.Url"];

            url = url.Replace("[DATE]", date.ToString("yyyyMMdd"));
            url = url.Replace("[VARIETY]", Commodity);

            if (!string.IsNullOrEmpty(Month))
            {
                url = url.Replace("[CONTRACT]", Commodity + Month);
            }

            return url;
        }
    }
}
