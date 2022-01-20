using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class Students:IPerson
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Students()
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
        /// Gets or sets the ST_NAME value.
        /// </summary>
        public virtual String ST_NAME { get; set; }

        /// <summary>
        /// Gets or sets the PARENT_PHONE value.
        /// </summary>
        public virtual String PARENT_PHONE { get; set; }

        /// <summary>
        /// Gets or sets the ST_CODE value.
        /// </summary>
        public virtual String ST_CODE { get; set; }

		/// <summary>
		/// Gets or sets the ST_BIRTH_DATE value.
		/// </summary>
		public virtual DateTime? ST_BIRTH_DATE { get; set; }

		/// <summary>
		/// Gets or sets the ST_PHONE value.
		/// </summary>
		public virtual String ST_PHONE { get; set; }

		/// <summary>
		/// Gets or sets the ST_ADDR value.
		/// </summary>
		public virtual String ST_ADDR { get; set; }

		/// <summary>
		/// Gets or sets the ST_CLASS value.
		/// </summary>
		public virtual Int32? ST_CLASS { get; set; }

		/// <summary>
		/// Gets or sets the ST_STAT value.
		/// </summary>
		public virtual Int32? ST_STAT { get; set; }

		/// <summary>
		/// Gets or sets the LAST_UPDATE_DATE value.
		/// </summary>
		public virtual DateTime? LAST_UPDATE_DATE { get; set; }

        public virtual Int32? CURRENT_CLASS { get; set; }

        public virtual Int32? CURRENT_CLASS_GRADE { get; set; }

        public virtual Int32? REG_CLASS { get; set; }
        public virtual string ST_NOTES { get; set; }
        public virtual decimal? COURSE_FEES { get; set; }
        public virtual DateTime? JOIN_DATE { get; set; }
        public virtual DateTime? REFUND_DATE { get; set; }
        public virtual bool? REFUNDED { get; set; }
        
        #endregion

        public static int GetLatestStudentID()
        {
            return DataAccess.GetLatest("Students", "ID");
        }
        public static Students GetLatestStudent()
        {
            var st = new DataAccess().ExecuteSQL<Students>("select * from Students where ID=(SELECT ISNULL(MAX(ID),0) FROM Students)");
            if (st.Count > 0)
            {
                return st[0];
            }
            else { return null; }
        }

        public virtual List<Subjects> StudentSubjects { get 
        {
            if (ST_CLASS != null) { return new Subjects() { CLASS_GRADE_ID = ST_CLASS }.Search(); } else { return null; }
        } }
		#region Overrides
		public override String ToString()
		{
			return "Students" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Students test = new Students();
                   test.ST_NAME = this.ST_NAME;
                   if (da.IsExists(test))
            {
                sm.Message = "Already Exist ... ";
                sm.Type = MessageType.Stop;
            }
            else 
            {
                this.CURRENT_CLASS_GRADE = this.ST_CLASS ;
                this.CURRENT_CLASS = this.REG_CLASS;
                da.Insert(this);
                sm.Attachment = GetLatestStudent();
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
		public virtual List<Students> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Students>().ToList();
                   }


        public virtual Students GetByCode(string code) 
        {
            return (Students)new DataAccess().GetByColumn("ST_CODE", code, this);
        }
		public virtual Students GetByID() {  
                     return (Students)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Students> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Students>().ToList();
                  }
		#endregion

            public new string GetType()
            {
                return "Students";
            }
    }
}