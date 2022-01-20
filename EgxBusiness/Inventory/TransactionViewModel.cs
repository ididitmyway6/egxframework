using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxBusiness.Inventory
{
    public class TransactionViewModel
    {
        public static List<EgxProduct> Products { get { return new EgxProduct().GetAll(); } }

        public static List<Categ> FilterCategory(int prodNumber) 
        {
            return new Categ() { PROD_ID = prodNumber }.Search();
        }

        public static EgxCustomer GetCustomer(int id) 
        {
            return new EgxCustomer() { ID=id }.GetByID();
        }

        public static void BankApproving(int customerID, DateTime date)
        {

            EgxCustomer customer = GetCustomer(customerID);
            customer.BANK_RESPONSE = 1;
            Orders o = new Orders() { CUSTOMER = customer.ID }.Search().FirstOrDefault();
            o.IS_HOLD = false;
            CreateDeduction(o.ONO.Value, o.TOT.Value * Decimal.Parse(EgxSetting.Get("BankApprCom")) / 100, -15, customerID, date, "خصم عمولة موافقة لحساب البنك ");
            customer.Update();
            o.Update();
        }

        public static void BankRejecting(int customerID, DateTime date)
        {
            EgxCustomer customer = GetCustomer(customerID);
            customer.BANK_RESPONSE = -1;
            Orders o = new Orders() { CUSTOMER = customer.ID }.Search().FirstOrDefault();
            o.IS_HOLD = true;
            o.CancelOrder(o.ONO.Value,-10);
            CreateDeduction(o.ONO.Value, o.TOT.Value * Decimal.Parse(EgxSetting.Get("Oncashpay")) / 100, -16, customerID, date, "رد قيمة بيع اجل");
            customer.Update();
            o.Update();
        }

        public static void ResetBankState(int customerID,DateTime date) 
        {
            EgxCustomer customer = GetCustomer(customerID);
            Orders o = new Orders() { CUSTOMER = customer.ID }.Search().FirstOrDefault();
            if (customer.BANK_RESPONSE == -1)
            {
                Orders.RemoveCancelation(o.ONO.Value, -10);
                o.IS_HOLD = true;
                customer.BANK_RESPONSE = 0;
                CreateCashReceipt(o.ONO.Value, o.TOT.Value * Decimal.Parse(EgxSetting.Get("Oncashpay")) / 100, -16, customerID, date, "استرجاع عميل لقائمة الانتظار");
                customer.Update();
                o.Update();
            }
            else if (customer.BANK_RESPONSE == 1)
            {
                customer.BANK_RESPONSE = 0;
                o.IS_HOLD = true;
                CreateCashReceipt(o.ONO.Value, o.TOT.Value * Decimal.Parse(EgxSetting.Get("BankApprCom")) / 100, -15, customerID, date, "خصم عمولة موافقة لحساب البنك ");
                customer.Update();
                o.Update();
            }
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
                    if (SystemSetting.CurrentUser != null) { dtl.USR_ID = SystemSetting.CurrentUser.ID; dtl.USR_NAME = SystemSetting.CurrentUser.USER_NAME; }
                    dtl.TR_DATE = DateTime.Now;
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

        public static int MakeSalesOrderWithVoucher(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid, int unuioID = -1, int customer = -1, string comment = null)
        {
            int ono = 0;
            ono = MakeSalesOrder(orderDetailsList, orderDate, total, paid, unuioID, customer);
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
            inv.CreateVoucher(ono, paid, orderDate, "OK", -10, unuioID, customer);
            return ono;


        }
        public static int MakeSalesOrderWithVoucher(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid, int unuioID = -1, int customer = -1, string comment = null,bool isFreeOrder=false)
        {
            int ono = 0;
            if (!isFreeOrder)
            {
                ono = MakeSalesOrder(orderDetailsList, orderDate, total, paid, unuioID, customer);
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
                inv.CreateVoucher(ono, 0, orderDate, "OK", -10, unuioID, customer);
            }
            else 
            {
                ono = MakeSalesOrder(orderDetailsList, orderDate, 0, 0, unuioID, -100);
                Invoices inv = new Invoices();
                inv.CreateVoucher(ono, 0, orderDate, "OK", -10, unuioID, -100);
            }
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
 

        public static int MakePurchaseOrder(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid, int supplier,int locationID=-1)
        {
            int ono = -1;
            try
            {
                Orders order = new Orders();
                 ono = (int)order.CreatePurchaseOrder(orderDate, total, paid, supplier,locationID).Attachment;
                foreach (OrderDetails dtl in orderDetailsList)
                {
                    dtl.TRANS_TYPE = -9;
                    dtl.ONO = ono;
                    dtl.INV_SITE = locationID;
                    dtl.TR_DATE = DateTime.Now;
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

        public static void ccc() { }

        public static void CreateQuickPurchaseOrder(List<OrderDetails> details, int ono, decimal total, int supplierID, string comment, bool isBank)
        {
            Invoices inv = new Invoices();
            inv.CreateInvoice(ono, total, DateTime.Now, "OK", -9);
            TransactionViewModel.CreateDeduction(ono,
                total, -9, supplierID, DateTime.Now, comment, isBank);

            foreach (OrderDetails dtl in details)
            {
                dtl.DELIVERED_QNT = dtl.QUANTITY;
                dtl.TR_DATE = DateTime.Now;
                dtl.Update();
                //Register stock details
                ItemsControlller.RegisterItems(dtl.ID.Value, dtl.QUANTITY.Value, DateTime.Now.Date, comment, -9);
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

        public static int CreateQuickSalesOrder(List<OrderDetails> orderDetailsList, DateTime orderDate, decimal total, decimal paid,string comment, int unuioID = -1, int customer = -1)
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

        public static void PayUnionCommision(int unionID,string rec_name,DateTime dateFrom,DateTime dateTo, DateTime date, decimal amount,string comment="دفع نسبة نقابة", bool isBank = false) 
        {
            AccountTransactions at = new AccountTransactions();
            UnionsTransactions uTrans = new UnionsTransactions();
            Unions u = new Unions() {ID=unionID}.GetByID();
            uTrans.UID = unionID;
            var uTransaction = uTrans.Search().Where(ux => dateFrom.Date >= ux.FROM_DATE && dateFrom.Date <= ux.TO_DATE).ToList();
            if (uTransaction.Count == 0)
            {
                uTrans.FROM_DATE = dateFrom.Date;
                uTrans.TO_DATE = dateTo.Date;
                uTrans.AMOUNT = amount;
                uTrans.COMMENT = comment;
                uTrans.COMMISION = u.U_COMMISION.Value;
                uTrans.IS_BANK = isBank;
                uTrans.PAID = amount;
                uTrans.REC_NAME = "";
                uTrans.TR_DATE = date.Date;
                uTrans.REC_NAME = rec_name;
                uTrans.UID = unionID;
                uTrans.USR_NAME = SystemSetting.CurrentUser.USER_NAME;
                at.SupType = unionID;
                at.TransactionType = -3;
                at.ACTUAL_DATE = date;
                at.Amount = -amount;
                at.Comment = comment;
                at.IsBank = isBank;
                at.OrderID = -1;
                at.TransClosed = 1;
                uTrans.Insert();
                at.Insert();
            }
            else {
                MessageBox.Show("هذه النقابة تم دفع قيمة نسبة المبيعات المستحقه لها فى هذا الشهر");
            }
           
        }
    
    }
}
