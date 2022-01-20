using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Egx;
namespace Egx.EgxBusiness.Inventory
{
    public class NM
    {
                   public enum REF_TYPE {ALL,LEVEL1,LEVEL2,LEVEL3,LEVEL4};
                   public enum WEB_PAYMENT_METHOD {PREPAID,COM_BANK };
        public SystemMessage RegisterCustomer(EgxCustomer customer)
        {
            SystemMessage sm = new SystemMessage();

            return sm;
        }
        
        public static  List<EgxCustomer> GetCustomerNetwork(string customerCode, REF_TYPE selectedRef) 
        {
            List<EgxCustomer> lst = new List<EgxCustomer>();
            EgxDataAccess.DataAccess da = new EgxDataAccess.DataAccess();
            if (selectedRef == REF_TYPE.ALL)
            {
                lst = da.ExecuteSQL<EgxCustomer>("select * from Customers where NM_CUSTOMER_REF='" + customerCode + "' or NM_REF_L2='" + customerCode + "' or NM_REF_L3='" + customerCode + "'  or NM_REF_L4='" + customerCode + "'");
            }
            else if (selectedRef == REF_TYPE.LEVEL1)
            {
                lst = da.ExecuteSQL<EgxCustomer>("select * from Customers where NM_CUSTOMER_REF='" + customerCode + "' ");

            }
            else if (selectedRef == REF_TYPE.LEVEL2)
            {
                lst = da.ExecuteSQL<EgxCustomer>("select * from Customers where  NM_REF_L2='" + customerCode + "'");

            }
            else if (selectedRef == REF_TYPE.LEVEL3)
            {
                lst = da.ExecuteSQL<EgxCustomer>("select * from Customers where  NM_REF_L3='" + customerCode + "'");

            }
            else if(selectedRef== REF_TYPE.LEVEL4)
            {
                lst = da.ExecuteSQL<EgxCustomer>("select * from Customers where NM_REF_L4='" + customerCode + "'");

            }
            return lst;
        }

       
        public static DataTable getNetworkNumericDetails(string code) 
        {
            string sql =@"select (select COUNT(*) from Customers where NM_CUSTOMER_REF='"+code+"') as l1,";
                   sql+=" (select COUNT(*) from Customers where NM_REF_L2 ='"+code+"') as l2,";
                   sql += "(select COUNT(*) from Customers where NM_REF_L3 ='" + code + "') as l3, ";
                   sql += " (select COUNT(*) from Customers where NM_REF_L4 ='" + code + "') as l4";
                   return (DataTable)EgxDataAccess.DataAccess.GetQryData(sql, EgxDataAccess.EgxDataType.Mssql, EgxDataAccess.EgxData.DataTable);
        }
        
        
        public static SystemMessage CtrateNMTransaction(int customerID,string customerName,DateTime transDate,string transType,decimal amount) 
        {
            SystemMessage sm = new SystemMessage();
            NM_Transactions nmt = new NM_Transactions();
            nmt.TRANS_TYPE = transType;
            nmt.TRANS_DATE = transDate.Date.Date;
            nmt.CUSTOMER_NAME = customerName;
            nmt.CUSTOMER_ID = customerID;
            nmt.AMOUNT = amount;
            nmt.Insert();
            return sm;
        }
       
        
        public static void UpdateCustomerNetwork(EgxCustomer customer, Orders order,DateTime date)
        {
            decimal ccomm = 0, l1comm = 0, l2comm = 0, l3comm = 0,l4comm=0;
            EgxCustomer directRef = new EgxCustomer() { CUST_CODE = customer.NM_CUSTOMER_REF }.Search().FirstOrDefault();
            l1comm = (Egx.EgxBusiness.EgxSetting.Get("l1comm").ToDecimal() * order.TOT.Value) / 100;
            l2comm = (Egx.EgxBusiness.EgxSetting.Get("l2comm").ToDecimal() * order.TOT.Value) / 100;
            l3comm = (Egx.EgxBusiness.EgxSetting.Get("l3comm").ToDecimal() * order.TOT.Value) / 100;
            l4comm = (Egx.EgxBusiness.EgxSetting.Get("l4comm").ToDecimal() * order.TOT.Value) / 100;
            if (directRef != null)
            {
                directRef.Credit = directRef.Credit.GetValueOrDefault(0) + l1comm;
                ccomm -= l1comm;
                if (customer.NM_REF_L2 != null && customer.NM_REF_L2 != "0")
                {
                    EgxCustomer l2 = new EgxCustomer() { CUST_CODE = customer.NM_REF_L2 }.Search().FirstOrDefault();
                    l2.Credit = l2.Credit.GetValueOrDefault(0) + l2comm;
                    l2.Update();
                    ccomm -= l2comm;
                    NM.CtrateNMTransaction(l2.ID.Value, l2.CUST_NAME, date, "SCOM", l2comm);
                }
                if (customer.NM_REF_L3 != null && customer.NM_REF_L3 != "0")
                {
                    EgxCustomer l3 = new EgxCustomer() { CUST_CODE = customer.NM_REF_L3 }.Search().FirstOrDefault();
                    l3.Credit = l3.Credit.GetValueOrDefault(0) + l3comm;
                    l3.Update();
                    ccomm -= l3comm;
                    NM.CtrateNMTransaction(l3.ID.Value, l3.CUST_NAME, date, "SCOM", l3comm);


                }
                if (customer.NM_REF_L4 != null && customer.NM_REF_L4 != "0")
                {
                    EgxCustomer l4 = new EgxCustomer() { CUST_CODE = customer.NM_REF_L4 }.Search().FirstOrDefault();
                    l4.Credit = l4.Credit.GetValueOrDefault(0) + l4comm;
                    l4.Update();
                    ccomm -= l4comm;
                    NM.CtrateNMTransaction(l4.ID.Value, l4.CUST_NAME, date, "SCOM", l4comm);


                }
                directRef.Update();
                NM.CtrateNMTransaction(directRef.ID.Value, directRef.CUST_NAME, date.Date, "DSCOM", l1comm);
                TransactionViewModel.CreateDeduction(order.ONO.Value, Math.Abs(ccomm), -101, -1, date, "خصم نسب الاعضاء");
            }
            else 
            {
                // do nothing
            }
        }

        public static Orders _RegisterSalesOrderWeb(int customerID, string customerName, DateTime date, decimal total, decimal paid, List<Categ> OrderDetail,WEB_PAYMENT_METHOD paymentMethod)
        {
            Orders o = new Orders();
            int ono = o.CreateSalesOrder(date, total, paid, -1, customerID, true, true).Attachment.ToInt32(-1);
            OrderDetails odet = new OrderDetails();
            foreach (Categ dgvr in OrderDetail)
             {
                 var x = dgvr;
                 odet.CreateOrderLine(ono, x.ID.Value, 1, TransactionType.SALES_ORDER);
             }
           
            Egx.EgxBusiness.Inventory.TransactionViewModel.CreateCashReceipt(ono, paid, TransactionType.SALES_ORDER.ToInt32(), -1, date, "بيع منتج");
            NM.CtrateNMTransaction(customerID, customerName, date, "SO", paid);
            o.ONO = ono;
            return o;
        }

        private static Orders _RegisterSalesOrder(int customerID, string customerName, DateTime date, decimal total, decimal paid, DataGridView dgvOrderDetail, bool orderDetailsBinded = false) 
        {
            Orders o = new Orders();
            int ono = o.CreateSalesOrder(date, total, paid, -1, customerID, true, true).Attachment.ToInt32(-1);
            OrderDetails odet = new OrderDetails();
            //in frmNMRigesterCustomer uses Tag to bind Categ Data to grid view rows
            if (orderDetailsBinded)
            {
                foreach (DataGridViewRow dgvr in dgvOrderDetail.Rows)
                {
                    var x = dgvr.Tag.As<Categ>();
                    odet.CreateOrderLine(ono, x.ID.Value, 1, TransactionType.SALES_ORDER);
                }
            }
            else //in frmNMSalesOrder uses DataBoundItem OrderDetails to bind order details to grid rows
            {
                foreach (DataGridViewRow dgvr in dgvOrderDetail.Rows)
                {
                    var x = dgvr.DataBoundItem.As<OrderDetails>();
                    odet.CreateOrderLine(ono, x.PRODUCT_ID.Value, 1, TransactionType.SALES_ORDER);
                }
            }
            Egx.EgxBusiness.Inventory.TransactionViewModel.CreateCashReceipt(ono, paid, TransactionType.SALES_ORDER.ToInt32(), -1, date, "بيع منتج");
            NM.CtrateNMTransaction(customerID, customerName, date, "SO", paid);
            o.ONO = ono;
            return o;
        }
        public static Orders RegisterSalesOrder(int customerID,string customerName,DateTime date,decimal total,decimal paid,DataGridView dgvOrderDetail,bool orderDetailsBinded=false,bool payFromCurrentCredit=false)
        {
            if (!payFromCurrentCredit)
            {
               return  _RegisterSalesOrder(customerID, customerName, date, total, paid, dgvOrderDetail, orderDetailsBinded);
            }
            else 
            {
                string sql = "select * from NM_Transactions ";
                 sql+="where CUSTOMER_ID='"+customerID+"' ";
                 sql+="and MONTH(NM_Transactions.TRANS_DATE)>='"+date.Month.ToString()+"' and MONTH(NM_Transactions.TRANS_DATE)<'"+date.Month.ToString()+"'+1 ";
                List<NM_Transactions> lst = new EgxDataAccess.DataAccess().ExecuteSQL<NM_Transactions>(sql);
                if (lst.Where(tr => tr.TRANS_TYPE == "DSCOM" || tr.TRANS_TYPE == "SO").Count() > 0)
                {
                    if (GetCreditFromTransactions(lst) >= total)
                    {
                        CtrateNMTransaction(customerID, customerName, date.Date, "PUR", total);
                       return _RegisterSalesOrder(customerID, customerName, date, total, paid, dgvOrderDetail, orderDetailsBinded);
                    }
                    else { MessageBox.Show("عفوا رصيدك لا يسمح"); }
                }
                else { MessageBox.Show("لا تستطيع الصرف من الرصيد ، يجب ان تنفذ عملية شراء واحدة او عملية بيع مباشر واحدة على الاقل"); }
            }
            return null;
        }
        public static Orders RegisterSalesOrderWeb(int customerID, string customerName, DateTime date, decimal total, decimal paid, List<Categ> OrderDetail, WEB_PAYMENT_METHOD paymentMethod,out SystemMessage systemMessage)
        {
            Orders order=null;
            systemMessage = new SystemMessage();
            EgxCustomer cust = new EgxCustomer() { ID = customerID }.GetByID();
            if (paymentMethod== WEB_PAYMENT_METHOD.PREPAID)
            {
                if (cust.PrepaidCredit >= total)
                {
                    order=_RegisterSalesOrderWeb(customerID, customerName, date, total, paid, OrderDetail, paymentMethod);
                    cust.PrepaidCredit = cust.PrepaidCredit.GetValueOrDefault(0) - total;
                    cust.Update();
                    if (order != null) { systemMessage.Message = "تمت العملية بنجاح"; systemMessage.Type = MessageType.Pass; } else { systemMessage.Message = "فشل فى اتمام العملية برجاء المحاولة فى وقت لاحق"; systemMessage.Type = MessageType.Stop; }
                    return order;
                }
                else { systemMessage.Message = "عفوا رصيدك لا يسمح"; systemMessage.Type = MessageType.Stop; return null; }
            }
            else
            {
                string sql = "select * from NM_Transactions ";
                sql += "where CUSTOMER_ID='" + customerID + "' ";
                sql += "and MONTH(NM_Transactions.TRANS_DATE)>='" + date.Month.ToString() + "' and MONTH(NM_Transactions.TRANS_DATE)<'" + date.Month.ToString() + "'+1 ";
                List<NM_Transactions> lst = new EgxDataAccess.DataAccess().ExecuteSQL<NM_Transactions>(sql);
                if (lst.Where(tr => tr.TRANS_TYPE == "DSCOM" || tr.TRANS_TYPE == "SO").Count() > 0)
                {
                    if (GetCreditFromTransactions(lst) >= total)
                    {
                        CtrateNMTransaction(customerID, customerName, date.Date, "PUR", total);
                        order= _RegisterSalesOrderWeb(customerID, customerName, date, total, paid, OrderDetail, paymentMethod);
                        if (order != null) { systemMessage.Message = "تمت العملية بنجاح"; systemMessage.Type = MessageType.Pass; cust.Credit = cust.Credit.GetValueOrDefault(0) - total; cust.Update(); } else { systemMessage.Message = "فشل فى اتمام العملية برجاء المحاولة فى وقت لاحق"; systemMessage.Type = MessageType.Stop; }
                        return order;
                    }
                    else { systemMessage.Message = "عفوا رصيدك لا يسمح"; systemMessage.Type = MessageType.Stop; }
                }
                else { systemMessage.Message = "لا تستطيع الصرف من الرصيد ، يجب ان تنفذ عملية شراء واحدة او عملية بيع مباشر واحدة على الاقل"; systemMessage.Type = MessageType.Stop; }
            }
            return null;
        }
        //استخراج حركات عميل معين فى فترة معينة
        public static List<NM_Transactions> GetCustomerTransactions(int customerID, DateTime from ,DateTime to) 
        {
            List<NM_Transactions> lst = new List<NM_Transactions>();
            lst = new EgxDataAccess.DataAccess().ExecuteSQL<NM_Transactions>("select * from nm_transactions where customer_id ='"+customerID.ToString()+"' and trans_date between "+from.Date.SqlDateTime()+" and "+to.Date.SqlDateTime()+" ");
            return lst;
        }
        //حساب المبالغ لعميل معين له حركات فى فترة معينة
        public static SystemMessage CalculateDues(List<NM_Transactions> transactions)
        {
            SystemMessage sm = new SystemMessage();
            try
            {
                int x = transactions.Where(t => t.TRANS_TYPE == "SO" || t.TRANS_TYPE == "DSCOM").Count();
                if (x > 0)
                {
                    sm.Attachment = transactions.Where(t => t.TRANS_TYPE == "SCOM" || t.TRANS_TYPE == "DSCOM").Sum(t => t.AMOUNT).GetValueOrDefault(0) - transactions.Where(t => t.TRANS_TYPE == "PUR" || t.TRANS_TYPE == "PAY" || t.TRANS_TYPE == "BANKTRANS").Sum(t => t.AMOUNT).GetValueOrDefault(0);
                    sm.Type = MessageType.Pass;
                }
                else
                {
                    sm.Type = MessageType.Stop;
                    sm.Message = "يجب ان يوجد امر شراء او عمولة بيع مباشرة على الاقل";
                }
            }
            catch (Exception exc) { sm.Type = MessageType.Fail; sm.Message = exc.Message; }
            return sm;
        }
        //دفع المبالغ المستحقة لعميل معين له حركات فى فترة معينة
        public static SystemMessage PayTransactions(List<NM_Transactions> transactions,DateTime trasactionDate,EgxCustomer CustomerToUpdate=null,decimal? paid=null) 
        {
            
            SystemMessage sm = new SystemMessage();
            try
            {

                //شرط توافر احد الشرطين اما بيع منتج بشكل مباشر او عمل امر شراء
                if (transactions!=null&&transactions.Where(t => t.TRANS_TYPE == "SO" || t.TRANS_TYPE == "DSCOM").Count()>0)
                {
                    sm.Attachment = transactions.Where(t => t.TRANS_TYPE == "SCOM" || t.TRANS_TYPE == "DSCOM").Sum(t => t.AMOUNT).GetValueOrDefault(0) - transactions.Where(t => t.TRANS_TYPE == "PUR" || t.TRANS_TYPE == "PAY" || t.TRANS_TYPE == "BANKTRANS").Sum(t => t.AMOUNT).GetValueOrDefault(0);
                    if (paid.GetValueOrDefault(0)>0&&paid.GetValueOrDefault(0) <= CustomerToUpdate.Credit.GetValueOrDefault(0))
                    {
                        NM_Transactions trans = new NM_Transactions();
                       // trans.AMOUNT = sm.Attachment.ToDecimal(0);
                        trans.AMOUNT = paid.GetValueOrDefault(0);
                        trans.CUSTOMER_ID = transactions[0].CUSTOMER_ID.Value;
                        trans.CUSTOMER_NAME = transactions[0].CUSTOMER_NAME;
                        trans.TRANS_TYPE = "PAY";
                        trans.TRANS_DATE = trasactionDate.Date;
                        trans.EntryDate = DateTime.Now.Date;
                        trans.Insert();
                        //  TransactionViewModel.CreateDeduction(-1, sm.Attachment.ToDecimal(0), 1000, -1, DateTime.Now.Date, "قبض مستحقات عميل");
                         TransactionViewModel.CreateDeduction(-1, paid.GetValueOrDefault(0), 1000, -1, DateTime.Now.Date, "قبض مستحقات عميل");
                        if (CustomerToUpdate != null) { CustomerToUpdate.Credit = CustomerToUpdate.Credit.GetValueOrDefault(0) - paid.GetValueOrDefault(0); CustomerToUpdate.Update(); }
                        sm.Type = MessageType.Pass;
                    }
                    else { sm.Type = MessageType.Stop; sm.Message = "لا يوجد رصيد مستحق لك فى الشهر الحالى"; }
                }
                else
                {
                    sm.Type = MessageType.Stop;
                    sm.Message = "يجب ان يوجد امر شراء او عمولة بيع مباشرة على الاقل";
                }
            }
            catch (Exception exc) { sm.Type = MessageType.Fail; sm.Message = exc.Message; }
            return sm;
        }
       
        public static decimal GetCustomerCredit(int customerID, int month) 
        {
            return 0;
        }

        public static decimal GetCreditFromTransactions(List<NM_Transactions> transactions) 
        {
            decimal dues = 0;
            decimal deductions = 0;
            dues = transactions.Where(tr => tr.TRANS_TYPE == "SCOM" || tr.TRANS_TYPE == "DSCOM").Sum(tr => tr.AMOUNT).GetValueOrDefault(0);
            deductions = transactions.Where(tr => tr.TRANS_TYPE == "PAY" || tr.TRANS_TYPE == "PUR").Sum(tr => tr.AMOUNT).GetValueOrDefault(0);
            return dues-deductions;
        }

        public static decimal GetCustomerCredit(int customerID)
        {
            //string sql = "SELECT (select ISNULL(SUM(Amount),0) from NM_Transactions where CUSTOMER_ID='" + customerID.ToString() + "' and TRANS_TYPE='DSCOM')+((select ISNULL(SUM(Amount),0) from NM_Transactions where CUSTOMER_ID='" + customerID.ToString() + "' and TRANS_TYPE='SCOM'))- (select ISNULL(SUM(Amount),0) from NM_Transactions where CUSTOMER_ID='" + customerID.ToString() + "' and TRANS_TYPE='PUR')-(select ISNULL(SUM(Amount),0) from NM_Transactions where CUSTOMER_ID='" + customerID.ToString() + "' and TRANS_TYPE='PAY')";
            string sql = "select ISNULL(Credit,0) from Customers WHERE ID=" + customerID + " ";
            return EgxDataAccess.DataAccess.GetSqlValue(sql, EgxDataAccess.EgxDataType.Mssql).ToDecimal(0);
        }
        public static decimal GetCustomerPrepaidCredit(int customerID)
        {
            //string sql = "SELECT (select ISNULL(SUM(Amount),0) from NM_Transactions where CUSTOMER_ID='" + customerID.ToString() + "' and TRANS_TYPE='DSCOM')+((select ISNULL(SUM(Amount),0) from NM_Transactions where CUSTOMER_ID='" + customerID.ToString() + "' and TRANS_TYPE='SCOM'))- (select ISNULL(SUM(Amount),0) from NM_Transactions where CUSTOMER_ID='" + customerID.ToString() + "' and TRANS_TYPE='PUR')-(select ISNULL(SUM(Amount),0) from NM_Transactions where CUSTOMER_ID='" + customerID.ToString() + "' and TRANS_TYPE='PAY')";
            string sql = "select ISNULL(PrepaidCredit,0) from Customers WHERE ID=" + customerID + " ";
            return EgxDataAccess.DataAccess.GetSqlValue(sql, EgxDataAccess.EgxDataType.Mssql).ToDecimal(0);
        }
        public static decimal GetCustomerCreditInCurrentMonth(int customerID,string month) 
        {
            string sql = " SELECT ( ";
            sql+=" (select ISNULL(sum(t.AMOUNT),0) from( ";
            sql += " select * from NM_Transactions where MONTH(TRANS_DATE)>='" + month + "' and MONTH(TRANS_DATE)<'" + month + "'+1 and CUSTOMER_ID='" + customerID.ToString() +"') t ";
            sql+=" where t.TRANS_TYPE='SCOM' or t.TRANS_TYPE='DSCOM') - ";
            sql+=" (select ISNULL(sum(t.AMOUNT),0) from( ";
            sql+=" select * from NM_Transactions where MONTH(TRANS_DATE)>='"+month+"' and MONTH(TRANS_DATE)<'"+month+"'+1 and CUSTOMER_ID='"+customerID.ToString()+"') t ";
            sql += " where t.TRANS_TYPE='PAY' or t.TRANS_TYPE='PUR' or t.TRANS_TYPE='BANKTRANS') ) ";
            return EgxDataAccess.DataAccess.GetSqlValue(sql, EgxDataAccess.EgxDataType.Mssql).ToDecimal(0);

        }

        public static List<Dictionary<string,object>> GetSalesActivity(int customerID) 
        {
            List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
            Dictionary<string, object> dic;
            StringBuilder sb = new StringBuilder();
            string sql = "SELECT COUNT(*) Q,MONTH(TRANS_DATE) M FROM NM_Transactions WHERE CUSTOMER_ID='"+customerID.ToString()+"' AND (TRANS_TYPE='DSCOM' OR TRANS_TYPE='SCOM') AND YEAR(TRANS_DATE)='"+DateTime.Now.Year.ToString()+"' GROUP BY MONTH(TRANS_DATE)";
            DataTable dt = EgxDataAccess.DataAccess.GetDataTable(sql);
            foreach (DataRow dr in dt.Rows) 
            {
                dic = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == "Q")
                    {
                        dic.Add(col.ColumnName, dr[col].ToString());
                    }
                    else
                    {
                        dic.Add(col.ColumnName, dr[col].ToString());
                    }
                }
                lst.Add(dic);
            }
            return lst;
        }

        public static SystemMessage isValidBankToPay(int customerID,DateTime transDate,decimal total) 
        {
            string sql = "select * from NM_Transactions ";
            sql += "where CUSTOMER_ID='" + customerID + "' ";
            sql += "and MONTH(NM_Transactions.TRANS_DATE)>='" + transDate.Month.ToString() + "' and MONTH(NM_Transactions.TRANS_DATE)<'" + transDate.Month.ToString() + "'+1 ";
            List<NM_Transactions> lst = new EgxDataAccess.DataAccess().ExecuteSQL<NM_Transactions>(sql);
            if (lst.Where(tr => tr.TRANS_TYPE == "DSCOM" || tr.TRANS_TYPE == "SO").Count() > 0)
            {
                if (GetCreditFromTransactions(lst) >= total)
                {
                    return new SystemMessage() { Type= MessageType.Pass };
                }
                else { return new SystemMessage() { Type= MessageType.Stop, Message="عفوا رصيدك لا يسمح" }; }
            }
            else { return new SystemMessage() { Type= MessageType.Stop, Message= "لا تستطيع الصرف من الرصيد ، يجب ان تنفذ عملية شراء واحدة او عملية بيع مباشر واحدة على الاقل" }; }
        }

        public SystemMessage ActivateAlmajalCard(string code, EgxCustomer customer)
        {
            SystemMessage sm = new SystemMessage();
            var cards = new EgxDataAccess.DataAccess().ExecuteSQL<Cards>("select * from Cards where CARD_CODE='" + code + "'").ToList();
            if (cards.Count == 1)
            {
                if (cards[0].CID.GetValueOrDefault() > 0)
                {
                    sm.Message = "هذا الكارت تم استخدامة بالفعل";
                    sm.Type = MessageType.Stop;
                }
                else
                {
                    //Register Paid Card .....................................................
                    cards[0].CID = customer.ID;
                    cards[0].REG_DATE = DateTime.Now.Date;
                    cards[0].Update();
                    //Add Card Credit to Customer's Account........................................................................................................................................................................................
                    
                    customer.PrepaidCredit = customer.PrepaidCredit.GetValueOrDefault(0) + cards[0].CARD_VALUE.Value;
                    customer.exp = DateTime.Now.Date.AddDays(Convert.ToDouble(cards[0].ValidationDays)).Date;
                    customer.Update();
                    TransactionViewModel.CreateCashReceipt(-1, Convert.ToDecimal(cards[0].CARD_VALUE.Value), 201, customer.ID.Value, DateTime.Now.Date, "اضافة رصيد لعميل");
                        sm.Type = MessageType.Pass;
                        sm.Message = "تم تفعيل الكارت بنجاح و اضافتة الى سلة منتجاتك";
                }
            }
            else
            {
                sm.Message = "رقم الكارت غير صحيح";
                sm.Type = MessageType.Stop;
            }
            return sm;
        }

        public SystemMessage ActivateCard(string code, int cid)
        {
            SystemMessage sm = new SystemMessage();
            var cards = new EgxDataAccess.DataAccess().ExecuteSQL<Cards>("select * from Cards where CARD_CODE='" + code + "'").ToList();
            if (cards.Count == 1)
            {
                if (cards[0].CID.GetValueOrDefault() > 0)
                {
                    sm.Message = "هذا الكارت تم استخدامة بالفعل";
                    sm.Type = MessageType.Stop;
                }
                else
                {
                    //Register Paid Card .....................................................
                    cards[0].CID = cid;
                    cards[0].REG_DATE = DateTime.Now.Date;
                    cards[0].Update();
                    //Add Card Credit to Customer's Account........................................................................................................................................................................................
                    EgxCustomer customer = new EgxCustomer() { ID = cid }.GetByID();
                    //TranslatedCard card = TranslateCard(code);
                    Categ category = new Categ() { ID=cards[0].REL_CATEG}.GetByID();
                    customer.PrepaidCredit = customer.PrepaidCredit.GetValueOrDefault(0) + category.CATEG_PRICE.Value;
                    customer.Update();
                    TransactionViewModel.CreateCashReceipt(-1, category.CATEG_PRICE.Value, 201, customer.ID.Value, DateTime.Now.Date, "اضافة رصيد لعميل");
                    //Create Sales Order and Update Customers Network..............................................................................................................................................................................
                    List<Categ> odet = new List<Categ>();
                    odet.Add(category);
                    var outOrder = RegisterSalesOrderWeb(customer.ID.Value, customer.CUST_NAME, DateTime.Now.Date, odet[0].CATEG_PRICE.GetValueOrDefault(0), odet[0].CATEG_PRICE.GetValueOrDefault(0), odet, WEB_PAYMENT_METHOD.PREPAID, out sm);
                    if (outOrder != null)
                    {
                        NM.UpdateCustomerNetwork(customer, outOrder, DateTime.Now.Date);
                        cards[0].CID = cid;
                        cards[0].REG_DATE = DateTime.Now.Date;
                        cards[0].Update();
                        sm.Type = MessageType.Pass;
                        sm.Message = "تم تفعيل الكارت بنجاح و اضافتة الى سلة منتجاتك";
                    }

                }
            }
            else
            {
                sm.Message = "رقم الكارت غير صحيح";
                sm.Type = MessageType.Stop;
            }
            return sm;
        }
        public SystemMessage ActivateInarCard(string code, int cid)
        {
            SystemMessage sm = new SystemMessage();
            var cards = new EgxDataAccess.DataAccess().ExecuteSQL<Cards>("select * from Cards where CARD_CODE='" + code + "'").ToList();
            if (cards.Count == 1)
            {
                if (cards[0].CID.GetValueOrDefault() > 0)
                {
                    sm.Message = "هذا الكارت تم استخدامة بالفعل";
                    sm.Type = MessageType.Stop;
                }
                else
                {
                    if (cards[0].REL_PRODUCT != null && cards[0].REL_PRODUCT == "wfr") 
                    {
                        return ActivateCard(code, cid);
                    }
                    //Add Card Credit to Customer's Account........................................................................................................................................................................................
                    EgxCustomer customer = new EgxCustomer() { ID = cid }.GetByID();
                    TranslatedCard card = TranslateCard(code);

                    customer.PrepaidCredit = customer.PrepaidCredit.GetValueOrDefault(0) + cards[0].CARD_VALUE.ToDecimal(0);
                    customer.Update();
                    TransactionViewModel.CreateCashReceipt(-1, cards[0].CARD_VALUE.ToDecimal(0), 201, customer.ID.Value, DateTime.Now.Date, "اضافة رصيد لعميل");
                    cards[0].CID = cid;
                    cards[0].REG_DATE = DateTime.Now.Date;
                    cards[0].Update();
                    sm.Type = MessageType.Pass;
                    sm.Message = "تم تفعيل الحساب بنجاح";

                }
            }
            else
            {
                sm.Message = "رقم الكارت غير صحيح";
                sm.Type = MessageType.Stop;
            }
            return sm;
        }

        public static TranslatedCard TranslateCard(string cardCode) 
        {
            //xxxx-xxxx-xxxx-xxx
            //xxxxxxx(generated)-x(class)-xxx(Merchant)-xxxx(ProductionDate)
            TranslatedCard card = new TranslatedCard();
            card.GENERATED = cardCode.Substring(0, 7);
            card.CLASS = cardCode.Substring(7, 1);
            return card;
        }

        public struct TranslatedCard 
        {
            public string GENERATED { get; set; }
            public string CLASS { get; set; }
            public string MERCHANT { get; set; }
            public string PRODUCTION_DATE { get; set; }
            public Categ GetCategory() 
            {
                return new Categ().GetByCode(CLASS); 
            }

        }

        public bool IsActivatedCustomer(int customerID) 
        {
            var x = EgxDataAccess.DataAccess.GetDataTable("select * from (select *,  DATEADD(MONTH,(CASE  WHEN CARD_VALUE= 50 THEN 6 WHEN CARD_VALUE= 35 THEN 3 else 12 end ) ,REG_DATE)exp from Cards) calc where calc.exp>"+DateTime.Now.Date.SqlDateTime()+" and CID='"+customerID+"'");
            if (x.Rows.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }



        public List<NMMyProductsVM> GetCustomerProducts(int cid) 
        {
            return new NMMyProductsVM() {CUSTOMER=cid, OTYPE=-10  }.Search();
        }

        public bool IsValidProductForCustomer(int cid,int prodID) 
        {
            bool isValid = false;
            if (new NMMyProductsVM() { PRODUCT_ID=prodID, CUSTOMER=cid }.Search().Count > 0)
            {
                isValid = true;
            }
            else if (new Gifts() { CID = cid, PRODUCT_ID = prodID }.Search().Count > 0)
            {
                isValid = true;
            }
            else 
            {
                isValid = false;
            }
            return isValid;
        }


        public SystemMessage GetCustomerNameByCode(string code)
        {
            SystemMessage sm = new SystemMessage();
            try
            {
                var name = EgxDataAccess.DataAccess.GetSqlValue("select CUST_NAME from Customers WHERE CUST_CODE='" + code + "'", EgxDataAccess.EgxDataType.Mssql);
                if (name != null)
                {
                    sm.Attachment = name.ToString();
                    sm.Message = "OK";
                    sm.Type = MessageType.Pass;
                }
                else
                {
                    sm.Attachment = "لا يوجد عميل مسجل بهذا الكود";
                    sm.Message = "OK";
                    sm.Type = MessageType.Stop;
                }
            }catch(Exception exc){
                sm.Type = MessageType.Fail;
                sm.Message = exc.Message;
            }
            return sm;
        }



        #region طريقة اخرى للقبض
        public static SystemMessage CalculateDuesFromSql(int cid,DateTime from , DateTime to) 
        {
            SystemMessage sm = new SystemMessage();
            string sql = "select * from nm_transactions where customer_id =" + cid.ToString() + " and trans_date between " + from.Date.SqlDateTime() + " and " + to.Date.SqlDateTime() + " ";
            return sm;
        }

        public static SystemMessage PayTransactions(DateTime trDate, EgxCustomer customerToUpdate = null, decimal? paid = null, decimal? trDateCredit = null)
        {
            SystemMessage sm = new SystemMessage();
            string sql = "select isnull(count(ID),0) from NM_Transactions where CUSTOMER_ID=" + customerToUpdate.ID.Value + " and MONTH(NM_Transactions.TRANS_DATE)=" + trDate.Date.Month + " and YEAR(NM_Transactions.TRANS_DATE)=" + trDate.Date.Year + " and (TRANS_TYPE='SCOM' or TRANS_TYPE = 'DSCOM')";
            if (EgxDataAccess.DataAccess.GetSqlValue(sql, EgxDataAccess.EgxDataType.Mssql).ToInt32() > 0)
            {
                if (paid.GetValueOrDefault(0) > 0 && paid.GetValueOrDefault(0) <= customerToUpdate.Credit.GetValueOrDefault(0) && paid.GetValueOrDefault(0) <= trDateCredit.GetValueOrDefault(0))
                {
                    NM_Transactions trans = new NM_Transactions();
                    trans.AMOUNT = paid.GetValueOrDefault(0);
                    trans.CUSTOMER_ID = customerToUpdate.ID.Value;
                    trans.CUSTOMER_NAME = customerToUpdate.CUST_NAME;
                    trans.TRANS_TYPE = "PAY";
                    trans.TRANS_DATE = trDate.Date;
                    trans.EntryDate = DateTime.Now.Date;
                    trans.Insert();
                    TransactionViewModel.CreateDeduction(-1, paid.GetValueOrDefault(0), 1000, -1, DateTime.Now.Date, "قبض مستحقات عميل");
                    if (customerToUpdate != null) { customerToUpdate.Credit = customerToUpdate.Credit.GetValueOrDefault(0) - paid.GetValueOrDefault(0); customerToUpdate.Update(); }
                    sm.Type = MessageType.Pass;
                }
                else { sm.Type = MessageType.Stop; sm.Message = "لا يوجد رصيد مستحق لك فى الشهر الحالى"; }
            }
            else
            {
                sm.Type = MessageType.Stop; sm.Message = "يجب ان تقوم بعملية شراء واحدة او عملية بيع مباشرة على الاقل";
            }
            return sm;
        }
        
        #endregion


        public SystemMessage RegisterGift(int cid,string code) 
        {
            SystemMessage sm = new SystemMessage();
            Gifts gift = new Gifts().GetByCode(code);
            if (gift != null)
            {
                if (gift.CID==null)
                {
                    gift.CID = cid;

                    if (gift.Update().Type == MessageType.Pass)
                    {
                        sm.Message = "تم تفعيل الكوبون بنجاح";
                        sm.Attachment = new NMGiftsVM() { CID = cid, CODE = code }.Search()[0];
                    }
                    else { sm.Message = "حدث خطأ برجاء المحاولة فى وقت لاحق"; }
                }
                else 
                {
                    sm.Message = "هذا الكوبون تم تفعيله من قبل";
                }
            }
            else 
            {
                sm.Message = "عفوا ، الكود غير صحيح";
            }
            
            return sm;
        }

        public List<NMGiftsVM> GetCustomerGifts(int cid) 
        {
            return new NMGiftsVM() { CID=cid }.Search();
        }

        public bool ConfirmMail(string mail, string code) 
        {
            var cust = new EgxCustomer() { REAL_EMAIL=mail,ConfirmationCode=code}.Search();
            if (cust.Count > 0)
            {
                cust[0].IsConfirmed = true;
                cust[0].Update();
                return true;
            }
            else 
            {
                return false;
            }
        }


    }
}
