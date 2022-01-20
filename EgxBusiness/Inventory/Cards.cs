using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Cards
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Cards()
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
		/// Gets or sets the CARD_CODE value.
		/// </summary>
		public virtual String CARD_CODE { get; set; }

		/// <summary>
		/// Gets or sets the CARD_VALUE value.
		/// </summary>
		public virtual Int32? CARD_VALUE { get; set; }

		/// <summary>
		/// Gets or sets the REG_DATE value.
		/// </summary>
		public virtual DateTime? REG_DATE { get; set; }

        /// <summary>
        /// Gets or sets the CID value.
        /// </summary>
        public virtual Int32? CID { get; set; }

        /// <summary>
        /// Gets or sets the CID value.
        /// </summary>
        public virtual String REL_PRODUCT { get; set; }


        /// <summary>
        /// Gets or sets the CID value.
        /// </summary>
        public virtual bool? AUTO_PURCHASE { get; set; }
        
        /// <summary>
        /// Gets or sets the CID value.
        /// </summary>
        public virtual Int32? REL_CATEG { get; set; }

        /// <summary>
        /// Gets or sets the IsRecharge Card value.
        /// </summary>
        public virtual bool? IS_RCG { get; set; }

        public virtual int? ValidationDays { get; set; }

        
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Cards" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Cards test = new Cards();
                   test.CARD_CODE = this.CARD_CODE;
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
		public virtual List<Cards> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Cards>().ToList();
                   }



		public virtual Cards GetByID() {  
                     return (Cards)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Cards> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Cards>().ToList();
                  }
		#endregion
	}
}