using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class ItemsTransactions
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public ItemsTransactions()
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
		/// Gets or sets the TR_TYPE value.
		/// </summary>
		public virtual Int32? TR_TYPE { get; set; }

		/// <summary>
		/// Gets or sets the ITEM_CODE value.
		/// </summary>
		public virtual Int32? ITEM_ID { get; set; }

		/// <summary>
		/// Gets or sets the QIN value.
		/// </summary>
		public virtual float? QIN { get; set; }

		/// <summary>
		/// Gets or sets the QOUT value.
		/// </summary>
		public virtual float? QOUT { get; set; }

		/// <summary>
		/// Gets or sets the TR_DATE value.
		/// </summary>
		public virtual DateTime? TR_DATE { get; set; }

		/// <summary>
		/// Gets or sets the ORDER_LINE value.
		/// </summary>
		public virtual Int32? ORDER_LINE { get; set; }

		/// <summary>
		/// Gets or sets the COMMENT value.
		/// </summary>
		public virtual String COMMENT { get; set; }

		/// <summary>
		/// Gets or sets the USR value.
		/// </summary>
		public virtual String USR { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "ItemsTransactions" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
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
		public virtual List<ItemsTransactions> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<ItemsTransactions>().ToList();
                   }



		public virtual ItemsTransactions GetByID() {  
                     return (ItemsTransactions)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<ItemsTransactions> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<ItemsTransactions>().ToList();
                  }
		#endregion
	}
}