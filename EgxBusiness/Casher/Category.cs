using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class Category
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction

                public Category()
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
		/// Gets or sets the CategoryName value.
		/// </summary>
		public virtual String CategoryName { get; set; }

		/// <summary>
		/// Gets or sets the Description value.
		/// </summary>
		public virtual String Description { get; set; }

		/// <summary>
		/// Gets or sets the CategoryDiscount value.
		/// </summary>
		public virtual float? CategoryDiscount { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Categories";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                {
                    Category test = new Category();
                    test.CategoryName = this.CategoryName;
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


        public virtual List<Category> Search() 
                   {
                       return new DataAccess().Search(this).Cast<Category>().ToList();
                   }



        public virtual Category GetByID()
        {
            return (Category)new DataAccess().GetByID(ID.Value, this);
                  }


        public virtual List<Category> GetAll() 
                  {
                      return new DataAccess().GetTopItems(100, this).Cast<Category>().ToList();
                  }
	#endregion
	}
}