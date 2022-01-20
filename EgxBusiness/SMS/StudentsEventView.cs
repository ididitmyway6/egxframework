using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class StudentsEventView
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
         public StudentsEventView()
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
        /// Gets or sets the EventName value.
        /// </summary>
        public virtual String EventName { get; set; }

		/// <summary>
		/// Gets or sets the STATE value.
		/// </summary>
		public virtual Int32? EventState { get; set; }

        /// <summary>
        /// Gets or sets the REMARK value.
        /// </summary>
        public virtual String CourseName { get; set; }

        /// <summary>
        /// Gets or sets the EventStartDate value.
        /// </summary>
        public virtual DateTime? EventStartDate { get; set; }

        /// <summary>
        /// Gets or sets the EventEndDate value.
        /// </summary>
        public virtual DateTime? EventEndDate { get; set; }

        public virtual Int32? STATE { get; set; }

        public virtual Int32? MARK { get; set; }

        public virtual bool? CERT_DONE { get; set; }

        #endregion

		#region Overrides
		public override String ToString()
		{
            return "StudentsEventView";
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                {
                    sm = new SystemMessage();
                da.Insert(this);
      
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
        public virtual List<StudentsEventView> Search() 
                   {
                       return new DataAccess().Search(this).Cast<StudentsEventView>().ToList();
                   }



        public virtual StudentsEventView GetByID()
        {
            return (StudentsEventView)new DataAccess().GetByID(ID.Value, this);
                  }


        public virtual List<StudentsEventView> GetAll() 
                  {
                      return new DataAccess().GetTopItems(100, this).Cast<StudentsEventView>().ToList();
                  }
		#endregion
 

  
	}
}