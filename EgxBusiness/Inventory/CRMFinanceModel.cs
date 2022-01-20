using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx;
using Egx.EgxBusiness.Inventory;
namespace Egx.EgxBusiness.Inventory
{
    public class CRMFinanceModel
    {
        TransactionViewModel trvm;
        public CRMFinanceModel() 
        {
            trvm = new TransactionViewModel();
            EgxCustomer cust = new EgxCustomer();
            
        }

        public static void PayService(List<OrderDetails> det,int customerID,decimal total,decimal paid,string comment,DateTime date) 
        {
            TransactionViewModel.CreateQuickSalesOrder(det, date, total, paid,comment, -1, customerID);
        }

    }
}
