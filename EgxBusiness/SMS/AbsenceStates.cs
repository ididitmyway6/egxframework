using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class AbsenceStates
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public AbsenceStates()
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
		/// Gets or sets the STATE_NAME value.
		/// </summary>
		public virtual String STATE_NAME { get; set; }

		/// <summary>
		/// Gets or sets the STATE_DESC value.
		/// </summary>
		public virtual String STATE_DESC { get; set; }

		/// <summary>
		/// Gets or sets the EVAL_EFFECTIVE value.
		/// </summary>
		public virtual Boolean? EVAL_EFFECTIVE { get; set; }

		/// <summary>
		/// Gets or sets the DEDUCTED_VALUE value.
		/// </summary>
		public virtual Double? DEDUCTED_VALUE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "AbsenceStates" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   AbsenceStates test = new AbsenceStates();
                   test.STATE_NAME = this.STATE_NAME;
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
		public virtual List<AbsenceStates> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<AbsenceStates>().ToList();
                   }



		public virtual AbsenceStates GetByID() {  
                     return (AbsenceStates)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<AbsenceStates> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<AbsenceStates>().ToList();
                  }
		#endregion
	}
}