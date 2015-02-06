using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public abstract class DceDataCrawler :DataCrawler
    {
        public DceDataCrawler()
        {
            ContentEncoding = Encoding.GetEncoding("GB2312");
        }
    }
}
