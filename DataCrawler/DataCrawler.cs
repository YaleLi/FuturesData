

using DataType;

namespace FuturesDataCrawler
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
        protected string DateFormat { get; set; } = GlobalDefinition.DateFormat;
        protected IFormatProvider DateFormatterProvider { get; set; } = GlobalDefinition.FormatProvider;
        protected Encoding ContentEncoding { get; set; }
        public ILogger RuntimeLogger { get; set; }
        abstract protected Uri BuildUrl(DateTime transactionDate);
        protected string PullData(Uri url)
        {
            if(null==url)
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

                if (null != RuntimeLogger)
                {
                    RuntimeLogger.Log(url.ToString()+" Downloaded");
                }
            }
            catch (WebException e)
            {
                if (null != RuntimeLogger)
                {
                    RuntimeLogger.Log(e.GetType().ToString() + "===" + e.Message + ": " + url);
                }
                content = null;
            }

            return content;
        }

        public void PullData(DateTime start, DateTime end, Action<string, DateTime> dataHandler)
        {
            DateTime endTime = end;
            int publishTime = Int32.Parse(ConfigurationManager.AppSettings["DataPublishTime"], NumberStyles.Any, new CultureInfo("zh-Hans"));
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
                    date = date.AddDays(-1);
                    continue;
                }
                for (int i = 0; i < 3; i++)
                {
                    content = PullData(BuildUrl(date));
                    if (null != content)
                    {
                        break;
                    }
                }
                if (null != dataHandler && !string.IsNullOrEmpty(content))
                {
                    dataHandler(content, date);
                }
                date = date.AddDays(-1);
            }

        }
    }
}
