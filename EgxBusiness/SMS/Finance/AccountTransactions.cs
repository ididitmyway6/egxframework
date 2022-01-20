using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS.Finance
{

	[Serializable]
	public class AccountTransactions
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public AccountTransactions()
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
		/// Gets or sets the TransactionType value.
		/// </summary>
		public virtual Int32? TransactionType { get; set; }

		/// <summary>
		/// Gets or sets the Amount value.
		/// </summary>
		public virtual Decimal? Amount { get; set; }

		/// <summary>
		/// Gets or sets the SupType value.
		/// </summary>
		public virtual Int32? SupType { get; set; }

		/// <summary>
		/// Gets or sets the TransactionDate value.
		/// </summary>
		public virtual DateTime? TransactionDate { get; set; }

		/// <summary>
		/// Gets or sets the Comment value.
		/// </summary>
		public virtual String Comment { get; set; }

		/// <summary>
		/// Gets or sets the OrderID value.
		/// </summary>
		public virtual Int32? OrderID { get; set; }

        public virtual Boolean? IsBank { get; set; }

        public virtual DateTime? ACTUAL_DATE { get; set; }
		/// <summary>
		/// Gets or sets the TransClosed value.
		/// </summary>
		public virtual Int32? TransClosed { get; set; }

        public static decimal CurrentCredit { get { return decimal.Parse(DataAccess.GetSqlValue("select isnull(sum(amount),0) from AccountTransactions ", EgxDataType.Mssql).ToString()); } }

        public static decimal CurrentBankCredit { get { return decimal.Parse(DataAccess.GetSqlValue("select isnull(sum(amount),0) from AccountTransactions where IsBank=1 ", EgxDataType.Mssql).ToString()); } }

        public static decimal CurrentCashCredit { get { return decimal.Parse(DataAccess.GetSqlValue("select isnull(sum(amount),0) from AccountTransactions where IsBank=0 or IsBank is null", EgxDataType.Mssql).ToString()); } }
		
        #endregion

		#region Overrides
		public override String ToString()
		{
			return "AccountTransactions" ;
		}
		#endregion

		#region CRUD Methods


        public static decimal ToBank(decimal amount, DateTime date)
        {
            TransactionViewModel.CreateDeduction(-1, amount, -7, -1, date, "Transfered from cash ", false);
            TransactionViewModel.CreateCashReceipt(-1, amount, -7, -1, date, "Incoming credit to bank", true);
            return amount;
        }

        public static decimal ToCash(decimal amount, DateTime date)
        {
            TransactionViewModel.CreateCashReceipt(-1, amount, -7, -1, date, "Incoming credit to cash", false);
            TransactionViewModel.CreateDeduction(-1, amount, -7, -1, date, "Transfered from bank to cash", true);
            return amount;
        }

		public virtual SystemMessage Insert() 
                {
                    sm = new SystemMessage();
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
		public virtual List<AccountTransactions> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<AccountTransactions>().ToList();
                   }



		public virtual AccountTransactions GetByID() {  
                     return (AccountTransactions)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<AccountTransactions> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<AccountTransactions>().ToList();
                  }
		#endregion
	}
}