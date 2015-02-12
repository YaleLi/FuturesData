using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;
using FuturesDataCrawler;
using HtmlAgilityPack;

namespace DataParser
{
    public static class DceCommodityCodeHelper
    {
        private static Dictionary<string, string> CodeMap { get; set; }=null;

        private static void Initialize()
        {
            CodeMap = new Dictionary<string, string>();
        }

        private static void ParseCommodityCode(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return;
            }

            var htmlParser = new HtmlDocument();
            htmlParser.LoadHtml(htmlText);

            var codeSection = htmlParser.DocumentNode.SelectNodes("//select[@name=\"Pu00021_Input.variety\"]");
            var codeOptions = codeSection.Descendants();

            foreach (var codeOption in codeOptions)
            {
                string name = codeOption.InnerText;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var attr = codeOption.PreviousSibling.Attributes.Where(a => a.Name.Equals("value"));
                    if (null != attr && attr.Count() == 1)
                    {
                        CodeMap[name] = attr.First().Value;
                    }
                }
            }
        }
        public static string GetCommodityCode(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "";
            }
            string codeName = name.Trim();
            if (null == CodeMap)
            {
                Initialize();
            }

            if (!CodeMap.ContainsKey(codeName))
            {
                var crawler = new DceCommodityCodeCrawler();
                DateTime tmpDate = new DateTime(2015, 2, 9);
                crawler.PullData(tmpDate, tmpDate, (text, transactionDate) => ParseCommodityCode(text));
            }

            if (!CodeMap.ContainsKey(codeName))
            {
                throw new HtmlParseException("Dce Commodity Code Parse Exception: Cannot Find code of "+ codeName);
            }

            return CodeMap[codeName];
        }
    }
}
