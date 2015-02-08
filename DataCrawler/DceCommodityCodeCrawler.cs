using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuturesDataCrawler;

namespace DataCrawler
{
    public class DceCommodityCodeCrawler : DceDataCrawler
    {
        protected override Uri BuildUrl(DateTime transactionDate)
        {
            string url = ConfigurationManager.AppSettings["Dce.Commodity.Code.Url"];
            return new Uri(url);
        }
    }
}
