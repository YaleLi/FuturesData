using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;
using HtmlAgilityPack;
using Utility;

namespace DataParser
{
    public class ShfeTransactionParser : ITransactionParser
    {
        public Collection<ContractTransactionInfo> GetContractList(string htmlText, DateTime transactionDate)
        {
            var emptyResult = new Collection<ContractTransactionInfo>();

            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return emptyResult;
            }


            int start = htmlText.IndexOf('[');
            int end = htmlText.IndexOf("总计", StringComparison.Ordinal);
            if (-1 == start || -1==end)
            {
                return emptyResult;
            }
            ++start;
            while (end>start && htmlText[end]!= '}')
            {
                --end;
            }
            if (end <= start)
            {
                return emptyResult;
            }

            return ParseValidContent(htmlText.Substring(start, end - start + 1), transactionDate);

        }
        public Collection<ContractTransactionInfo> GetTopContracts(string htmlText, int count, DateTime transactionDate)
        {
            var contracts = GetContractList(htmlText, transactionDate);
            if (null==contracts || contracts.Count==0)
            {
                return new Collection<ContractTransactionInfo>();
            }

            var topContracts = new Collection<ContractTransactionInfo>();

            var contractGroups = contracts.GroupBy(c => c.Commodity);
            foreach (var group in contractGroups)
            {
                var sortedContracts = group.OrderByDescending(c => c.Volume).ToArray();

                for (int i = 0; i < count; i++)
                {
                    if (i < sortedContracts.Count())
                    {
                        topContracts.Add(sortedContracts[i]);
                    }
                }
            }

            return topContracts;
        }

        private static Collection<ContractTransactionInfo> ParseValidContent(string content, DateTime transactionDate)
        {
            var result = new Collection<ContractTransactionInfo>();
            if (string.IsNullOrEmpty(content))
            {
                return result;
            }

            Dictionary<string, string> lineDictionary = new Dictionary<string, string>();
            var lines = content.Split(new char[] {'{', '}'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                lineDictionary.Clear();
                var pairs = line.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in pairs)
                {
                    var kv = pair.Split(new char[] {':'});
                    string key = kv[0].Trim(new char[] {'\"'}).Trim();
                    string value = kv[1].Trim(new char[] { '\"' }).Trim();
                    lineDictionary[key] = value;
                }

                var info = BuildContractInfo(lineDictionary, transactionDate);
                if (null != info)
                {
                    result.Add(info);
                }
            }

            return result;
        }

        private static ContractTransactionInfo BuildContractInfo(Dictionary<string, string> lineDictionary, DateTime transactionDate)
        {
            if (null == lineDictionary || lineDictionary.Count == 0)
            {
                return null;
            }
            int index = lineDictionary["PRODUCTID"].IndexOf('_');
            if (index < 0)
            {
                return null;
            }
            string commodity = lineDictionary["PRODUCTID"].Substring(0, index);

            string month = lineDictionary["DELIVERYMONTH"];
            if (string.IsNullOrEmpty(month) || month.Equals("小计"))
            {
                return null;
            }

            double open = DoubleUtility.Parse(lineDictionary["OPENPRICE"], GlobalDefinition.FormatProvider, -1);
            double high = DoubleUtility.Parse(lineDictionary["HIGHESTPRICE"], GlobalDefinition.FormatProvider, -1);
            double low = DoubleUtility.Parse(lineDictionary["LOWESTPRICE"], GlobalDefinition.FormatProvider, -1);
            double close = DoubleUtility.Parse(lineDictionary["CLOSEPRICE"], GlobalDefinition.FormatProvider, -1);
            double settle = DoubleUtility.Parse(lineDictionary["SETTLEMENTPRICE"], GlobalDefinition.FormatProvider,
                -1);
            int volume = Int32.Parse(lineDictionary["VOLUME"], NumberStyles.Any, GlobalDefinition.FormatProvider);
            int position = Int32.Parse(lineDictionary["OPENINTEREST"], NumberStyles.Any,
                GlobalDefinition.FormatProvider);

            return new ContractTransactionInfo(transactionDate, "shfe", commodity, month, open, high, low, close,
                settle, volume, position);
        }
    }
}
