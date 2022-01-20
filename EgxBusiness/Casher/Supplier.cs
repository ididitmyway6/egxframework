using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
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