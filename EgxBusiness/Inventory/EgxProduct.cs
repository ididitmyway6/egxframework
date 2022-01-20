using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using System.ComponentModel;

namespace Egx.EgxBusiness.Inventory
{
	#region Comments

	[Serializable]
	public class EgxProduct
	{
                public DataAccess da;
                public SystemMessage sm;

                public EgxProduct()
		{
                     da = new DataAccess();
                     sm = new SystemMessage();
		}

	
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
        [Browsable(false)]
        public string SearchKey { get; set; }
        [DisplayName("„”·”·")]
		public virtual Int32? ID { get; set; }
        [DisplayName("ﬂÊœ «·„‰ Ã")]
        public virtual String PRODUCT_CODE { get; set; }
        [DisplayName("«·«”„")]
		public virtual String PRODUCT_NAME { get; set; }

       [DisplayName("«·Ê’›")]
        public virtual String DESCRIPTION { get; set; }

        [DisplayName("‰Ê⁄ «·„‰ Ã ")]
       // [Browsable(false)]
       public virtual Int32? PRODUCT_TYPE { get; set; }

        //[DisplayName("ÿ»Ì⁄… «·„‰ Ã")]
        //public virtual String PRODUCT_TYPE_NAME { get { if (PRODUCT_TYPE != null)
        //{ return new AppCode().GetItem(38, PRODUCT_TYPE.Value).ITEM_NAME; } else { return null; } } }

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
                    EgxProduct c = new EgxProduct() { PRODUCT_NAME = this.PRODUCT_NAME };
                   if (da.IsExists(c))
            {
                sm.Message = "";
                sm.Type = MessageType.Stop;
                return sm;
            }
            else 
            {
                da.Insert(this);
                sm.Type = MessageType.Pass;
                return sm;
            }

                }

		
		public virtual SystemMessage Update() { 
                     try
                      {
                        da.Update(this, "ID");
                        sm.Message = "";
                        sm.Type = MessageType.Stop;
                        return sm;
                      }
                     catch
                       {
                            return sm;
                       }
                  }

		public virtual SystemMessage Delete() { 
                              try {
                                    da.Delete(this, "ID");
                                    sm.Type = MessageType.Pass;
                                    return sm; }
                              catch { sm.Type = MessageType.Fail; return sm; }
                    }


        public virtual List<EgxProduct> Search() 
                   {
                       return new DataAccess().Search(this).Cast<EgxProduct>().ToList();
                   }



        public virtual EgxProduct GetByID()
        {
            return (EgxProduct)new DataAccess().GetByID(ID.Value, this);
                  }


        public virtual List<EgxProduct> GetAll() 
                  {
                      return new DataAccess().GetTopItems(100, this).Cast<EgxProduct>().ToList();
                  }
	#endregion
	}
}