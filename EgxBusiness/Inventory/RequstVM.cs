using Egx.EgxDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness.Inventory
{
    public class RequstVM
    {
        public DataAccess da;
        public SystemMessage sm;
        public virtual Int32? ID { get; set; }
        public virtual string CUST_NAME { get; set; }
        public virtual string CUST_PHONE { get; set; }
        public virtual string CUST_CODE { get; set; }
        public virtual string CUST_ADD { get; set; }
        public virtual string EMAIL_ADDR { get; set; }
        public virtual DateTime? LAST_UPDATE_DATE { get; set; }
        public virtual Int32? REQUEST_STATUS { get; set; }
        public virtual string SALES_USER_NAME { get; set; }
        public virtual Int32? SALES_USER_ID { get; set; }
        public virtual Int32? SELLER_ID { get; set; }
        public virtual Int32? CUSTOMER_ID { get; set; }
        public virtual String OPERATOR_NAME { get; set; }
        public virtual String ADMIN_NAME { get; set; }
        public virtual Int32? OPERATOR_ID { get; set; }
        public virtual Int32? ADMIN_ID { get; set; }
        public virtual string RO { get; set; }
        public virtual string LAST_UPDATE_USER_NAME { get; set; }
        public virtual DateTime? CREATION_DATE { get; set; }
        public virtual decimal? COURRIER_RATE { get; set; }
        public virtual DateTime? DEPARTURE_DATE { get; set; }
        public virtual decimal? TARGET_PRICE { get; set; }
        public virtual double? DIMENTIONS { get; set; }
        public virtual double? SIZE_KILO { get; set; }
        public virtual string LoadingPort { get; set; }
        public virtual string LandingPort { get; set; }
        public virtual string ContainerType { get; set; }
        public virtual string ShippingType { get; set; }
        public virtual int? ticket_status { get; set; }
        //pol,pod,vol,container_type,early_date_of_dep,target_rate
        //change_password
        //100list request to handle
        //......................................................................
        #region Overrides
        public override String ToString()
        {
            return "RequestVM";
        }
        #endregion

        #region CRUD Methods
        public RequstVM() 
        {
            da = new DataAccess();
            sm = new SystemMessage();
        }
        public virtual SystemMessage Insert()
        {

            da.Insert(this);
            sm.Message = "OK ...";
            sm.Type = MessageType.Pass;
            sm.Attachment = new DataAccess().GetLatestID(this);
            return sm;
        }

        public virtual SystemMessage Update()
        {
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

        public virtual SystemMessage Delete()
        {
            try
            {
                da.Delete(this, "ID");
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
            }
            catch
            {
                sm.Message = "Error ...";
                sm.Type = MessageType.Fail;
            }
            return sm;
        }
        public virtual List<RequstVM> Search()
        {
            return new DataAccess().Search(this).Cast<RequstVM>().ToList();
        }



        public virtual RequstVM GetByID()
        {
            return (RequstVM)new DataAccess().GetByID(ID.Value, this);
        }


        public virtual List<RequstVM> GetAll()
        {
            return new DataAccess().GetTopItems(1000, this).Cast<RequstVM>().ToList();
        }
        #endregion
    }
}
