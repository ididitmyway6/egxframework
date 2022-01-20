using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class RequestDetails :IRequestDetails
	{
	            public DataAccess da;
                public SystemMessage sm;
                public enum CONTAINER_TYPE { FULL = 0, PARTIAL = 1, AIR_FREIGHT = 2, INLAND_FREIGHT = 3, GENERAL_CARGO = 4,RORO=5 }
                public enum PAYMENT_TYPE { PREPAID = 0, OTHER = 1}
                public enum REQUEST_STATE { PENDING=0,TASK=2,WAITING_CONFIRM=3,CONFIRMED=1,NOT_CONFIRMED=-1}
                #region Construction
		public RequestDetails()
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
		/// Gets or sets the REQUEST_ID value.
		/// </summary>
		public virtual Int32? REQUEST_ID { get; set; }

		/// <summary>
		/// Gets or sets the IS_FULL_CONTAINER value.
		/// </summary>
        public virtual Int32? CONTAINER_METHOD { get; set; }

		/// <summary>
		/// Gets or sets the LOADING_PORT value.
		/// </summary>
		public virtual String LOADING_PORT { get; set; }

		/// <summary>
		/// Gets or sets the LANDING_PORT value.
		/// </summary>
		public virtual String LANDING_PORT { get; set; }

		/// <summary>
		/// Gets or sets the DEPARTURE_DATE value.
		/// </summary>
		public virtual DateTime? DEPARTURE_DATE { get; set; }

		/// <summary>
		/// Gets or sets the CARGO_NAME value.
		/// </summary>
		public virtual String CARGO_NAME { get; set; }

		/// <summary>
		/// Gets or sets the QNT value.
		/// </summary>
		public virtual Int32? QNT { get; set; }

		/// <summary>
		/// Gets or sets the CARGO_TYPE value.
		/// </summary>
		public virtual Int32? CARGO_TYPE { get; set; }

		/// <summary>
		/// Gets or sets the CARGO_SIZE_TON value.
		/// </summary>
		public virtual Double? CARGO_SIZE_TON { get; set; }

		/// <summary>
		/// Gets or sets the HEIGHT value.
		/// </summary>
		public virtual Double? HEIGHT { get; set; }

		/// <summary>
		/// Gets or sets the WIDTH value.
		/// </summary>
		public virtual Double? WIDTH { get; set; }

		/// <summary>
		/// Gets or sets the ALTITUDE value.
		/// </summary>
		public virtual Double? ALTITUDE { get; set; }

		/// <summary>
		/// Gets or sets the SIZE_KILO value.
		/// </summary>
		public virtual Double? SIZE_KILO { get; set; }

		/// <summary>
		/// Gets or sets the FREE_TIME value.
		/// </summary>
		public virtual Int32? FREE_TIME { get; set; }

		/// <summary>
		/// Gets or sets the IS_REEFER value.
		/// </summary>
		public virtual Boolean? IS_REEFER { get; set; }

		/// <summary>
		/// Gets or sets the REEFER_TEMP value.
		/// </summary>
		public virtual Double? REEFER_TEMP { get; set; }

		/// <summary>
		/// Gets or sets the REEFER_OTHER value.
		/// </summary>
		public virtual String REEFER_OTHER { get; set; }

		/// <summary>
		/// Gets or sets the IS_DANGEROUS value.
		/// </summary>
		public virtual Boolean? IS_DANGEROUS { get; set; }

		/// <summary>
		/// Gets or sets the DANGER_FILE value.
		/// </summary>
		public virtual String DANGER_FILE { get; set; }

		/// <summary>
		/// Gets or sets the UN_NUMBER value.
		/// </summary>
		public virtual String UN_NUMBER { get; set; }

		/// <summary>
		/// Gets or sets the UN_CLASS value.
		/// </summary>
		public virtual String UN_CLASS { get; set; }

		/// <summary>
        /// Gets or sets the PAYMENT_TYPE value.
		/// </summary>
		public virtual Int32? PAYMENT_METHOD { get; set; }

		

		/// <summary>
		/// Gets or sets the EX_WORK value.
		/// </summary>
		public virtual Boolean? EX_WORK { get; set; }

		/// <summary>
		/// Gets or sets the CUSTOMS value.
		/// </summary>
		public virtual Boolean? CUSTOMS { get; set; }

		/// <summary>
		/// Gets or sets the TRUCKING value.
		/// </summary>
		public virtual Boolean? TRUCKING { get; set; }

		/// <summary>
		/// Gets or sets the INVOICE value.
		/// </summary>
		public virtual Boolean? INVOICE { get; set; }

		/// <summary>
		/// Gets or sets the CO value.
		/// </summary>
		public virtual Boolean? CO { get; set; }

		/// <summary>
		/// Gets or sets the PACKING_LIST value.
		/// </summary>
		public virtual Boolean? PACKING_LIST { get; set; }

		/// <summary>
		/// Gets or sets the REMARKS value.
		/// </summary>
		public virtual String REMARKS { get; set; }

		/// <summary>
		/// Gets or sets the LAST_UPDATE_BY value.
		/// </summary>
		public virtual Int32? LAST_UPDATE_BY { get; set; }

		/// <summary>
		/// Gets or sets the LAST_UPDATE_DATE value.
		/// </summary>
		public virtual DateTime? LAST_UPDATE_DATE { get; set; }

		/// <summary>
		/// Gets or sets the SALES_USER_ID value.
		/// </summary>
		public virtual Int32? SALES_USER_ID { get; set; }

		/// <summary>
		/// Gets or sets the SALES_USER_NAME value.
		/// </summary>
		public virtual String SALES_USER_NAME { get; set; }

		/// <summary>
		/// Gets or sets the LAST_UPDATE_USER_NAME value.
		/// </summary>
		public virtual String LAST_UPDATE_USER_NAME { get; set; }

		/// <summary>
		/// Gets or sets the LAST_UPDATE_USER_TYPE value.
		/// </summary>
		public virtual Int32? LAST_UPDATE_USER_TYPE { get; set; }

		/// <summary>
		/// Gets or sets the REQUEST_STATUS value.
		/// </summary>
		public virtual Int32? REQUEST_STATUS { get; set; }

		/// <summary>
		/// Gets or sets the SELLING_RATE value.
		/// </summary>
		public virtual Double? SELLING_RATE { get; set; }

		/// <summary>
		/// Gets or sets the NET_RATE value.
		/// </summary>
		public virtual Double? NET_RATE { get; set; }

		/// <summary>
		/// Gets or sets the ETA value.
		/// </summary>
		public virtual DateTime? ETA { get; set; }

		/// <summary>
		/// Gets or sets the ETD value.
		/// </summary>
        public virtual DateTime? ETD { get; set; }

		/// <summary>
		/// Gets or sets the MBL_NUMBER value.
		/// </summary>
        public virtual Int32? MBL_NUMBER { get; set; }

		/// <summary>
		/// Gets or sets the SHIPPING_LINE value.
		/// </summary>
        public virtual Int32? SHIPPING_LINE { get; set; }

		/// <summary>
		/// Gets or sets the SHIPPING_RET value.
		/// </summary>
		public virtual Decimal? SHIPPING_RET { get; set; }

		/// <summary>
		/// Gets or sets the TRANSIT_TIME value.
		/// </summary>
        public virtual Int32? TRANSIT_TIME { get; set; }

		/// <summary>
		/// Gets or sets the CUSTOM_NET_RATE value.
		/// </summary>
		public virtual Decimal? CUSTOM_NET_RATE { get; set; }

		/// <summary>
		/// Gets or sets the EX_WORK_NET_RATE value.
		/// </summary>
		public virtual Decimal? EX_WORK_NET_RATE { get; set; }

		/// <summary>
		/// Gets or sets the TRUCKING_NET_RATE value.
		/// </summary>
		public virtual Decimal? TRUCKING_NET_RATE { get; set; }

		/// <summary>
		/// Gets or sets the CO_LEGALIZATION_NET_RATE value.
		/// </summary>
		public virtual Decimal? CO_LEGALIZATION_NET_RATE { get; set; }

		/// <summary>
		/// Gets or sets the INVOICE_LEGALIZATION_NET_RATE value.
		/// </summary>
		public virtual Decimal? INVOICE_LEGALIZATION_NET_RATE { get; set; }

		/// <summary>
		/// Gets or sets the MBL_FILE value.
		/// </summary>
		public virtual String MBL_FILE { get; set; }

		/// <summary>
		/// Gets or sets the HBL_FILE value.
		/// </summary>
		public virtual String HBL_FILE { get; set; }

		/// <summary>
		/// Gets or sets the INVOICE_FILE value.
		/// </summary>
		public virtual String INVOICE_FILE { get; set; }

		/// <summary>
		/// Gets or sets the CO_FILE value.
		/// </summary>
		public virtual String CO_FILE { get; set; }

		/// <summary>
		/// Gets or sets the OTHER_FILES value.
		/// </summary>
		public virtual String OTHER_FILES { get; set; }
        /// <summary>
        /// Gets or sets the CUSTOM_NET_RATE value.
        /// </summary>
        public virtual Decimal? CUSTOM_SELLING_RATE { get; set; }

        /// <summary>
        /// Gets or sets the EX_WORK_NET_RATE value.
        /// </summary>
        public virtual Decimal? EX_WORK_SELLING_RATE { get; set; }

        /// <summary>
        /// Gets or sets the TRUCKING_NET_RATE value.
        /// </summary>
        public virtual Decimal? TRUCKING_SELLING_RATE { get; set; }

        /// <summary>
        /// Gets or sets the CO_LEGALIZATION_NET_RATE value.
        /// </summary>
        public virtual Decimal? CO_LEGALIZATION_SELLING_RATE { get; set; }

        /// <summary>
        /// Gets or sets the INVOICE_LEGALIZATION_NET_RATE value.
        /// </summary>
        public virtual Decimal? INVOICE_LEGALIZATION_SELLING_RATE { get; set; }
        public virtual Decimal? TARGET_PRICE { get; set; }
        public virtual Int32? TARGET_PRICE_CURRENCY { get; set; }
        public virtual string SHIPPER_NAME { get; set; }
        public virtual string SHIPPER_ADDRESS { get; set; }
        public virtual Int32? CUSTOMER_ID { get; set; }
        public virtual decimal? PACKING_LIST_NET_RATE { get; set; }
        public virtual decimal? PACKING_LIST_SELLING_RATE { get; set; }
        public virtual Int32? OPERATOR_ID { get; set; }
        public virtual Int32? ADMIN_ID { get; set; }
        public virtual Decimal? OF_SELLING_RATE { get; set; }
        public virtual Decimal? OF_NET_RATE { get; set; }
        public virtual Int32? OP_FREE_TIME { get; set; }
        public virtual Int32? CUSTOMS_CURRENCY { get; set; }
        public virtual Int32? EXWORK_CURRENCY { get; set; }
        public virtual Int32? TRUCKING_CURRENCY { get; set; }
        public virtual Int32? CO_CURRENCY { get; set; }
        public virtual Int32? INVOICE_CURRENCY { get; set; }
        public virtual Int32? PACKING_CURRENCY { get; set; }
        public virtual Int32? OF_CURRENCY { get; set; }
        public virtual String OTHER2_FILE { get; set; }
        public virtual String OPERATOR_NAME { get; set; }
        public virtual String ADMIN_NAME { get; set; }
        public virtual double? DIMENTIONS { get; set; }
        public virtual decimal? COCC_LEGALIZATION_SELLING_RATE { get; set; }
        public virtual decimal? INVOICECC_LEGALIZATION_SELLING_RATE { get; set; }
        public virtual decimal? PACKINGLISTCC_LEGALIZATION_SELLING_RATE { get; set; }
        public virtual decimal? COCC_LEGALIZATION_NET_RATE { get; set; }
        public virtual decimal? INVOICECC_LEGALIZATION_NET_RATE { get; set; }
        public virtual decimal? PACKINGLISTCC_LEGALIZATION_NET_RATE { get; set; }
        public virtual decimal? AGENCY_PROFIT { get; set; }
        public virtual bool? INVOICE_CC { get; set; }
        public virtual bool? CO_CC { get; set; }
        public virtual bool? PACKING_LIST_CC { get; set; }
        public virtual double? ACCOUNTING_QNT { get; set; }
        public virtual decimal? COURRIER_RATE { get; set; }
        public virtual DateTime? CREATION_DATE { get; set; }
        public virtual string RO { get; set; }
        public virtual Int32? DEFAULT_CURRENCY { get; set; }
        public virtual Decimal? NET_PROFIT { get; set; }
        public virtual string FCL_COMMENT { get; set; }
        #endregion

		#region Overrides
		public override String ToString()
		{
			return "RequestDetails" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                {
                    this.CREATION_DATE = DateTime.Now.Date;
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
                sm.Attachment = new DataAccess().GetLatestID(this);
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
		public virtual List<RequestDetails> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<RequestDetails>().ToList();
                   }



		public virtual RequestDetails GetByID() {  
                     return (RequestDetails)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<RequestDetails> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(1000,this).Cast<RequestDetails>().ToList();
                  }
		#endregion
	}
}