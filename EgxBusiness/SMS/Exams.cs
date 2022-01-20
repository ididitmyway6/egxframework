using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness.EgxSMS;
namespace Egx.EgxBusiness.EgxSMS
{
   public class Exams
    {
       DataAccess da;
       SystemMessage sm;
       public int? ID { get; set; }
       public int? STUDENT { get; set; }
       public int? SUBJ { get; set; }
       public float? MARK { get; set; }
       public int? CLASS_GRADE { get; set; }
       public int? YEAR { get; set; }
       public int? MONTH { get; set; }
       public int? SEMESTER { get; set; }
       public string ST_NAME { get; set; }
       public String SUBJECT_NAME
       {
           get;
           set;
       }
       public override string ToString()
       {
           return "ExamsView";
       }

       public Exams() 
       {
           da = new DataAccess();
           sm = new SystemMessage();
       }
       public virtual SystemMessage Insert()
       {
           try
           {
               da.Insert(this);
               sm.Message = "Exam Inserted Successfully";
               sm.Type = MessageType.Pass;
           }
           catch(Exception exc)
           {
               sm.Type = MessageType.Fail;
               sm.Message = exc.Message;
           }
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
       public virtual List<Exams> Search()
       {
           return new DataAccess().Search(this).Cast<Exams>().ToList();
       }



       public virtual Exams GetByID()
       {
           return (Exams)new DataAccess().GetByID(ID.Value, this);
       }

       public static bool Exists(int student,int subject,int month,int year,out Exams exam) 
       {
           List<Exams> exms = new Exams() { STUDENT = student, SUBJ = subject, MONTH = month, YEAR = year }.Search();
           if (exms.Count > 0)
           {
               exam =new Exams(){ ID=exms[0].ID}.GetByID();
               return true;
           }
           else
           {
               exam = null;
               return false;
           }

           
       }
       public virtual List<Exams> GetAll()
       {
           return new DataAccess().GetTopItems(100, this).Cast<Exams>().ToList();
       }

       public SystemMessage UpdateExam(int SubjectNo, int StudentID, int Month, int year,float mark,int semester) 
       {
           sm = new SystemMessage();
           Exams exm =new Exams(){SUBJ=SubjectNo, STUDENT=StudentID, MONTH=Month,YEAR=year};
           Students st=new Students(){ID=StudentID}.GetByID();
           var lst = exm.Search();
           if (lst.Count > 0)
           {
               lst[0].MARK = mark;
               DataAccess.ExecData("UPDATE Exams_Test SET MARK='"+mark.ToString()+"' WHERE ID='"+lst[0].ID.ToString()+"'");
           }
           else 
           {
               exm.MARK = mark;
               exm.CLASS_GRADE = st.ST_CLASS;
               exm.SEMESTER = semester;
               exm.Insert();
           }
           return sm;
       }
    }
}
