using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;
using HtmlAgilityPack;

namespace DataParser
{
    public class CzceDealerPositionParser :IDealerPositionParser
    {
        private HtmlDocument htmlParser = new HtmlDocument();
        public Collection<DealerPositionInfo> GetDealerPositionList(string htmlText, DateTime transactionDate)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return new Collection<DealerPositionInfo>();
            }

            htmlParser.LoadHtml(htmlText);
            var tables = htmlParser.DocumentNode.SelectNodes("//table[@class=\"table\"]");
            if (null == tables)
            {
                return new Collection<DealerPositionInfo>();
            }

            var result = new Collection<DealerPositionInfo>();
            foreach (var table in tables)
            {
                var info = ExtractDealerPositionInfo(table, transactionDate);
                if (null != info)
                {
                    result.Add(info);
                }
                

            }

            return result;
        }

        private static DealerPositionInfo ExtractDealerPositionInfo(HtmlNode table, DateTime transactionDate)
        {
            if (null == table)
            {
                return null;
            }

            var headerRow = table.Descendants("tr").First();
            var textBlock = headerRow.Descendants("b");
            if (null == textBlock)
            {
                return null;
            }
            string commodity = "";
            string month = "";
            string text = textBlock.First().InnerText;
            ExtractCommodityContract(text, out commodity, out month);
            if (string.IsNullOrEmpty(commodity))
            {
                return null;
            }

            var rows = table.Descendants("tr").Skip(2);
            StringBuilder vDealers = new StringBuilder();
            StringBuilder bDealers = new StringBuilder();
            StringBuilder sDealers = new StringBuilder();

            foreach (var row in rows)
            {
                var columns = row.Descendants("td").ToArray();
                AppendDealers(columns, 1, ref vDealers);
                AppendDealers(columns, 4, ref bDealers);
                AppendDealers(columns, 7, ref sDealers);
            }

            return new DealerPositionInfo(transactionDate, commodity, month, vDealers.ToString(), bDealers.ToString(), sDealers.ToString());
        }

        private static void AppendDealers(HtmlNode[] columns, int start, ref StringBuilder dealer)
        {
            if (columns.Length < start+2 || 
                columns[start].InnerText.Trim().Equals("-") || 
                columns[start].InnerText.Trim().Equals("&nbsp;"))
            {
                return;
            }
            string name = columns[start].InnerText.Trim();
            int amount = Int32.Parse(columns[start + 1].InnerText, NumberStyles.Any, GlobalDefinition.FormatProvider);

            dealer.Append(name + "=" + amount+";");
        }
        private static void ExtractCommodityContract(string text, out string commodity, out string month)
        {
            commodity = "";
            month = "";
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            var tmpText = text.Split(new string[] {"&nbsp;", "："}, StringSplitOptions.RemoveEmptyEntries);
            if (tmpText.Length >= 1)
            {
                var keyText = tmpText[1].Trim();
                int index = keyText.Length - 1;
                while (index>=0 && keyText[index]<0xf7)
                {
                    --index;
                }
                ++index;
                var cc = keyText.Substring(index);
                index = 0;
                while (index < cc.Length && !Char.IsDigit(cc[index]))
                {
                    ++index;
                }
                commodity = cc.Substring(0, index);
                month = cc.Substring(index);
            }
        }
    }
}
