using Egx.EgxBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxBusiness.GL
{
    public class TRBBB
    {
        public List<Inventories> InventoriesList { get { return new Inventories().GetAll(); } }
        public List<Banks> BanksList { get {return  new Banks().GetAll(); } }
   
       // public List<Customer> CustomersList { get { return new Customer().GetAll(); } }
        public TRBBB() 
        {            
        }

        public List<Banks> GetBranchesList(int? parent) 
        {
            return new Banks() { PARENT= parent}.Search();
        }

        public SystemMessage UpdateInventory(int ID , Decimal? Credit) 
        {
            SystemMessage sm = new SystemMessage();
            try
            {
                Inventories inv = new Inventories() { ID = ID }.GetByID();
                if (Credit.Value == inv.INV_BEGIN_CREDIT) { MessageBox.Show("يجب ادخال قيمة جديدة لرصيد اول المدة"); return null; }
                inv.INV_BEGIN_CREDIT = Credit;
                inv.Update();
                AccountTransaction accTr = new AccountTransaction();
                accTr.SupType = inv.ID;
                accTr.TR_TYPE = "TR-BEGIN-INV";
                accTr.Type = 0;
                if (accTr.Search().Count > 0)
                {
                    AccountTransaction ac = accTr.Search()[0];
                    ac.Remark = "UPDATED";
                    ac.LastUpdate = DateTime.Now;
                    ac.Update();
                }
                accTr.Date = DateTime.Now;
                accTr.Amount = (Double)Credit;
                accTr.Insert();
                sm.Type = MessageType.Pass;
                sm.Message = "OK . .. ... ....";
            }
            catch(Exception ex) {
                sm.Type = MessageType.Fail;
                sm.Message = ex.Message;
            }
            return sm;
        }

        public SystemMessage DeleteTransactionsForInventory(int id) 
        {
            SystemMessage sm = new SystemMessage();
            List<AccountTransaction> lst = new AccountTransaction() { SupType=id }.Search();
            Inventories inv = new Inventories() {  ID=id}.GetByID();

            foreach (AccountTransaction accTr in lst) 
            {
                if (accTr.Remark != "DELETED")
                {
                if (accTr.Amount == (Double)inv.INV_BEGIN_CREDIT.Value )
                {
                    accTr.LastUpdate = DateTime.Now;
                    accTr.Remark = "DELETED";
                  sm =  accTr.Update();
                }
                else 
                {
                  sm =   accTr.Delete();
                }
                }
            }
            inv.INV_BEGIN_CREDIT = 0;
            inv.Update();
            sm.Message = "OK ....";
            return sm;
        }



        public SystemMessage UpdateBank(int ID, Decimal? Credit)
        {
            SystemMessage sm = new SystemMessage();
            try
            {
                Banks bank = new Banks() { ID = ID }.GetByID();
                if (Credit.Value == bank.BANK_BEGIN_CREDIT) { MessageBox.Show("يجب ادخال قيمة جديدة لرصيد اول المدة"); return null; }
                bank.BANK_BEGIN_CREDIT = Credit;
                bank.Update();
                AccountTransaction accTr = new AccountTransaction();
                accTr.SupType = bank.ID;
                accTr.TR_TYPE = "TR-BEGIN-BANK";
                accTr.Type = 0;
                if (accTr.Search().Count > 0)
                {
                    AccountTransaction ac = accTr.Search()[0];
                    ac.Remark = "UPDATED";
                    ac.LastUpdate = DateTime.Now;
                    ac.Update();
                }
                accTr.Date = DateTime.Now;
                accTr.Amount = (Double)Credit;
                accTr.Insert();
                sm.Type = MessageType.Pass;
                sm.Message = "OK . .. ... ....";
            }
            catch (Exception ex)
            {
                sm.Type = MessageType.Fail;
                sm.Message = ex.Message;
            }
            return sm;
        }

        public SystemMessage DeleteTransactionsForBank(int id)
        {
            SystemMessage sm = new SystemMessage();
            List<AccountTransaction> lst = new AccountTransaction() { SupType = id }.Search();
            Banks bank = new Banks() { ID = id }.GetByID();

            foreach (AccountTransaction accTr in lst)
            {
                if (accTr.Remark != "DELETED")
                {
                    if (accTr.Amount == (Double)bank.BANK_BEGIN_CREDIT.Value)
                    {
                        accTr.LastUpdate = DateTime.Now;
                        accTr.Remark = "DELETED";
                        sm = accTr.Update();
                    }
                    else
                    {
                        sm = accTr.Delete();
                    }
                }
            }
            bank.BANK_BEGIN_CREDIT = 0;
            bank.Update();
            sm.Message = "OK ....";
            return sm;
        }

    }
}
