using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Orders
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Orders()
		{
		    da = new DataAccess();
            sm=  new SystemMessage();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the ONO value.
		/// </summary>
		public virtual Int32? ONO { get; set; }

		/// <summary>
		/// Gets or sets the ODATE value.
		/// </summary>
		public virtual DateTime? ODATE { get; set; }

		/// <summary>
		/// Gets or sets the CUSTOMER value.
		/// </summary>
		public virtual Int32? CUSTOMER { get; set; }

		/// <summary>
		/// Gets or sets the SUPPLIER value.
		/// </summary>
		public virtual Int32? SUPPLIER { get; set; }

		/// <summary>
		/// Gets or sets the INV_SITE value.
		/// </summary>
		public virtual Int32? INV_SITE { get; set; }

		/// <summary>
		/// Gets or sets the OTYPE value.
		/// </summary>
		public virtual Int32? OTYPE { get; set; }

		/// <summary>
		/// Gets or sets the INVOICE_STATE value.
		/// </summary>
		public virtual Boolean? INVOICE_DONE { get; set; }

		/// <summary>
		/// Gets or sets the OSTATE value.
		/// </summary>
        public virtual Boolean? IS_DELIVERED { get; set; }

		/// <summary>
		/// Gets or sets the OPARENT value.
		/// </summary>
		public virtual Int32? OPARENT { get; set; }

		/// <summary>
		/// Gets or sets the OREF value.
		/// </summary>
		public virtual Int32? OREF { get; set; }

        public virtual Int32? UNION_ID { get; set; }
		/// <summary>
		/// Gets or sets the TOT value.
		/// </summary>
		public virtual Decimal? TOT { get; set; }

        public virtual Decimal? PAID { get; set; }

        public virtual Boolean? IS_HOLD { get; set; }

        public virtual string USR_NAME { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Orders" ;
		}
		#endregion

		#region CRUD Methods



        public static bool IsDelivered(int ono, int type) 
        {
            if (DataAccess.GetDataTable("select * from OrderDetails where TRANS_TYPE='" + type + "' and ONO='" + ono + "' and ISNULL(DELIVERED_QNT,0)<>(QUANTITY)").Rows.Count > 0)
            {
                return false;
            }
            else { return true; }
        } 

        public static bool IsFinishedOrder(int ono,int tr_type) 
        {
            if (new DataAccess().IsExists(new Orders() { ONO = ono, OTYPE = tr_type }))
            {
                decimal result = decimal.Parse(DataAccess.GetSqlValue("select isnull(sum(TOTAL),0) from invoices where OrderNumber='" + ono.ToString() + "' and tr_type='" + tr_type.ToString() + "' ", EgxDataType.Mssql).ToString());
                decimal compared = decimal.Parse(DataAccess.GetSqlValue("select isnull(sum(TOT),0) from Orders where ONO ='" + ono.ToString() + "' AND OTYPE='" + tr_type.ToString() + "'", EgxDataType.Mssql).ToString());

                if (result == compared && new OrderDetails() { ONO=ono,TRANS_TYPE=tr_type }.Search().Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else { return false; }
        }

        public static decimal UpdateTotal(int orderNumber,int orderType) 
        {
            decimal result = 0;
            result=(decimal)DataAccess.GetSqlValue("select count(net) from OrderDetails where ONO="+orderNumber+" and otype='"+orderNumber+"'", EgxDataType.Mssql);
            return result;
        }

        public static int LastSalesOrder()
        {

            return (int)DataAccess.GetSqlValue("select isnull(max(ono),0) from orders where otype=-10", EgxDataType.Mssql);
        }

        public static int LastPurchaseOrder()
        {

            return (int)DataAccess.GetSqlValue("select isnull(max(ono),0) from orders where otype=-9", EgxDataType.Mssql);
        }

        public static int LastReturnSalesOrder()
        {

            return (int)DataAccess.GetSqlValue("select isnull(max(ono),0) from orders where otype=-8", EgxDataType.Mssql);
        }

        public static int LastReturnPurchaseOrder()
        {

            return (int)DataAccess.GetSqlValue("select isnull(max(ono),0) from orders where otype=-4", EgxDataType.Mssql);
        }
		
        public virtual SystemMessage Insert() 
                {
                    if (this.OTYPE == -10)
                    {
                        this.ONO = Orders.LastSalesOrder() + 1;
                    }
                    else if (this.OTYPE == -9)
                    {
                        this.ONO = Orders.LastPurchaseOrder() + 1;
                    }
                    else if (this.OTYPE == -8)
                    {
                        this.ONO = Orders.LastReturnSalesOrder() + 1;
                    }
                    else
                    {
                        this.ONO = Orders.LastReturnPurchaseOrder() + 1;
                    }
                    if (!DBConfig.IsWebApp)
                    {
                        this.USR_NAME = SystemSetting.CurrentUser.USER_NAME;
                    }
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
                   return sm;
                }
				
		public virtual SystemMessage Update() { 
            try
                {

                    da.Update(this, new List<string> { "ONO","OTYPE"});
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



        public virtual SystemMessage CreateSalesOrder(DateTime orderDate,decimal total,decimal paid,int unionID,int customer,bool delivered=false,bool invoiced=false) 
        {
            sm = new SystemMessage();
            this.OTYPE = -10;
            this.UNION_ID = unionID;
            this.CUSTOMER = customer;
            this.ODATE = orderDate;
            this.TOT = total;
            this.PAID = paid;
            this.IS_DELIVERED = delivered;
            this.INVOICE_DONE = invoiced;
            this.Insert();
            sm.Attachment = this.ONO;
            return sm;
        }

        public virtual SystemMessage CreatePurchaseOrder(DateTime orderDate, decimal total, decimal paid, int supplier,int locationID=-1,bool delivered = false, bool invoiced = false)
        {
            sm = new SystemMessage();
            this.OTYPE = -9;
            this.SUPPLIER = supplier;
            this.ODATE = orderDate;
            this.TOT = total;
            this.PAID = paid;
            this.IS_DELIVERED = delivered;
            this.INVOICE_DONE = invoiced;
            this.INV_SITE = locationID;
            this.Insert();
            sm.Attachment = this.ONO;
            return sm;
        }
        public virtual SystemMessage CreateReturnSalesOrder(int OrderRef,DateTime transactionDate,decimal total) 
        {
            sm = new SystemMessage();
            Orders o = new Orders() { ONO = OrderRef, OTYPE = -10 }.Search().FirstOrDefault();
            o.ODATE = transactionDate;
            o.OREF = OrderRef;
            o.OTYPE = -8;
            o.TOT = total;
            o.PAID = 0;
            sm = o.Insert();
            sm.Attachment = o.ONO;
            return sm;
        }

        public virtual SystemMessage CreateReturnPurchaseOrder(int OrderRef, DateTime transactionDate, decimal total)
        {
            sm = new SystemMessage();
            Orders o = new Orders() { ONO = OrderRef, OTYPE = -9 }.Search().FirstOrDefault();
            o.ODATE = transactionDate;
            o.OREF = OrderRef;
            o.OTYPE = -4;
            o.TOT = total;
            o.PAID = total;
            sm = o.Insert();
            sm.Attachment = o.ONO;
            return sm;
        }

        public static void SetHoldState(int ono, int otype,bool state)
        {
            Orders order = new Orders() { ONO = ono, OTYPE = otype }.Search().FirstOrDefault();
            order.IS_HOLD = state;
            order.Update();
        }

  
        public static decimal UpdateTotal(int orderNumber, int tr_type,decimal total) 
        {
            Orders o = new Orders() { ONO=orderNumber,OTYPE=tr_type}.Search().FirstOrDefault();
            o.TOT = total;
            o.Update();
            return total;
        }
        public virtual SystemMessage CancelOrder(int orderNumber, int otype)
        {
            sm = new SystemMessage();
            Orders o = new Orders() { ONO = orderNumber, OTYPE = otype }.Search().FirstOrDefault();
            o.IS_HOLD = true;
            o.Update();
            List<OrderDetails> detList = new List<OrderDetails>();
            detList = new OrderDetails() { ONO = orderNumber, TRANS_TYPE = otype }.Search();
            foreach (OrderDetails dtl in detList)
            {
                dtl.IS_CANCELED = true;
                dtl.Update();
            }
            return sm;
        }
        public static  SystemMessage RemoveCancelation(int orderNumber, int otype)
        {
           SystemMessage sms = new SystemMessage();
            Orders o = new Orders() { ONO = orderNumber, OTYPE = otype }.Search().FirstOrDefault();
            o.IS_HOLD = false;
            o.Update();
            List<OrderDetails> detList = new List<OrderDetails>();
            detList = new OrderDetails() { ONO = orderNumber, TRANS_TYPE = otype }.Search();
            foreach (OrderDetails dtl in detList)
            {
                dtl.IS_CANCELED = false;
                dtl.Update();
            }
            return sms;
        }

        //public virtual SystemMessage PaySalesOrder(Orders orderToPay) 
        //{
        //    sm = new SystemMessage();
        //    orderToPay.INVOICE_DONE = true;
        //    orderToPay.IS_DONE = true;
        //    orderToPay.Update();
        //    return sm;
        //}

        //public virtual SystemMessage CreatePurchaseOrder(int supplier, int customer, DateTime orderDate, decimal total,int Qantity)
        //{
        //    sm = new SystemMessage();
        //    this.INVOICE_DONE = false;
        //    this.INV_SITE = 0;
        //    this.IS_DONE = false;
        //    this.OTYPE = -9;
        //    this.SUPPLIER = supplier;
        //    this.CUSTOMER = customer;
        //    this.ODATE = orderDate;
        //    this.TOT = total;
        //    this.Insert();
        //    return sm;
        //}
        

       

        public virtual List<Orders> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Orders>().ToList();
                   }



		public virtual Orders GetByID() {  
                     return (Orders)new DataAccess().GetByColumn("ONO",ONO.Value, this);
                  }


	    public virtual List<Orders> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Orders>().ToList();
                  }
		#endregion
	}
}