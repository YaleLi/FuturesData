using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public class ShfeDealerPositionCrawler : ShfeDataCrawler
    {
        protected override string BuildUrl(DateTime date)
        {
            string url = ConfigurationManager.AppSettings["Shfe.Dealer.Position.Url"];

            return url.Replace("[DATE]", date.ToString("yyyyMMdd"));
        }
    }
}
