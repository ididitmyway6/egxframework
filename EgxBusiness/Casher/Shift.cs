using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class Shift
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Shift()
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
		/// Gets or sets the StartDate value.
		/// </summary>
		public virtual DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets the EndDate value.
		/// </summary>
		public virtual DateTime? EndDate { get; set; }

		/// <summary>
		/// Gets or sets the StartTime value.
		/// </summary>
		public virtual String StartTime { get; set; }

		/// <summary>
		/// Gets or sets the EndTime value.
		/// </summary>
		public virtual String EndTime { get; set; }

		/// <summary>
		/// Gets or sets the OpenedByUser value.
		/// </summary>
		public virtual Int32? OpenedByUser { get; set; }

		/// <summary>
		/// Gets or sets the ShiftUser value.
		/// </summary>
		public virtual Int32? ShiftUser { get; set; }

		/// <summary>
		/// Gets or sets the StartingBalance value.
		/// </summary>
		public virtual Int32? StartingBalance { get; set; }

		/// <summary>
		/// Gets or sets the CloseBalance value.
		/// </summary>
		public virtual Int32? CloseBalance { get; set; }

		/// <summary>
		/// Gets or sets the PreviousShift value.
		/// </summary>
		public virtual Int32? PreviousShift { get; set; }

		/// <summary>
		/// Gets or sets the IsClosed value.
		/// </summary>
		public virtual Boolean? IsClosed { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Shifts";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   Shift test = new Shift();
                   //test.COMARISON_KEY = this.COMARISON_KEY
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

		
		public virtual List<Shift> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Shift>().ToList();
                   }



		public virtual Shift GetByID() {  
                     return (Shift)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Shift> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Shift>().ToList();
                  }
	#endregion
	}
}