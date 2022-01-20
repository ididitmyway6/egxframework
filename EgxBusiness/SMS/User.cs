using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class User : EgxObject
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public User()
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
		/// Gets or sets the USER_NAME value.
		/// </summary>
		public virtual String USER_NAME { get; set; }

		/// <summary>
		/// Gets or sets the PASSWORD value.
		/// </summary>
		public virtual String PASSWORD { get; set; }

		/// <summary>
		/// Gets or sets the ENTRY_DATE value.
		/// </summary>
		public virtual DateTime? ENTRY_DATE { get; set; }

        public virtual int? SCOPE { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Users";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   User test = new User();
                   test.USER_NAME = this.USER_NAME;
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

		
		public virtual List<User> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<User>().ToList();
                   }



		public virtual User GetByID() {  
                     return (User)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<User> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<User>().ToList();
                  }
	#endregion
	

public List<User> GetAll<User>()
{
                                 return new DataAccess().GetTopItems(100,this).Cast<User>().ToList();
}

}
}