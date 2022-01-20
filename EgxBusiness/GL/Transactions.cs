using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness;

namespace Egx.EgxBusiness.GL
{

	[Serializable]
	public class Transactions
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Transactions()
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
		/// Gets or sets the TransactionsName value.
		/// </summary>
		public virtual String TransactionsName { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Transactions" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Transactions test = new Transactions();
                   test.TransactionsName = this.TransactionsName;
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
		public virtual List<Transactions> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Transactions>().ToList();
                   }



		public virtual Transactions GetByID() {  
                     return (Transactions)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Transactions> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Transactions>().ToList();
                  }
		#endregion
	}
}