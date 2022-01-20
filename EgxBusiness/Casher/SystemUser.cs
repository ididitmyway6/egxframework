using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class SystemUser
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public SystemUser()
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
		/// Gets or sets the SystemUsername value.
		/// </summary>
		public virtual String SystemUsername { get; set; }

		/// <summary>
		/// Gets or sets the SystemPassword value.
		/// </summary>
		public virtual String SystemPassword { get; set; }

		/// <summary>
		/// Gets or sets the SystemDisplayName value.
		/// </summary>
		public virtual String SystemDisplayName { get; set; }

		/// <summary>
		/// Gets or sets the DefaultLocation value.
		/// </summary>
		public virtual Int32? DefaultLocation { get; set; }

		/// <summary>
		/// Gets or sets the InventoryPermissions value.
		/// </summary>
		public virtual Boolean? InventoryPermissions { get; set; }

		/// <summary>
		/// Gets or sets the ProdCatPermissions value.
		/// </summary>
		public virtual Boolean? ProdCatPermissions { get; set; }

		/// <summary>
		/// Gets or sets the OrderPermissions value.
		/// </summary>
		public virtual Boolean? OrderPermissions { get; set; }

		/// <summary>
		/// Gets or sets the InvoicePermissions value.
		/// </summary>
		public virtual Boolean? InvoicePermissions { get; set; }

		/// <summary>
		/// Gets or sets the UserMgtPermissions value.
		/// </summary>
		public virtual Boolean? UserMgtPermissions { get; set; }

		/// <summary>
		/// Gets or sets the SuppliersPermissions value.
		/// </summary>
		public virtual Boolean? SuppliersPermissions { get; set; }

		/// <summary>
		/// Gets or sets the PurchasePermissions value.
		/// </summary>
		public virtual Boolean? PurchasePermissions { get; set; }

		/// <summary>
		/// Gets or sets the ShiftPermissions value.
		/// </summary>
		public virtual Boolean? ShiftPermissions { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "SystemUsers";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   SystemUser test = new SystemUser();
                   test.SystemDisplayName = this.SystemDisplayName;
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

		
		public virtual List<SystemUser> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<SystemUser>().ToList();
                   }



		public virtual SystemUser GetByID() {  
                     return (SystemUser)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<SystemUser> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<SystemUser>().ToList();
                  }
	#endregion
	}
}