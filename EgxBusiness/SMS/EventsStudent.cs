using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class EventsStudent
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public EventsStudent()
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
		/// Gets or sets the ST_ID value.
		/// </summary>
		public virtual Int32? ST_ID { get; set; }

		/// <summary>
		/// Gets or sets the ST_NAME value.
		/// </summary>
		public virtual String ST_NAME { get; set; }

		/// <summary>
		/// Gets or sets the EID value.
		/// </summary>
		public virtual Int32? EID { get; set; }

		/// <summary>
		/// Gets or sets the MARK value.
		/// </summary>
		public virtual Double? MARK { get; set; }

		/// <summary>
		/// Gets or sets the STATE value.
		/// </summary>
		public virtual Int32? STATE { get; set; }

        /// <summary>
        /// Gets or sets the REMARK value.
        /// </summary>
        public virtual String REMARK { get; set; }

        /// <summary>
        /// Gets or sets the REMARK value.
        /// </summary>
        public virtual bool? CERT_DONE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "EventsStudent" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   EventsStudent test = new EventsStudent();
                   test.EID = this.EID;
                   test.ST_ID = this.ST_ID;
                   if (da.IsExists(test))
            {
                sm.Message = "a student with the same event already registered ";
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
		public virtual List<EventsStudent> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<EventsStudent>().ToList();
                   }



		public virtual EventsStudent GetByID() {  
                     return (EventsStudent)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<EventsStudent> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<EventsStudent>().ToList();
                  }
		#endregion

            public static List<Students> GetEventStudents(int eventID,bool deprecateRefunded=false) 
            {

                List<Students> stList = new List<Students>();
                List<EventsStudent> est = new List<EventsStudent>();
                List<int?> sIDs = new List<int?>(); ;
                if (deprecateRefunded) 
                {
                    sIDs = new EventsStudent() { EID = eventID }.Search().Where((st => st.STATE != SystemConstants.StudentState.CANCELLED.ToInt32() && st.STATE != SystemConstants.StudentState.REFUND.ToInt32())).Select(s => s.ST_ID).ToList();
                }
                else
                {
                     sIDs = new EventsStudent() { EID = eventID }.Search().Select(s => s.ST_ID).ToList();
                }
                if (sIDs.Count > 0)
                {
                    int last = sIDs[sIDs.Count - 1].Value;
                    StringBuilder sb = new StringBuilder();
                    foreach (int sID in sIDs)
                    {
                        if (sID != last)
                        {
                            sb.Append(sID.ToString() + ",");
                        }
                        else
                        {
                            sb.Append(sID.ToString());
                        }
                    }
                    DataAccess da = new DataAccess();
                    return da.ExecuteSQL<Students>("select * from Students where ID in(" + sb.ToString() + ")");
                }
                else { return new List<Students>(); }
            }

            public static List<Events> GetPendingResults() 
            {
                DataAccess da = new DataAccess();
                return da.ExecuteSQL<Events>("select DISTINCT EID,CourseDaysTimes,CourseName,Capacity,CourseID,EventState,MaxCapacity,Events.ID,EventName,EventStartDate,EventEndDate from [Events]  join EventsStudent on Events.ID=EventsStudent.EID WHERE MARK IS NULL OR CERT_DONE<>1");
            }

            public static List<EventsStudent> GetEventStudentsExceptReufunded(int eventID)
            {
                return new EventsStudent() { EID = eventID }.Search().Where(es => es.STATE != -1).ToList();
            }
            public static List<EventsStudent> GetEventStudentsWithSpecificState(SystemConstants.StudentState state, int eventID,bool condition) 
            {
                if (condition) 
                {
                    return new EventsStudent() { EID = eventID }.Search().Where(es => es.STATE == state.ToInt32()).ToList();
                } 
                else 
                {
                    return new EventsStudent() { EID = eventID }.Search().Where(es => es.STATE != state.ToInt32()).ToList();
                }
            }
            
	}

}