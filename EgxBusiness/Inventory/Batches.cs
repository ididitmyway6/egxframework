using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using System.Data;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Batches
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Batches()
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
        /// Gets or sets the BATCH_ID value.
        /// </summary>
        public virtual Int32? CATEG_ID { get; set; }

        /// <summary>
        /// Gets or sets the BATCH_ID value.
        /// </summary>
        public virtual String CATEG_NAME { get; set; }

		/// <summary>
		/// Gets or sets the BATCH_DATE value.
		/// </summary>
		public virtual DateTime? BATCH_DATE { get; set; }

		/// <summary>
		/// Gets or sets the PROD_DATE value.
		/// </summary>
		public virtual DateTime? PROD_DATE { get; set; }

		/// <summary>
		/// Gets or sets the EXP_DATE value.
		/// </summary>
		public virtual DateTime? EXP_DATE { get; set; }
        public virtual float? QNT { get; set; }
        public virtual bool? isFirstPeriod { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Batches" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Batches test = new Batches();
                 
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
		public virtual List<Batches> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Batches>().ToList();
                   }



		public virtual Batches GetByID() {  
                     return (Batches)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Batches> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Batches>().ToList();
                  }

            public static List<Batches> ListExp(bool getExpired)
            {
                if (getExpired)
                {
                    return new DataAccess().ExecuteSQL<Batches>("select * from batches where qnt<>0 and  exp_date<="+DateTime.Now.SqlDateTime()+"");
                }
                else
                {
                    return new DataAccess().ExecuteSQL<Batches>("select * from batches where qnt<>0 and DATEDIFF(DAY, " + DateTime.Now.SqlDateTime() + ", exp_date)<=" + EgxSetting.Get("expalertperiod") + "");
                }
            }

            public static float GetBatchQuantityOf(int categID) 
            {
                var x = DataAccess.GetQryData("select isnull(sum(qnt),0) from batches where categ_id='"+categID.ToString()+"'", EgxDataType.Mssql, EgxData.DataRow);
                if (x != null)
                {
                    return float.Parse(((DataRow)x)[0].ToString());
                }
                else { return 0; }
                
            }
		#endregion
	}
}