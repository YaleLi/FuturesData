using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;
using HtmlAgilityPack;
using Utility;

namespace DataParser
{
    public class CzceTransactionParser : ITransactionParser
    {
        private HtmlDocument htmlParser = new HtmlDocument();
        public Collection<ContractTransactionInfo> GetContractList(string htmlText, DateTime transactionDate)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return new Collection<ContractTransactionInfo>();
            }
            htmlParser.LoadHtml(htmlText);
            var table = htmlParser.DocumentNode.SelectNodes("//table[@id=\"senfe\"]");
            if (null == table)
            {
                return  new Collection<ContractTransactionInfo>();
            }

            var result = new Collection<ContractTransactionInfo>();
            var rows = table.Descendants("tr").Skip(1);
            foreach (var row in rows)
            {
                var columns = row.Descendants("td").ToArray();
                if (null == columns || columns.Count() == 0 || !Char.IsDigit(columns[0].InnerText.Last()))
                {
                    continue;
                }

                int index = columns[0].InnerText.Length - 1;
                while (index>=0 && Char.IsDigit(columns[0].InnerText[index]))
                {
                    --index;
                }
                string commodity = columns[0].InnerText.Substring(0, index + 1);
                string month = columns[0].InnerText.Substring(index + 1);

                double open = DoubleUtility.Parse(columns[2].InnerText, GlobalDefinition.FormatProvider, -1);
                double high = DoubleUtility.Parse(columns[3].InnerText, GlobalDefinition.FormatProvider, -1);
                double low = DoubleUtility.Parse(columns[4].InnerText, GlobalDefinition.FormatProvider, -1);
                double close = DoubleUtility.Parse(columns[5].InnerText, GlobalDefinition.FormatProvider, -1);
                double settle = DoubleUtility.Parse(columns[6].InnerText, GlobalDefinition.FormatProvider, -1);

                int volume = Int32.Parse(columns[9].InnerText, NumberStyles.Any, GlobalDefinition.FormatProvider);
                int position = Int32.Parse(columns[10].InnerText, NumberStyles.Any, GlobalDefinition.FormatProvider);

                result.Add(new ContractTransactionInfo(transactionDate, "czce", commodity, month, open, high, low, close, settle, volume, position));
            }

            return result;
        }

        public Collection<ContractTransactionInfo> GetTopContracts(string htmlText, int count, DateTime transactionDate)
        {
            var contracts = GetContractList(htmlText, transactionDate);

            return DealerPositionParserHelper.GetTopContracts(contracts, count);
        }
    }
}
