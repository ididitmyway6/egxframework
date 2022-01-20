using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness;

namespace Egx.EgxBusiness.GL
{

	[Serializable]
	public class AccountsTree
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
                public AccountsTree()
                {

                    da = new DataAccess();
                    sm = new SystemMessage();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
        public virtual Int32? ID { get; set; }
        public virtual Int32? SNO { get; set; }

        /// <summary>
        /// Gets or sets the ACCOUNT_NAME value.
        /// </summary>
        public virtual String ACCOUNT_NAME { get; set; }

        /// <summary>
        /// Gets or sets the ACCOUNT_NAME value.
        /// </summary>
        public virtual String CODE { get; set; }

		/// <summary>
		/// Gets or sets the DESCRIPTION value.
		/// </summary>
		public virtual String DESCRIPTION { get; set; }

		/// <summary>
		/// Gets or sets the CREDIT value.
		/// </summary>
		public virtual Decimal? CREDIT { get; set; }

		/// <summary>
		/// Gets or sets the PARENT_ACCOUNT value.
		/// </summary>
		public virtual Int32? PARENT_ACCOUNT { get; set; }

        /// <summary>
        /// Gets or sets the CHILD_ACCOUNT value.
        /// </summary>
        public virtual Int32? CHILD_ACCOUNT { get; set; }

        /// <summary>
        /// Gets or sets the REL_ACCOUNT value.
        /// </summary>
        public virtual Int32? REL_ACCOUNT { get; set; }

        /// <summary>
        /// Gets or sets the FULL_ID value.
        /// </summary>
        public virtual Int32? FULL_ID { get; set; }


        /// <summary>
        /// Gets or sets the FULL_ID value.
        /// </summary>
        public virtual Int32? LEV { get; set; }
        public virtual Int32? LEV5 { get; set; }

        public virtual Boolean? ACC_CYC { get; set; }

		/// <summary>
		/// Gets or sets the SYSTEM_DEFINED value.
		/// </summary>
		public virtual Boolean? SYSTEM_DEFINED { get; set; }
        public virtual Boolean? IS_ACTIVE { get; set; }
        public virtual Boolean? IS_COLLECTOR { get; set; }
        public virtual Boolean? isTrialBalance { get; set; }
        public virtual Boolean? haveSupAccounts { get; set; }
        public virtual Boolean? ALLOW_TRANS { get; set; }
        #endregion

		#region Overrides
		public override String ToString()
		{
			return "AccountsTree" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                {
                    AccountsTree test = new AccountsTree();
                    AccountsTree test2 = new AccountsTree();
                    test.ACCOUNT_NAME = this.ACCOUNT_NAME;
                    test2.CODE = this.CODE;
                   if (da.IsExists(test))
            {
                sm.Message = "ÌÊÃœ Õ”«» »‰›” «·«”„ °° »—Ã«¡ «·œﬁ… ›Ï  ”„Ì… «·Õ”«» Ê  „ÌÌ“Â";
                sm.Type = MessageType.Stop;
            }
                   else if (da.IsExists(test2)) 
                   {
                       sm.Message = "ÌÊÃœ Õ”«» »‰›” «·ﬂÊœ °° »—Ã«¡ «·œﬁ… ›Ï  ﬂÊÌœ «·Õ”«»« ";
                       sm.Type = MessageType.Stop;
                   }
                   else
                   {

                       if (da.Insert(this) == 1)
                       {
                           sm.Message = "OK ...";
                           sm.Type = MessageType.Pass;
                           sm.Attachment = DataAccess.GetLatest("AccountsTree", "ID");
                       }
                   }
                   return sm;
                }

        public virtual SystemMessage Update()
        {
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

        public virtual SystemMessage UpdateByCode()
        {
            try
            {
                da.Update(this, "CODE");
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
		public virtual List<AccountsTree> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<AccountsTree>().ToList();
                   }



		public virtual AccountsTree GetByID() {  
                     return (AccountsTree)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<AccountsTree> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<AccountsTree>().ToList();
                  }
		#endregion

            public static int GetLastSerial() 
            {
                return DataAccess.GetLatest("AccountsTree", "SNO");
            }
            public static AccountsTree GetByCode(string code) 
            {
                return new AccountsTree() { CODE=code }.Search().FirstOrDefault();
            }
            public bool HaveValues() 
            {
                return DataAccess.GetSqlValue("select isnull(count(*),0) ", EgxDataType.Mssql).ToInt32() > 0;
            }
            public static SystemMessage DeleteAccount(int accountID)
            {
                SystemMessage sm = new SystemMessage();
                try
                {
                    AccountsTree a = new AccountsTree() { ID=accountID }.GetByID();
                    if (a.LEV == 2)
                    {
                        DataAccess.ExecData("DELETE FROM AccountsTree WHERE ID=" + accountID + " OR  CHILD_ACCOUNT=" + accountID + "  ");
                    }
                    if (a.LEV == 3)
                    {
                        DataAccess.ExecData("DELETE FROM AccountsTree WHERE ID=" + accountID + " OR  REL_ACCOUNT=" + accountID + "  ");
                    }
                    if (a.LEV == 4)
                    {
                        DataAccess.ExecData("DELETE FROM AccountsTree WHERE ID=" + accountID + " OR  LEV5=" + accountID + "  ");
                    }
                        sm.Type = MessageType.Pass;
                }
                catch { sm.Type = MessageType.Fail; }
                return sm;
            }
              
	}
}