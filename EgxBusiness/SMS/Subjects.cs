using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class Subjects
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Subjects()
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
		/// Gets or sets the CLASS_GRADE_ID value.
		/// </summary>
		public virtual Int32? CLASS_GRADE_ID { get; set; }

		/// <summary>
		/// Gets or sets the SUBJECT_NAME value.
		/// </summary>
		public virtual String SUBJECT_NAME { get; set; }

		/// <summary>
		/// Gets or sets the SNO value.
		/// </summary>
		public virtual String SNO { get; set; }

        public virtual float? PASS_DEG { get; set; }

        public virtual float? MAX_MARK { get; set; }

        public virtual float? DISCOUNT { get; set; }

        public virtual decimal? PRICE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Subjects" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                  // Subjects test = new Subjects();
                 //  test.SNO = this.SNO;
               //    if (da.IsExists(test))
           // {
            //    sm.Message = "Already Exist ... ";
            //    sm.Type = MessageType.Stop;
         //   }
          //  else 
          //  {
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
           // }
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
		public virtual List<Subjects> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Subjects>().ToList();
                   }



		public virtual Subjects GetByID() {  
                     return (Subjects)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Subjects> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Subjects>().ToList();
                  }
		#endregion

            public static object getMostFrequent() 
            {
               return DataAccess.GetQryData("select top 5 OrderDetails.PRODUCT_NAME,count(*) AS FREQ from OrderDetails group by OrderDetails.PRODUCT_NAME", EgxDataType.Mssql, EgxData.DataTable);
            }
	}
}