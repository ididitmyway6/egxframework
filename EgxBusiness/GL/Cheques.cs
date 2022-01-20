using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness;
using Egx.EgxBusiness.GL;

namespace Egx.EgxBusiness.GL
{

	[Serializable]
	public class Cheques
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Cheques()
		{
		    da = new DataAccess();
            sm=  new SystemMessage();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
		public virtual Int32? ID { get; set; }

		/// <summary>
		/// Gets or sets the CODE value.
		/// </summary>
		public virtual string CODE { get; set; }

		/// <summary>
		/// Gets or sets the CUSTOMER_ID value.
		/// </summary>
		public virtual Int32? CUSTOMER_ID { get; set; }

        /// <summary>
        /// Gets or sets the INVBANK value.
        /// </summary>
        public virtual Int32? CASH { get; set; }

        /// <summary>
        /// Gets or sets the EXCHANGE_BANK value.
        /// </summary>
        public virtual string EXCHANGE_BANK { get; set; }

		/// <summary>
		/// Gets or sets the BANK value.
		/// </summary>
		public virtual Int32? BANK { get; set; }

		/// <summary>
		/// Gets or sets the CRUAL_DATE value.
		/// </summary>
		public virtual DateTime? AQ_DATE { get; set; }

		/// <summary>
		/// Gets or sets the IS_FIRST_BENIFIC value.
		/// </summary>
        public virtual Boolean? IS_FIRST_BENIFIC { get; set; }
        public virtual Boolean paid { get; set; }


		/// <summary>
		/// Gets or sets the IS_LINED value.
		/// </summary>
		public virtual Boolean? IS_LINED { get; set; }
        public virtual DateTime? RECEIVING_DATE { get; set; }
        public virtual Int32? NOTE_TYPE { get; set; }
        public virtual string RECIPIENT_NAME { get; set; }
        public virtual Int32? SUPPLIER_ID { get; set; }
        public virtual string STATE { get; set; }
        public virtual float? BANK_EXPENSE { get; set; }
        public virtual decimal? NET { get; set; }
        public virtual decimal? Balance { get; set; }
        public virtual string COMMENT { get; set; }
        public virtual Int32? CHECKING_ACCOUNT { get; set; }
        public virtual Int32? BANK_EXPENSE_ACCOUNT { get; set; }
        public virtual Int32? CHECKS_UNDERCOLLECTION { get; set; }


		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Cheques" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
                   return sm;
                }
				
		public virtual SystemMessage Update() { 
            try
                {
                    da.Update(this, "ID");
                    sm.Message = "OK ...";
                    sm.Type = MessageType.Pass;
                }
            catch
                {
                    sm.Message = "Error"; sm.Type = MessageType.Fail;
                }
                    return sm;
                  }
				  
		public virtual SystemMessage Delete() { 
                              try {
                                    da.Delete(this, "ID");
                                    sm.Message = "OK ...";
                                    sm.Type = MessageType.Pass;
                                  }
                              catch { sm.Message = "Error ...";
                                      sm.Type = MessageType.Fail;
                                     }
                           return sm;
                    }
		public virtual List<Cheques> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Cheques>().ToList();
                   }



		public virtual Cheques GetByID() {  
                     return (Cheques)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Cheques> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Cheques>().ToList();
                  }
		#endregion

        #region Business Methods

            public SystemMessage RegisterNewCheque() 
            {
                sm = new SystemMessage();
                this.Insert();
                return sm;
            }

            public SystemMessage ReceiveReceivableNotes(string rec_name,DateTime aqDate,DateTime rec_date,decimal? balance,string chequeNo,string exBank,bool isLined,bool isBen,int bankExpenseAccount,int checking_account=-1,int checkingUnderCollecting=-1,int customer=-1,int supplier=-1,Banks bank=null,int cash=-1,string comment=null) 
            {
                int? bankID = null;
                if (bank != null) { bankID = AccountsTree.GetByCode(bank.CODE).ID; } else { bank = new Banks(); }
                Journal debit = new Journal();
                Journal credit = new Journal();
                sm = new SystemMessage();
                int maxAction = debit.MaxActionID+1;
                this.CODE = chequeNo;
                this.AQ_DATE = aqDate.Date;
                this.BANK = bankID;
                this.CUSTOMER_ID = customer;
                this.EXCHANGE_BANK = exBank;
                this.CASH = cash;
                this.IS_LINED = isLined;
                this.IS_FIRST_BENIFIC = isBen;
                this.RECEIVING_DATE = rec_date.Date;
                this.NOTE_TYPE = 1;
                this.SUPPLIER_ID = supplier;
                this.RECIPIENT_NAME = rec_name;
                this.STATE = "PENDING";
                this.CHECKING_ACCOUNT = checking_account;
                this.CHECKS_UNDERCOLLECTION = checkingUnderCollecting;
                this.BANK_EXPENSE_ACCOUNT = bankExpenseAccount;
                this.BANK_EXPENSE = bank.DEDUCTION_PERCENTAGE.GetValueOrDefault(0);
                this.Balance = balance;
                this.NET = this.Balance - Convert.ToDecimal((this.BANK_EXPENSE.GetValueOrDefault(0) / 100)) * this.Balance.GetValueOrDefault(0);
                RegisterNewCheque();
                if (bankID != null)
                {
                    new Journal().CreateDebitRecord(maxAction, 11, balance, checking_account, -1, comment,null,chequeNo);
                    new Journal().CreateCreditRecord(maxAction, 11, balance, customer, -1, comment,null,chequeNo);
                    int nextAction = maxAction++;
                    new Journal().CreateDebitRecord(nextAction, 11, balance, checkingUnderCollecting, -1, comment,null,chequeNo);
                    new Journal().CreateCreditRecord(nextAction, 11, balance, checking_account, -1, comment,null,chequeNo);
                }
                else 
                {
                    new Journal().CreateDebitRecord(maxAction, 11, balance, checking_account, -1, comment,null,chequeNo);
                    new Journal().CreateCreditRecord(maxAction, 11, balance, customer, -1, comment,null,chequeNo);
                }
                return sm;
            }

            public SystemMessage CheckCollecting(string checkNo,int checksUnderCollectingAccount,int bankExpenses,int checkingAccount,string comment) 
            {
                SystemMessage sm = new SystemMessage();
                Cheques ck = new Cheques() { CODE = checkNo, NOTE_TYPE = 1,STATE="PENDING" }.Search().LastOrDefault();
                if (ck != null)
                {
                    Journal debit = new Journal();
                    Journal credit = new Journal();
                    int newAction = Journal.GetMaxJournal(ck.AQ_DATE.Value.Year) + 1;
                    if (ck.BANK != null)
                    {
                        debit.CreateDebitRecord(newAction, 12, ck.Balance - ck.NET, bankExpenses, -1, comment, null, ck.CODE);
                        debit.CreateDebitRecord(newAction, 12, ck.NET, ck.BANK, -1, comment, null, ck.CODE);
                        credit.CreateCreditRecord(newAction, 12, ck.Balance, checksUnderCollectingAccount, -1, comment, null, ck.CODE);
                    }
                    else 
                    {
                        debit.CreateDebitRecord(newAction, 12, ck.Balance, ck.CASH, -1, comment, null, ck.CODE);
                        credit.CreateCreditRecord(newAction, 12, ck.Balance, checkingAccount , -1, comment, null, ck.CODE);

                    }
                    ck.STATE = "DONE";
                    ck.Update();
                }
                return sm;
            }
            
            public SystemMessage RejectReceivableNotes(string chequeCode,string comment) 
            {
                sm = new SystemMessage();
                Cheques ck = new Cheques() { CODE=chequeCode }.Search().LastOrDefault();
                if (ck != null)
                {
                    if (ck.STATE == "PENDING")
                    {
                        Journal debit = new Journal();
                        Journal credit = new Journal();
                        int sno = Journal.GetMaxJournal(ck.AQ_DATE.Value.Year)+1;
                        if (ck.BANK != null)
                        {
                            // «·«” ·«„ »‰ﬂÏ
                            debit.CreateDebitRecord(sno, 13, ck.NET.GetValueOrDefault(0), ck.CHECKING_ACCOUNT, -1, comment, null, ck.CODE);
                            debit.CreateDebitRecord(sno, 13, ck.Balance.GetValueOrDefault(0) - ck.NET.GetValueOrDefault(0), ck.BANK_EXPENSE_ACCOUNT, -1, comment, null, ck.CODE);
                            credit.CreateCreditRecord(sno, 13, ck.NET.GetValueOrDefault(0), ck.CHECKS_UNDERCOLLECTION, -1, comment, null, ck.CODE);
                            credit.CreateCreditRecord(sno, 13, ck.Balance.GetValueOrDefault(0) - ck.NET.GetValueOrDefault(0), ck.BANK, -1, comment, null, ck.CODE);
                            sno = Journal.GetMaxJournal(ck.AQ_DATE.Value.Year) + 1;
                            debit.CreateDebitRecord(sno, 13, ck.Balance.GetValueOrDefault(0), ck.CUSTOMER_ID, -1, comment, null, ck.CODE);
                            credit.CreateCreditRecord(sno, 13, ck.Balance.GetValueOrDefault(0), ck.CHECKING_ACCOUNT, -1, comment, null, ck.CODE);

                        }
                        else 
                        {
                            // «·«” ·«„ ‰ﬁœÏ
                            debit.CreateDebitRecord(sno, 13, ck.Balance.GetValueOrDefault(0), ck.CUSTOMER_ID, -1, comment, null, ck.CODE);
                            credit.CreateCreditRecord(sno, 13, ck.Balance.GetValueOrDefault(0), ck.CHECKING_ACCOUNT, -1, comment, null, ck.CODE);

                        }
                        ck.STATE = "REJECTED";
                        ck.Update();
                        }
                    else
                    {
                        if (ck.STATE == "DONE") sm.Message = "Â–« «·‘Ìﬂ  „ ’—›…"; else sm.Message = "Â–« «·‘Ìﬂ  „ —œÂ „‰ ﬁ»·";
                        sm.Type = MessageType.Stop;
                    }
                }
                else 
                {
                    sm.Message = "·« ÌÊÃœ ‘Ìﬂ »Â–« «·ﬂÊœ";
                    sm.Type = MessageType.Stop;
                }
                return sm;
            }

            public SystemMessage WriteCheck(DateTime writingDate,DateTime aqDate,decimal balance, string CheckNo,int supplier,string supplierName,AccountsTree bankAccount,int checkingAccount,string  comment) 
            {
                SystemMessage sm = new SystemMessage();
                Journal debit = new Journal();
                Journal credit = new Journal();
                this.CODE = CheckNo;
                this.CHECKING_ACCOUNT = checkingAccount;
                this.Balance = balance;
                this.AQ_DATE = aqDate.Date;
                this.BANK = bankAccount.ID;
                this.COMMENT = comment;
                this.NET = this.Balance;
                this.NOTE_TYPE = -1;
                this.RECEIVING_DATE = writingDate.Date;
                this.RECIPIENT_NAME = supplierName;
                this.STATE = "PENDING";
                this.SUPPLIER_ID = supplier;
                this.RegisterNewCheque();
                int action = Journal.GetMaxJournal(aqDate.Year)+1;
                debit.CreateDebitRecord(action, 14, balance, supplier, -1, comment, null, CheckNo);
                credit.CreateCreditRecord(action, 14, balance, checkingAccount, -1, comment, null, CheckNo);
                return sm;
            }

            public SystemMessage CheckPay(Cheques checkToPay,string comment) 
            {
                SystemMessage sm = new SystemMessage();
                checkToPay.STATE = "DONE";
                Journal debit = new Journal();
                Journal credit = new Journal();
                int action = Journal.GetMaxJournal(checkToPay.AQ_DATE.Value.Year) + 1;
                debit.CreateDebitRecord(action, 15, checkToPay.Balance, checkToPay.CHECKING_ACCOUNT, -1, comment, null, checkToPay.CODE);
                credit.CreateCreditRecord(action, 15, checkToPay.Balance, checkToPay.BANK, -1, comment, null, checkToPay.CODE);
                checkToPay.Update();
                return sm;
            }

            public SystemMessage CheckReturn(Cheques checkToReturn, string comment) 
            {
                SystemMessage sm = new SystemMessage();
                if (checkToReturn.STATE == "PENDING") 
                {
                    checkToReturn.STATE = "RETURNED";
                    Journal debit = new Journal();
                    Journal credit = new Journal();
                    int action = Journal.GetMaxJournal(checkToReturn.AQ_DATE.Value.Year) + 1;
                    debit.CreateDebitRecord(action, 15, checkToReturn.Balance, checkToReturn.CHECKING_ACCOUNT, -1, comment, null, checkToReturn.CODE);
                    credit.CreateCreditRecord(action, 15, checkToReturn.Balance, checkToReturn.SUPPLIER_ID, -1, comment, null, checkToReturn.CODE);
                    checkToReturn.Update();
                }
                return sm;
            }
        #endregion
    }
}