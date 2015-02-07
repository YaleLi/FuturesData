using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDataCrawler
{
    public class DceDealerPositionCrawler :DceDataCrawler
    {
        public string Commodity { get; private set; }
        public string Month { get; private set; }

        public DceDealerPositionCrawler(string commodity, string month) : base()
        {
            Commodity = string.IsNullOrEmpty(commodity) ? "" : commodity.Trim();
            Month = string.IsNullOrEmpty(month) ? "" : month.Trim();
        }
        protected override Uri BuildUrl(DateTime transactionDate)
        {
            string url = string.IsNullOrEmpty(Month) ? url = ConfigurationManager.AppSettings["Dce.Dealer.Position.Commodity.Url"] 
                                                     : ConfigurationManager.AppSettings["Dce.Dealer.Position.Contract.Url"];

            url = url.Replace("[DATE]", transactionDate.ToString(DateFormat, DateFormatterProvider));
            url = url.Replace("[VARIETY]", Commodity);

            if (!string.IsNullOrEmpty(Month))
            {
                url = url.Replace("[CONTRACT]", Commodity + Month);
            }

            return new Uri(url);
        }
    }
}
