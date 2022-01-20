using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class NewsFeedComments
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public NewsFeedComments()
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
		/// Gets or sets the UserID value.
		/// </summary>
		public virtual Int32? UserID { get; set; }

		/// <summary>
		/// Gets or sets the IsInarUser value.
		/// </summary>
		public virtual Boolean? IsInarUser { get; set; }

		/// <summary>
		/// Gets or sets the Comment value.
		/// </summary>
		public virtual String Comment { get; set; }

		/// <summary>
		/// Gets or sets the CommentDate value.
		/// </summary>
		public virtual DateTime? CommentDate { get; set; }

		/// <summary>
		/// Gets or sets the TotalLikes value.
		/// </summary>
		public virtual Int32? TotalLikes { get; set; }

		/// <summary>
		/// Gets or sets the TotalDislikes value.
		/// </summary>
		public virtual Int32? TotalDislikes { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "NewsFeedComments" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   NewsFeedComments test = new NewsFeedComments();
                   test.Comment = this.Comment;
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
		public virtual List<NewsFeedComments> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<NewsFeedComments>().ToList();
                   }



		public virtual NewsFeedComments GetByID() {  
                     return (NewsFeedComments)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<NewsFeedComments> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<NewsFeedComments>().ToList();
                  }
		#endregion
	}
}