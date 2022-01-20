using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using System.Data;

namespace Egx.EgxBusiness.Inventory
{
	

	[Serializable]
	public class Categ
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Categ()
		{
                     da = new DataAccess();
                     sm = new SystemMessage();
		}

		
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
		public virtual Int32? ID { get; set; }

        /// <summary>
        /// Gets or sets the PROD_ID value.
        /// </summary>
        public virtual Int32? PROD_ID { get; set; }
        /// <summary>
        /// Gets or sets the POINTS value.
        /// </summary>
        public virtual Int32? POINTS { get; set; }

		/// <summary>
		/// Gets or sets the SUP_CATEG value.
		/// </summary>
		public virtual Int32? SUP_CATEG { get; set; }

		/// <summary>
		/// Gets or sets the CATEG_CODE value.
		/// </summary>
		public virtual String CATEG_CODE { get; set; }

        /// <summary>
        /// Gets or sets the CATEG_NAME value.
        /// </summary>
        public virtual String CATEG_NAME { get; set; }

        /// <summary>
        /// Gets or sets the CATEG_IMG value.
        /// </summary>
        public virtual String CATEG_IMG { get; set; }

		/// <summary>
		/// Gets or sets the CATEG_DES value.
		/// </summary>
		public virtual String CATEG_DES { get; set; }

		/// <summary>
		/// Gets or sets the CATEG_UNIT value.
		/// </summary>
		public virtual Int32? CATEG_UNIT { get; set; }

		/// <summary>
		/// Gets or sets the CATEG_PRICE value.
		/// </summary>
		public virtual Decimal? CATEG_PRICE { get; set; }

		/// <summary>
		/// Gets or sets the CATEG_DISCOUNT value.
		/// </summary>
        public virtual float? CATEG_DISCOUNT { get; set; }

        /// <summary>
        /// Gets or sets the REORDER_POINT value.
        /// </summary>
        public virtual Int32? REORDER_POINT { get; set; }
        
        /// <summary>
        /// Gets or sets the CATEG_COLOR value.
        /// </summary>
        public virtual Int32? CATEG_COLOR { get; set; }

        /// <summary>
        /// Gets or sets the CATEG_COST value.
        /// </summary>
        public virtual Decimal? CATEG_COST { get; set; }

        /// <summary>
        /// Gets or sets the DEFAULT_SUPPLIER value.
        /// </summary>
        public virtual Int32? DEFAULT_SUPPLIER { get; set; }

        /// <summary>
        /// Gets or sets the DEFAULT_SUPPLIER value.
        /// </summary>
        public virtual Int32? SNO { get; set; }

        /// <summary>
        /// Gets or sets the DEFAULT_SUPPLIER value.
        /// </summary>
        public virtual float? SALES_TAX { get; set; }

        /// <summary>
        /// Gets or sets the DEFAULT_SUPPLIER value.
        /// </summary>
        public virtual float? REV_VALUE { get; set; }

		/// <summary>
		/// Gets or sets the IS_INV value.
		/// </summary>
		public virtual Boolean? IS_INV { get; set; }
        public virtual float? FirstQNT { get; set; }
        public virtual bool? IS_PUBLISHED { get; set; }
        public virtual int? VendorID { get; set; }
        public virtual bool? IS_BANNED { get; set; }
        public virtual string BAN_TEXT { get; set; }
        public virtual bool? IS_FEATURED { get; set; }
        public virtual bool? SystemProduct { get; set; }
        //Extra Field 
        public virtual string VendorName { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Categories";
		}

		
		#endregion





		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                {
                    Categ cat = new Categ() {  CATEG_CODE=this.CATEG_CODE};
                   if (da.IsExists(cat))
            {
                sm.Message = "هذا المنتج مسجل من قبل";
                           sm.Type= MessageType.Stop;
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
                              sm.Type = MessageType.Pass;
                              DataAccess.ExecData("update itemsstock set ITEM_NAME='"+this.CATEG_NAME+"' where item_id=" + this.ID + "");
                              DataAccess.ExecData("update  batches set categ_name='"+this.CATEG_NAME+"' where categ_id=" + this.ID + "");
                              return sm;
                      }
                     catch
                       {
                           sm.Message = "";
                           sm.Type = MessageType.Fail;
                            return sm;
                       }
                  }
        public virtual SystemMessage UpdateWithoutBatches() 
        {
            try
            {

                da.Update(this, "ID");
                sm.Message = "";
                sm.Type = MessageType.Pass;
                return sm;
            }
            catch
            {
                sm.Message = "";
                sm.Type = MessageType.Fail;
                return sm;
            }
        }

		public virtual SystemMessage Delete() { 
                              try {

                                var x =  DataAccess.GetSqlValue("select isnull(count(*),0) from orderdetails where product_id=" + this.ID + "", EgxDataType.Mssql);
                                  if (x.ToInt32() == 0)
                                  {
                                      da.Delete(this, "ID");
                                      sm.Message = "OK ....";
                                      sm.Type = MessageType.Pass;
                                      DataAccess.ExecData("delete from itemsstock where item_id=" + this.ID + "");
                                      DataAccess.ExecData("delete from batches where categ_id=" + this.ID + "");
                                  }
                                  else 
                                  {
                                      sm.Message = "لا تستطيع حذف المنتج لانه مستخدم بالفعل";
                                      sm.Type = MessageType.Stop;
                                  }
                                    return sm; 
                              }
                              catch
                              {
                                  sm.Message = "Fail ...";
                                  sm.Type = MessageType.Fail;
                                  return sm; }
                    }

		
		public virtual List<Categ> Search() 
                   {
                       return new DataAccess().Search(this).Cast<Categ>().ToList();
                   }



        public virtual Categ GetByID()
        {
            return (Categ)new DataAccess().GetByID(ID.Value, this);
                  }


        public virtual List<Categ> GetAll() 
                  {
                      return new DataAccess().GetTopItems(100, this).Cast<Categ>().ToList();
                  }

        public virtual Categ GetByCode(string code) 
        {
            return (Categ)new DataAccess().GetByColumn("CATEG_CODE",code, this);
        }
        public static int GetLastSerial(int supplier) 
        {
            return DataAccess.GetSqlValue("select isnull(max(SNO),0) from Categories where DEFAULT_SUPPLIER=" + supplier.ToString(), EgxDataType.Mssql).ToInt32();
        }
        public static string GetNextCode(int supllier,string supplier_code) 
        {
            string x = (DataAccess.GetSqlValue("select isnull(max(SNO),0) from Categories where DEFAULT_SUPPLIER=" + supllier.ToString(), EgxDataType.Mssql).ToInt32() + 1).ToString(EgxSetting.Get("CodingZeroDigits"));
            return supplier_code + x;

        }
	#endregion

        public DataTable GetProductSheet() 
        {
            return DataAccess.GetDataTable("select categ_code,ISNULL((select PRODUCT_NAME from Products where ID= PROD_ID),'لايوجد')PROD_NAME,CATEG_NAME,ISNULL(CATEG_PRICE,0) AS PRICE,ISNULL(CATEG_COST,0) AS COST ,ISNULL(FirstQNT,0) AS FQNT,ISNULL((select SUM(QOH) FROM ItemsStock WHERE ITEM_ID=Categories.ID),0) AS QOH from Categories");
        }
	}
}