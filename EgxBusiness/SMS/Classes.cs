using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class Classes
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Classes()
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
        /// Gets or sets the MAX_CAPS value.
        /// </summary>
        public virtual Int32? MAX_CAPS { get; set; }

		/// <summary>
		/// Gets or sets the CLASS_NAME value.
		/// </summary>
		public virtual String CLASS_NAME { get; set; }

		/// <summary>
		/// Gets or sets the CLASS_TYPE value.
		/// </summary>
		public virtual Int32? CLASS_TYPE { get; set; }

		/// <summary>
		/// Gets or sets the STATE value.
		/// </summary>
		public virtual Int32? STATE { get; set; }

		/// <summary>
		/// Gets or sets the RMRK value.
		/// </summary>
		public virtual String RMRK { get; set; }

		/// <summary>
		/// Gets or sets the CLASS_CODE value.
		/// </summary>
		public virtual String CLASS_CODE { get; set; }
        public virtual string START_TIME_HH { get; set; }
        public virtual string START_TIME_MM { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Classes" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Classes test = new Classes();
                   test.CLASS_CODE = this.CLASS_CODE;
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
		public virtual List<Classes> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Classes>().ToList();
                   }



		public virtual Classes GetByID() {  
                     return (Classes)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Classes> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Classes>().ToList();
                  }
		#endregion
	}
}