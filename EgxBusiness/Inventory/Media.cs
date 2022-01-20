using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Media
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Media()
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
		/// Gets or sets the FILE_NAME value.
		/// </summary>
		public virtual String FILE_NAME { get; set; }

		/// <summary>
		/// Gets or sets the FILE_DESC value.
		/// </summary>
		public virtual String FILE_DESC { get; set; }

		/// <summary>
		/// Gets or sets the FILE_LINK value.
		/// </summary>
		public virtual String FILE_LINK { get; set; }

		/// <summary>
		/// Gets or sets the FILE_TYPE value.
		/// </summary>
		public virtual String FILE_TYPE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Media" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
                sm.Attachment = this;
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
		public virtual List<Media> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Media>().ToList();
                   }



		public virtual Media GetByID() {  
                     return (Media)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Media> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Media>().ToList();
                  }
		#endregion
	}
}