using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness;
using Egx;
namespace Egx.EgxBusiness.GL
{

	[Serializable]
	public class Journal
	{
	            public DataAccess da;
                public SystemMessage sm;

                int sno = 0;
		#region Construction
       public Journal()
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
		/// Gets or sets the DEBIT value.
		/// </summary>
		public virtual Decimal? DEBIT { get; set; }

		/// <summary>
		/// Gets or sets the CREDIT value.
		/// </summary>
		public virtual Decimal? CREDIT { get; set; }

		/// <summary>
		/// Gets or sets the COMMENT value.
		/// </summary>
        public virtual String COMMENT { get; set; }

        public virtual String AccountName { get; set; }

        public virtual String ParentName { get; set; }

        public virtual String GrandName { get; set; }

		/// <summary>
		/// Gets or sets the REG_DATE value.
		/// </summary>
		public virtual DateTime? REG_DATE { get; set; }

		/// <summary>
		/// Gets or sets the EVENT_DATE value.
		/// </summary>
		public virtual DateTime? EVENT_DATE { get; set; }

		/// <summary>
		/// Gets or sets the ACTION_ID value.
		/// </summary>
		public virtual Int32? ACTION_ID { get; set; }

		/// <summary>
		/// Gets or sets the SNO value.
		/// </summary>
		public virtual Int32? SNO { get; set; }

		/// <summary>
		/// Gets or sets the TR_CODE value.
		/// </summary>
		public virtual Int32? TR_CODE { get; set; }

		/// <summary>
		/// Gets or sets the ACC_ID value.
		/// </summary>
		public virtual Int32? ACC_ID { get; set; }

        public virtual Int32? PARENT_ACC_ID { get; set; }

        public virtual Int32? CHILD_ACC_ID { get; set; }

        public virtual Int64? CHQ_NO { get; set; }

        public virtual Int32? CHQ_STATUS { get; set; }

        public int MaxActionID { get { return GetMaxJournal(); } }
		#endregion


		#region Overrides
		public override String ToString()
		{
			return "Journal" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
        {
            try
            {
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
            }
            catch (Exception ex) { }
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
		public virtual List<Journal> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Journal>().ToList();
                   }



		public virtual Journal GetByID() {  
                     return (Journal)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Journal> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Journal>().ToList();
                  }
		#endregion 

            public static int GetMaxJournal()
            {
                EgxDataType dtype = (DataAccess.__db_swtich_mode) ? EgxDataType.GLConnection : EgxDataType.Mssql;
                int maxAction = (int)DataAccess.GetSqlValue("select isnull(max(ACTION_ID),0) from Journal", dtype);
                return maxAction;
            }
            public static int GetMaxJournal(int year)
            {
                EgxDataType dtype = (DataAccess.__db_swtich_mode) ? EgxDataType.GLConnection : EgxDataType.Mssql;
                int maxAction = (int)DataAccess.GetSqlValue("select isnull(max(ACTION_ID),0) from Journal where YEAR(REG_DATE)=" + year.ToString(),dtype);
                return maxAction;
            }


            public SystemMessage CreateDebitRecord(int actionID,int? transactionID,Decimal? balance, int? accountID, int? parentAccount,string comment=null, int? childAccount = null,string checkNumber="0")
            {
                sm = new SystemMessage();
                this.DEBIT = balance;
                this.ACC_ID = accountID;
                this.PARENT_ACC_ID = parentAccount;
                this.CHILD_ACC_ID = childAccount;
                this.REG_DATE = DateTime.Now;
                this.ACTION_ID =actionID;
                this.TR_CODE = transactionID;
                this.COMMENT = comment;
                da.Insert(this);
                
                return sm;
            }

            public SystemMessage CreateCreditRecord(int actionID, int? transactionID, Decimal? balance, int? accountID, int? parentAccount = null, string comment = null, int? childAccount = null, string checkNumber ="0")
            {
                sm = new SystemMessage();
                this.CREDIT = balance;
                this.ACC_ID = accountID;
                this.PARENT_ACC_ID = parentAccount;
                this.CHILD_ACC_ID = childAccount;
                this.REG_DATE = DateTime.Now;
                this.ACTION_ID = actionID;
                this.TR_CODE = transactionID;
                this.COMMENT = comment;
                da.Insert(this);
                return sm;
            }

            
            
    
    }
}