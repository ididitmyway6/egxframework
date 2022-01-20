using Egx.EgxDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness.GL
{
   public  class JournalWideView: Journal
    {
       public override string ToString()
       {
           return "JournalWideView";
       }
       public static List<JournalWideView> GetWideView(DateTime fromDate, DateTime toDate)
       {
           return new DataAccess().ExecuteSQL<JournalWideView>("select * from JournalWideView where REG_DATE>=" + fromDate.SqlDateTime() + " and REG_DATE<=" + toDate.SqlDateTime() + " ");
       }

       public static DataTable GetAccountCreditByTransaction(DateTime fromDate, DateTime toDate, string accountName)
       {
           string sql = @"select REG_DATE , AccountName ,COMMENT ,DEBIT ,CREDIT , (select SUM(ISNULL(DEBIT,0)-ISNULL(CREDIT,0)) FROM JournalWideView WHERE JournalWideView.AccountName=JWV.AccountName AND REG_DATE between " + fromDate.SqlDateTime() + " and JWV.REG_DATE ) + (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView where JournalWideView.AccountName='" + accountName + "' and REG_DATE<" + fromDate.SqlDateTime() + ") as BALANCE from JournalWideView JWV  WHERE AccountName= '" + accountName + "' AND REG_DATE between " + fromDate.SqlDateTime() + " and " + toDate.SqlDateTime() + " ORDER BY REG_DATE";
           return DataAccess.GetDataTable(sql);
       }
       public static DataTable GetTrialBalance(DateTime fromDate, DateTime toDate)
       {
           //Old Original  string sql = @"SELECT (SELECT CODE FROM AccountsTree WHERE ACCOUNT_NAME=X.TRIAL) AS CODE,* FROM (SELECT TRIAL, ISNULL(SUM(DEBIT),0) AS DEBIT,ISNULL(SUM(CREDIT),0) AS CREDIT ,(SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView where  JournalWideView.AccountName = TRIAL AND REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE],( ISNULL(SUM(DEBIT),0) -ISNULL(SUM(CREDIT),0)+ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView where  JournalWideView.AccountName = TRIAL AND REG_DATE<" + fromDate.SqlDateTime() + " ),0) ) AS [FINAL_CREDIT] FROM PRE_TRIAL_VIEW  WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " GROUP BY TRIAL) X";
           //old string sql = @"SELECT (SELECT CODE FROM AccountsTree WHERE ACCOUNT_NAME=X.TRIAL) AS CODE,* FROM (SELECT TRIAL, ISNULL(SUM(DEBIT),0) AS DEBIT,ISNULL(SUM(CREDIT),0) AS CREDIT ,(SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW TV where  TV.TRIAL = PRE_TRIAL_VIEW.TRIAL AND TV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE],( ISNULL(SUM(DEBIT),0) -ISNULL(SUM(CREDIT),0)+ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW TV where  TV.TRIAL = PRE_TRIAL_VIEW.TRIAL AND TV.REG_DATE<" + fromDate.SqlDateTime() + " ),0) ) AS [FINAL_CREDIT] FROM PRE_TRIAL_VIEW  WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " GROUP BY TRIAL) X";
           string sql = @"SELECT (SELECT CODE FROM AccountsTree WHERE ACCOUNT_NAME=X.TRIAL) AS CODE, * FROM (
                          SELECT  TRIAL, ISNULL(SUM(DEBIT),0) AS DEBIT,ISNULL(SUM(CREDIT),0) AS CREDIT , (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW TV where  TV.TRIAL = PRE_TRIAL_VIEW.TRIAL AND TV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE], ( ISNULL(SUM(DEBIT),0) -ISNULL(SUM(CREDIT),0)+ISNULL(  (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW TV where  TV.TRIAL = PRE_TRIAL_VIEW.TRIAL AND TV.REG_DATE<" + fromDate.SqlDateTime() + " ),0) ) AS [FINAL_CREDIT] FROM pre_trial_view  WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " GROUP BY TRIAL ) X union all  select * from   ( select  CODE,AT.ACCOUNT_NAME as TRIAL,(select DEBIT=0) as debit ,(select CREDIT=0) as CREDIT , (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW PV where  PV.TRIAL = at.ACCOUNT_NAME AND PV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE] ,(SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW PV where  PV.TRIAL = at.ACCOUNT_NAME AND PV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [FINAL_CREDIT] FROM AccountsTree at where LEV=4 )  y  where  not exists(select code from pre_trial_view ptv WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " and ptv.TRIAL=y.TRIAL )";
           return DataAccess.GetDataTable(sql);
       }

       public static decimal GetBeginningBalance(string accountName, DateTime beforeDate,string accountLevel) 
       {
           decimal res = 0;
           if (accountLevel == "1")
           {
               string sql = "SELECT (ISNULL(SUM(jwv.DEBIT),0)-ISNULL(SUM(jwv.CREDIT),0)) FROM JournalWideView jwv where jwv.GrandName=JournalWideView.GrandName and REG_DATE<" + beforeDate.SqlDateTime() + "";
           }
           else if (accountLevel == "2") 
           {
               string sql = "SELECT (ISNULL(SUM(jwv.DEBIT),0)-ISNULL(SUM(jwv.CREDIT),0)) FROM JournalWideView jwv where jwv.GrandName=JournalWideView.GrandName and REG_DATE<" + beforeDate.SqlDateTime() + "";
           }
           else if (accountLevel == "3")
           {
               string sql = "SELECT (ISNULL(SUM(jwv.DEBIT),0)-ISNULL(SUM(jwv.CREDIT),0)) FROM JournalWideView jwv where jwv.GrandName=JournalWideView.GrandName and REG_DATE<" + beforeDate.SqlDateTime() + "";
           }
           else if (accountLevel == "4") 
           {
               string sql = "SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW TV where  TV.TRIAL = '"+accountName+"' AND TV.REG_DATE<" + beforeDate.SqlDateTime() + "";
               res = DataAccess.GetSqlValue(sql, EgxDataType.Mssql).ToDecimal(0);
           }
               return res;
       }

       public static DataTable GetCurrentAccountCredit(string accountCode) 
       {
           var Acc = AccountsTree.GetByCode(accountCode);
           if (Acc != null)
           {
               string sql = " select Code, (select account_name from AccountsTree where CODE=z.CODE)as AccountName ,balance  from ( ";
               sql +=" select  AccountsTree.CODE";
               sql+=" , (isnull(SUM(DEBIT),0)-isnull(SUM(Journal.CREDIT),0)) as balance";
               sql+=" from Journal JOIN AccountsTree ";
               sql+=" on Journal.ACC_ID=AccountsTree.ID ";
               sql += " where ACC_ID=" + Acc.ID + " or PARENT_ACCOUNT=" + Acc.ID + " or CHILD_ACCOUNT=" + Acc.ID + " or LEV5=" + Acc.ID + " or REL_ACCOUNT=" + Acc.ID + "";
               sql+=" group by AccountsTree.CODE) z";
               return DataAccess.GetDataTable(sql);
           }
           else return null;
       }

       public static DataTable GetTrialBalanceByLevel(DateTime fromDate, DateTime toDate,string level)
       {
           string sql = "";
           if (level == "4")
           {
               //Old Original  string sql = @"SELECT (SELECT CODE FROM AccountsTree WHERE ACCOUNT_NAME=X.TRIAL) AS CODE,* FROM (SELECT TRIAL, ISNULL(SUM(DEBIT),0) AS DEBIT,ISNULL(SUM(CREDIT),0) AS CREDIT ,(SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView where  JournalWideView.AccountName = TRIAL AND REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE],( ISNULL(SUM(DEBIT),0) -ISNULL(SUM(CREDIT),0)+ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView where  JournalWideView.AccountName = TRIAL AND REG_DATE<" + fromDate.SqlDateTime() + " ),0) ) AS [FINAL_CREDIT] FROM PRE_TRIAL_VIEW  WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " GROUP BY TRIAL) X";
               //old string sql = @"SELECT (SELECT CODE FROM AccountsTree WHERE ACCOUNT_NAME=X.TRIAL) AS CODE,* FROM (SELECT TRIAL, ISNULL(SUM(DEBIT),0) AS DEBIT,ISNULL(SUM(CREDIT),0) AS CREDIT ,(SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW TV where  TV.TRIAL = PRE_TRIAL_VIEW.TRIAL AND TV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE],( ISNULL(SUM(DEBIT),0) -ISNULL(SUM(CREDIT),0)+ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW TV where  TV.TRIAL = PRE_TRIAL_VIEW.TRIAL AND TV.REG_DATE<" + fromDate.SqlDateTime() + " ),0) ) AS [FINAL_CREDIT] FROM PRE_TRIAL_VIEW  WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " GROUP BY TRIAL) X";
                sql = @"SELECT (SELECT CODE FROM AccountsTree WHERE ACCOUNT_NAME=X.TRIAL) AS CODE, * FROM (
                          SELECT  TRIAL, ISNULL(SUM(DEBIT),0) AS DEBIT,ISNULL(SUM(CREDIT),0) AS CREDIT , (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW TV where  TV.TRIAL = PRE_TRIAL_VIEW.TRIAL AND TV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE], ( ISNULL(SUM(DEBIT),0) -ISNULL(SUM(CREDIT),0)+ISNULL(  (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW TV where  TV.TRIAL = PRE_TRIAL_VIEW.TRIAL AND TV.REG_DATE<" + fromDate.SqlDateTime() + " ),0) ) AS [FINAL_CREDIT] FROM pre_trial_view  WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " GROUP BY TRIAL ) X union all  select * from   ( select  CODE,AT.ACCOUNT_NAME as TRIAL,(select DEBIT=0) as debit ,(select CREDIT=0) as CREDIT , (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW PV where  PV.TRIAL = at.ACCOUNT_NAME AND PV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE] ,(SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM PRE_TRIAL_VIEW PV where  PV.TRIAL = at.ACCOUNT_NAME AND PV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [FINAL_CREDIT] FROM AccountsTree at where LEV=4 )  y  where  not exists(select code from pre_trial_view ptv WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " and ptv.TRIAL=y.TRIAL )";
           }
           else if (level == "3")
           {
               sql = " SELECT (SELECT CODE FROM AccountsTree where AccountsTree.ACCOUNT_NAME=TBL.TRIAL) AS CODE,* FROM (";
               sql += " SELECT JournalWideView.ParentName AS TRIAL,ISNULL(SUM(DEBIT),0) AS DEBIT,ISNULL(SUM(JournalWideView.CREDIT),0) AS CREDIT ,";
               sql += " ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView jwv where jwv.ParentName=JournalWideView.ParentName and REG_DATE<" + fromDate.SqlDateTime() + "),0) AS BEG_BALANCE";
               sql += ", ISNULL(SUM(DEBIT),0)-ISNULL(SUM(JournalWideView.CREDIT),0)+ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView jwv where jwv.ParentName=JournalWideView.ParentName and REG_DATE<" + fromDate.SqlDateTime() + "),0) AS FINAL_CREDIT";
               sql += " FROM JournalWideView join AccountsTree on AccountsTree.ACCOUNT_NAME=JournalWideView.ParentName ";
               sql += " where JournalWideView.REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " ";
               sql += " GROUP BY JournalWideView.ParentName";
               sql += " ) TBL";
               sql += " union all";
               sql += " select * from ( select  CODE,AT.ACCOUNT_NAME as TRIAL,(select DEBIT=0) as debit ,(select CREDIT=0) as CREDIT ";
               sql += ", (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView JWV where  JWV.ParentName = at.ACCOUNT_NAME AND JWV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE] ";
               sql += ", (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView JWV where  JWV.ParentName = at.ACCOUNT_NAME AND JWV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [FINAL_CREDIT]";
               sql += " FROM AccountsTree at where LEV=3 )  y";
               sql += " where  not exists(select code from JournalWideView JV WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " and JV.ParentName=y.TRIAL ) ";
           }
           else if (level == "1")
           {
               sql = " SELECT (SELECT CODE FROM AccountsTree where AccountsTree.ACCOUNT_NAME=TBL.TRIAL) AS CODE,* FROM (";
               sql += " SELECT JournalWideView.GrandName AS TRIAL,ISNULL(SUM(DEBIT),0) AS DEBIT,ISNULL(SUM(JournalWideView.CREDIT),0) AS CREDIT ,";
               sql += " ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView jwv where jwv.GrandName=JournalWideView.GrandName and REG_DATE<" + fromDate.SqlDateTime() + "),0) AS BEG_BALANCE";
               sql += ", ISNULL(SUM(DEBIT),0)-ISNULL(SUM(JournalWideView.CREDIT),0)+ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView jwv where jwv.GrandName=JournalWideView.GrandName and REG_DATE<" + fromDate.SqlDateTime() + "),0) AS FINAL_CREDIT";
               sql += " FROM JournalWideView join AccountsTree on AccountsTree.ACCOUNT_NAME=JournalWideView.GrandName ";
               sql += " where JournalWideView.REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " ";
               sql += " GROUP BY JournalWideView.GrandName";
               sql += " ) TBL";
               sql += " union all";
               sql += " select * from ( select  CODE,AT.ACCOUNT_NAME as TRIAL,(select DEBIT=0) as debit ,(select CREDIT=0) as CREDIT ";
               sql += ", (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView JWV where  JWV.GrandName = at.ACCOUNT_NAME AND JWV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE] ";
               sql += ", (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView JWV where  JWV.GrandName = at.ACCOUNT_NAME AND JWV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [FINAL_CREDIT]";
               sql += " FROM AccountsTree at where LEV=1 )  y";
               sql += " where  not exists(select code from JournalWideView JV WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " and JV.GrandName=y.TRIAL ) ";
           }
           else if (level == "2")
           {
               sql = " SELECT (SELECT CODE FROM AccountsTree where AccountsTree.ACCOUNT_NAME=TBL.TRIAL) AS CODE,* FROM (";
               sql += " SELECT JournalWideView.GrandChild AS TRIAL,ISNULL(SUM(DEBIT),0) AS DEBIT,ISNULL(SUM(JournalWideView.CREDIT),0) AS CREDIT ,";
               sql += " ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView jwv where jwv.GrandChild=JournalWideView.GrandChild and REG_DATE<" + fromDate.SqlDateTime() + "),0) AS BEG_BALANCE";
               sql += ", ISNULL(SUM(DEBIT),0)-ISNULL(SUM(JournalWideView.CREDIT),0)+ISNULL((SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView jwv where jwv.GrandChild=JournalWideView.GrandChild and REG_DATE<" + fromDate.SqlDateTime() + "),0) AS FINAL_CREDIT";
               sql += " FROM JournalWideView join AccountsTree on AccountsTree.ACCOUNT_NAME=JournalWideView.GrandChild ";
               sql += " where JournalWideView.REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " ";
               sql += " GROUP BY JournalWideView.GrandChild";
               sql += " ) TBL";
               sql += " union all";
               sql += " select * from ( select  CODE,AT.ACCOUNT_NAME as TRIAL,(select DEBIT=0) as debit ,(select CREDIT=0) as CREDIT ";
               sql += ", (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView JWV where  JWV.GrandChild = at.ACCOUNT_NAME AND JWV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [BEG_BALANCE] ";
               sql += ", (SELECT (ISNULL(SUM(DEBIT),0)-ISNULL(SUM(CREDIT),0)) FROM JournalWideView JWV where  JWV.GrandChild = at.ACCOUNT_NAME AND JWV.REG_DATE<" + fromDate.SqlDateTime() + " ) as [FINAL_CREDIT]";
               sql += " FROM AccountsTree at where LEV=2 )  y";
               sql += " where  not exists(select code from JournalWideView JV WHERE REG_DATE BETWEEN " + fromDate.SqlDateTime() + " AND " + toDate.SqlDateTime() + " and JV.GrandChild=y.TRIAL ) ";
           }
               return DataAccess.GetDataTable(sql);
       }
    }
}
