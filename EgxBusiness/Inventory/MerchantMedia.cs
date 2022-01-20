using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class MerchantMedia
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public MerchantMedia()
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
		/// Gets or sets the MerchantID value.
		/// </summary>
		public virtual Int32? MerchantID { get; set; }

		/// <summary>
		/// Gets or sets the MediaFile value.
		/// </summary>
		public virtual String MediaFile { get; set; }

		/// <summary>
		/// Gets or sets the FileType value.
		/// </summary>
		public virtual String FileType { get; set; }

		/// <summary>
		/// Gets or sets the MediaDescription value.
		/// </summary>
		public virtual String MediaDescription { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "MerchantMedia" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   MerchantMedia test = new MerchantMedia();
                   test.MediaFile = this.MediaFile;
                   if (da.IsExists(test))
            {
                sm.Message = "Already Exist ... ";
                sm.Type = MessageType.Stop;
            }
            else 
            {
                da.Insert(this);
                sm.Attachment = da.GetLatestID(this);
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
		public virtual List<MerchantMedia> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<MerchantMedia>().ToList();
                   }



		public virtual MerchantMedia GetByID() {  
                     return (MerchantMedia)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<MerchantMedia> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<MerchantMedia>().ToList();
                  }
		#endregion
	}
}