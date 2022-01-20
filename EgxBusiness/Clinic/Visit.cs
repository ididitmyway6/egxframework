using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using System.Windows.Forms;

namespace Egx.EgxBusiness.Clinic
{

	[Serializable]
	public class Visit
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Visit()
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
		/// Gets or sets the CHKUP_DATE value.
		/// </summary>
		public virtual DateTime? CHKUP_DATE { get; set; }

		/// <summary>
		/// Gets or sets the WEIGHT value.
		/// </summary>
		public virtual Int32? WEIGHT { get; set; }

		/// <summary>
		/// Gets or sets the SYMPTOMS value.
		/// </summary>
		public virtual String SYMPTOMS { get; set; }

		/// <summary>
		/// Gets or sets the DIAGNOSIS value.
		/// </summary>
		public virtual String DIAGNOSIS { get; set; }

		/// <summary>
		/// Gets or sets the PRESCRIPTION value.
		/// </summary>
		public virtual String PRESCRIPTION { get; set; }
        public virtual Int32? P_ID { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Visits";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                {

                    string sql = "select * from visits where CHKUP_DATE between #" + this.CHKUP_DATE.SqlDateTime(true) + "# and #2030-04-05# and p_id=" + this.P_ID.ToString() + "";
                    var test = new DataAccess().ExecuteSQL<Visit>(sql);
                    if (test.Count > 0)
                    {
                        DialogResult res = MessageBox.Show("You have already registered this patient today are you sure to insert this record again ", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (res == DialogResult.OK)
                        {
                            da.Insert(this);
                            sm.Message = "OK ...";
                            sm.Type = MessageType.Pass;
                        }
                        else
                        {
                            sm.Message = "Visit is already registered ... ";
                            sm.Type = MessageType.Stop;
                        }
                        return sm;

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

		public static Visit GetLastSession(int pid)
        {
            DataAccess daccess = new DataAccess();
            List<Visit> lst = daccess.ExecuteSQL<Visit>("select top  1 * from visits where P_ID="+pid+" order by CHKUP_DATE desc");
            if (lst.Count > 0)
            {
                return lst[0];
            }
            else { return null; }
        }

		public virtual List<Visit> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Visit>().ToList();
                   }



		public virtual Visit GetByID() {  
                     return (Visit)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Visit> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Visit>().ToList();
                  }
	#endregion
	}
}