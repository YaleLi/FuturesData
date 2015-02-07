using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDataCrawler
{
    public class ShfeDealerPositionCrawler : ShfeDataCrawler
    {
        protected override Uri BuildUrl(DateTime transactionDate)
        {
            string url = ConfigurationManager.AppSettings["Shfe.Dealer.Position.Url"];

            return new Uri(url.Replace("[DATE]", transactionDate.ToString(DateFormat, DateFormatterProvider)));
        }
    }
}
