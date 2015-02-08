using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;
using HtmlAgilityPack;

namespace DataParser
{
    public class DceDealerPositionParser : IDealerPositionParser
    {
        private HtmlDocument htmlParser = new HtmlDocument();
        public Collection<DealerPositionInfo> GetDealerPositionList(string htmlText, DateTime transactionDate)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return new Collection<DealerPositionInfo>();
            }

            htmlParser.LoadHtml(htmlText);
            string commodity = "";
            string month = "";
            GetCommodityContract(htmlParser, out commodity, out month);
            if (string.IsNullOrWhiteSpace(commodity))
            {
                return new Collection<DealerPositionInfo>();
            }

            var tableContent = htmlParser.DocumentNode.SelectNodes("//table[@class=\"table\"]");
            if (null == tableContent || tableContent.Count<2)
            {
                return new Collection<DealerPositionInfo>();
            }

            var rows = tableContent[1].Descendants("tr").Skip(1);

            StringBuilder vDealers = new StringBuilder();
            StringBuilder bDealers = new StringBuilder();
            StringBuilder sDealers = new StringBuilder();
            foreach (var row in rows)
            {
                var columns = row.Descendants("td");
                if (columns.Count() != 12)
                {
                    continue;
                }
                AppendDealers(columns, 0, ref vDealers);
                AppendDealers(columns, 4, ref bDealers);
                AppendDealers(columns, 8, ref sDealers);
            }

            var result = new Collection<DealerPositionInfo>();
            result.Add(new DealerPositionInfo(transactionDate, commodity, month, vDealers.ToString(), bDealers.ToString(), sDealers.ToString()));

            return result;
        }

        private static void AppendDealers(IEnumerable<HtmlNode> columns, int start, ref StringBuilder dealers)
        {
            if (null==dealers)
            {
                dealers = new StringBuilder();
            }
            if (null == columns || start>=columns.Count() || start<0)
            {
                return;
            }
            var columnArray = columns.ToArray();
            if (Char.IsDigit(columnArray[start].InnerText.First()))
            {
                string name = columnArray[start + 1].InnerText.Trim();
                int amount = Int32.Parse(columnArray[start + 2].InnerText, NumberStyles.Any, GlobalDefinition.FormatProvider);
                dealers.Append(name + "=" + amount + ";");
            }
        }

        private static void GetCommodityContract(HtmlDocument parser, out string commodity, out string month)
        {
            commodity = "";
            month = "";
            if (null == parser)
            {
                return;
            }
            var nodes = parser.DocumentNode.SelectNodes("//div[@class=\"title2\"]");
            if (null == nodes)
            {
                return;
            }
            var nodeText = nodes.First().InnerText;
            int start = nodeText.IndexOf("：", StringComparison.Ordinal)+1;
            int end = start+1;
            while (start > 0 && end<nodeText.Length && !Char.IsWhiteSpace(nodeText[end]))
            {
                ++end;
            }
            string keyText = nodeText.Substring(start, end - start).Trim();
            start = 0;
            while (start<keyText.Length && !Char.IsDigit(keyText[start]))
            {
                ++start;
            }
            commodity = keyText.Substring(0, start);
            month = start == keyText.Length ? "" : keyText.Substring(start);
        }
    }
}
