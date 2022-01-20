using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using System.Data;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class User : EgxObject,IUser
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
        public virtual String RECOVERY_CODE { get; set; }
        public virtual DateTime? RECOVERY_EXP { get; set; }

		/// <summary>
		/// Gets or sets the ENTRY_DATE value.
		/// </summary>
		public virtual DateTime? ENTRY_DATE { get; set; }
        public virtual DateTime? UnbanDate { get; set; }

        public virtual int? SCOPE { get; set; }

        public virtual string UserFullName { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string AdminVFC { get; set; }
        public virtual string AdminName { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual bool? IsOnline { get; set; }
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
public DataTable GetUsersJournal(DateTime fromDate ,DateTime toDate) 
{
    return DataAccess.GetDataTable("select ISNULL(SUM(NET),0) as amount ,OrderDetails.USR_ID,USR_NAME,CAST(tr_date AS DATE) as tr_date from OrderDetails  where CAST(tr_date AS DATE)   between "+fromDate.SqlDateTime()+" and "+toDate.SqlDateTime()+" group by OrderDetails.USR_ID , OrderDetails.USR_NAME,CAST(tr_date AS DATE)");
}

public SystemMessage RequestNewPassword(string Identity) 
{
    SystemMessage sm = new SystemMessage();
    string sql = "select * from Customers where EMAIL_ADDR='"+Identity+"' or REAL_EMAIL='"+Identity+"' ";
    var customer = new DataAccess().ExecuteSQL<EgxCustomer>(sql);
    if (customer.Count > 0)
    {

        sm.Attachment = customer[0];
        sm.Message = " تم ارسال رسالة لبريدك الالكترونى "+customer[0].REAL_EMAIL;
        var u = new User() { USER_NAME=customer[0].EMAIL_ADDR }.Search();
        if (u.Count > 0) 
        {
            u[0].RECOVERY_CODE  = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("+", "").Replace("=", "");
            u[0].RECOVERY_EXP = DateTime.Now.AddDays(1);
            u[0].Update();
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "mail.basil.arvixe.com";
            client.Port = 25;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("password@inareg.com", "3343402");
            System.Net.Mail.MailAddressCollection to = new System.Net.Mail.MailAddressCollection();
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.IsBodyHtml = true;
            mail.To.Add(customer[0].REAL_EMAIL);
            mail.From = new System.Net.Mail.MailAddress("password@inareg.com", "نظام اينار");
            mail.Subject = "طلب تغيير كلمة السر";
            string link = "http://www.inareg.com/CPanel/ResetPassword?code="+u[0].RECOVERY_CODE+"&uid="+u[0].ID.Value+"";
            mail.Body = "<a href='"+link+"'>يمكنك طلب تغيير كلمة السر الخاصة بك من خلال الضغط على هذا الرابط</a><br/>ملاحظة : هذا الرابط صالح لمدة 24 ساعة فقط ";
            client.Send(mail);
           // client.Send("نظام اينار", "ziad.elnaggar1@yahoo.com", "طلب تغيير كلمة السر", "Click this link to request new password www.inareg.com/CPanel/ResetPassword?code=" + u[0].RECOVERY_CODE + "&uid="+u[0].ID.Value+"");
        }
        sm.Type = MessageType.Pass;
    }
    else 
    {
        sm.Message = "عفوا النظام لم يتمكن من التعرف على تحقيق الشخصية المدخل";
        sm.Type = MessageType.Stop;
    }
    return sm;
}

public bool IsValidPasswordRecovery(string code) 
{
    var u = new User() { RECOVERY_CODE = code }.Search();
    if (u.Count > 0 && u[0].RECOVERY_EXP.GetValueOrDefault().Date<=DateTime.Now.Date) 
    {
        return true;
    }else
    {
        return false;
    }
}

public EgxCustomer FindCustomerByIdentity(string identity) 
{
    var c = new DataAccess().ExecuteSQL<EgxCustomer>("select * from Customers where   EMAIL_ADDR= '" + identity + "'  or CUST_CODE='" + identity + "' or REAL_EMAIL='" + identity + "'");
    if (c.Count > 0)
    {
        return c[0];
    }
    else 
    {
        return null;
    }
}

public string UserScope { get { return ResolveUserScope(this.SCOPE); } }

private string ResolveUserScope(int? scope)
{
    if (scope == null) { return ""; }
    string res = "";
    switch (scope)
    {
        case 1: res = "SALES"; break;
        case 2: res = "OPERATION"; break;
        case 3: res = "ACCOUNTANT"; break;
        case 4: res = "OTHER"; break;
        case 5: res = "GM"; break;
        case 0: res = "ADMIN"; break;
    }
    return res;
}
}
}