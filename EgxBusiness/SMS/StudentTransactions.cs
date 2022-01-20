using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class StudentTransactions
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public StudentTransactions()
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
		/// Gets or sets the STUDENT value.
		/// </summary>
		public virtual Int32? STUDENT { get; set; }

		/// <summary>
		/// Gets or sets the ST_TR_ID value.
		/// </summary>
		public virtual Int32? ST_TR_ID { get; set; }

		/// <summary>
		/// Gets or sets the FROM_DATE value.
		/// </summary>
		public virtual DateTime? FROM_DATE { get; set; }

		/// <summary>
		/// Gets or sets the TO_DATE value.
		/// </summary>
		public virtual DateTime? TO_DATE { get; set; }

		/// <summary>
		/// Gets or sets the TR_CLASS value.
		/// </summary>
		public virtual Int32? TR_CLASS { get; set; }

		/// <summary>
		/// Gets or sets the TR_MONTH value.
		/// </summary>
		public virtual Int32? TR_MONTH { get; set; }

        /// <summary>
        /// Gets or sets the TR_YEAR value.
        /// </summary>
        public virtual Int32? TR_YEAR { get; set; }

        /// <summary>
        /// Gets or sets the TR_YEAR value.
        /// </summary>
        public virtual Int32? TR_TYPE { get; set; }

		/// <summary>
		/// Gets or sets the AMOUNT value.
		/// </summary>
		public virtual Decimal? AMOUNT { get; set; }

		/// <summary>
		/// Gets or sets the STARS value.
		/// </summary>
		public virtual Int32? STARS { get; set; }

		/// <summary>
		/// Gets or sets the NOTE value.
		/// </summary>
		public virtual String NOTE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "StudentTransactions" ;
		}
		#endregion

		#region CRUD Methods


        public static int GetNextTransactionID() 
        {
            return DataAccess.GetLatest("StudentTransactions", "ST_TR_ID")+1;
        }

		public virtual SystemMessage Insert() 
                { 
                   StudentTransactions test = new StudentTransactions();
                   test.ST_TR_ID = this.ST_TR_ID;
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
		public virtual List<StudentTransactions> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<StudentTransactions>().ToList();
                   }



		public virtual StudentTransactions GetByID() {  
                     return (StudentTransactions)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<StudentTransactions> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<StudentTransactions>().ToList();
                  }
		#endregion
	}
}