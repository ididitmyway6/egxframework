using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Unions
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Unions()
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
		/// Gets or sets the U_NAME value.
		/// </summary>
		public virtual String U_NAME { get; set; }

		/// <summary>
		/// Gets or sets the U_COMMISION value.
		/// </summary>
		public virtual Double? U_COMMISION { get; set; }

		/// <summary>
		/// Gets or sets the U_CODE value.
		/// </summary>
		public virtual String U_CODE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Unions" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Unions test = new Unions();
                   test.U_CODE = this.U_CODE;
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
		public virtual List<Unions> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Unions>().ToList();
                   }



		public virtual Unions GetByID() {  
                     return (Unions)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Unions> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Unions>().ToList();
                  }
		#endregion
	}
}