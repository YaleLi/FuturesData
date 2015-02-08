using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataParser
{
    sealed class DealerPositionParserHelper
    {
        private DealerPositionParserHelper() { }
        public static Collection<ContractTransactionInfo> GetTopContracts(IEnumerable<ContractTransactionInfo> contracts, int count)
        {
            if (null == contracts || contracts.Count() == 0)
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
    }
}
