using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCrawler
{
    public class DataCrawler
    {
        public void PullData(DateTime start, DateTime end, Func<string, DateTime> dataHandler)
        {
            DateTime endTime = end;
            if(end.Hour < 18)
            {

            }
            if(end.CompareTo(start) < 0)
            {
                return;
            }

            if(null != dataHandler)
            {
                dataHandler()
            }
        }
    }
}
