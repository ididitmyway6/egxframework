using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using System.ComponentModel;

namespace Egx.EgxBusiness.Inventory
{
    #region Comments

    [Serializable]
    public class EgxCustomer
    {
        public DataAccess da;
        public SystemMessage sm;
        public EgxCustomer()
        {
            da = new DataAccess();
            sm = new SystemMessage();
        }


    #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ID value.
        /// </summary>
        [Browsable(false)]
        public string SearchKey { get; set; }

        public static int Count { get { return (int)DataAccess.GetSqlValue("select isnull(count(*),0) from customers", EgxDataType.Mssql); } }
        public virtual Int32? ID { get; set; }

        public virtual Int32? BANK_RESPONSE { get; set; }

        public virtual bool? BANK_INSTALLMENT { get; set; }

        public virtual string NM_CUSTOMER_REF { get; set; }
        public virtual string CUST_CODE { get; set; }
        public virtual string NM_REF_L2 { get; set; }
        public virtual string NM_REF_L3 { get; set; }
        public virtual string NM_REF_L4 { get; set; }
        public virtual DateTime? NM_JOIN_DATE { get; set; }
        public virtual decimal? Credit { get; set; }
        public virtual decimal? PrepaidCredit { get; set; }
        public virtual string ConfirmationCode { get; set; }
        public virtual bool? IsConfirmed { get; set; }
        public virtual int? CheckNumber { get; set; }
        public virtual string ProfilePicture { get; set; }
        public virtual bool? GIVE_COMMITION { get; set; }
        public virtual decimal? DIRECT_CREDIT { get; set; }
        public virtual decimal? CheckCounter { get; set; }
        public virtual int? Issues { get; set; }
        public virtual DateTime? exp { get; set; }
        public virtual String VFC { get; set; }
        public virtual String BillingAddress { get; set; }
        public virtual String FBAccount { get; set; }
        public virtual bool? PaymentVerified { get; set; }
        public virtual int? IsBanned { get; set; }
        public virtual int? ProfileImageTumb { get; set; }
        public static EgxCustomer GetLatestCustomer() 
        {
            return new EgxCustomer() {ID=DataAccess.GetLatest("Customers","ID") }.GetByID();
        }

        /// <summary>
        /// Gets or sets the CUST_NAME value.
        /// </summary>
        [DisplayName("ÇáÇÓã")]
        public virtual String CUST_NAME { get; set; }

        /// <summary>
        /// Gets or sets the CUST_ADD value.
        /// </summary>
        [DisplayName("ÇáÚäæÇä")]
        public virtual String CUST_ADD { get; set; }

        [DisplayName("ÚÏÏ äÞÇØ Çæá ÇáãÏÉ")]
        public virtual Int32? START_POINTS { get; set; }

        /// <summary>
        /// Gets or sets the CUST_PHONE value.
        /// </summary>
        [DisplayName("ÇáåÇÊÝ1")]
        public virtual String CUST_PHONE { get; set; }

        /// <summary>
        /// Gets or sets the CUST_PHONE2 value.
        /// </summary>
        [DisplayName("ÇáåÇÊÝ2")]
        public virtual String CUST_PHONE2 { get; set; }

        /// <summary>
        /// Gets or sets the CUST_PHONE3 value.
        /// </summary>
        [Browsable(false)]
        public virtual String CUST_PHONE3 { get; set; }

        /// <summary>
        /// Gets or sets the CUST_PHONE4 value.
        /// </summary>
        [Browsable(false)]
        public virtual String CUST_PHONE4 { get; set; }

        /// <summary>
        /// Gets or sets the CUST_ID_NO value.
        /// </summary>
        [DisplayName("ÑÞã ÇáÈØÇÞÉ")]
        public virtual String CUST_ID_NO { get; set; }

        /// <summary>
        /// Gets or sets the CUST_CREDIT_LIMIT value.
        /// </summary>
        [DisplayName("ÇáÍÏ ÇáÇÆÊãÇäì")]
        public virtual float? CUST_CREDIT_LIMIT { get; set; }

        /// <summary>
        /// Gets or sets the IS_ACTIVE value.
        /// </summary>
        [DisplayName("ãÝÚá")]
        public virtual Boolean? IS_ACTIVE { get; set; }

        /// <summary>
        /// Gets or sets the ENTERED_BY value.
        /// </summary>
        [Browsable(false)]
        public virtual Int32? ENTERED_BY { get; set; }

        public virtual Int32? CURRENT_POINTS { get; set; }

        /// <summary>
        /// Gets or sets the ENTRY_DATE value.
        /// </summary>
        [Browsable(false)]
        public virtual DateTime? ENTRY_DATE { get; set; }

        /// <summary>
        /// Gets or sets the ENTRY_TIME value.
        /// </summary>
        [Browsable(false)]
        public virtual DateTime? ENTRY_TIME { get; set; }
        [DisplayName("ÇáÏæáÉ")]
        public virtual String COUNTRY { get; set; }
        [DisplayName("ÇáÈÑíÏ")]
        public virtual String EMAIL_ADDR { get; set; }
        [DisplayName("ÇáãÏíäÉ")]
        public virtual String CITY { get; set; }
        public virtual String pwd { get; set; }
        public virtual String retype_pwd { get; set; }
        public virtual String REAL_EMAIL { get; set; }
        public virtual Int32? SELLER_ID { get; set; }
        public virtual bool? ACTIVE_PURCHASER { get; set; }
        #endregion

        #region Overrides

        public override String ToString()
        {
            return "Customers";
        }


        #endregion

        #region CRUD Methods

        public virtual SystemMessage Insert()
        {
            EgxCustomer c = new EgxCustomer() { CUST_CODE = this.CUST_CODE };
            if (da.IsExists(c))
            {
                sm.Message = "رقم الكود مسجل من قبل";
                sm.Type = MessageType.Stop;
                return sm;
            }
            else
            {
                da.Insert(this);
                sm.Message = "ÊãÊ ÇáÇÖÇÝÉ ÈäÌÇÍ";
                sm.Type = MessageType.Pass;
                sm.Attachment = DataAccess.GetLatest(this.ToString(), "ID");
                return sm;
            }

        }


        /// <summary>
        /// It's Only Used For Network Marketing System Waffar & Inar :)
        /// </summary>
        /// <returns>SystemMessage</returns>
        public virtual SystemMessage InsertNM()
        {
            EgxCustomer c = new EgxCustomer() { CUST_CODE = this.CUST_CODE };
            if (da.IsExists(c))
            {
                //sm.Message = "رقم البطاقة مسجل من قبل";
                //sm.Type = MessageType.Stop;
                int N = 0;
                c.CUST_CODE = this.CUST_CODE + "1";
                while (da.IsExists(c))
                {
                    N = Char.GetNumericValue(c.CUST_CODE[11]).ToInt32() + 1;
                    c.CUST_CODE = c.CUST_CODE.Substring(0, c.CUST_CODE.Length - 1);
                    c.CUST_CODE = c.CUST_CODE + N.ToString();
                }
                this.CUST_CODE = c.CUST_CODE;
                da.Insert(this);
                sm.Message = "ÊãÊ ÇáÇÖÇÝÉ ÈäÌÇÍ";
                sm.Type = MessageType.Pass;
                sm.Attachment = DataAccess.GetLatest(this.ToString(), "ID");

                return sm;
            }
            else
            {
                da.Insert(this);
                sm.Message = "ÊãÊ ÇáÇÖÇÝÉ ÈäÌÇÍ";
                sm.Type = MessageType.Pass;
                sm.Attachment = DataAccess.GetLatest(this.ToString(), "ID");
                return sm;
            }

        }


        public virtual SystemMessage Update()
        {
            try
            {
                da.Update(this, "ID");
                sm.Message = "Êã ÇáÊÚÏíá";
                sm.Type = MessageType.Pass;
                return sm;
            }
            catch(Exception e)
            {
                sm.Message = e.Message;
                sm.Type = MessageType.Fail;
                return sm;
            }
        }

        public virtual SystemMessage Delete()
        {
            try
            {
                da.Delete(this, "ID");
                sm.Type = MessageType.Pass;
                sm.Message = "Êã ÇáÍÐÝ";
                return sm;
            }
            catch(Exception e) 
            {
                sm.Message = e.Message;
                sm.Type = MessageType.Fail;
                return sm;
            }
        }


        public virtual List<EgxCustomer> Search()
        {
            return new DataAccess().Search(this).Cast<EgxCustomer>().ToList();
        }



        public virtual EgxCustomer GetByID()
        {
            return (EgxCustomer)new DataAccess().GetByID(ID.Value, this);
        }

        public SystemMessage SetInstallmentWaiting(int customerID) 
        {
            this.ID = customerID;
            this.BANK_INSTALLMENT = true;
            this.BANK_RESPONSE = 0;
            return this.Update();
        }

        public SystemMessage SetInstallmentOk(int customerID)
        {
            this.ID = customerID;
            this.BANK_INSTALLMENT = true;
            this.BANK_RESPONSE = 1;
            return this.Update();
        }
        public SystemMessage SetInstallmentRejected(int customerID)
        {
            this.ID = customerID;
            this.BANK_INSTALLMENT = true;
            this.BANK_RESPONSE = -1;
            return this.Update();
        }


        public virtual List<EgxCustomer> GetAll()
        {
            return new DataAccess().GetTopItems(100, this).Cast<EgxCustomer>().ToList();
        }
        public EgxCustomer GetByCode(string code) 
        {
            return new DataAccess().ExecuteSQL<EgxCustomer>("select * from Customers where cust_code='"+code+"'").FirstOrDefault();
        }
        #endregion
    }
}