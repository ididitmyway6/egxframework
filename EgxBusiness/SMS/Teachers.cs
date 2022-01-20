using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class Teachers:IPerson
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Teachers()
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
        /// Gets or sets the TNAME value.
        /// </summary>
        public virtual String TNAME { get; set; }


        /// <summary>
        /// Gets or sets the CODE value.
        /// </summary>
        public virtual String CODE { get; set; }

		/// <summary>
		/// Gets or sets the BIRTHDATE value.
		/// </summary>
		public virtual DateTime? BIRTHDATE { get; set; }

		/// <summary>
		/// Gets or sets the PHONE value.
		/// </summary>
		public virtual String PHONE { get; set; }

		/// <summary>
		/// Gets or sets the ADDRESS value.
		/// </summary>
		public virtual String ADDRESS { get; set; }

		/// <summary>
		/// Gets or sets the NOTES value.
		/// </summary>
		public virtual String NOTES { get; set; }

		/// <summary>
		/// Gets or sets the IDENTIFICATION value.
		/// </summary>
		public virtual String IDENTIFICATION { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Teachers" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Teachers test = new Teachers();
                   test.TNAME = this.TNAME;
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
		public virtual List<Teachers> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Teachers>().ToList();
                   }



		public virtual Teachers GetByID() {  
                     return (Teachers)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Teachers> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Teachers>().ToList();
                  }
		#endregion

            public new string GetType()
            {
                return "Teachers";
            }
    }
}