

namespace DataCrawler
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using Utility;

    public abstract class DataCrawler
    {
        protected Encoding ContentEncoding { get; set; }
        public ILogger RuntimeLogger { get; set; }
        abstract protected string BuildUrl(DateTime date);
        protected string PullData(DateTime date, string url)
        {
            if(string.IsNullOrEmpty(url))
            {
                return "";
            }

            string content = null;
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                var response = (HttpWebResponse) request.GetResponse();
                //var receiveStream = response.GetResponseStream();
                var reader = new StreamReader(response.GetResponseStream(), ContentEncoding);
                content = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                if (null != RuntimeLogger)
                {
                    RuntimeLogger.Log(e.GetType().ToString() + "===" + e.Message + ": " + url);
                }
            }

            return content;
        }

        public void PullData(DateTime start, DateTime end, Action<string, DateTime> dataHandler)
        {
            DateTime endTime = end;
            int publishTime = Int32.Parse(ConfigurationManager.AppSettings["DataPublishTime"], NumberStyles.Any);
            if (endTime.Date.Equals(DateTime.Now.Date) && endTime.Hour<publishTime)
            {
                endTime = endTime.AddDays(-1);
            }
            if(endTime.CompareTo(start) < 0)
            {
                return;
            }

            DateTime date = endTime;
            string content = "";
            while(date.CompareTo(start) >= 0)
            {
                if (date.DayOfWeek== DayOfWeek.Saturday || date.DayOfWeek==DayOfWeek.Sunday)
                {
                    continue;
                }
                content = PullData(date, BuildUrl(date));
                if (null != dataHandler && !string.IsNullOrEmpty(content))
                {
                    dataHandler(content, date);
                }
                date = date.AddDays(-1);
            }

        }
    }
}
