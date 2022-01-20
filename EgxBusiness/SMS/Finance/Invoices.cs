using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS.Finance
{

	[Serializable]
	public class Invoices
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Invoices()
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
		/// Gets or sets the OrderNumber value.
		/// </summary>
		public virtual Int32? OrderNumber { get; set; }

		/// <summary>
		/// Gets or sets the Total value.
		/// </summary>
        public virtual Decimal? Total { get; set; }
        public virtual Decimal? CCPAID { get; set; }

		/// <summary>
		/// Gets or sets the InvoiceDate value.
		/// </summary>
		public virtual DateTime? InvoiceDate { get; set; }

		/// <summary>
		/// Gets or sets the Customer value.
		/// </summary>
		public virtual Int32? Customer { get; set; }

		/// <summary>
		/// Gets or sets the Type value.
		/// </summary>
		public virtual String Type { get; set; }

		/// <summary>
		/// Gets or sets the InvoiceStatus value.
		/// </summary>
		public virtual String InvoiceStatus { get; set; }

        public virtual Int32? InvoiceNumber { get; set; }

        public virtual Int32? TR_TYPE { get; set; }

        public virtual Int32? Supplier { get; set; }
        public virtual Int32? CCMonth { get; set; }
        public virtual Int32? CCYEAR { get; set; }
        public virtual DateTime? CCDATE { get; set; }
        
        #endregion

		#region Overrides
		public override String ToString()
		{
			return "Invoices" ;
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
		public virtual List<Invoices> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Invoices>().ToList();
                   }



		public virtual Invoices GetByID() {  
                     return (Invoices)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Invoices> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Invoices>().ToList();
                  }
		#endregion

            public SystemMessage CreateVoucher(int ono, decimal total, DateTime date, string status, int tr_type,int unionID = -1, int customer = -1,int Supplier=-1)
            {
                sm = new SystemMessage();
                Invoices inv = new Invoices();
                inv.InvoiceNumber = GetLatestVoucher() + 1;
                inv.OrderNumber = ono;
                inv.Supplier = Supplier;
                inv.Total = total;
                inv.Type = InvoiceType.VOUCHER.ToString() ;
                inv.InvoiceDate = date;
                inv.InvoiceStatus = status;
                inv.TR_TYPE = tr_type;
                inv.Customer = customer;
                inv.Insert();
                sm.Attachment = inv.InvoiceNumber;
                return sm;
            }

            public static bool IsDoneInvoice(int ono, int type)
            {
                decimal result = decimal.Parse(DataAccess.GetSqlValue("select isnull(sum(TOTAL),0) from invoices where type='INVOICE' AND OrderNumber='" + ono.ToString() + "' and tr_type='" + type.ToString() + "' ", EgxDataType.Mssql).ToString());
                decimal compared = decimal.Parse(DataAccess.GetSqlValue("select isnull(sum(TOT),0) from Orders where ONO ='" + ono.ToString() + "' AND OTYPE='" + type.ToString() + "'", EgxDataType.Mssql).ToString());
                if (result == compared) { return true; } else { return false; }
            }
            public static bool IsDoneVoucher(int ono, int type)
            {
                decimal result = decimal.Parse(DataAccess.GetSqlValue("select isnull(sum(TOTAL),0) from invoices where type='VOUCHER' AND OrderNumber='" + ono.ToString() + "' and tr_type='" + type.ToString() + "' ", EgxDataType.Mssql).ToString());
                decimal compared = decimal.Parse(DataAccess.GetSqlValue("select isnull(sum(TOT),0) from Orders where ONO ='" + ono.ToString() + "' AND OTYPE='" + type.ToString() + "'", EgxDataType.Mssql).ToString());
                if (result == compared) { return true; } else { return false; }
            }

            public SystemMessage CreateInvoice(int ono, decimal total, DateTime date, string status,int tr_type, int unionID = -1, int customer = -1,int Supplier =-1)
            {
                sm = new SystemMessage();
                Invoices inv = new Invoices();
                inv.InvoiceNumber = GetLatestInvoice() + 1;
                inv.OrderNumber = ono;
                inv.Total =total;
                inv.Type = InvoiceType.INVOICE.ToString();
                inv.InvoiceDate = date;
                inv.InvoiceStatus = status;
                inv.Customer = customer;
                inv.TR_TYPE = tr_type;
                inv.Supplier = Supplier;
                inv.Insert();
                return sm;
            }
            
        
        public static int GetLatestInvoice() 
            {
                return Int32.Parse( DataAccess.GetSqlValue("select isnull(max(InvoiceNumber),0) from invoices where Type='INVOICE' ", EgxDataType.Mssql).ToString());
            }
            public static int GetLatestVoucher() 
            {
                return Int32.Parse(DataAccess.GetSqlValue("select isnull(max(InvoiceNumber),0) from invoices where Type='VOUCHER'", EgxDataType.Mssql).ToString());
            }
	}

    public enum InvoiceType{VOUCHER,INVOICE};
}