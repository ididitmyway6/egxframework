using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class OrderDetails
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public OrderDetails()
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
        /// Gets or sets the ID value.
        /// </summary>
        public virtual Int32? SUPPLIER { get; set; }

        /// <summary>
        /// Gets or sets the ID value.
        /// </summary>
        public virtual float? DELIVERED_QNT { get; set; }

		/// <summary>
		/// Gets or sets the ONO value.
		/// </summary>
		public virtual Int32? ONO { get; set; }

		/// <summary>
		/// Gets or sets the PRODUCT_ID value.
		/// </summary>
		public virtual Int32? PRODUCT_ID { get; set; }

		/// <summary>
		/// Gets or sets the PRODUCT_NAME value.
		/// </summary>
		public virtual String PRODUCT_NAME { get; set; }

		/// <summary>
		/// Gets or sets the INV_SITE value.
		/// </summary>
		public virtual Int32? INV_SITE { get; set; }

		/// <summary>
		/// Gets or sets the QUANTITY value.
		/// </summary>
		public virtual float? QUANTITY { get; set; }

		/// <summary>
		/// Gets or sets the PRICE value.
		/// </summary>
		public virtual Decimal? PRICE { get; set; }

		/// <summary>
		/// Gets or sets the TOT value.
		/// </summary>
		public virtual Decimal? TOT { get; set; }

		/// <summary>
		/// Gets or sets the DISCOUNT value.
		/// </summary>
		public virtual float? DISCOUNT { get; set; }

		/// <summary>
		/// Gets or sets the NET value.
		/// </summary>
		public virtual Decimal? NET { get; set; }

		/// <summary>
		/// Gets or sets the NOTE value.
		/// </summary>
		public virtual String NOTE { get; set; }

		/// <summary>
		/// Gets or sets the RETURNED_QNT value.
		/// </summary>
		public virtual float? RETURNED_QNT { get; set; }

		/// <summary>
		/// Gets or sets the TRANS_TYPE value.
		/// </summary>
		public virtual Int32? TRANS_TYPE { get; set; }

        public virtual Boolean? IS_CANCELED { get; set; }
        public virtual DateTime? EXP_DATE { get; set; }
        public virtual int? USR_ID { get; set; }
        public virtual string USR_NAME { get; set; }
        public virtual DateTime? TR_DATE { get; set; }
        public virtual int? OrderStatus { get; set; }
        public virtual int? CustomerID { get; set; }
        public virtual int? VendorID { get; set; }
        public virtual String CancellationText { get; set; }
        public virtual String VFC_SENDER { get; set; }
        public virtual String VFC_RECEIVER { get; set; }
        public virtual String ADMIN_NAME { get; set; }
        public virtual Int32? ADMIN_ID { get; set; }
        public virtual bool? CustomerStrike { get; set; }
        public virtual bool? SupplierStrike { get; set; }
        public virtual DateTime? LastUpdate { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "OrderDetails" ;
		}
		#endregion

		#region CRUD Methods

        public static int GetLast() 
        {
            return  DataAccess.GetSqlValue("select isnull(max(id),-1) from OrderDetails", EgxDataType.Mssql).ToInt32();
        }
		public virtual SystemMessage Insert() 
                {
                    this.LastUpdate = DateTime.Now;
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
            
                   return sm;
                }
				
		public virtual SystemMessage Update() { 
            try
                {
                    this.LastUpdate = DateTime.Now;
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
		public virtual List<OrderDetails> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<OrderDetails>().ToList();
                   }

        public SystemMessage CreateOrderLine(int ono, int productID, int Qantity, TransactionType type)
        {
            sm = new SystemMessage();
            Categ product = new Categ() { ID = productID }.GetByID();
            
            this.PRODUCT_NAME = product.CATEG_NAME;
            this.PRODUCT_ID = product.ID;
            this.INV_SITE = 0;
            this.PRICE = product.CATEG_PRICE;
            this.DISCOUNT = product.CATEG_DISCOUNT;
            this.QUANTITY = Qantity;
            this.TRANS_TYPE = (int)type;
            this.TOT = this.PRICE * Qantity;
            this.NET = this.TOT - Convert.ToDecimal(((Convert.ToSingle(this.TOT) * product.CATEG_DISCOUNT) / 100));
            this.ONO = ono;
            this.Insert();
            return sm;
        }




        public static decimal GetOrderNet(int orderNumber, int tr_type)
        {
            return Convert.ToDecimal(DataAccess.GetSqlValue("select isnull(sum(net),0) from OrderDetails where ono =" + orderNumber.ToString() + " trans_type='" + tr_type + "'", EgxDataType.Mssql));
        }
        public SystemMessage ReturnSalesOrderDetail(OrderDetails orderDetail,int orderRef,float qnt) 
        {
            sm = new SystemMessage();
            OrderDetails prevDtl = new OrderDetails() { ID = orderDetail.ID }.GetByID();
            if (orderDetail.RETURNED_QNT > orderDetail.DELIVERED_QNT)
            {
                sm.Message = "«·ﬂ„Ì… «·„— Ã⁄… «ﬂ»— „‰ «·ﬂ„Ì… «·„»«⁄…";
                sm.Type = MessageType.Stop;
            }
            else
            {
                orderDetail.RETURNED_QNT = orderDetail.RETURNED_QNT.GetValueOrDefault(0) + qnt;
                orderDetail.Update();
                OrderDetails returnedSalesItem = new OrderDetails();
                returnedSalesItem.TRANS_TYPE = -8;
                returnedSalesItem.DISCOUNT = orderDetail.DISCOUNT.GetValueOrDefault();
                returnedSalesItem.INV_SITE = orderDetail.INV_SITE.GetValueOrDefault();
                returnedSalesItem.PRICE = orderDetail.PRICE.GetValueOrDefault();
                returnedSalesItem.QUANTITY = orderDetail.RETURNED_QNT.GetValueOrDefault() - prevDtl.RETURNED_QNT.GetValueOrDefault();
                returnedSalesItem.RETURNED_QNT = 0;
                returnedSalesItem.ONO = orderRef;
                returnedSalesItem.PRODUCT_ID = orderDetail.PRODUCT_ID;
                returnedSalesItem.PRODUCT_NAME = orderDetail.PRODUCT_NAME;
                returnedSalesItem.TOT = Convert.ToDecimal(float.Parse(returnedSalesItem.PRICE.ToString()) * returnedSalesItem.QUANTITY);
                returnedSalesItem.NET = returnedSalesItem.TOT - Convert.ToDecimal(((Convert.ToSingle(returnedSalesItem.TOT) * orderDetail.DISCOUNT) / 100));
                returnedSalesItem.SUPPLIER = orderDetail.SUPPLIER;
                returnedSalesItem.TR_DATE = DateTime.Now;

                returnedSalesItem.Insert();
                sm.Message = " „  «·⁄„·Ì… »‰Ã«Õ";
                sm.Type = MessageType.Pass;

            }
            return sm;
        }

        public SystemMessage ReturnPurchaseOrderDetail(OrderDetails orderDetail, int orderRef, int qnt)
        {
            sm = new SystemMessage();
            OrderDetails prevDtl = new OrderDetails() { ID = orderDetail.ID }.GetByID();
            if (orderDetail.RETURNED_QNT > orderDetail.DELIVERED_QNT)
            {
                sm.Message = "«·ﬂ„Ì… «·„— Ã⁄… «ﬂ»— „‰ «·ﬂ„Ì… «·„‘ —«Â";
                sm.Type = MessageType.Stop;
            }
            else
            {
                orderDetail.RETURNED_QNT = orderDetail.RETURNED_QNT.GetValueOrDefault(0) + qnt;
                orderDetail.Update();
                OrderDetails returnedPurchaseItem = new OrderDetails();
                returnedPurchaseItem.TRANS_TYPE = -4;
                returnedPurchaseItem.INV_SITE = orderDetail.INV_SITE.GetValueOrDefault();
                returnedPurchaseItem.PRICE = orderDetail.PRICE.GetValueOrDefault();
                returnedPurchaseItem.QUANTITY = orderDetail.RETURNED_QNT.GetValueOrDefault() - prevDtl.RETURNED_QNT.GetValueOrDefault();
                returnedPurchaseItem.RETURNED_QNT = 0;
                returnedPurchaseItem.ONO = orderRef;
                returnedPurchaseItem.PRODUCT_ID = orderDetail.PRODUCT_ID;
                returnedPurchaseItem.PRODUCT_NAME = orderDetail.PRODUCT_NAME;
                returnedPurchaseItem.TOT = Convert.ToDecimal(float.Parse( returnedPurchaseItem.PRICE.ToString()) * returnedPurchaseItem.QUANTITY);
                returnedPurchaseItem.NET = returnedPurchaseItem.TOT;
                returnedPurchaseItem.SUPPLIER = orderDetail.SUPPLIER;
                returnedPurchaseItem.TR_DATE = DateTime.Now;

                returnedPurchaseItem.Insert();
                sm.Message = " „  «·⁄„·Ì… »‰Ã«Õ";
                sm.Type = MessageType.Pass;

            }
            return sm;
        }
        public virtual OrderDetails GetByID()
        {
            return (OrderDetails)new DataAccess().GetByID(ID.Value, this);
        }


	        public virtual List<OrderDetails> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<OrderDetails>().ToList();
                  }
		#endregion

            public string CustomerName { get; set; }
            public string VendorName { get; set; }
            public List<OrderDetails> getOrderDetailsList(int adminID, int requestType) 
            {
                List<String> extra = new List<string>();
                extra.Add("CustomerName");
                extra.Add("VendorName");
                var result = new List<OrderDetails>();
                string sql = @"select *,
                    (select CUST_NAME from Customers where id =od.CustomerID ) as CustomerName ,
                    (select CUST_NAME from Customers where id =od.VendorID ) as VendorName 
                    from OrderDetails od where ADMIN_ID='" + adminID.ToString()+"' and OrderStatus='"+requestType+"'";
                result = da.ExecuteSQL<OrderDetails>(sql,extra);
                return result;
            }
	}
}