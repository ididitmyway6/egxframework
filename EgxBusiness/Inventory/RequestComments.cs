using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class RequestComments
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public RequestComments()
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
		/// Gets or sets the REQUEST_ID value.
		/// </summary>
		public virtual Int32? REQUEST_ID { get; set; }

		/// <summary>
		/// Gets or sets the COMMENT value.
		/// </summary>
		public virtual String COMMENT { get; set; }

		/// <summary>
		/// Gets or sets the USER_NAME value.
		/// </summary>
		public virtual String USER_NAME { get; set; }

		/// <summary>
		/// Gets or sets the SCOPE value.
		/// </summary>
		public virtual String SCOPE { get; set; }
        public virtual DateTime? COMMENT_DATE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "RequestComments" ;
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
		public virtual List<RequestComments> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<RequestComments>().ToList();
                   }



		public virtual RequestComments GetByID() {  
                     return (RequestComments)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<RequestComments> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<RequestComments>().ToList();
                  }
		#endregion
	}
}