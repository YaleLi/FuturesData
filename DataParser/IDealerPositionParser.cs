using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType;

namespace DataParser
{
    public interface IDealerPositionParser
    {
        Collection<DealerPositionInfo> GetDealerPositionList(string htmlText, DateTime transactionDate);
    }
}
