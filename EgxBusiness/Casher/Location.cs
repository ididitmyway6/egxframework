using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class Location
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Location()
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
		/// Gets or sets the LocationName value.
		/// </summary>
		public virtual String LocationName { get; set; }

		/// <summary>
		/// Gets or sets the LocationDescription value.
		/// </summary>
		public virtual String LocationDescription { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Locations";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   Location test = new Location();
                   test.LocationName = this.LocationName;
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

		
		public virtual List<Location> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Location>().ToList();
                   }



		public virtual Location GetByID() {  
                     return (Location)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Location> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Location>().ToList();
                  }
	#endregion
	}
}