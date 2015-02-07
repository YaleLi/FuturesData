using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDataCrawler
{
    public class CzceDealerPositionCrawler :CzceDataCrawler
    {
        protected override Uri BuildUrl(DateTime transactionDate)
        {
            string url = ConfigurationManager.AppSettings["Czce.Dealer.Position.Url"];

            return BuildUrl(url, transactionDate);
        }
    }
}
