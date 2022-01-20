using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class UserTags
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public UserTags()
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
		/// Gets or sets the UID value.
		/// </summary>
		public virtual Int32? UID { get; set; }

		/// <summary>
		/// Gets or sets the TID value.
		/// </summary>
		public virtual Int32? TID { get; set; }
        public virtual String TagLocation { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "UserTags" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   UserTags test = new UserTags();
                   test.TID = this.TID;
                   test.UID = this.UID;
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
		public virtual List<UserTags> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<UserTags>().ToList();
                   }



		public virtual UserTags GetByID() {  
                     return (UserTags)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<UserTags> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<UserTags>().ToList();
                  }
		#endregion
            public string tagName { get; set; }
            public string userName { get; set; }
            public IEnumerable<UserTags> LoadUserTags(int uid=-1) 
            {
                string sql = "";
                if (uid == -1)
                {
                     sql = "select *,(select CUST_NAME from Customers where ID = ut.uid ) as userName , (select TagName from Tags where ID = ut.tid ) as tagName from UserTags ut ";
                }
                else 
                {
                    sql = "select *,(select CUST_NAME from Customers where ID = ut.uid ) as userName , (select TagName from Tags where ID = ut.tid ) as tagName from UserTags ut where ut.uid='"+uid+"' ";
                }
                var result = da.ExecuteSQL<UserTags>(sql, new List<string>() { "tagName","userName" });
                return result;
            }
	}
}