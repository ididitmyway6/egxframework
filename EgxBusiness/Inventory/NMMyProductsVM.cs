using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class NMMyProductsVM
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
                public NMMyProductsVM()
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
		public virtual String PRODUCT_NAME { get; set; }

		/// <summary>
		/// Gets or sets the PID value.
		/// </summary>
		public virtual Int32? OTYPE { get; set; }

		/// <summary>
		/// Gets or sets the CNAME value.
		/// </summary>
		public virtual Int32? CUSTOMER { get; set; }

		/// <summary>
		/// Gets or sets the PNAME value.
		/// </summary>
		public virtual DateTime? ODATE { get; set; }

		/// <summary>
		/// Gets or sets the CATEG value.
		/// </summary>
		public virtual Int32? PRODUCT_ID { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
            return "NMMyProductsVM";
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                {
                    NMMyProductsVM test = new NMMyProductsVM();
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
        public virtual List<NMMyProductsVM> Search() 
                   {
                       return new DataAccess().Search(this).Cast<NMMyProductsVM>().ToList();
                   }



        public virtual NMMyProductsVM GetByID()
        {
            return (NMMyProductsVM)new DataAccess().GetByID(ID.Value, this);
                  }


        public virtual List<NMMyProductsVM> GetAll() 
                  {
                      return new DataAccess().GetTopItems(100, this).Cast<NMMyProductsVM>().ToList();
                  }
		#endregion
	}
}