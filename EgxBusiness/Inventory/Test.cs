using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using System.ComponentModel;

namespace Egx.EgxBusiness.Inventory
{
    public class Test
    {

        public int? ID { get; set; }
        public string STR { get; set; }
        public int? INTGR { get; set; }
        public float? FLOT { get; set; }
        public DateTime? DAT { get; set; }
        public bool? BOOL { get; set; }
        public decimal? DEC { get; set; }
        public int? IDt { get; set; }
        public string STRt { get; set; }
        public int? INTGRt { get; set; }
        public float? FLOTt { get; set; }
        public DateTime? DATt { get; set; }
        public bool? BOOLt { get; set; }
        public decimal? DECt { get; set; }
        SystemMessage sm;
        public override string ToString()
        {
            return "Test";
        }
        public DataAccess da;
        public Test()
        {
            da = new DataAccess();
            DECt = 10;
            BOOLt = false;
            DATt = null;
            FLOTt = 1.2f;
            INTGRt = 1;

            STRt = null;

        }
        public SystemMessage Insert()
        {
            sm = new SystemMessage();
            Test t = new Test();
            t.STR = this.STR;
            if (!da.IsExists(t))
            {
                da.Insert(this);
                sm.Message = "OK ..";
                sm.Type = MessageType.Pass;
            }
            else { sm.Type = MessageType.Stop; sm.Message = "Already Exist"; }
            return sm;
        }

        public void Update()
        {
            da.Update(this, "ID");
        }

        public Test GetByID()
        {
            return (Test)da.GetByID(this.ID.Value, this);
        }

        public List<Test> GetAll()
        {
            return da.GetTopItems(100, this).Cast<Test>().ToList();
        }


        public void Delete()
        {
            da.Delete(this, "ID");
        }


    }
}
