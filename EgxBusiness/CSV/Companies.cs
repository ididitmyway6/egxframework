using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.CSV
{
	

	[Serializable]
	public class Companies
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Companies()
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
		/// Gets or sets the CMP_NAME value.
		/// </summary>
		public virtual String CMP_NAME { get; set; }

		/// <summary>
		/// Gets or sets the LEGAL_NAME value.
		/// </summary>
		public virtual String LEGAL_NAME { get; set; }

		/// <summary>
		/// Gets or sets the ADDR value.
		/// </summary>
		public virtual String ADDR { get; set; }

		/// <summary>
		/// Gets or sets the COUNTRY value.
		/// </summary>
		public virtual String COUNTRY { get; set; }

		/// <summary>
		/// Gets or sets the SECTOR value.
		/// </summary>
		public virtual String SECTOR { get; set; }

		/// <summary>
		/// Gets or sets the CMP_TYPE value.
		/// </summary>
		public virtual Int32? CMP_TYPE { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Companies";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   Companies test = new Companies();
                   test.CMP_NAME = this.CMP_NAME;
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

		
		public virtual List<Companies> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Companies>().ToList();
                   }



		public virtual Companies GetByID() {  
                     return (Companies)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Companies> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Companies>().ToList();
                  }
	#endregion
	}
}