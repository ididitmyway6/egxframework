using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness;
namespace Egx.EgxBusiness.GL
{
	

	[Serializable]
	public class Customer
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Customer()
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
        /// Gets or sets the Name value.
        /// </summary>
        public virtual String CName { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public virtual String PHONE2 { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public virtual String FAX { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public virtual String WEB_SITE { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public virtual int? GL_SALES_ACCOUNT { get; set; }

        
        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public virtual String CODE { get; set; }

		/// <summary>
		/// Gets or sets the City value.
		/// </summary>
		public virtual Int32? City { get; set; }

		/// <summary>
		/// Gets or sets the Country value.
		/// </summary>
		public virtual String Country { get; set; }

		/// <summary>
		/// Gets or sets the Phone value.
		/// </summary>
		public virtual String Phone { get; set; }

		/// <summary>
		/// Gets or sets the Email value.
		/// </summary>
		public virtual String Email { get; set; }

		/// <summary>
		/// Gets or sets the Address value.
		/// </summary>
		public virtual String Address { get; set; }

		/// <summary>
		/// Gets or sets the BlackList value.
		/// </summary>
		public virtual Boolean? BlackList { get; set; }

		/// <summary>
		/// Gets or sets the CreditLimit value.
		/// </summary>
		public virtual float? CreditLimit { get; set; }

        public virtual Int32? CustomerLevel { get; set; }

        public virtual Int32? Bank { get; set; }

        public virtual Int32? BankBranch { get; set; }

        public virtual Int32? REL_ACC { get; set; }

        public virtual Int32? REL_ACC_PARENT { get; set; }

        public virtual Int32? REL_ACC_CHILD { get; set; }

        public virtual decimal? BANK_FIRST_PERIOD_CREDIT { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Customers";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   Customer test = new Customer();
                   test.CName = this.CName;
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


        public virtual List<Customer> Search() 
                   {
                       return new DataAccess().Search(this).Cast<Customer>().ToList();
                   }



        public virtual Customer GetByID()
        {
            return (Customer)new DataAccess().GetByID(ID.Value, this);
                  }


        public virtual List<Customer> GetAll() 
                  {
                      return new DataAccess().GetTopItems(100, this).Cast<Customer>().ToList();
                  }
	#endregion
	}
}