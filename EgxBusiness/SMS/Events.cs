using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class Events
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Events()
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
		/// Gets or sets the EventName value.
		/// </summary>
		public virtual String EventName { get; set; }

		/// <summary>
		/// Gets or sets the CourseName value.
		/// </summary>
		public virtual String CourseName { get; set; }

        /// <summary>
        /// Gets or sets the CourseID value.
        /// </summary>
        public virtual Int32? CourseID { get; set; }

        /// <summary>
        /// Gets or sets the CourseID value.
        /// </summary>
        public virtual string CourseDaysTimes { get; set; }

		/// <summary>
		/// Gets or sets the Capacity value.
		/// </summary>
		public virtual Int32? Capacity { get; set; }

		/// <summary>
		/// Gets or sets the EventStartDate value.
		/// </summary>
		public virtual DateTime? EventStartDate { get; set; }

		/// <summary>
		/// Gets or sets the EventEndDate value.
		/// </summary>
		public virtual DateTime? EventEndDate { get; set; }

		/// <summary>
		/// Gets or sets the EventState value.
		/// </summary>
		public virtual Int32? EventState { get; set; }

        public virtual Int32? MaxCapacity { get; set; }

        public string Time { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Events" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Attachment = GetLastEventID();
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
		public virtual List<Events> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Events>().ToList();
                   }



		public virtual Events GetByID() {  
                     return (Events)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Events> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Events>().ToList();
                  }

            public static List<Events> GetRunningCourses() 
            {
                Events e = new Events();
                e.EventState = SystemConstants.EventState.STARTED.ToInt32();
               return e.Search().Where(evt => (evt.EventStartDate.Value.Date <= DateTime.Now.Date && evt.EventEndDate.Value.Date >= DateTime.Now.Date)).ToList();
            }
          
            public static List<Events> GetTodayCourses() 
            {
                List<Events> lst = new List<Events>();
                List<Events> retLst = new List<Events>();
                string day = DateTime.Now.DayOfWeek.ToString().Remove(3);
                string[] days;
                lst = GetRunningCourses();
                foreach (Events e in lst) 
                {
                    if (!String.IsNullOrEmpty(e.CourseDaysTimes))
                    {
                        days = e.CourseDaysTimes.Split(';');
                      //  if (days.Contains(day)) { retLst.Add(e); }
                        if (days.Where(d => d.Contains(day)).Count() > 0) 
                        {
                            e.Time = days.Where(d => d.Contains(day)).First().split(',')[1];
                            retLst.Add(e);
                        }
                        if (days.Length > 0) { Array.Clear(days, 0, days.Length - 1); }
                    }
                }
                return retLst;
            }
            
            public static List<Events> GetStartedOrNotStartedEvents() 
            {
                return new DataAccess().ExecuteSQL<Events>("select * from events where EventState=-1 or EventState=1");
            }    
       
            public static int GetLastEventID() 
            {
                return DataAccess.GetLatest("Events", "ID");
            }

            public static void CancelEvent(Events evnt) 
            {
                evnt.EventState = SystemConstants.EventState.CANCELLED.ToInt32();
                evnt.Update();
                DataAccess.ExecData("UPDATE EventsStudent SET STATE=-2 WHERE EID=" + evnt.ID.ToString());
                DataAccess.ExecData("UPDATE EventInstructors SET STATE=-2 WHERE EVENT_ID=" + evnt.ID.ToString());
                DataAccess.ExecData("UPDATE OrderDetails SET IS_CANCELED=1 WHERE EID="+evnt.ID.ToString());
                decimal totalPaid = (decimal)DataAccess.GetSqlValue("SELECT ISNULL(SUM(PAID),0) FROM Orders  JOIN OrderDetails on OrderDetails.ONO=Orders.ONO WHERE OrderDetails.EID=" + evnt.ID.ToString() + " AND TRANS_TYPE=-10 AND OTYPE=-10 ", EgxDataType.Mssql);
                decimal totalRefunded = (decimal)DataAccess.GetSqlValue("SELECT ISNULL(SUM(Orders.TOT),0) FROM Orders  JOIN OrderDetails on OrderDetails.ONO=Orders.ONO WHERE OrderDetails.EID=" + evnt.ID.ToString() + "AND  ISNULL(OrderDetails.RETURNED_QNT,0)=0 AND TRANS_TYPE=-8 AND OTYPE=-8 ", EgxDataType.Mssql);
                decimal netRefunded = totalPaid - totalRefunded;
                Finance.TransactionViewModel.CreateDeduction(-1, netRefunded, SystemConstants.EventState.CANCELLED.ToInt32(), -1, DateTime.Now.Date, "Returned fees of event " + evnt.EventName + " cancellation ");

            }
        
        #endregion
	}
}