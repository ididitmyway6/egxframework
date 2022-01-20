using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxBusiness.EgxSMS.Finance
{
    public class TransactionViewModel
    {
        public static List<Subjects> Products { get { return new Subjects().GetAll(); } }

        public static List<Subjects> FilterCategory(int classNo) 
        {
            return new Subjects() {  CLASS_GRADE_ID = classNo }.Search();
        }

        public static Students GetStudent(int id) 
        {
            return new Students() { ID = id }.GetByID();
        }

        public static int MakeSalesOrder(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid,int unuioID=-1,int customer= -1) 
        {
            Orders newOrder = new Orders();
            try
            {
                int ono = (int)newOrder.CreateSalesOrder(orderDate, total, paid, unuioID,customer).Attachment;
                foreach (OrderDetails dtl in orderDetailsList)
                {
                    dtl.ONO = ono;
                    dtl.Insert();
                }
                return ono;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return -1;
            }
        }

        public static int MakeSalesOrderWithVoucher(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid, int unuioID = -1, int customer = -1,string comment=null) 
        {
            int ono = 0;
           ono= MakeSalesOrder(orderDetailsList, orderDate, total, paid, unuioID, customer);
            Invoices inv = new Invoices();
            AccountTransactions at = new AccountTransactions();
            at.Amount = paid;
            at.Comment = comment;
            at.OrderID = ono;
            at.TransactionType = -10;
            at.TransactionDate = orderDate;
            at.TransClosed = 1;
            at.SupType = customer;
            at.Insert();
            inv.CreateVoucher(ono, paid, orderDate, "OK",-10, unuioID,customer);
            return ono;
             

        }

        public static void PayQuickSalesOrder(Orders order, decimal amount, string comment, int customer, bool isBank) 
        {
            Invoices inv = new Invoices();
            inv.Customer = customer;
            inv.InvoiceDate = DateTime.Now;
            inv.InvoiceNumber = Invoices.GetLatestInvoice();
            inv.InvoiceStatus = "OK";
            inv.OrderNumber = order.ONO;
            inv.Supplier = -1;
            inv.Total = order.TOT;
            inv.TR_TYPE = -1;
            inv.Type = "INVOICE";
            inv.Insert();
            order.IS_DELIVERED = true;
            order.INVOICE_DONE = true;
            order.Update();
            
        }

        public static void PaySalesOrder(Orders order,decimal amount,string comment,int customer,bool isBank) 
        {
            Invoices inv = new Invoices();
            if (order.TOT != order.PAID)
            {
                order.PAID = order.PAID.GetValueOrDefault(0) +amount;
            }
            if (order.PAID > order.TOT)
            {
                MessageBox.Show("المبلغ المدفوع اكبر من المطلوب");
            }
            else
            {
                order.Update();
            }
            if (Invoices.IsDoneVoucher(order.ONO.Value, -10))
            {
                if (Orders.IsDelivered(order.ONO.Value, -10) && !Invoices.IsDoneInvoice(order.ONO.Value, -10))
                {
                    inv.CreateInvoice(order.ONO.Value, order.TOT.Value, DateTime.Now, "OK", -10,-1,customer);
                    order.INVOICE_DONE = true;
                    order.IS_DELIVERED = true;
                    order.Update();
                }
            }
            else
            {
                inv.CreateVoucher(order.ONO.Value, amount, DateTime.Now, "OK", -10,-1,customer);
                TransactionViewModel.CreateCashReceipt(order.ONO.Value,
                    amount, -10, order.CUSTOMER.Value, DateTime.Now,comment,isBank);
                if (Invoices.IsDoneVoucher(order.ONO.Value, -10) && Orders.IsDelivered(order.ONO.Value, -10))
                {
                    inv.CreateInvoice(order.ONO.Value, order.TOT.Value, DateTime.Now, "OK", -10,-1,customer);
                    order.INVOICE_DONE = true;
                    order.IS_DELIVERED = true;
                    order.Update();
                }


            }
        }

        public static int MakePurchaseOrder(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid, int supplier)
        {
            int ono = -1;
            try
            {
                Orders order = new Orders();
                 ono = (int)order.CreatePurchaseOrder(orderDate, total, paid, supplier).Attachment;
                foreach (OrderDetails dtl in orderDetailsList)
                {
                    dtl.TRANS_TYPE = -9;
                    dtl.ONO = ono;
                    dtl.Insert();
                }
            }
            catch
            {

            }
            return ono;
        }

        public static void CreateDeduction(int orderID, decimal value, int tr_type, int supType, DateTime date, string comment,bool isBank=false)
        {
            AccountTransactions at = new AccountTransactions();
            at.Amount = -value;
            at.Comment = comment;
            at.OrderID = orderID;
            at.SupType = supType;
            at.TransactionDate = DateTime.Now;
            at.TransactionType = tr_type;
            at.TransClosed = 1;
            at.IsBank = isBank;
            at.Insert();
        }

        public static void CreateCashReceipt(int orderID, decimal value, int tr_type, int supType, DateTime date, string comment,bool isBank=false)
        {
            AccountTransactions at = new AccountTransactions();
            at.Amount = value;
            at.Comment = comment;
            at.OrderID = orderID;
            at.SupType = supType;
            at.TransactionDate = DateTime.Now;
            at.TransactionType = tr_type;
            at.TransClosed = 1;
            at.IsBank = isBank;
            at.Insert();
        }

        public static void CreateQuickPurchaseOrder(List<OrderDetails> details, int ono, decimal total, int supplierID, string comment, bool isBank)
        {
            Invoices inv = new Invoices();
            inv.CreateInvoice(ono, total, DateTime.Now, "OK", -9);
            TransactionViewModel.CreateDeduction(ono,
                total, -9, supplierID, DateTime.Now, comment, isBank);

            foreach (OrderDetails dtl in details)
            {
                dtl.DELIVERED_QNT = dtl.QUANTITY;
                dtl.Update();
                //Register stock details
              //  ItemsControlller.RegisterItems(dtl.ID.Value, dtl.QUANTITY.Value, DateTime.Now.Date, comment, -9);
            }
            Orders o = new Orders() { ONO = ono, OTYPE = -9 }.Search().FirstOrDefault();
            o.PAID = total;
            o.IS_DELIVERED = true;
            o.INVOICE_DONE = true;
            o.Update();
        }

        public static int MakePurchaseOrderWithInvoice(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid, int supplier)
        {
            try
            {
                Orders order = new Orders();
                int ono = (int)order.CreatePurchaseOrder(orderDate, total, paid, supplier).Attachment;

            }
            catch
            {

            }
            return 0;
        }

        public static int CreateQuickSalesOrder(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid, int unuioID = -1, int customer = -1) 
        {
            //int oResult=MakeSalesOrder(orderDetailsList, orderDate, total, paid, -1, -1);
            Orders newOrder = new Orders();
            try
            {
                int ono = (int)newOrder.CreateSalesOrder(orderDate, total, paid, unuioID, customer).Attachment;
                foreach (OrderDetails dtl in orderDetailsList)
                {
                    dtl.ONO = ono;
                    dtl.DELIVERED_QNT = dtl.QUANTITY;
                    dtl.Insert();
                }

                if (ono != -1) 
                {
                    newOrder = new Orders() { ONO=ono,OTYPE=-10 }.Search().FirstOrDefault();
                    newOrder.IS_DELIVERED = true;
                    newOrder.INVOICE_DONE = true;
                    newOrder.Update();
                    Invoices inv = new Invoices();
                    inv.Customer = customer;
                    inv.InvoiceDate = orderDate;
                    inv.InvoiceNumber = Invoices.GetLatestInvoice() + 1;
                    inv.InvoiceStatus = "DONE";
                    inv.OrderNumber = ono;
                    inv.Total = total;
                    inv.TR_TYPE = -10;
                    inv.Type = "INVOICE";
                    inv.Insert();
                    CreateCashReceipt(ono, paid, -10, -1, orderDate, "بيع فورى لعميل");
                
                }

                return ono;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return -1;
            }
            
           
        }


        public static int CreateQuickSalesOrder(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid,string comment, int unuioID = -1, int customer = -1,bool deslivered=false,bool invoiced=false)
        {
            Orders newOrder = new Orders();
            try
            {
                int ono = (int)newOrder.CreateSalesOrder(orderDate, total, paid, unuioID, customer).Attachment;
                foreach (OrderDetails dtl in orderDetailsList)
                {
                    dtl.ONO = ono;
                    dtl.DELIVERED_QNT = dtl.QUANTITY;
                    dtl.Insert();
                }

                if (ono != -1)
                {
                    newOrder = new Orders() { ONO = ono, OTYPE = -10 }.Search().FirstOrDefault();
                    newOrder.IS_DELIVERED = true;
                    newOrder.INVOICE_DONE = true;
                    newOrder.Update();
                    Invoices inv = new Invoices();
                    inv.Customer = customer;
                    inv.InvoiceDate = orderDate;
                    inv.InvoiceNumber = Invoices.GetLatestInvoice() + 1;
                    inv.InvoiceStatus = "DONE";
                    inv.OrderNumber = ono;
                    inv.Total = total;
                    inv.TR_TYPE = -10;
                    inv.Type = "INVOICE";
                    inv.Insert();
                    CreateCashReceipt(ono, paid, -10, -1, orderDate, comment);

                }

                return ono;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return -1;
            }


        }
    }
}
