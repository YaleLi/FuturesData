using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuturesDataCrawler
{
    public abstract class DceDataCrawler :DataCrawler
    {
        protected DceDataCrawler()
        {
            ContentEncoding = Encoding.GetEncoding("GB2312");
        }
    }
}
