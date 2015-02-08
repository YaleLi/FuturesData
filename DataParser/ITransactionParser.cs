using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataParser
{
    interface ITransactionParser
    {
        Collection<ContractTransactionInfo> GetContractList(string htmlText, DateTime transactionDate);
        Collection<ContractTransactionInfo> GetTopContracts(string htmlText, int count, DateTime transactionDate);
    }
}
