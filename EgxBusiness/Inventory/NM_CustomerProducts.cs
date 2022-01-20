using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class NM_CustomerProducts
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public NM_CustomerProducts()
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
		/// Gets or sets the CID value.
		/// </summary>
		public virtual Int32? CID { get; set; }

		/// <summary>
		/// Gets or sets the PID value.
		/// </summary>
		public virtual Int32? PID { get; set; }

		/// <summary>
		/// Gets or sets the CNAME value.
		/// </summary>
		public virtual String CNAME { get; set; }

		/// <summary>
		/// Gets or sets the PNAME value.
		/// </summary>
		public virtual String PNAME { get; set; }

		/// <summary>
		/// Gets or sets the CATEG value.
		/// </summary>
		public virtual String CATEG { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "NM_CustomerProducts" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                NM_CustomerProducts test = new NM_CustomerProducts();
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
		public virtual List<NM_CustomerProducts> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<NM_CustomerProducts>().ToList();
                   }



		public virtual NM_CustomerProducts GetByID() {  
                     return (NM_CustomerProducts)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<NM_CustomerProducts> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<NM_CustomerProducts>().ToList();
                  }
		#endregion
	}
}