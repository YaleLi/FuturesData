using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ModelFirstTest;

namespace DataParser
{
    public class ShfeTransactionParser
    {
        private HtmlDocument htmlParser = new HtmlDocument();


        public List<ContractTransactionInfo> GetContractList(string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText.Trim()))
            {
                return new List<ContractTransactionInfo>();
            }

            htmlParser.LoadHtml(htmlText);
            var result = new List<ContractTransactionInfo>();


            return result;

        }
        public List<ContractTransactionInfo> GetTopContracts(string htmlText, int count)
        {
            var contracts = GetContractList(htmlText);
            if (null==contracts || contracts.Count==0)
            {
                return new List<ContractTransactionInfo>();
            }

            var topContracts = new List<ContractTransactionInfo>();

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
    }
}
