using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Alerts
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Alerts()
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
		/// Gets or sets the ALERT_NAME value.
		/// </summary>
        public virtual String ALERT_NAME { get; set; }
        public virtual String CTYPE { get; set; }

		/// <summary>
		/// Gets or sets the ALERT_DATE value.
		/// </summary>
		public virtual DateTime? ALERT_DATE { get; set; }
        public virtual Int32? CID { get; set; }
        public virtual Int32? OID { get; set; }
        public virtual Int32? VID { get; set; }
        public virtual Int32? ADMIN_ID { get; set; }
        public virtual bool? SEEN { get; set; }

		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Alerts" ;
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
		public virtual List<Alerts> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Alerts>().ToList();
                   }



		public virtual Alerts GetByID() {  
                     return (Alerts)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Alerts> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Alerts>().ToList();
                  }

            public virtual List<Alerts> GetTopTen(int objectId,bool isAdmin=false)
            {
                if(isAdmin)
                return new DataAccess().ExecuteSQL<Alerts>("select * from alerts where ADMIN_ID="+objectId.ToString());
                else
                return new DataAccess().ExecuteSQL<Alerts>("select * from alerts where CID=" + objectId.ToString() + " OR VID=" + objectId.ToString());
            }
		#endregion
	}
}