using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class Attendance
	{
	            public DataAccess da;
                public SystemMessage sm;
                public List<AbsenceStates> abStates { get; set; }
		#region Construction
		public Attendance()
		{
		    da = new DataAccess();
            sm=  new SystemMessage();
            abStates = new AbsenceStates().GetAll();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
		public virtual Int32? ID { get; set; }

        public virtual Int32? EventID { get; set; }
        /// <summary>
        /// Gets or sets the RECORD_DATE value.
        /// </summary>
        public virtual DateTime? RECORD_DATE { get; set; }

        /// <summary>
        /// Gets or sets the RECORD_DATE value.
        /// </summary>
        public virtual Int32? TR_ID { get; set; }


		/// <summary>
		/// Gets or sets the RECORD_ID value.
		/// </summary>
		public virtual Int32? RECORD_ID { get; set; }

		/// <summary>
		/// Gets or sets the IS_STUDENT value.
		/// </summary>
		public virtual Boolean? IS_STUDENT { get; set; }

		/// <summary>
		/// Gets or sets the IS_FULL_ATTEND value.
		/// </summary>
		public virtual Boolean? IS_FULL_ATTEND { get; set; }

		/// <summary>
		/// Gets or sets the NOTES value.
		/// </summary>
		public virtual String NOTES { get; set; }

        public virtual Int32? CLASS { get; set; }

        public virtual Int32? CLASS_GRADE { get; set; }

        public virtual Int32? ABSENCE_STATE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Attendance" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
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
		public virtual List<Attendance> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Attendance>().ToList();
                   }



		public virtual Attendance GetByID() {  
                     return (Attendance)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Attendance> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Attendance>().ToList();
                  }
          
        //Very Important note for CRM System ---> CLASS Field Works as EventID
        public SystemMessage RegisterAbsence(IPerson person, bool isFullAttendance, DateTime date, string notes = null,int eventID=-1,int courseID=-1)
            {
                sm = new SystemMessage();

                if (person.GetType() == "Students")
                {
                    var x = person as Students;
                    if (GetStudentsAbsenceList(date.Date, eventID < 0 ? x.CURRENT_CLASS.Value : eventID).Contains(x.ID.Value)) { return sm = new SystemMessage() { Type = MessageType.Pass, Message = "Updated" }; }
                    else
                    {
                        if (eventID < 0)
                        {
                            return new Attendance() { ABSENCE_STATE = 0, IS_STUDENT = true, RECORD_ID = x.ID, IS_FULL_ATTEND = isFullAttendance, RECORD_DATE = date.Date, NOTES = notes, CLASS_GRADE = x.CURRENT_CLASS_GRADE, CLASS = x.CURRENT_CLASS }.Insert();
                        }
                        else 
                        {
                            return new Attendance() { ABSENCE_STATE = 0, IS_STUDENT = true, RECORD_ID = x.ID, IS_FULL_ATTEND = isFullAttendance, RECORD_DATE = date.Date, NOTES = notes, CLASS_GRADE = courseID, CLASS = eventID }.Insert();
                        }
                    }
                }
                else
                {
                    var y = person as Teachers;
                    return new Attendance() {ABSENCE_STATE=0, IS_STUDENT = false, RECORD_ID = y.ID, IS_FULL_ATTEND = IS_FULL_ATTEND, RECORD_DATE = date, NOTES = notes }.Insert();
                }
            } // ok


            public static List<int> GetStudentsAbsenceList(DateTime date, int classID) 
            {

                List<int> lst = new List<int>();
                Attendance at = new Attendance();
                at.CLASS = classID;
                at.RECORD_DATE = date.Date;
                at.IS_STUDENT = true;
                at.IS_FULL_ATTEND = true;
                var result = at.Search();
                foreach (Attendance attend in result) 
                {
                    lst.Add(attend.RECORD_ID.Value);
                }
                return lst;
            } //ok
            public SystemMessage UnregisterAbsence(IPerson person, DateTime date,int eventID=-1)
            {
                sm = new SystemMessage();
                Attendance at = new Attendance();
                if (person.GetType() == "Students")
                {
                    var x = person as Students;
                    if (eventID < 0) { at.CLASS = x.CURRENT_CLASS; } else { at.CLASS = eventID; }
                    if (eventID < 0) { at.CLASS_GRADE = x.CURRENT_CLASS_GRADE; }
                    at.IS_STUDENT = true;
                    at.RECORD_DATE = date.Date;
                    at.RECORD_ID = x.ID;
                    var record = at.Search();
                    at = record.Count == 1 ? record[0] : null;
                    if (at != null) { return at.Delete(); } else { sm = new SystemMessage(); sm.Message = "No such record"; sm.Type = MessageType.Stop; return sm; }
                }
                else
                {
                    var y = person as Teachers;
                    at.IS_STUDENT = false;
                    at.RECORD_ID = y.ID;
                    at.RECORD_DATE = date;
                    var record = at.Search();
                    at = record.Count == 1 ? record[0] : null;
                    if (at != null) { return at.Delete(); } else { sm = new SystemMessage(); sm.Message = "No such record"; sm.Type = MessageType.Stop; return sm; }
                }
            } //ok
            public static bool IsAbsent(DateTime date,Students student,out Attendance returnedRecord) 
            {
                Attendance at = new Attendance();
                at.RECORD_DATE = date.Date;
                at.IS_STUDENT = true;
                at.IS_FULL_ATTEND = true;
                at.RECORD_ID = student.ID;
                List<Attendance> lst =at.Search();
                if (lst.Count > 0) { returnedRecord = lst[0]; return true; } else { returnedRecord = null; return false; }
                
            }
            public static List<AttendanceVM> getStudentSheet(DateTime from, DateTime to, IPerson student) 
            {

                Attendance att = new Attendance();
                List<AttendanceVM> sheet = new List<AttendanceVM>();
                while (from.Date <= to.Date) 
                {
                    if (IsAbsent(from, (Students)student, out att))
                    {
                        if (att.ABSENCE_STATE == 0)
                        {
                            sheet.Add(new AttendanceVM { Notes = "€Ì«» »œÊ‰ ”»»", DATE = from.Date, IS_ABSENT = true, ID = ((Students)student).ID.Value, ST_NAME = ((Students)student).ST_NAME, student = ((Students)student) });
                        }
                        else
                        {

                            sheet.Add(new AttendanceVM { Notes = att.abStates.Where(abs => abs.ID == att.ABSENCE_STATE).First().STATE_NAME, DATE = from.Date, IS_ABSENT = true, ID = ((Students)student).ID.Value, ST_NAME = ((Students)student).ST_NAME, student = ((Students)student) });

                        }
                    }
                    else
                    {
                        if (IsHoliday(from))
                        {
                            sheet.Add(new AttendanceVM { Notes = "«Ã«“… —”„Ì…", DATE = from.Date, IS_ABSENT = false, ID = ((Students)student).ID.Value, ST_NAME = ((Students)student).ST_NAME, student = ((Students)student) });

                        }
                        else
                        {
                            sheet.Add(new AttendanceVM { DATE = from.Date, IS_ABSENT = false, ID = ((Students)student).ID.Value, ST_NAME = ((Students)student).ST_NAME, student = ((Students)student) });
                        }
                    }
                    from = from.AddDays(1);
                }
                return sheet;
            }
            public SystemMessage RegisterAbsenceWithTransaction(IPerson person, bool isFullAttendance, DateTime dateFrom,DateTime dateTo,int absenceState, string notes = null,int eventID=-1,int courseID=-1)
            {
                int transID = StudentTransactions.GetNextTransactionID();
                StudentTransactions trans = new StudentTransactions();
                trans.ST_TR_ID = transID;
                trans.FROM_DATE = dateFrom.Date;
                trans.TO_DATE = dateTo.Date;
                trans.STUDENT = (person as Students).ID;
                trans.AMOUNT = dateTo.Subtract(dateFrom).Days+1;
                trans.NOTE = notes;
                trans.TR_CLASS = (person as Students).CURRENT_CLASS;
                trans.TR_MONTH = dateFrom.Month;
                trans.TR_YEAR = dateFrom.Year;
                trans.TR_TYPE = -10;
                trans.Insert();
                if (person.GetType() == "Students")
                {
                    while (dateFrom.Date <= dateTo.Date) 
                    {
                        var x = person as Students;
                        if (GetStudentsAbsenceList(dateFrom.Date, eventID < 0 ? x.CURRENT_CLASS.Value : eventID).Contains(x.ID.Value))
                        {
                            UnregisterAbsence(x, dateFrom);
                            if (eventID < 0)
                            {
                                new Attendance() { ABSENCE_STATE = absenceState, IS_STUDENT = true, RECORD_ID = x.ID, IS_FULL_ATTEND = isFullAttendance, RECORD_DATE = dateFrom.Date, NOTES = notes, CLASS_GRADE = x.CURRENT_CLASS_GRADE, CLASS = x.CURRENT_CLASS, TR_ID = transID }.Insert();
                            }
                            else
                            {
                                new Attendance() { ABSENCE_STATE = absenceState, IS_STUDENT = true, RECORD_ID = x.ID, IS_FULL_ATTEND = isFullAttendance, RECORD_DATE = dateFrom.Date, NOTES = notes, CLASS_GRADE =courseID , CLASS = eventID, TR_ID = transID }.Insert();
                            }
                        }
                        else
                        {
                            if (eventID < 0)
                            {
                                new Attendance() { ABSENCE_STATE = absenceState, IS_STUDENT = true, RECORD_ID = x.ID, IS_FULL_ATTEND = isFullAttendance, RECORD_DATE = dateFrom.Date, NOTES = notes, CLASS_GRADE = x.CURRENT_CLASS_GRADE, CLASS = x.CURRENT_CLASS, TR_ID = transID }.Insert();
                            }
                            else
                            {
                                new Attendance() { ABSENCE_STATE = absenceState, IS_STUDENT = true, RECORD_ID = x.ID, IS_FULL_ATTEND = isFullAttendance, RECORD_DATE = dateFrom.Date, NOTES = notes, CLASS_GRADE = courseID, CLASS = eventID, TR_ID = transID }.Insert();
                            }
                        }
                        dateFrom = dateFrom.Date.AddDays(1);
                    }
                    
                    return new SystemMessage() { Message=" „  «·⁄„·Ì… »‰Ã«Õ", Type= MessageType.Pass};
                }
                else
                {
                    //Teachers Module
                    return new SystemMessage();
                }
            }

            public static bool IsHoliday(DateTime date)
            {
                return SystemSetting.GetOfficialHolidays().Contains(date);
            }
		#endregion
	}
}