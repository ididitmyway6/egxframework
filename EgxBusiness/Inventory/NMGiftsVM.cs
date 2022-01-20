using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class NMGiftsVM
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
                public NMGiftsVM()
		{
		    da = new DataAccess();
            sm=  new SystemMessage();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
                public virtual Int32? PRODUCT_ID { get; set; }
                public virtual Int32? CID { get; set; }

		/// <summary>
		/// Gets or sets the CID value.
		/// </summary>
		public virtual String CODE { get; set; }

		/// <summary>
		/// Gets or sets the PID value.
		/// </summary>
		public virtual String CATEG_NAME { get; set; }

	
		#endregion

		#region Overrides
		public override String ToString()
		{
            return "NMGiftsVM";
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                {
                    NMGiftsVM test = new NMGiftsVM();
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
                                    da.Delete(this, "CODE");
                                    sm.Message = "OK ...";
                                    sm.Type = MessageType.Pass;
                                  }
                              catch { sm.Message = "Error ...";
                                      sm.Type = MessageType.Fail;
                                     }
                           return sm;
                    }
        public virtual List<NMGiftsVM> Search() 
                   {
                       return new DataAccess().Search(this).Cast<NMGiftsVM>().ToList();
                   }



        public virtual NMGiftsVM GetByID()
        {
            return (NMGiftsVM)new DataAccess().GetByID(PRODUCT_ID.Value, this);
                  }


        public virtual List<NMGiftsVM> GetAll() 
                  {
                      return new DataAccess().GetTopItems(100, this).Cast<NMGiftsVM>().ToList();
                  }
		#endregion
	}
}