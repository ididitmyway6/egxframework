using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness.Inventory
{
    public class NMSmart:NM
    {
        public new static void UpdateCustomerNetwork(EgxCustomer customer, Orders order, DateTime date)
        {
            //if (customer.GIVE_COMMITION.GetValueOrDefault(true))
            //{
                decimal ccomm = 0, l1comm = 0, l2comm = 0, l3comm = 0, l4comm = 0,l1extraComm=0;
                string[] l1commSetting = EgxSetting.Get("l1comm").split(';');
                string[] l1extraCommSetting = EgxSetting.Get("l1extracomm").split(';');
                string[] l2commSetting = EgxSetting.Get("l2comm").split(';');
                string[] l3commSetting = EgxSetting.Get("l3comm").split(';');
                string[] l4commSetting = EgxSetting.Get("l4comm").split(';');
                int deductedCheckGrade = EgxSetting.Get("deducted_check_grade").ToInt32();
                decimal checkValue = EgxSetting.Get("check_value").ToDecimal();
                EgxCustomer directRef = new EgxCustomer() { CUST_CODE = customer.NM_CUSTOMER_REF }.Search().FirstOrDefault();
                l1comm = l1commSetting[1] == "p" ? (l1commSetting[0].ToDecimal() * order.TOT.Value) / 100 : l1commSetting[0].ToDecimal();
                l2comm = l2commSetting[1] == "p" ? (l2commSetting[0].ToDecimal() * order.TOT.Value) / 100 : l2commSetting[0].ToDecimal();
                l3comm = l3commSetting[1] == "p" ? (l3commSetting[0].ToDecimal() * order.TOT.Value) / 100 : l3commSetting[0].ToDecimal();
                l4comm = l4commSetting[1] == "p" ? (l4commSetting[0].ToDecimal() * order.TOT.Value) / 100 : l4commSetting[0].ToDecimal();
                l1extraComm = l1extraCommSetting[1] == "p" ? (l1extraCommSetting[0].ToDecimal() * order.TOT.Value) / 100 : l1extraCommSetting[0].ToDecimal();
                if (directRef != null) // && firectRef.IsActive? if needed
                {
                    if (directRef.IS_ACTIVE.GetValueOrDefault(false) && directRef.ACTIVE_PURCHASER.GetValueOrDefault(false))
                    {
                        if (directRef.CheckCounter + l1comm >= deductedCheckGrade)
                        {
                            directRef.CheckCounter = deductedCheckGrade - (directRef.CheckCounter + l1comm);
                            //Handle 4th check here ............................
                            UpdateCustomerNetwork(directRef, new Orders() { ONO = -500, TOT = checkValue }, DateTime.Now.Date);
                        }
                        else
                        {
                            directRef.Credit = directRef.Credit.GetValueOrDefault(0) + l1comm;
                            directRef.DIRECT_CREDIT = directRef.DIRECT_CREDIT.GetValueOrDefault(0) + l1extraComm;
                            directRef.CheckCounter = directRef.CheckCounter.GetValueOrDefault(0) + l1comm;
                            ccomm -= l1comm + l1extraComm;
                            NM.CtrateNMTransaction(directRef.ID.Value, directRef.CUST_NAME, date.Date, "DSCOM", l1comm + l1extraComm);
                        }
                        directRef.Update();
                    }
                    if (customer.NM_REF_L2 != null && customer.NM_REF_L2 != "0")
                    {
                        EgxCustomer l2 = new EgxCustomer() { CUST_CODE = customer.NM_REF_L2 }.Search().FirstOrDefault();
                        if (l2.IS_ACTIVE.GetValueOrDefault(false) && l2.ACTIVE_PURCHASER.GetValueOrDefault(false))
                        {
                            if (l2.CheckCounter + l2comm >= deductedCheckGrade)
                            {
                                l2.CheckCounter = deductedCheckGrade - (l2.CheckCounter + l2comm);
                                UpdateCustomerNetwork(l2, new Orders() { ONO = -500, TOT = checkValue }, DateTime.Now.Date);
                            }
                            else
                            {
                                l2.Credit = l2.Credit.GetValueOrDefault(0) + l2comm;
                                l2.CheckCounter = l2.CheckCounter.GetValueOrDefault(0) + l2comm;
                                ccomm -= l2comm;
                                NM.CtrateNMTransaction(l2.ID.Value, l2.CUST_NAME, date, "SCOM", l2comm);
                            }
                            l2.Update();
                        }
                    }
                    if (customer.NM_REF_L3 != null && customer.NM_REF_L3 != "0")
                    {
                        EgxCustomer l3 = new EgxCustomer() { CUST_CODE = customer.NM_REF_L3 }.Search().FirstOrDefault();
                        if (l3.IS_ACTIVE.GetValueOrDefault(false) && l3.ACTIVE_PURCHASER.GetValueOrDefault(false))
                        {
                            if (l3.CheckCounter + l3comm >= deductedCheckGrade)
                            {
                                l3.CheckCounter = deductedCheckGrade - (l3.CheckCounter + l3comm);
                                //Handle 4th check here ............................
                                UpdateCustomerNetwork(l3, new Orders() { ONO = -500, TOT = checkValue }, DateTime.Now.Date);

                            }
                            else
                            {
                                l3.Credit = l3.Credit.GetValueOrDefault(0) + l3comm;
                                l3.CheckCounter = l3.CheckCounter.GetValueOrDefault(0) + l3comm;
                                ccomm -= l3comm;
                                NM.CtrateNMTransaction(l3.ID.Value, l3.CUST_NAME, date, "SCOM", l3comm);
                            }
                            l3.Update();
                        }
                    }
                    if (customer.NM_REF_L4 != null && customer.NM_REF_L4 != "0")
                    {
                        EgxCustomer l4 = new EgxCustomer() { CUST_CODE = customer.NM_REF_L4 }.Search().FirstOrDefault();
                        if (l4.IS_ACTIVE.GetValueOrDefault(false) && l4.ACTIVE_PURCHASER.GetValueOrDefault(false))
                        {
                            if (l4.CheckCounter + l4comm >= deductedCheckGrade)
                            {
                                l4.CheckCounter = deductedCheckGrade - (l4.CheckCounter + l4comm);
                                //Handle 4th check here ............................
                                UpdateCustomerNetwork(l4, new Orders() { ONO = -500, TOT = checkValue }, DateTime.Now.Date);
                            }
                            else
                            {
                                l4.Credit = l4.Credit.GetValueOrDefault(0) + l4comm;
                                l4.CheckCounter = l4.CheckCounter.GetValueOrDefault(0) + l4comm;
                                ccomm -= l4comm;
                                NM.CtrateNMTransaction(l4.ID.Value, l4.CUST_NAME, date, "SCOM", l4comm);
                            }
                            l4.Update();
                        }
                    }
                    TransactionViewModel.CreateDeduction(order.ONO.Value, -ccomm, -101, -1, date, "خصم نسب الاعضاء");
                }
                else
                {
                    // do nothing
                }
               // customer.GIVE_COMMITION = false;
               // customer.Update();
            //}
        }

        public new SystemMessage ActivateCard(string code, int cid)
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
                    //Activate Customer and Update Card  .....................................................
                    cards[0].CID = cid;
                    cards[0].REG_DATE = DateTime.Now.Date;
                    cards[0].Update();
                    EgxCustomer customer = new EgxCustomer() { ID = cid }.GetByID();
                    customer.IS_ACTIVE = true;
                    customer.Update();
                    TransactionViewModel.CreateCashReceipt(-1, cards[0].CARD_VALUE.Value, 201, customer.ID.Value, DateTime.Now.Date, "تفعيل عضوية عميل");
                    sm.Message = "تم تفعيل حسابك بنجاح";
                    sm.Type = MessageType.Pass;
                    return sm;
                }
            }
            else
            {
                sm.Message = "رقم الكارت غير صحيح";
                sm.Type = MessageType.Stop;
            }
            return sm;
        }

        public new static SystemMessage isValidBankToPay(int customerID, DateTime transDate, decimal total)
        {
            string sql = "select * from NM_Transactions ";
            sql += "where CUSTOMER_ID='" + customerID + "' ";
            sql += "and MONTH(NM_Transactions.TRANS_DATE)>='" + transDate.Month.ToString() + "' and MONTH(NM_Transactions.TRANS_DATE)<'" + transDate.Month.ToString() + "'+1 ";
            List<NM_Transactions> lst = new EgxDataAccess.DataAccess().ExecuteSQL<NM_Transactions>(sql);
            if (lst.Where(tr => tr.TRANS_TYPE == "DSCOM" || tr.TRANS_TYPE == "SO").Count() > 0)
            {
                if (GetCreditFromTransactions(lst) >= total)
                {
                    return new SystemMessage() { Type = MessageType.Pass };
                }
                else { return new SystemMessage() { Type = MessageType.Stop, Message = "عفوا رصيدك لا يسمح" }; }
            }
            else { return new SystemMessage() { Type = MessageType.Stop, Message = "لا تستطيع الصرف من الرصيد ، يجب ان تنفذ عملية شراء واحدة او عملية بيع مباشر واحدة على الاقل" }; }
        }

        public new static Orders RegisterSalesOrderWeb(int customerID, string customerName, DateTime date, decimal total, decimal paid, List<Categ> OrderDetail, WEB_PAYMENT_METHOD paymentMethod, out SystemMessage systemMessage)
        {
            Orders order = null;
            systemMessage = new SystemMessage();
            EgxCustomer cust = new EgxCustomer() { ID = customerID }.GetByID();
                if (cust.Credit >= total)
                {
                    if (cust.IS_ACTIVE.GetValueOrDefault(false))
                    {
                        order = _RegisterSalesOrderWeb(customerID, customerName, date, total, paid, OrderDetail, paymentMethod);
                        cust.Credit = cust.Credit.GetValueOrDefault(0) - total;
                        cust.ACTIVE_PURCHASER = true;
                        cust.Update();
                        if (order != null) { systemMessage.Message = "تمت العملية بنجاح"; systemMessage.Type = MessageType.Pass; } else { systemMessage.Message = "فشل فى اتمام العملية برجاء المحاولة فى وقت لاحق"; systemMessage.Type = MessageType.Stop; }
                        return order;
                    }
                    else 
                    {
                        systemMessage.Message = "عفوا يجب تفعيل حسابك اولا"; 
                        systemMessage.Type = MessageType.Stop;
                        return null;
                    }
                }
                else 
                { 
                    systemMessage.Message = "عفوا رصيدك لا يسمح";
                    systemMessage.Type = MessageType.Stop;
                    return null; 
                }
        }

        public SystemMessage RechargeAccount(string code, int cid)
        {
            SystemMessage sm = new SystemMessage();
            var cards = new EgxDataAccess.DataAccess().ExecuteSQL<Cards>("select * from Cards where IS_RCG=1 AND CARD_CODE='" + code + "'").ToList();
            if (cards.Count == 1)
            {
                if (cards[0].CID.GetValueOrDefault() > 0)
                {
                    sm.Message = "هذا الكارت تم استخدامة بالفعل";
                    sm.Type = MessageType.Stop;
                }
                else
                {
                    //Add Card Credit to Customer's Account........................................................................................................................................................................................
                    EgxCustomer customer = new EgxCustomer() { ID = cid }.GetByID();
                    TranslatedCard card = TranslateCard(code);

                    customer.Credit = customer.Credit.GetValueOrDefault(0) + cards[0].CARD_VALUE.ToDecimal(0);
                    customer.Update();
                    TransactionViewModel.CreateCashReceipt(-1, cards[0].CARD_VALUE.ToDecimal(0), 201, customer.ID.Value, DateTime.Now.Date, "اضافة رصيد لعميل");
                    cards[0].CID = cid;
                    cards[0].REG_DATE = DateTime.Now.Date;
                    cards[0].Update();
                    sm.Type = MessageType.Pass;
                    sm.Attachment = customer.Credit;
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
    }
}
