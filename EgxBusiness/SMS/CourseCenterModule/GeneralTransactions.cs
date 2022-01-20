using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness.EgxSMS.Finance;
using System.Data;
namespace Egx.EgxBusiness.EgxSMS.CourseCenterModule
{
    public class GeneralTransactions
    {
        public enum TRANSACTION_TYPES { PAY_FEES=1,DELETE_STUDENT=2}
        public enum EXAM_TYPE { DAILY=0,MONTHLY=1,NONE=-1}
        public static SystemMessage UpdateDailySheet(DateTime date,List<CCStudentEvaluation> evaluationData,bool NoHomework,EXAM_TYPE examType,int MaxMark) 
        {
            bool defaultIsAbsent = EgxSetting.Get("AutomatedAttendance").ToInt32() == 1 ? true : false ;
            SystemMessage sm = new SystemMessage();
            List<string> cmdList = new List<string>();
            bool hw = false;
            foreach (CCStudentEvaluation evalData in evaluationData) 
            {
                if (NoHomework) { hw = false; } else { hw = evalData.Homework.GetValueOrDefault(false); }

                var x = DataAccess.GetDataRow("select * from CCStudentEvaluation where EvaluationDate=" + date.SqlDateTime() + " and st_id='" + evalData.ST_ID.ToString() + "'");
                if (x == null)
                {

                    cmdList.Add("INSERT INTO CCStudentEvaluation(ST_ID,DELAYING,MaxMark,GroupID,ST_NAME,Homework,HomeworkNotes,EvaluationTest,TestType,EvaluationDate,IsAbsent,ST_CODE,NoHomework) values('" + evalData.ST_ID.ToString() + "','" + evalData.DELAYING.GetValueOrDefault(0).ToString() + "','" + MaxMark.ToString() + "','" + evalData.GroupID.ToString() + "','" + evalData.ST_NAME + "'," + hw.IntBool() + ",'" + evalData.HomeworkNotes + "' ,'" + (examType != EXAM_TYPE.NONE ? evalData.EvaluationTest.GetValueOrDefault(0).ToString() : "0") + "','" + examType.ToInt32().ToString() + "'," + evalData.EvaluationDate.SqlDateTime() + "," + evalData.IsAbsent.GetValueOrDefault(defaultIsAbsent).IntBool() + ",'" + evalData.ST_CODE + "'," + NoHomework.IntBool() + ")");
                }
                else 
                {
                    cmdList.Add("UPDATE CCStudentEvaluation SET MaxMark='" + MaxMark.ToString() + "',DELAYING='" + evalData.DELAYING.GetValueOrDefault(0).ToString() + "', Homework='" + hw.IntBool() + "',HomeworkNotes='" + evalData.HomeworkNotes + "',EvaluationTest='" + (examType != EXAM_TYPE.NONE ? evalData.EvaluationTest.GetValueOrDefault(0).ToString() : "0") + "',TestType='" + examType.ToInt32().ToString() + "',IsAbsent='" + evalData.IsAbsent.GetValueOrDefault(defaultIsAbsent).IntBool() + "',NoHomework='" + NoHomework.IntBool() + "' WHERE ST_ID='" + evalData.ST_ID.ToString() + "' AND EvaluationDate=" + evalData.EvaluationDate.SqlDateTime() + " ");
                }
            }
            DataAccess.ExecTransaction(cmdList);
            return sm;
        }
        public static SystemMessage PayCourseFees(Students student, decimal paid, int month,int year, DateTime transactionDate) 
        {
            SystemMessage sm = new SystemMessage();
            sm.Message = "عفوا ، لم تتم العملية";
            sm.Type = MessageType.Stop;
            Invoices invoice = new Invoices(){ CCMonth=month,CCYEAR=year, Customer=student.ID.Value};
            var invcs = invoice.Search();
            if (invcs.Count == 0)
            {
                invoice.CCPAID = paid;
                invoice.CCDATE = new DateTime(year, month, 28);
                invoice.InvoiceDate = transactionDate;
                if (student.COURSE_FEES == paid) { invoice.InvoiceStatus = "DONE"; } else { invoice.InvoiceStatus = "PARTIAL"; }
                invoice.OrderNumber = -1;
                invoice.Total = student.COURSE_FEES.Value;
                invoice.TR_TYPE = 1;
                if (invoice.Insert().Type == MessageType.Pass)
                {
                    if (new Egx.EgxBusiness.EgxSMS.Finance.AccountTransactions() { ACTUAL_DATE = transactionDate, Amount = paid, Comment = "تحصيل مصروفات", IsBank = false, OrderID = -1, SupType = student.ID, TransactionDate = transactionDate, TransactionType = 1, TransClosed = 1 }.Insert().Type == MessageType.Pass)
                    {
                        sm.Type = MessageType.Pass;
                        sm.Message = "تمت العملية بنجاح";
                    }
                }
            }
            else 
            {
                invoice = invcs[0];
                if (invoice.InvoiceStatus == "DONE")
                {
                    sm.Type = MessageType.Pass;
                    sm.Message = "تم التحصيل من قبل";
                }
                else
                {
                    if (invoice.CCPAID + paid >= invoice.Total)
                    {
                        paid = invoice.Total.Value - invoice.CCPAID.GetValueOrDefault(0);
                        invoice.InvoiceStatus = "DONE";
                        invoice.CCPAID = invoice.Total;
                    }
                    else 
                    {
                        invoice.CCPAID += paid;
                    }
                    invoice.Update();
                    if (new Egx.EgxBusiness.EgxSMS.Finance.AccountTransactions() { ACTUAL_DATE = transactionDate, Amount =paid, Comment = "تحصيل باقى مصروفات", IsBank = false, OrderID = -1, SupType = student.ID, TransactionDate = transactionDate, TransactionType = 1, TransClosed = 1 }.Insert().Type == MessageType.Pass)
                    {
                        sm.Type = MessageType.Pass;
                        sm.Message = "تمت العملية بنجاح";
                    }
                }
            }
            return sm;
        }
        public static SystemMessage RegisterAttendance(Students student,out CCStudentEvaluation e) 
        {
            SystemMessage sm = new SystemMessage();
            CCStudentEvaluation eval = new CCStudentEvaluation();
            Classes c = new Classes() { ID = student.CURRENT_CLASS.Value }.GetByID();

            var x = DataAccess.GetDataRow("select * from CCStudentEvaluation where EvaluationDate=" + DateTime.Now.Date.SqlDateTime() + " and st_id='" + student.ID.Value.ToString() + "'");
            if (x != null) 
            {
                sm.Attachment = x;
                sm.Message = "هذا الطالب مسجل فى تقارير اليوم";
                if (x["IsAbsent"].ToString() == "True")
                {
                    DataAccess.ExecData("update CCStudentEvaluation SET IsAbsent=0 , DELAYING='" + DateTime.Now.TimeOfDay.Subtract(TimeSpan.Parse(c.START_TIME_HH + ":" + c.START_TIME_MM)).TotalMinutes.ToInt32() + "' where EvaluationDate=" + DateTime.Now.Date.SqlDateTime() + " and st_id='" + student.ID.Value.ToString() + "'");
                    sm.Type = MessageType.Pass;
                    e = new CCStudentEvaluation() { GroupID = x["GroupID"].ToInt32(), ST_CODE = x["ST_CODE"].ToString(), ST_NAME = x["ST_NAME"].ToString(), DELAYING = DateTime.Now.TimeOfDay.Subtract(TimeSpan.Parse(c.START_TIME_HH + ":" + c.START_TIME_MM)).TotalMinutes.ToInt32() };
                }
                else
                {
                    sm.Type = MessageType.Stop;
                    e = null;
                }
            } else
            {
                eval.DELAYING = DateTime.Now.TimeOfDay.Subtract(TimeSpan.Parse(c.START_TIME_HH+":"+c.START_TIME_MM)).TotalMinutes.ToInt32();
                eval.EvaluationDate = DateTime.Now.Date.Date;
                eval.GroupID = student.CURRENT_CLASS.Value;
                eval.IsAbsent = false;
                eval.ST_CODE = student.ST_CODE;
                eval.ST_ID = student.ID.Value;
                eval.ST_NAME = student.ST_NAME;
                eval.viaBarCode = true;
                sm=eval.Insert();
                e = eval;
            }
            return sm;
        }
        public static List<CCStudentEvaluation> GetEvalDayList() 
        {
            return new DataAccess().ExecuteSQL<CCStudentEvaluation>("select * from CCStudentEvaluation where IsAbsent=0 and  EvaluationDate=" + DateTime.Now.Date.SqlDateTime() + " ");
        }
        
        public static decimal GetDuesFromStudent(int studentID,decimal courseFees, DateTime fromDate, DateTime toDate) 
        {
            int monthes =Egx.EgxBusiness.Date.CalculateDifferenceInMonth(fromDate,toDate);
            decimal req = courseFees * monthes;
            return req - DataAccess.GetSqlValue("select ISNULL(SUM(ccpaid),0) from Invoices where Invoices.Customer='" + studentID.ToString() + "' and( Invoices.CCDate>=" + new DateTime(fromDate.Year, fromDate.Month, 28).SqlDateTime() + " and Invoices.CCDate<=" + new DateTime(toDate.Year, toDate.Month, 28).SqlDateTime() + ")", EgxDataType.Mssql).ToDecimal(0);
        }
        public static decimal GetRefundAmount(int studentID, decimal courseFees, DateTime refundDate) 
        {
            int BeforeDay = EgxSetting.Get("BeforeDay").ToInt32();
            int CompleteRefundBeforeDay = EgxSetting.Get("CompleteRefundBeforeDay").ToInt32();
            float BeforeDayPer = EgxSetting.Get("BeforeDayPercent").ToFloat(0)/100;
            float AfterDayPer = EgxSetting.Get("AfterDayPercent").ToFloat(0) / 100;
            if (CompleteRefundBeforeDay>0 && refundDate.Day < CompleteRefundBeforeDay) 
            {
                return courseFees;
            } 
            else 
            {
                if (refundDate.Day < BeforeDay)
                {
                    return (courseFees * Convert.ToDecimal(BeforeDayPer));
                }
                else
                {
                    return (courseFees * Convert.ToDecimal(AfterDayPer));
                }
            }
            
            
        }
        public static SystemMessage DeleteStudent(Students student,DateTime refundDate,decimal amount) 
        {
            SystemMessage sm = new SystemMessage();
            try
            {
                if (amount == 0)
                {
                    student.REFUNDED = true;
                    student.REFUND_DATE = refundDate;
                    student.Update();
                    sm.Message = "تم الغاء قيد الطالب بنجاح";
                    sm.Type = MessageType.Pass;
                }
                else
                {
                    if (new Egx.EgxBusiness.EgxSMS.Finance.AccountTransactions() { ACTUAL_DATE = DateTime.Now.Date, Amount = amount, Comment = "تسوية حساب طالب", IsBank = false, OrderID = -1, SupType = student.ID, TransactionDate = refundDate.Date, TransactionType = 2, TransClosed = 1 }.Insert().Type == MessageType.Pass)
                    {
                        student.REFUNDED = true;
                        student.REFUND_DATE = refundDate;
                        student.Update();
                        sm.Message = "تم الغاء قيد الطالب بنجاح";
                        sm.Type = MessageType.Pass;
                    }
                    else
                    {
                        sm.Message = "فشل فى تنفيذ العملية";
                        sm.Type = MessageType.Stop;
                    }
                }
            }
            catch(Exception exc)
            {
                sm.Message = exc.Message;
                sm.Type = MessageType.Fail;
            }

            return sm;
        }

        #region System Reports Queries
        public static IEnumerable<DateTime> GetDateRange(DateTime startingDate,DateTime endingDate) 
        {
            List<DateTime> allDates=new List<DateTime>();
            for (DateTime i = startingDate; i <= endingDate; i = i.AddMonths(1))
            {
                allDates.Add(i);
            }
            return allDates;
        }
        public static List<RptFees> GetStudentFeesBetween(int studentID, decimal req, DateTime from, DateTime to) 
        {
            DataTable dt = new DataTable();
            StringBuilder subQury = new StringBuilder();
            string sql = "";
            List<DateTime> dateRange = GetDateRange(from, to).ToList();
            foreach (DateTime date in dateRange) 
            {
                if (date == dateRange[dateRange.Count - 1])
                {
                    subQury.Append("select [Month]='" + date.Month.ToString() + "',[Year]='" + date.Year.ToString() + "',[Required]='" + req.ToString() + "',[InvoiceDate]='" + date.Month.ToString() + "/" + date.Year.ToString() + "',(select sum(ccpaid) from Invoices where CCMonth='" + date.Month.ToString() + "' and ccyear='" + date.Year.ToString() + "' and Customer='" + studentID.ToString() + "')as Paid ");
                }
                else 
                {
                    subQury.Append("select [Month]='" + date.Month.ToString() + "',[Year]='" + date.Year.ToString() + "',[Required]='" + req.ToString() + "',[InvoiceDate]='" + date.Month.ToString() + "/" + date.Year.ToString() + "',(select sum(ccpaid) from Invoices where CCMonth='" + date.Month.ToString() + "' and ccyear='" + date.Year.ToString() + "' and Customer='" + studentID.ToString() + "')as Paid union all ");
                }
            }
            sql = "select * from ("+subQury.ToString()+") dummy";
            var data = DataAccess.GetDataTable(sql);
            List<RptFees> fees = new List<RptFees>();
            foreach (DataRow dr in data.Rows) 
            {
                fees.Add(new RptFees() { InvoiceDate = (string)dr["InvoiceDate"], Month = dr["Month"].ToInt32(0), Paid = dr["Paid"].ToDecimal(0), Required = dr["Required"].ToDecimal(0) });
            }
            return fees;
        }

        #endregion
    }
}
