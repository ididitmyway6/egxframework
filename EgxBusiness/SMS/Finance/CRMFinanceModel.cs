using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx;
using Egx.EgxBusiness.Inventory;
using System.Windows.Forms;
namespace Egx.EgxBusiness.EgxSMS.Finance
{
    public class CRMFinanceModel
    {
        TransactionViewModel trvm;
        public CRMFinanceModel() 
        {
            trvm = new TransactionViewModel();
            
        }
        public static int Pay(List<OrderDetails> det, int customerID, decimal total, decimal paid, string comment, DateTime date) 
        {
            int ono = -1;
                Orders newOrder = new Orders();
                Invoices inv = new Invoices();
                if (total == paid)
                {
                    ono = newOrder.CreateSalesOrder(DateTime.Now.Date, total, paid, -1, customerID, true, true).Attachment.ToInt32();
                }
                else
                {
                    ono = newOrder.CreateSalesOrder(DateTime.Now.Date, total, paid, -1, customerID, false, true).Attachment.ToInt32();
                }
                foreach (OrderDetails dtl in det)
                {
                    dtl.ONO = ono;
                    dtl.DELIVERED_QNT = dtl.QUANTITY;
                    dtl.Insert();
                }
                inv.Customer = customerID;
                inv.InvoiceDate = date;
                inv.InvoiceNumber = Invoices.GetLatestInvoice() + 1;
                inv.InvoiceStatus = "DONE";
                inv.OrderNumber = ono;
                inv.Total = paid;
                inv.TR_TYPE = -10;
                inv.Type = "INVOICE";
                inv.Insert();
                TransactionViewModel.CreateCashReceipt(ono, paid, -10,customerID, date, comment);
            return ono;
        }
        public static void PayService(List<OrderDetails> det,int customerID,decimal total,decimal paid,string comment,DateTime date) 
        {
            if (total == paid)
            {
                TransactionViewModel.CreateQuickSalesOrder(det, date, total, paid, comment, -1, customerID, true, true);
            }
            else
            {
                TransactionViewModel.CreateQuickSalesOrder(det, date, total, paid, comment, -1, customerID, false, true);
            }
        }
        public static void CompletePayment(Orders order,int customerID,decimal amount,string comment="Complete Customer Payment") 
        {
            order.IS_DELIVERED = true;
            order.PAID += amount;
            order.Update();
            Invoices inv = new Invoices();
            inv.CreateInvoice(order.ONO.Value, amount, DateTime.Now.Date, "", -10, -1, customerID, -1);
            TransactionViewModel.CreateCashReceipt(order.ONO.Value, amount, -10, customerID, DateTime.Now.Date, comment);
        }
        public static SystemMessage Refund(OrderDetails det , EventsStudent customer , Events evnt,decimal amount) 
        {
            SystemMessage sm = new SystemMessage();
            Orders refOreder = new Orders() { OTYPE = -10, ONO = det.ONO }.Search().First();
            refOreder.IS_HOLD = true;
            refOreder.Update();
            Orders o = new Orders();
            int ono = o.CreateReturnSalesOrder(det.ONO.Value, DateTime.Now.Date, amount).Attachment.ToInt32();
            OrderDetails detail = new OrderDetails();
            detail = det;
            det.RETURNED_QNT = 1;
            det.Update();
            detail.ONO = ono;
            detail.RETURNED_QNT = 0;
            detail.DISCOUNT = 0f;
            detail.DELIVERED_QNT = 1;
            detail.QUANTITY = 1;
            detail.TRANS_TYPE = -8;
            detail.Insert();
            customer.STATE = SystemConstants.StudentState.REFUND.ToInt32();
            customer.Update();
            evnt.Capacity = evnt.Capacity.GetValueOrDefault(0) - 1;
            evnt.Update();

            TransactionViewModel.CreateDeduction(ono, amount, -8, customer.ID.Value, DateTime.Now.Date, "Refunding");
            return sm;
        }
    }
}
