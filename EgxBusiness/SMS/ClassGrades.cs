using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class ClassGrades
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public ClassGrades()
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
		/// Gets or sets the GRADE_NAME value.
		/// </summary>
		public virtual String GRADE_NAME { get; set; }

		/// <summary>
		/// Gets or sets the GRADE_ORDER value.
		/// </summary>
		public virtual Int32? GRADE_ORDER { get; set; }

        public virtual float? GRADE_PASS_MARK { get; set; }

        public virtual string DES { get; set; }
        public virtual string CODE { get; set; }

        
        #endregion

		#region Overrides
		public override String ToString()
		{
			return "ClassGrades" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   ClassGrades test = new ClassGrades();
                   test.GRADE_NAME = this.GRADE_NAME;
                   if (da.IsExists(test))
            {
                sm.Message = "ÌÃ» «⁄«œ… ’Ì«€… «· — Ì» ";
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
		public virtual List<ClassGrades> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<ClassGrades>().ToList();
                   }



		public virtual ClassGrades GetByID() {  
                     return (ClassGrades)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<ClassGrades> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<ClassGrades>().ToList();
                  }
		#endregion
	}
}