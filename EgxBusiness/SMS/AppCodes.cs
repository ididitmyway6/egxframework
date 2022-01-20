using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class AppCodes
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public AppCodes()
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
		/// Gets or sets the Code value.
		/// </summary>
		public virtual Int32? Code { get; set; }

		/// <summary>
		/// Gets or sets the AR_Desc value.
		/// </summary>
		public virtual String AR_Desc { get; set; }

		/// <summary>
		/// Gets or sets the EN_Desc value.
		/// </summary>
		public virtual String EN_Desc { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "[AppCodes] " ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   AppCodes test = new AppCodes();
                   //test.COMARISON_KEY = this.COMARISON_KEY
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
		public virtual List<AppCodes> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<AppCodes>().ToList();
                   }



		public virtual AppCodes GetByID() {  
                     return (AppCodes)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<AppCodes> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<AppCodes>().ToList();
                  }
		#endregion
	}
}