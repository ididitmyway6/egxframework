using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using System.ComponentModel;
using System.Data;

namespace Egx.EgxBusiness.Inventory
{
	#region Comments

	[Serializable]
	public class EgxSupplier
	{
                public DataAccess da;
                public SystemMessage sm;
		
		public EgxSupplier()
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
        
		public virtual Int32? ID { get; set; }

		/// <summary>
		/// Gets or sets the CUST_NAME value.
		/// </summary>
        [DisplayName("«·«”„")]
		public virtual String SUP_NAME { get; set; }

		/// <summary>
		/// Gets or sets the CUST_ADD value.
		/// </summary>
        [DisplayName("«·⁄‰Ê«‰")]
		public virtual String SUP_ADD { get; set; }

		/// <summary>
		/// Gets or sets the CUST_PHONE value.
		/// </summary>
        [DisplayName("«·Â« ›1")]
		public virtual String SUP_PHONE { get; set; }

		/// <summary>
		/// Gets or sets the CUST_PHONE2 value.
		/// </summary>
        [DisplayName("«·Â« ›2")]
        public virtual String SUP_PHONE2 { get; set; }

		/// <summary>
		/// Gets or sets the CUST_PHONE3 value.
		/// </summary>
         [Browsable(false)]
        public virtual String SUP_PHONE3 { get; set; }

		/// <summary>
		/// Gets or sets the CUST_PHONE4 value.
		/// </summary>
        [Browsable(false)]
        public virtual String SUP_PHONE4 { get; set; }

		/// <summary>
		/// Gets or sets the CUST_ID_NO value.
		/// </summary>
       [DisplayName("—ﬁ„ «·»ÿ«ﬁ…")]
        public virtual String SUP_ID_NO { get; set; }

		/// <summary>
		/// Gets or sets the CUST_CREDIT_LIMIT value.
		/// </summary>
        [DisplayName("«·Õœ «·«∆ „«‰Ï")]
        public virtual float? SUP_CREDIT_LIMIT { get; set; }

		/// <summary>
		/// Gets or sets the IS_ACTIVE value.
		/// </summary>
        [DisplayName("„›⁄·")]
		public virtual Boolean? IS_ACTIVE { get; set; }

		/// <summary>
		/// Gets or sets the ENTERED_BY value.
		/// </summary>
        [Browsable(false)]
        public virtual Int32? ENTERED_BY { get; set; }

        /// <summary>
        /// Gets or sets the ENTRY_DATE value.
        /// </summary>
        [Browsable(false)]
        public virtual DateTime? ENTRY_DATE { get; set; }


        /// <summary>
        /// Gets or sets the SUP_TYPE value.
        /// </summary>
        public virtual Int32? SUP_TYPE { get; set; }

        /// <summary>
        /// Gets or sets the SUP_TYPE value.
        /// </summary>
        public virtual Int32? MerchantCategoryID { get; set; }
        public virtual string SUP_PHOTO { get; set; }

        /// <summary>
        /// Gets or sets the SUP_CODE value.
        /// </summary>
        public virtual String SUP_CODE { get; set; }

		/// <summary>
		/// Gets or sets the ENTRY_TIME value.
		/// </summary>
        [Browsable(false)]
        public virtual DateTime? ENTRY_TIME { get; set; }
        [DisplayName("«·œÊ·…")]
        public virtual String COUNTRY { get; set; }
        [DisplayName("«·»—Ìœ")]
        public virtual String EMAIL_ADDR { get; set; }
        [DisplayName("«·„œÌ‰…")]
        public virtual String CITY { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Suppliers";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                {
                    EgxSupplier c = new EgxSupplier() { SUP_NAME = this.SUP_NAME };
                   if (da.IsExists(c))
            {
                sm.Message = "";
                sm.Type = MessageType.Stop;
                return sm;
            }
            else 
            {
                da.Insert(this);
                sm.Message = "";
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
                           sm.Message = "";
                           sm.Type = MessageType.Stop;
                            return sm;
                       }
                  }

		public virtual SystemMessage Delete() { 
                              try {
                                    da.Delete(this, "ID");
                                    sm.Message = "";
                                    sm.Type = MessageType.Stop;
                                    return sm; }
                              catch
                              {
                                  sm.Message = "";
                                  sm.Type = MessageType.Stop;
                                  return sm;
                              }
                    }

		
		public virtual List<EgxSupplier> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<EgxSupplier>().ToList();
                   }



		public virtual EgxSupplier GetByID() {  
                     return (EgxSupplier)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<EgxSupplier> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<EgxSupplier>().ToList();
                  }
	#endregion

            public DataTable GetSuppliersReport(DateTime? fromDate=null,DateTime? toDate=null,int supplier=-1) 
            {
                if (fromDate == null && toDate == null && supplier > 0)
                {
                    return DataAccess.GetDataTable("select SUM(quantity) as QNT,PRODUCT_NAME,(select SUP_NAME FROM Suppliers WHERE Suppliers.ID=SUPPLIER) AS SUPPLIER_NAME from OrderDetails where SUPPLIER ="+supplier.ToString()+"  group by SUPPLIER,PRODUCT_ID,PRODUCT_NAME");
                }
                else if (fromDate == null && toDate == null && supplier == -1)
                {
                    return DataAccess.GetDataTable("select SUM(quantity) as QNT,PRODUCT_NAME,(select SUP_NAME FROM Suppliers WHERE Suppliers.ID=SUPPLIER) AS SUPPLIER_NAME from OrderDetails where SUPPLIER <>-1  group by SUPPLIER,PRODUCT_ID,PRODUCT_NAME");
                }
                else if (fromDate != null && toDate != null && supplier == -1)
                {
                    return DataAccess.GetDataTable("select SUM(quantity) as QNT,PRODUCT_NAME,(select SUP_NAME FROM Suppliers WHERE Suppliers.ID=SUPPLIER) AS SUPPLIER_NAME from OrderDetails where SUPPLIER <>-1 and( tr_date between "+fromDate.SqlDateTime()+" and "+toDate.SqlDateTime()+")  group by SUPPLIER,PRODUCT_ID,PRODUCT_NAME");
                }
                else 
                {
                    return DataAccess.GetDataTable("select SUM(quantity) as QNT,PRODUCT_NAME,(select SUP_NAME FROM Suppliers WHERE Suppliers.ID=SUPPLIER) AS SUPPLIER_NAME from OrderDetails where SUPPLIER ="+supplier.ToString()+" and( tr_date between " + fromDate.SqlDateTime() + " and " + toDate.SqlDateTime() + ")  group by SUPPLIER,PRODUCT_ID,PRODUCT_NAME");
                }
            }
	}
}