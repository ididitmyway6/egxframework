using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Gifts
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Gifts()
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
		/// Gets or sets the CODE value.
		/// </summary>
		public virtual String CODE { get; set; }

		/// <summary>
		/// Gets or sets the PRODUCT_ID value.
		/// </summary>
		public virtual Int32? PRODUCT_ID { get; set; }

		/// <summary>
		/// Gets or sets the CID value.
		/// </summary>
		public virtual Int32? CID { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Gifts" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Gifts test = new Gifts();
                   test.CODE = this.CODE;
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
		public virtual List<Gifts> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Gifts>().ToList();
                   }



		public virtual Gifts GetByID() {  
                     return (Gifts)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Gifts> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Gifts>().ToList();
                  }

            public virtual Gifts GetByCode(string code) 
            {
                return (Gifts)new DataAccess().GetByColumn("CODE", code, this);
            }
		#endregion
	}
}