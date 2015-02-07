using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;
using HtmlAgilityPack;

namespace DataParser
{
    public class ShfeDealerPositionParser : IDealerPositionParser
    {

        public List<DealerPositionInfo> GetDealerPositionList(string htmlText, DateTime transactionDate)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return new List<DealerPositionInfo>();
            }

            int start = htmlText.IndexOf('[')+1;
            int end = htmlText.IndexOf(']');
            if (0 == start || end < 0 || end <= start)
            {
                return new List<DealerPositionInfo>();
            }

            string content = htmlText.Substring(start, end - start);
            return ParseValidContent(content, transactionDate);
        }

        private List<DealerPositionInfo> ParseValidContent(string content, DateTime transactionDate)
        {
            if (string.IsNullOrEmpty(content))
            {
                return new List<DealerPositionInfo>();
            }

            StringBuilder vDealers = new StringBuilder();
            StringBuilder bDealers = new StringBuilder();
            StringBuilder sDealers = new StringBuilder();
            string currentContract = "";
            string commodity = "";
            string month = "";
            var result = new List<DealerPositionInfo>();

            var lines = content.Split(new char[] {'{', '}'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var lineDictionary = BuildLineDictionary(line);
                if (lineDictionary.Count == 0)
                {
                    continue;
                }
                if (!currentContract.Equals(lineDictionary["INSTRUMENTID"]))
                {
                    if (!string.IsNullOrEmpty(currentContract))
                    {
                        result.Add(new DealerPositionInfo(transactionDate, commodity, month, vDealers.ToString(), bDealers.ToString(), sDealers.ToString()));
                    }

                    vDealers.Clear();
                    bDealers.Clear();
                    sDealers.Clear();
                    currentContract = lineDictionary["INSTRUMENTID"];
                }

                int rank = Int32.Parse(lineDictionary["RANK"], NumberStyles.Any);
                if (rank < 1 || rank > 500)
                {
                    continue;
                }

                commodity = currentContract.Substring(0, currentContract.Length - 4);
                month = currentContract.Substring(currentContract.Length-4);

                AppendDealer(lineDictionary["PARTICIPANTABBR1"], lineDictionary["CJ1"], vDealers);
                AppendDealer(lineDictionary["PARTICIPANTABBR2"], lineDictionary["CJ2"], bDealers);
                AppendDealer(lineDictionary["PARTICIPANTABBR3"], lineDictionary["CJ3"], sDealers);
            }
            if (!string.IsNullOrEmpty(currentContract))
            {
                result.Add(new DealerPositionInfo(transactionDate, commodity, month, vDealers.ToString(), bDealers.ToString(), sDealers.ToString()));
            }

            return result;
        }

        private void AppendDealer(string dealer, string amount, StringBuilder target)
        {
            if (!string.IsNullOrEmpty(dealer) && !string.IsNullOrEmpty(amount))
            {
                target.Append(dealer + "=" + amount + ";");
            }
        }
        private Dictionary<string, string> BuildLineDictionary(string line)
        {
            if (string.IsNullOrEmpty(line.Trim()))
            {
                return new Dictionary<string, string>();
            }

            var result = new Dictionary<string, string>();
            var pairs = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (null != pairs && pairs.Length > 0)
            {
                foreach (var pair in pairs)
                {
                    var kv = pair.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    string key = kv[0].Trim(new char[] { '\"' }).Trim();
                    string value = kv[1].Trim(new char[] { '\"' }).Trim();
                    result[key] = value;
                }
            }

            return result;
        }
    }
}
