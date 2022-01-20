using Egx.EgxDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxBusiness.Inventory
{
    public class EgxTransactions
    {
        static SystemMessage sm;
        public static SystemMessage UpdateCard(int customerID, int points) 
        {
            sm = new SystemMessage();
            Transactions trans = new Transactions();
            EgxCustomer customer = new EgxCustomer() {ID=customerID}.GetByID();
            trans.TRANS_DATE = DateTime.Now;
            if (points > 0)
            {
                trans.TRANS_NAME = "شحن بطاقة عميل";
                trans.TRANS_TYPE = -10;
            }
            else if (points < 0)
            {
                trans.TRANS_NAME = "خصم من بطاقة عميل";
                trans.TRANS_TYPE = -9;
            }
            else 
            {
                trans.TRANS_NAME = "تم تصفير البطاقة";
                trans.TRANS_TYPE = -8;
            }
            
            trans.REL_ACCOUNT = customerID;
            trans.TRANS_USER_NAME = DBConfig.UserName;
            trans.AMOUNT = points;
            trans.AMOUNT_BY_POINTS = true;
            customer.CURRENT_POINTS=customer.CURRENT_POINTS.Value+ points;
            customer.Update();
            MessageBox.Show(trans.Insert().Message,"رسالة النظام");
            return new SystemMessage() {  Type= MessageType.Pass, Message="تمت عملية تحديث بطاقة العميل بنجاح" };
        }

        public static SystemMessage DeductCard(int customerID, int points)
        {
            sm = new SystemMessage();
            Transactions trans = new Transactions();
            EgxCustomer customer = new EgxCustomer() { ID = customerID }.GetByID();
            trans.TRANS_DATE = DateTime.Now;
            trans.TRANS_NAME = "نقاط مرتجعة";
            trans.TRANS_TYPE = -4;
            trans.REL_ACCOUNT = customerID;
            trans.TRANS_USER_NAME = DBConfig.UserName;
            trans.AMOUNT = -points;
            trans.AMOUNT_BY_POINTS = true;
            if (customer.CURRENT_POINTS >= points)
            {
                customer.CURRENT_POINTS = customer.CURRENT_POINTS.Value - points;
                customer.Update();
                MessageBox.Show(trans.Insert().Message, "رسالة النظام");
                return new SystemMessage() { Type = MessageType.Pass, Message = "تمت العملية بنجاح " +Environment.NewLine+"القيمة المقتطعة هى" +points.ToString()};
            }
            else
            {
                return new SystemMessage() { Type = MessageType.Pass, Message = "القيمة المقتطعة اكبر من عدد النقاط مع العميل" };
            }
        }


    }
}
