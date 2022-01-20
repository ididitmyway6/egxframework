using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness;

namespace Egx.EgxBusiness.GL
{

	[Serializable]
	public class Banks
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
                public Banks()
		{
		    da = new DataAccess();
            sm=  new SystemMessage();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the INV_CODE value.
		/// </summary>
		public virtual Int32? ID { get; set; }

        /// <summary>
        /// Gets or sets the INV_NAME value.
        /// </summary>
        public virtual Int32? PARENT { get; set; }

        /// <summary>
        /// Gets or sets the INV_NAME value.
        /// </summary>
        public virtual String BANK_NAME { get; set; }

        public virtual Int32? BRANCH { get; set; }
        /// <summary>
        /// Gets or sets the CODE value.
        /// </summary>
        public virtual String CODE { get; set; }

		/// <summary>
		/// Gets or sets the INV_BEGIN_CREDIT value.
		/// </summary>
        public virtual Decimal? BANK_BEGIN_CREDIT { get; set; }

		/// <summary>
		/// Gets or sets the NOTES value.
		/// </summary>
		public virtual String NOTES { get; set; }

        /// <summary>
        /// Gets or sets the NOTES value.
        /// </summary>
        public virtual Int32? CHILD { get; set; }

		/// <summary>
		/// Gets or sets the DEP_DATE value.
		/// </summary>
		public virtual DateTime? DEP_DATE { get; set; }

        public virtual Boolean? IS_MINE { get; set; }

        public virtual Int32? REL_ACC { get; set; }
        public virtual float? DEDUCTION_PERCENTAGE { get; set; }

		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Banks" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Banks test = new Banks();
                   test.CODE = this.CODE;
                   if (da.IsExists(test))
            {
                sm.Message = "Already Exist ... ";
                sm.Type = MessageType.Stop;
            }
            else 
            {
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
            }
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
		public virtual List<Banks> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Banks>().ToList();
                   }



		public virtual Banks GetByID() {  
                     return (Banks)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Banks> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Banks>().ToList();
                  }
		#endregion

            public static void AccountsTreeSync()
            {
                AccountsTree glBankAccount = AccountsTree.GetByCode(EgxSetting.Get("GLBankAccount"));
                if (glBankAccount != null)
                {
                    DataAccess.ExecData("delete  from Banks where ID<>null");
                    List<AccountsTree> bankAccounts = new List<AccountsTree>();
                    bankAccounts = new AccountsTree() { REL_ACCOUNT = glBankAccount.ID }.Search();
                    foreach (AccountsTree bank in bankAccounts)
                    {
                        if (bank.haveSupAccounts.GetValueOrDefault(false))
                        {
                            bankAccounts.Remove(bank);
                        }
                        else
                        {
                            new Banks() { CODE = bank.CODE, BANK_NAME = bank.ACCOUNT_NAME }.Insert();
                        }

                    }
                }
                else { }
            }
	}
}