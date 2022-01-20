using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class NewsFeed
	{
        public enum NewsFeedType {FAST_FEED=0,FULL_POST=1,ADV,EVENT=2 }
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public NewsFeed()
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
		/// Gets or sets the NewsDate value.
		/// </summary>
		public virtual DateTime? NewsDate { get; set; }

		/// <summary>
		/// Gets or sets the NewsType value.
		/// </summary>
		public virtual Int32? NewsType { get; set; }

		/// <summary>
		/// Gets or sets the NewsTitle value.
		/// </summary>
		public virtual String NewsTitle { get; set; }

		/// <summary>
		/// Gets or sets the NewsDescription value.
		/// </summary>
		public virtual String NewsDescription { get; set; }

		/// <summary>
		/// Gets or sets the NewsThumbnail value.
		/// </summary>
		public virtual String NewsThumbnail { get; set; }

		/// <summary>
		/// Gets or sets the IsFeatured value.
		/// </summary>
		public virtual Boolean? IsFeatured { get; set; }

		/// <summary>
		/// Gets or sets the IsEvent value.
		/// </summary>
		public virtual Boolean? IsEvent { get; set; }

		/// <summary>
		/// Gets or sets the EventFromDate value.
		/// </summary>
		public virtual DateTime? EventFromDate { get; set; }

		/// <summary>
		/// Gets or sets the EventToDate value.
		/// </summary>
		public virtual DateTime? EventToDate { get; set; }

		/// <summary>
		/// Gets or sets the EventTotalJoined value.
		/// </summary>
		public virtual Int32? EventTotalJoined { get; set; }

		/// <summary>
		/// Gets or sets the EventTotalMaybe value.
		/// </summary>
		public virtual Int32? EventTotalMaybe { get; set; }

		/// <summary>
		/// Gets or sets the EventTotalRefuse value.
		/// </summary>
		public virtual Int32? EventTotalRefuse { get; set; }

		/// <summary>
		/// Gets or sets the NewsLocationPoint value.
		/// </summary>
		public virtual String NewsLocationPoint { get; set; }

		/// <summary>
		/// Gets or sets the NewsLocationID value.
		/// </summary>
		public virtual Int32? NewsLocationID { get; set; }

		/// <summary>
		/// Gets or sets the NewsLocationName value.
		/// </summary>
		public virtual String NewsLocationName { get; set; }

		/// <summary>
		/// Gets or sets the NewsTotalLikes value.
		/// </summary>
		public virtual Int32? NewsTotalLikes { get; set; }

        /// <summary>
        /// Gets or sets the NewsTotalComments value.
        /// </summary>
        public virtual Int32? NewsTotalComments { get; set; }

		/// <summary>
		/// Gets or sets the IsPublished value.
		/// </summary>
		public virtual Boolean? IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the NewsTotalComments value.
        /// </summary>
        public virtual Int32? RelatedMerchantID { get; set; }

		#endregion

		#region Overrides
		public override String ToString()
		{
			return "NewsFeed" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   NewsFeed test = new NewsFeed();
                   test.NewsTitle = this.NewsTitle;
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
                this.ID = (int)DataAccess.GetSqlValue("select isnull(max(ID),0) from NewsFeed", EgxDataType.Mssql);
                sm.Attachment = this;

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
		public virtual List<NewsFeed> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<NewsFeed>().ToList();
                   }



		public virtual NewsFeed GetByID() {  
                     return (NewsFeed)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<NewsFeed> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<NewsFeed>().ToList();
                  }
		#endregion
	}
}