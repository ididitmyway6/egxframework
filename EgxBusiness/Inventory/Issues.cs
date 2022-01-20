using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Issues
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Issues()
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
		/// Gets or sets the IssueOwner value.
		/// </summary>
		public virtual Int32? IssueOwner { get; set; }

		/// <summary>
		/// Gets or sets the Accused value.
		/// </summary>
		public virtual Int32? Accused { get; set; }

		/// <summary>
		/// Gets or sets the TransactionType value.
		/// </summary>
		public virtual Int32? TransactionType { get; set; }

		/// <summary>
		/// Gets or sets the IssueText value.
		/// </summary>
		public virtual String IssueText { get; set; }

		/// <summary>
		/// Gets or sets the IssueType value.
		/// </summary>
		public virtual Int32? IssueType { get; set; }
        public virtual DateTime? IssueDate { get; set; }
        public virtual Int32? OrderID { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Issues" ;
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
		public virtual List<Issues> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Issues>().ToList();
                   }



		public virtual Issues GetByID() {  
                     return (Issues)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Issues> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100000,this).Cast<Issues>().ToList();
                  }
		#endregion
	}
}