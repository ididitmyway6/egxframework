using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS.CourseCenterModule
{

	[Serializable]
	public class CCStudentEvaluation
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public CCStudentEvaluation()
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
        public virtual Int32? GroupID { get; set; }

		/// <summary>
		/// Gets or sets the ST_ID value.
		/// </summary>
		public virtual Int32? ST_ID { get; set; }

		/// <summary>
		/// Gets or sets the ST_NAME value.
		/// </summary>
		public virtual String ST_NAME { get; set; }

		/// <summary>
		/// Gets or sets the Homework value.
		/// </summary>
		public virtual Boolean? Homework { get; set; }

		/// <summary>
		/// Gets or sets the HomeworkNotes value.
		/// </summary>
		public virtual String HomeworkNotes { get; set; }

		/// <summary>
		/// Gets or sets the EvaluationTest value.
		/// </summary>
		public virtual Double? EvaluationTest { get; set; }

		/// <summary>
		/// Gets or sets the TestType value.
		/// </summary>
        public virtual Int32? TestType { get; set; }
        public virtual Int32? MaxMark { get; set; }
        public virtual Int32? DELAYING { get; set; }

		/// <summary>
		/// Gets or sets the EvaluationDate value.
		/// </summary>
		public virtual DateTime? EvaluationDate { get; set; }

		/// <summary>
		/// Gets or sets the IsAbsent value.
		/// </summary>
        public virtual Boolean? IsAbsent { get; set; }
        public virtual Boolean? NoHomework { get; set; }
        public virtual Boolean? viaBarCode { get; set; }
        public virtual string ST_CODE { get; set; }
        #endregion

		#region Overrides
		public override String ToString()
		{
			return "CCStudentEvaluation" ;
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
		public virtual List<CCStudentEvaluation> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<CCStudentEvaluation>().ToList();
                   }



		public virtual CCStudentEvaluation GetByID() {  
                     return (CCStudentEvaluation)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<CCStudentEvaluation> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<CCStudentEvaluation>().ToList();
                  }
		#endregion
	}
}