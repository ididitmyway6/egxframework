using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness;
namespace Egx.EgxBusiness.GL
{
	

	[Serializable]
	public class Supplier
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Supplier()
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
		/// Gets or sets the FullName value.
		/// </summary>
		public virtual String FullName { get; set; }

		/// <summary>
		/// Gets or sets the City value.
		/// </summary>
		public virtual String City { get; set; }

		/// <summary>
		/// Gets or sets the Country value.
		/// </summary>
		public virtual String Country { get; set; }

		/// <summary>
		/// Gets or sets the Phone value.
		/// </summary>
		public virtual String Phone { get; set; }

		/// <summary>
		/// Gets or sets the Email value.
		/// </summary>
		public virtual String Email { get; set; }

		/// <summary>
		/// Gets or sets the Address value.
		/// </summary>
		public virtual String Address { get; set; }

		/// <summary>
		/// Gets or sets the IsBlackListed value.
		/// </summary>
		public virtual Boolean? BlackList { get; set; }
        public virtual int? ExpenseAccount { get; set; }
        public virtual int? REL_ACC { get; set; }
        public virtual decimal? CreditLimit { get; set; }
        public virtual String Phone2 { get; set; }
        public virtual String Fax { get; set; }
        public virtual String Website { get; set; }
        public virtual string CODE { get; set; }


		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Suppliers";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   Supplier test = new Supplier();
                   test.FullName = this.FullName;
                   if (da.IsExists(test))
            {
                sm.Message = "Already Exist ... ";
                sm.Type = MessageType.Stop;
            }
            else 
            {
                if (this.REL_ACC != null) 
                {
                    AccountsTree a = new AccountsTree() { ID = REL_ACC }.GetByID();
                    if (a.LEV == 3) 
                    {
                        new AccountsTree() { LEV=4,REL_ACCOUNT=a.ID,CHILD_ACCOUNT=a.CHILD_ACCOUNT,PARENT_ACCOUNT=a.PARENT_ACCOUNT,CODE=this.CODE,ACCOUNT_NAME=this.FullName }.Insert();
                    } 
                    else if (a.LEV == 4) 
                    {
                        new AccountsTree() { LEV = 5, LEV5 = a.ID,REL_ACCOUNT=a.REL_ACCOUNT, CHILD_ACCOUNT = a.CHILD_ACCOUNT, PARENT_ACCOUNT = a.PARENT_ACCOUNT, CODE = this.CODE,ACCOUNT_NAME=this.FullName }.Insert();
                    }
                }
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

		
		public virtual List<Supplier> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Supplier>().ToList();
                   }



		public virtual Supplier GetByID() {  
                     return (Supplier)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Supplier> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Supplier>().ToList();
                  }
	#endregion
	}
}