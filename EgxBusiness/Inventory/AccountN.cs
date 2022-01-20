using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class AccountN
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public AccountN()
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
		/// Gets or sets the NAME value.
		/// </summary>
		public virtual String NAME { get; set; }

		/// <summary>
		/// Gets or sets the NAT value.
		/// </summary>
		public virtual Int32? NAT { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "AccountN";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   AccountN test = new AccountN();
                   test.NAME = this.NAME;
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

		
		public virtual List<AccountN> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<AccountN>().ToList();
                   }



		public virtual AccountN GetByID() {  
                     return (AccountN)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<AccountN> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<AccountN>().ToList();
                  }
	#endregion
	}
}