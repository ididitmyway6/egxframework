using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
namespace Egx.EgxBusiness.Clinic
{
    public class Patient:EgxObject
    {
        public DataAccess da;
        public SystemMessage sm;
        public Int32? ID{get;set;}
        public string P_NAME { get; set; }
        public int? P_AGE { get; set; }
        public string P_ADDR { get; set; }
        public string P_PHONE { get; set; }
        public string MEDICAL_NOTES { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        public override string ToString()
        {
            return "Patients";
        }
        public Patient() 
        {
            da = new DataAccess();
            sm = new SystemMessage();
        }



        public SystemMessage Insert()
        {
            Patient test = new Patient();
            test.P_NAME = this.P_NAME;
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

        public SystemMessage Update()
        {
            try
            {
                da.Update(this, "ID");
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
            }
            catch { sm.Message = "Error"; sm.Type = MessageType.Fail; }
            return sm;
        }

        public SystemMessage Delete()
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

        public List<Patient> GetAll<Patient>()
        {
            return new DataAccess().GetTopItems(10000,this).Cast<Patient>().ToList();
        }

        public Patient GetByID(int id) 
        {
            return (Patient)new DataAccess().GetByID(id, this);
        }
    }
}
