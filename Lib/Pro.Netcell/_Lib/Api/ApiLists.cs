using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using Netcell.Data.Client;

using Netcell.Web;

namespace Netcell.Lib
{
    public class ApiLists
    {
        DalApi dal;
        public ApiLists()
        {
            dal = new DalApi();
        }

        ~ApiLists()
        {
            if (dal != null)
            {
                if (dal.Connection != null)
                {
                    dal.Connection.Close();
                }
                dal.Dispose();
            }
        }

        public DataTable Accounts_Category()
        {
            return dal.Accounts_Category();
        }

        public void Accounts_Category_Bind(ListControl ctl, string value)
        {
            ListDataHelper.BindList(ctl, "AccountCategoryName", "AccountCategoryId", Accounts_Category(), value);
        }

        public DataTable Countries()
        {
            return dal.Countries();
        }

        public void Countries_Bind(ListControl ctl, string value)
        {
            ListDataHelper.BindList(ctl, "CountryName", "CountryId", Countries(), value);
        }


         public ListItem[] Accounts_Type(UserType userType,bool readOnly)
        {
             List<ListItem> list=new List<ListItem>();
            switch (userType)
            {
                case UserType.Admin:
                    list.Add(new ListItem("לקוח","0"));
                    list.Add(new ListItem("חשבון אב","1"));
                    list.Add(new ListItem("חשבון בן","2"));
                    list.Add(new ListItem("עובד-מנהל","3"));
                    break;
                case UserType.Manager:
                    list.Add(new ListItem("חשבון בן","2"));
                    break;
                default:
                    if (readOnly)
                    {
                        list.Add(new ListItem("לקוח", "0"));
                        break;
                    }
                    else
                    throw new Exception("No permission for Accounts_Type");
            }

             return list.ToArray();
            //return dal.Accounts_Type();
        }

         public void Accounts_Type_Bind(ListControl ctl, UserType userType, string value, bool readOnly)
         {

             ListDataHelper.BindList(ctl, Accounts_Type(userType, readOnly), value);
         }


         //public DataTable Accounts_Type()
         //{
         //    return dal.Accounts_Type();
         //}

         //public void Accounts_Type_Bind(ListControl ctl, string value)
         //{
         //    BindList(ctl, "AccTypeName", "AccTypeId", Accounts_Type(), value);
         //}

        
         public void AccUsingType_Bind(ListControl ctl, string value)
         {
             ListDataHelper.BindList(ctl, ListDataHelper.GetListItems(typeof(AccUsingType)), value);
         }

         public void AccountBillingType_Bind(ListControl ctl, string value)
         {
             ListDataHelper.BindList(ctl, ListDataHelper.GetListItems(typeof(AccountBillingType)), value);
         }

         public DataTable Accounts_BillingType()
         {
             return dal.Accounts_BillingType();
         }

        public void Accounts_BillingType_Bind(ListControl ctl, string value)
        {
            ListDataHelper.BindList(ctl, "BillingTypeName", "BillingTypeId", Accounts_BillingType(), value);
        }
        public DataTable Accounts_PaymentType()
        {
            return dal.Accounts_PaymentType();
        }

        public void Accounts_PaymentType_Bind(ListControl ctl, string value)
        {
            ListDataHelper.BindList(ctl, "PayTypeName", "PayTypeId", Accounts_PaymentType(), value);
        }      

        //public DataTable Pricing_Code_Display()
        //{
        //    return dal.Pricing_Code_Display();
        //}

        //public void Pricing_Code_Display_Bind(ListControl ctl, string value)
        //{
        //    DataTable dt = Pricing_Code_Display();
        //    ListDataHelper.AddChoose(dt, "PriceName", "PriceCode", "בחירת קוד מחיר", -1);
        //    //dt.Rows.InsertAt(
        //    ListDataHelper.BindList(ctl, "PriceName", "PriceCode", dt, value);
        //}      

        #region static
        /*
        public static void AddChoose(DataTable dt, int textField, int valueField, string text, object value)
        {
            DataRow dr = dt.NewRow();
            dr[textField] = text;
            dr[valueField] = value;
            dt.Rows.InsertAt(dr, 0);
        }
        public static void AddChoose(DataTable dt, string textField, string valueField, string text, object value)
        {
            DataRow dr = dt.NewRow();
            dr[textField] = text;
            dr[valueField] = value;
            dt.Rows.InsertAt(dr, 0);
        }

        public static void AddChoose(ListControl ctl, string text, string value)
        {
           ctl.Items.Insert(0,new ListItem(text,value));
        }

        public static void BindList(ListControl ctl, string textField, string valueField, DataTable dt)
        {
            ctl.DataTextField = textField;
            ctl.DataValueField = valueField;
            ctl.DataSource = dt;
            ctl.DataBind();
        }
        public static void BindList(ListControl ctl, string textField, string valueField, DataTable dt, string selectedValue)
        {
            ctl.DataTextField = textField;
            ctl.DataValueField = valueField;
            ctl.DataSource = dt;
            ctl.DataBind();
            if (ctl.Items.Count>0 && selectedValue != null)
            {
                ctl.Text = selectedValue;
            }
        }
        public static void BindList(ListControl ctl, ListItem[] items, string selectedValue)
        {
            ctl.Items.Clear();
            ctl.Items.AddRange(items);
            if (ctl.Items.Count > 0 && selectedValue != null)
            {
                ctl.Text = selectedValue;
            }
        }
        public  static ListItem[] GetListItems(params string[] args)
        {
            List<ListItem> items= new List<ListItem>();
            foreach(string s in args)
            {
                items.Add(new ListItem(s));
            }
            return items.ToArray();
        }

        public static ListItem[] GetListItems(int from, int to)
        {
            List<ListItem> items = new List<ListItem>();
            for (int i = from; i <= to; i++)
            {
                items.Add(new ListItem(i.ToString()));
            }
            return items.ToArray();
        }

        public static ListItem[] GetListItemsWithValue(params string[] args)
        {
            List<ListItem> items = new List<ListItem>();
            for (int i = 0; i < args.Length; i++)
            {
                items.Add(new ListItem(args[i], args[i + 1]));
                i++;
            }
            return items.ToArray();
        }


        public static ListItem[] GetListItems(Type enumType)
        {
            string[] list = Enum.GetNames(enumType);
            int[] values =(int[]) Enum.GetValues(enumType);

            List<ListItem> items = new List<ListItem>();
            int i=0;
            foreach (string s in list)
            {
                items.Add(new ListItem(s,values[i].ToString()));
                i++;
            }
            return items.ToArray();
        }
        */

        public static ListItem[] GetPlatformTypes(bool addAll)
        {
            if (addAll)
                return ListDataHelper.GetListItemsWithValue("הכל", "0", "סלולאר", "1", "דואל", "2"); ;

            return ListDataHelper.GetListItemsWithValue("סלולאר", "1", "דואל", "2"); ;
        }

        //public static ListItem[] GetMethodCategory()
        //{
        //    return ListDataHelper.GetListItemsWithValue("הכל", "0", "מסרונים", "1", "סלולאר", "2", "דואל", "3"); ;
        //    //return ListDataHelper.GetListItemsWithValue("הכל", "ALL", "SMS", "SMS", "WAP", "WAP", "MMS", "MMS", "MAL", "MAL"); ;
        //}
        //public static ListItem[] GetMediaTypes()
        //{
        //    return ListDataHelper.GetListItemsWithValue("הכל", "ALL", "מסרונים", "SMS", "סלולאר", "WAP", "דואל", "MAL"); ;
        //    //return ListDataHelper.GetListItemsWithValue("הכל", "ALL", "SMS", "SMS", "WAP", "WAP", "MMS", "MMS", "MAL", "MAL"); ;
        //}
        //public static ListItem[] GetMediaBrowsTypes()
        //{
        //    return ListDataHelper.GetListItemsWithValue("הכל", "ALL", "סלולאר", "WAP", "דואל", "MAL"); ;
        //}
        //public static ListItem[] GetMediaCellTypes()
        //{
        //    return ListDataHelper.GetListItemsWithValue("הכל", "ALL", "מסרונים", "SMS", "סלולאר", "WAP"); ;
        //}

        public static ListItem[] GetCouponTypes()
        {
            return ListDataHelper.GetListItemsWithValue("None", "0", "ALL", "1", "Text", "2", "Image", "3", "Barcode", "4"); ;
        }

        public static ListItem[] GetCouponSourceTypes()
        {
            return ListDataHelper.GetListItemsWithValue("OneForAll", "0", "List", "1", "Random", "2"); ;
        }

        public static ListItem[] GetPromoFields()
        {
            return ListDataHelper.GetListItemsWithValue("בחירת שדה פרסונלי", "-1", "שדה 1", "0", "שדה 2", "1", "שדה 3", "2", "שדה 4", "3", "שדה 5", "4", "שדה 6", "5", "שדה 7", "6", "שדה 8", "7", "שדה 9", "8", "שדה 10", "9"); ;
        }
        /*
        public static ListItem[] DataTableToListItems(DataTable dt, string colText, string colValue)
        {
            if (dt == null)
                return null;
            List<ListItem> items = new List<ListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                items.Add(new ListItem(dr[colText].ToString(), dr[colValue].ToString()));
            }
            return items.ToArray();
        }

        public static ListItem[] DataTableToListItems(DataTable dt, string colText)
        {
            if (dt == null)
                return null;
            List<ListItem> items = new List<ListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                items.Add(new ListItem(dr[colText].ToString()));
            }
            return items.ToArray();
        }


        public static DataTable Dayes()
        {
            DataTable dt = new DataTable("Dayes");
            dt.Columns.Add("Value");
            dt.Columns.Add("Text");
            dt.Rows.Add(new object[] { "0", "א" });
            dt.Rows.Add(new object[] { "1", "ב" });
            dt.Rows.Add(new object[] { "2", "ג" });
            dt.Rows.Add(new object[] { "3", "ד" });
            dt.Rows.Add(new object[] { "4", "ה" });
            dt.Rows.Add(new object[] { "5", "ו" });
            dt.Rows.Add(new object[] { "6", "ש" });

            return dt;
        }
        public static DataTable TimeMode()
        {
            DataTable dt = new DataTable("TimeMode");
            dt.Columns.Add("Value");
            dt.Columns.Add("Text");
            dt.Rows.Add(new object[] { "0", "דקות" });
            dt.Rows.Add(new object[] { "1", "שעות" });
            return dt;
        }
        */

        public static ListItem[] GetUserTypes(bool isAdmin, bool enableManager)
        {
            if (isAdmin)
            {
                return ListDataHelper.GetListItems(typeof(UserType));
            }
            if (!enableManager)
            {
                return ListDataHelper.GetListItemsWithValue(UserType.Guest.ToString(), ((int)UserType.Guest).ToString(), UserType.User.ToString(), ((int)UserType.User).ToString()); ;
            }

            return ListDataHelper.GetListItemsWithValue(UserType.Guest.ToString(), ((int)UserType.Guest).ToString(), UserType.User.ToString(), ((int)UserType.User).ToString(), UserType.Manager.ToString(), ((int)UserType.Manager).ToString()); ;
        }
        //public static ListItem[] GetMediaTypes()
        //{
        //    return GetListItemsWithValue("הכל", "ALL", "SMS", "SMSMT", "WAP", "WAPMT", "MMS", "MMSMT", "MAL", "MALMT"); ;
        //}
        //public static ListItem[] GetMediaCellTypes()
        //{
        //    return GetListItemsWithValue("הכל", "ALL", "SMS", "SMSMT", "WAP", "WAPMT", "MMS", "MMSMT"); ;
        //}
        //public static ListItem[] GetCouponTypes()
        //{
        //    return GetListItemsWithValue("None", "0", "ALL", "1", "Text", "2", "Image", "3", "Barcode", "4"); ;
        //}

        //public static ListItem[] GetCouponSourceTypes()
        //{
        //    return GetListItemsWithValue("OneForAll", "0", "List", "1", "Random", "2"); ;
        //}

        //public static ListItem[] GetPromoFields()
        //{
        //    return GetListItemsWithValue("בחירת שדה פרסונלי", "-1", "שדה 1", "0", "שדה 2", "1", "שדה 3", "2", "שדה 4", "3", "שדה 5", "4", "שדה 6", "5", "שדה 7", "6", "שדה 8", "7", "שדה 9", "8", "שדה 10", "9"); ;
        //}
        #endregion

    }
}
