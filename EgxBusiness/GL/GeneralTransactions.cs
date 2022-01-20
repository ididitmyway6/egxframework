using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx;
using Egx.EgxDataAccess;
using Egx.EgxBusiness.GL;
using Egx.EgxBusiness;
namespace Egx.EgxBusiness.GL
{
    public class GeneralTransactions
    {         
        SystemMessage sm;
        private decimal _currentCredit=CreditNow();
        public decimal CurrentCredit { get { return _currentCredit; } set { onCreditChanging(null); _currentCredit = value; } }

        public  delegate void  CreditChanging(EventArgs arg);
        public  event CreditChanging CreditChangingEvent;
        public  virtual void onCreditChanging(EventArgs args) 
        {
            if (CreditChangingEvent != null) { CreditChangingEvent(args); }
        }
        //public SystemMessage ReceiveCash(DateTime actionDate, Customer customer, decimal balance,string comment, Banks bank = null, Inventories stock = null) 
        //{
           
        //    sm = new SystemMessage();
        //    int maxaction=Journal.GetMaxJournal()+1;
        //    Journal jDebit = new Journal();
        //    Journal jCredit = new Journal();
        //    jCredit.EVENT_DATE = actionDate;
        //    jCredit.SNO = 1;
        //    jCredit.TR_CODE = 3;
        //    jCredit.CreateCreditRecord(maxaction, 3, balance, customer.ID, customer.REL_ACC, comment);
        //    jDebit.EVENT_DATE = actionDate;
        //    jDebit.TR_CODE = 3;
        //    jDebit.SNO = 2;
        //    if (bank.ID != null)
        //    {
        //        jDebit.CreateDebitRecord(maxaction, 3, balance, bank.ID, bank.REL_ACC, comment);
        //    }
        //    else 
        //    {
        //        jDebit.CreateDebitRecord(maxaction, 3, balance, stock.ID, stock.REL_ACC, comment);
        //    }
            
        //    return sm; 
        //}

        public static SystemMessage ReceiveCash(DateTime actionDate, AccountsTree creidt, decimal balance, string commment, int bankID = -1,int cashID=-1, int? creditAccountParent = null)
        {
            SystemMessage sm = new SystemMessage();
            Journal j = new Journal();
            int maxJournal = Journal.GetMaxJournal() + 1;
            if (bankID != -1)
            {
                j.CreateDebitRecord(maxJournal, 1, balance, bankID, EgxSetting.Get("GLBankAccount").ToInt32(), commment);
            }
            else
            {
                AccountsTree acc = new AccountsTree() { CODE = EgxSetting.Get("GLCashAccount") }.Search().FirstOrDefault();
               // j.CreateDebitRecord(Journal.GetMaxJournal() + 1, 1, balance, cashID, bankID, commment);
                j.CreateDebitRecord(Journal.GetMaxJournal() + 1, 1, balance, cashID, acc.ID, commment);
            }
            j = new Journal();
            j.CreateCreditRecord(maxJournal, 1, balance, creidt.ID, creditAccountParent, commment);
            return sm;
        }

        public static SystemMessage Pay(DateTime actionDate, List<Journal> debitJournal, decimal balance, string commment, int bankID = -1, int cashID=-1,int? creditAccountParent = null)
        {
            SystemMessage sm = new SystemMessage();
            Journal j = new Journal();
            int maxJournal = Journal.GetMaxJournal() + 1;
            decimal debitSum = debitJournal.Sum(x => x.DEBIT).GetValueOrDefault(0);
            int cnt = debitJournal.Count();
            if (bankID != -1)
            {
                j.CreateCreditRecord(maxJournal, 2, debitSum, bankID, EgxSetting.Get("GLBankAccount").ToInt32(),cnt>1? "متمم عملية رقم"+Environment.NewLine+maxJournal.ToString(): debitJournal[0].COMMENT);
            }
            else
            {
                AccountsTree acc = new AccountsTree() { CODE = EgxSetting.Get("GLCashAccount") }.Search().FirstOrDefault();
                j.CreateCreditRecord(maxJournal, 2, debitSum, cashID, acc.ID, cnt > 1 ? "متمم عملية رقم" + Environment.NewLine + maxJournal.ToString() : debitJournal[0].COMMENT);
            }
            j = new Journal();
            foreach (Journal debitAccount in debitJournal)
            {
                debitAccount.ACTION_ID = maxJournal;
                debitAccount.Insert();
            }
            return sm;
        }
        public static decimal CreditNow()
        {
            return (decimal)DataAccess.GetSqlValue("select SUM(Journal.DEBIT)-SUM(Journal.CREDIT) from Journal", EgxDataType.Mssql);

        }
    }
}
