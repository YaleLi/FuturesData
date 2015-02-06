using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public class CzceDealerPositionCrawler :CzceDataCrawler
    {
        protected override string BuildUrl(DateTime date)
        {
            string url = ConfigurationManager.AppSettings["Czce.Dealer.Position.Url"];

            return BuildUrl(url, date);
        }
    }
}
