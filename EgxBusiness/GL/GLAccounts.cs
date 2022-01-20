using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness.GL
{
    public class GLAccounts
    {
        public enum GLAccountType 
        {BANK=1,CASH=2,INVENTORY=3,CUSTOMER=4,SUPPLIER=5 }
        public static AccountsTree GetGLAccount(GLAccountType accountType) 
        {
            string account_code="";
            switch (accountType) 
            {
                case GLAccountType.SUPPLIER:
                    account_code = EgxSetting.Get("GLSuppliersAccount");
                    break;
                case GLAccountType.CASH:
                    account_code = EgxSetting.Get("GLCashAccount");
                    break;
                case GLAccountType.BANK:
                    account_code = EgxSetting.Get("GLBankAccount");
                    break;
                case GLAccountType.CUSTOMER:
                    account_code = EgxSetting.Get("GLCustomersAccount");
                    break;
                case GLAccountType.INVENTORY:
                    account_code = EgxSetting.Get("GLInventoryAccount");
                    break;
            }
            if (account_code == "لم يتم ضبطه بعد" || account_code.Length == 0)
            {
                return null;
            }
            else
            {
                return AccountsTree.GetByCode(account_code);
            }
        }
    }
}
