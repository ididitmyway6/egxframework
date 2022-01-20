using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
namespace Egx.EgxBusiness.Inventory
{
    public class AppCode
    {
        public int? ID { get; set; }
        public string ITEM_NAME { get; set; }
        public int? PARENT_ID { get; set; }
        public DateTime? date { get; set; }
        
        public override string ToString()
        {
            return "AppCodes";
        }
        DataAccess da;
        public AppCode() { da = new DataAccess(); }
        public AppCode(int id, int parentID) { da = new DataAccess(); }
        public AppCode(string ItemName) { }

        public List<AppCode> GetCategoryItems(int CategoryID) 
        {
            AppCode ac = new AppCode();
            ac.PARENT_ID = CategoryID;
            return da.Search(ac).Cast<AppCode>().ToList();
        }

        public List<AppCode> GetCategories()
        {
            AppCode ac = new AppCode();
            ac.PARENT_ID = 0;
            return da.Search(ac).Cast<AppCode>().ToList();
        }

        public List<AppCode> GetNullCategoies() 
        {
            return new DataAccess().ExecuteSQL <AppCode> ("SELECT * FROM AppCodes WHERE PARENT_ID =0").Cast<AppCode>().ToList();
        }

        public AppCode GetItem(int CategoryID, int ItemID) 
        {
            AppCode ac = new AppCode();
            ac.ID = ItemID;
            ac.PARENT_ID = CategoryID;
            var result = da.Search(ac).Cast<AppCode>().ToList();
            if (result != null && result.Count>0) 
            {
                return result[0];
            } 
            else 
            {
                return null;
            }
        }

        public void Insert() 
        {
            SystemSetting.IsPaymentTransaction = false;
          //  SystemSetting.CurrentUser = 1;
            SystemSetting.date = this.date;
            SystemTracking st=new SystemTracking();
            da.Insert(this);
        }

        public void Update() 
        {
            da.Update(this, "ID");
        }

        public void Delete() 
        {
            da.Delete(this, "ID");
        }

        public SystemMessage DeleteItem(int itemID) 
        {
            SystemMessage sm = new SystemMessage();
            try
            {
                this.ID = itemID;
                new DataAccess().Delete(this, "ID");
                sm.Type = MessageType.Pass;
                sm.Message = "OK";
            }
            catch (Exception exc) 
            {
                sm.Type = MessageType.Fail;
                sm.Message = exc.Message;
            }
            return sm;
        }

        public List<AppCode> AppCodeSelect() 
        {
            return new DataAccess().GetTopItems(20, this).Cast<AppCode>().ToList();
        }
    }
}
