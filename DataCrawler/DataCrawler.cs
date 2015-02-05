using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public abstract class DataCrawler
    {
        abstract public void PullData(DateTime start, DateTime End, Func<string, DateTime> dataHandler);
    }
}
