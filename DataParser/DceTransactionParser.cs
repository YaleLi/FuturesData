using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;
using HtmlAgilityPack;
using Utility;

namespace DataParser
{
    public class DceTransactionParser : ITransactionParser
    {
        private HtmlDocument htmlParser = new HtmlDocument();
        public List<ContractTransactionInfo> GetContractList(string htmlText, DateTime transactionDate)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return new List<ContractTransactionInfo>();
            }

            htmlParser.LoadHtml(htmlText);
            var tableContent = htmlParser.DocumentNode.SelectNodes("//table[@class=\"table\"]");
            if (null == tableContent)
            {
                return new List<ContractTransactionInfo>();
            }

            var result = new List<ContractTransactionInfo>();
            foreach (var row in tableContent.Descendants("tr").Skip(1))
            {
                var columns = row.Descendants("td").ToArray();
                if (!Char.IsDigit(columns[1].InnerText.First()))
                {
                    continue;
                }

                string commodity = DceCommodityCodeHelper.GetCommodityCode(columns[0].InnerText);
                if (string.IsNullOrWhiteSpace(commodity))
                {
                    throw new HtmlParseException("DCE Daily Transaction Parse Error at" + transactionDate.ToString(GlobalDefinition.DateFormat, GlobalDefinition.FormatProvider));
                }
                string month = columns[1].InnerText.Trim();
                double open = DoubleUtility.Parse(columns[2].InnerText, GlobalDefinition.FormatProvider, -1);
                double high = DoubleUtility.Parse(columns[3].InnerText, GlobalDefinition.FormatProvider, -1);
                double low = DoubleUtility.Parse(columns[4].InnerText, GlobalDefinition.FormatProvider, -1);
                double close = DoubleUtility.Parse(columns[5].InnerText, GlobalDefinition.FormatProvider, -1);
                double settle = DoubleUtility.Parse(columns[7].InnerText, GlobalDefinition.FormatProvider, -1);
                int volume = Int32.Parse(columns[10].InnerText, NumberStyles.Any, GlobalDefinition.FormatProvider);
                int position = Int32.Parse(columns[11].InnerText, NumberStyles.Any, GlobalDefinition.FormatProvider);
                result.Add(new ContractTransactionInfo(transactionDate, "dce", commodity, month, open, high, low, close, settle, volume, position));
            }
            return result;
        }

        public List<ContractTransactionInfo> GetTopContracts(string htmlText, int count, DateTime transactionDate)
        {
            throw new NotImplementedException();
        }
    }
}
