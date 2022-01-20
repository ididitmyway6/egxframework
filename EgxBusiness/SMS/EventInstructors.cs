using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class EventInstructors
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public EventInstructors()
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
        /// Gets or sets the ID value.
        /// </summary>
        public virtual Int32? STATE { get; set; }

		/// <summary>
		/// Gets or sets the INST_ID value.
		/// </summary>
		public virtual Int32? INST_ID { get; set; }

		/// <summary>
		/// Gets or sets the INST_CODE value.
		/// </summary>
		public virtual String INST_CODE { get; set; }

		/// <summary>
		/// Gets or sets the INST_NAME value.
		/// </summary>
		public virtual String INST_NAME { get; set; }

		/// <summary>
		/// Gets or sets the COURSE_ID value.
		/// </summary>
		public virtual Int32? COURSE_ID { get; set; }

		/// <summary>
		/// Gets or sets the EVENT_ID value.
		/// </summary>
		public virtual Int32? EVENT_ID { get; set; }

		/// <summary>
		/// Gets or sets the EVENT_DATE value.
		/// </summary>
		public virtual DateTime? EVENT_DATE { get; set; }

		/// <summary>
		/// Gets or sets the EVENT_NAME value.
		/// </summary>
		public virtual String EVENT_NAME { get; set; }

		/// <summary>
		/// Gets or sets the RATE value.
		/// </summary>
		public virtual Int32? RATE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "EventInstructors" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   EventInstructors test = new EventInstructors();
                   test.INST_ID = this.INST_ID;
                   test.EVENT_ID = this.EVENT_ID;
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
		public virtual List<EventInstructors> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<EventInstructors>().ToList();
                   }



		public virtual EventInstructors GetByID() {  
                     return (EventInstructors)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<EventInstructors> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<EventInstructors>().ToList();
                  }
		#endregion
	}
}