using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{
    public enum OrderStatus { ON_HOLD = 3 , DELIVERED=1,REFUNDED=0,IN_PROGRESS=2,NOT_DELIVERED=-1,PAID=4,DONE=5,UNPAID=6,ISSUE=7 }

	[Serializable]
	public class ProductPreview
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
                public ProductPreview()
		{
		    da = new DataAccess();
            sm=  new SystemMessage();
		}
		#endregion
		#region Properties
                /// <summary>
                /// Gets or sets the ID value.
                /// </summary>
                public virtual Int32? CustomerID { get; set; }
                /// <summary>
                /// Gets or sets the ID value.
                /// </summary>
                public virtual Int32? ID { get; set; }

                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String PRODUCT_NAME { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String CUST_NAME { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String CUST_ADD { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String CUST_PHONE { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String EMAIL_ADDR { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String CUST_CODE { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String REAL_EMAIL { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String CATEG_NAME { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String CATEG_CODE { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String CATEG_DES { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual Double? CATEG_PRICE { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual Double? CATEG_DISCOUNT { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String CATEG_IMG { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual String BAN_TEXT { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual Boolean? IS_PUBLISHED { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual Boolean? IS_BANNED { get; set; }
                /// <summary>
                /// Gets or sets the CID value.
                /// </summary>
                public virtual Boolean? IS_FEATURED { get; set; }


        //For View Model ----

                public float? QUANTITY { get; set; }
                public string VFC_SENDER{get;set;}
                public string VFC_RECEIVER { get; set; }
                public DateTime? ORDER_DATE { get; set; }
                public string ADMIN_NAME { get; set; }
                public int? ADMIN_ID { get; set; }


        //End For View Model 

		#endregion

		#region Overrides
		public override String ToString()
		{
            return "ProductPreview";
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
        public virtual List<ProductPreview> Search() 
                   {
                       return new DataAccess().Search(this).Cast<ProductPreview>().ToList();
                   }



        public virtual ProductPreview GetByID()
        {
            return (ProductPreview)new DataAccess().GetByID(ID.Value, this);
                  }


        public virtual List<ProductPreview> GetAll() 
                  {
                      return new DataAccess().GetTopItems(100, this).Cast<ProductPreview>().ToList();
                  }
		#endregion

        public virtual void InsertOrder(int customerID) 
        {
            OrderDetails order = new OrderDetails();
            order.CustomerID = customerID;
            order.NET = Convert.ToDecimal( this.CATEG_PRICE.GetValueOrDefault(0)) * Convert.ToDecimal( QUANTITY.GetValueOrDefault(0));
            order.OrderStatus = (int)OrderStatus.ON_HOLD;
            order.PRICE = Convert.ToDecimal(this.CATEG_PRICE.GetValueOrDefault(0));
            order.PRODUCT_ID = this.ID.Value;
            order.PRODUCT_NAME = this.CATEG_NAME;
            order.QUANTITY =  this.QUANTITY.GetValueOrDefault(0);
            order.TOT = order.NET;
            order.TR_DATE = ORDER_DATE;
            order.VendorID = this.CustomerID;
            order.VFC_RECEIVER = VFC_RECEIVER;
            order.VFC_SENDER = VFC_SENDER;
            order.ADMIN_ID = ADMIN_ID;
            order.ADMIN_NAME = ADMIN_NAME;
            order.Insert();
        }
        public static string ResolveOrderStatus(int orderStatus) 
        {

            switch (orderStatus) 
            {
                    case 0 : return "تم الاسترجاع";
                    case 3 : return "معلق";
                    case 1 : return "تم التسليم";
                    case 2 : return "عملية ناجحة";
                    case -1: return "لم يتم الاستلام";
                    case 4: return "قيد التسليم";
                    case 5: return "تم الاستلام";
                    case 6: return "لم يتم الدفع";
                    case 7: return "مخالفة";
                    case 8: return "تم تأكيد الدفع";
                    case 10: return "طلب الغاء";
                    case 9: return "العميل لا يستجيب";
                    case 11: return "البائع لا يستجيب";
                    case 12: return "طلب استرجاع";
                    
            }
            return "";
        }
	}
}