using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class Product
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Product()
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
		/// Gets or sets the Code value.
		/// </summary>
		public virtual String Code { get; set; }

		/// <summary>
		/// Gets or sets the ProductName value.
		/// </summary>
		public virtual String ProductName { get; set; }

		/// <summary>
		/// Gets or sets the Unit value.
		/// </summary>
		public virtual String Unit { get; set; }

		/// <summary>
		/// Gets or sets the Description value.
		/// </summary>
		public virtual String Description { get; set; }

		/// <summary>
		/// Gets or sets the Price value.
		/// </summary>
		public virtual float? Price { get; set; }

		/// <summary>
		/// Gets or sets the Discount value.
		/// </summary>
		public virtual float? Discount { get; set; }

		/// <summary>
		/// Gets or sets the Category value.
		/// </summary>
		public virtual Int32? Category { get; set; }

		/// <summary>
		/// Gets or sets the ReorderPoint value.
		/// </summary>
		public virtual Int32? ReorderPoint { get; set; }

		/// <summary>
		/// Gets or sets the IsInventory value.
		/// </summary>
		public virtual Boolean? IsInventory { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Products";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   Product test = new Product();
                   test.Code = this.Code;
                   if (da.IsExists(test))
            {
                sm.Message = "Already Exist ... ";
                sm.Type = MessageType.Stop;
            }
            else 
            {
                if (da.Insert(this) > 0)
                {
                    Stock stock = new Stock();
                    stock.Category = this.Category.Value;
                    stock.Product = this.Code;
                    stock.IsInventory = this.IsInventory.Value;
                    stock.ReorderPoint = this.ReorderPoint;
                    stock.Discount = this.Discount.Value;
                    stock.Insert();
                    sm.Message = "OK ...";
                    sm.Type = MessageType.Pass;
                }
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

		
		public virtual List<Product> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Product>().ToList();
                   }



		public virtual Product GetByID() {  
                     return (Product)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Product> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Product>().ToList();
                  }
	#endregion
	}
}