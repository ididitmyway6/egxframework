using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class DeductionCategory
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public DeductionCategory()
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
		/// Gets or sets the DeductionName value.
		/// </summary>
		public virtual String DeductionName { get; set; }

		/// <summary>
		/// Gets or sets the CreditLimit value.
		/// </summary>
		public virtual Double? CreditLimit { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "DeductionCategories";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   DeductionCategory test = new DeductionCategory();
                   test.DeductionName = this.DeductionName;
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

		
		public virtual List<DeductionCategory> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<DeductionCategory>().ToList();
                   }



		public virtual DeductionCategory GetByID() {  
                     return (DeductionCategory)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<DeductionCategory> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<DeductionCategory>().ToList();
                  }
	#endregion
	}
}