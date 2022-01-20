using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness.Inventory
{
    public static class ItemsControlller
    {
 

        public static float RegisterItems(int OrderLine,float qnt, DateTime CurrentDate, string Comment,int tr_type=-1) 
        {
            OrderDetails odet = new OrderDetails() { ID=OrderLine }.GetByID();
            ItemsStock itemStock = new ItemsStock();
            //Batches Handling ....................
            Batches patch = new Batches();
            
            itemStock.INV_SITE = -1;
            itemStock.ITEM_ID = odet.PRODUCT_ID.Value;
            itemStock.ITEM_NAME = odet.PRODUCT_NAME;
            var x = ItemsStock.GetByID(odet.PRODUCT_ID.Value);
            if (x != null)
            {
                x.QOH = x.QOH.Value + qnt;
                x.Update();
            }
            else
            {
                itemStock.QOH = qnt;
                itemStock.Insert();
            }
            if (tr_type != -1) 
            {
                ItemsTransactions itmTrans = new ItemsTransactions();
                itmTrans.COMMENT = Comment;
                itmTrans.ITEM_ID = itemStock.ITEM_ID;
                itmTrans.ORDER_LINE = OrderLine;
                itmTrans.TR_DATE = DateTime.Now;
                itmTrans.USR = SystemSetting.CurrentUser.USER_NAME;
                if (tr_type == -10) 
                {
                    itmTrans.TR_TYPE = tr_type;
                    itmTrans.QOUT = Math.Abs(qnt);
                    //Batches Proccessing in case of Sales Order 
                    List<Batches> batch_out = new EgxDataAccess.DataAccess().ExecuteSQL<Batches>("select * from Batches where EXP_DATE=(select MIN(EXP_DATE) from Batches WHERE QNT>0 and categ_id='" + odet.PRODUCT_ID + "') and categ_id='" + odet.PRODUCT_ID + "' "); 
                    float batchQnt = 0;
                    if (batch_out.Count > 0)
                    {
                        while (batchQnt<= Math.Abs(qnt))
                        {
                            batch_out = new EgxDataAccess.DataAccess().ExecuteSQL<Batches>("select * from Batches where EXP_DATE=(select MIN(EXP_DATE) from Batches WHERE QNT>0 and categ_id='" + odet.PRODUCT_ID + "') and categ_id='" + odet.PRODUCT_ID + "' ");
                            batchQnt = batch_out[0].QNT.Value;
                            if (batchQnt >= Math.Abs(qnt))
                            {
                                batch_out[0].QNT = batch_out[0].QNT - Math.Abs(qnt);
                                batch_out[0].Update();
                                break;
                            }
                            else 
                            {
                                qnt = Math.Abs(qnt) - batch_out[0].QNT.Value;
                                batch_out[0].QNT = 0;
                                batch_out[0].Update();
                            }
                        }
                    }
                    
                    
                } else if (tr_type == -9) 
                {
                    itmTrans.TR_TYPE = tr_type;
                    itmTrans.QIN = qnt;
                    patch.BATCH_DATE = CurrentDate;
                    patch.CATEG_ID = odet.PRODUCT_ID;
                    patch.CATEG_NAME = odet.PRODUCT_NAME;
                    patch.EXP_DATE = odet.EXP_DATE;
                    patch.QNT = qnt;
                    patch.Insert();
                } else if (tr_type == -8) 
                {
                    itmTrans.TR_TYPE = tr_type;
                    itmTrans.QIN = qnt;
                    //Batches Proccessing in case of Sales Order 
                    List<Batches> batch_in = new EgxDataAccess.DataAccess().ExecuteSQL<Batches>("select * from Batches where EXP_DATE=(select MIN(EXP_DATE) from Batches WHERE QNT>0 and categ_id='" + odet.PRODUCT_ID + "') and categ_id='" + odet.PRODUCT_ID + "'");
                    if (batch_in.Count > 0)
                    {
                        batch_in[0].QNT = batch_in[0].QNT + qnt;
                        batch_in[0].Update();
                    }
                    else 
                    {
                        patch.BATCH_DATE = CurrentDate;
                        patch.CATEG_ID = odet.PRODUCT_ID;
                        patch.CATEG_NAME = odet.PRODUCT_NAME;
                        patch.EXP_DATE = DateTime.MinValue.Date;
                        patch.QNT = qnt;
                        patch.Insert();
                    }
                } else if (tr_type == -4) 
                {
                    itmTrans.TR_TYPE = tr_type;
                    itmTrans.QOUT = Math.Abs(qnt);
                }
                itmTrans.Insert();
            }
            return qnt;
        }
        public static int RegisterItems(int OrderLine, int qnt, DateTime CurrentDate, string Comment,int inventorySiteID, int tr_type = -1)
        {
            OrderDetails odet = new OrderDetails() { ID = OrderLine }.GetByID();
            ItemsStock itemStock = new ItemsStock();
            itemStock.INV_SITE = inventorySiteID;
            itemStock.ITEM_ID = odet.PRODUCT_ID.Value;
            itemStock.ITEM_NAME = odet.PRODUCT_NAME;
            var x = ItemsStock.GetByID(odet.PRODUCT_ID.Value,inventorySiteID);
            if (x != null)
            {
                x.QOH = x.QOH.Value + qnt;
                x.Update();
            }
            else
            {
                itemStock.QOH = qnt;
                itemStock.Insert();
            }
            if (tr_type != -1)
            {
                ItemsTransactions itmTrans = new ItemsTransactions();
                itmTrans.COMMENT = Comment;
                itmTrans.ITEM_ID = itemStock.ITEM_ID;
                itmTrans.ORDER_LINE = OrderLine;
                itmTrans.TR_DATE = DateTime.Now;
                itmTrans.USR = SystemSetting.CurrentUser.USER_NAME;
                if (tr_type == -10)
                {
                    itmTrans.TR_TYPE = tr_type;
                    itmTrans.QOUT = Math.Abs(qnt);
                }
                else if (tr_type == -9)
                {
                    itmTrans.TR_TYPE = tr_type;
                    itmTrans.QIN = qnt;
                }
                else if (tr_type == -8)
                {
                    itmTrans.TR_TYPE = tr_type;
                    itmTrans.QIN = qnt;
                }
                else if (tr_type == -4)
                {
                    itmTrans.TR_TYPE = tr_type;
                    itmTrans.QOUT = Math.Abs(qnt);
                }
                itmTrans.Insert();
            }
            return qnt;
        }

        public static float GetQNT(int prodID) 
        {
            var x = ItemsStock.GetByID(prodID);
            if (x != null)
            {
                return x.QOH.GetValueOrDefault(0);

            }
            else 
            {
                return 0;
            }
          
        }

        public static float RigesterFirstPeriodItem(float qnt,int prodID,string prodName) 
        {
            
            ItemsStock itemStock = new ItemsStock();
            itemStock.INV_SITE = -1;
            itemStock.ITEM_ID = prodID;
            itemStock.ITEM_NAME = prodName;
            var x = ItemsStock.GetByID(prodID);
            if (x != null)
            {
                x.QOH =  qnt;
                x.Update();
            }
            else
            {
                itemStock.QOH = qnt;
                itemStock.Insert();
            }
            return qnt;
        }
        public static bool IsValidQuantity(this OrderDetails detail, float qnt)
        {
            ItemsStock stock = ItemsStock.GetByID(detail.PRODUCT_ID.Value);
            if (stock != null)
            {
                if (qnt <= stock.QOH.Value) { return true; } else { return false; }
            }
            else
            {
                return false;
            }
        } 
 
    }
}
