using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Nistec.Xml;
using Netcell.Data.Client;
using Netcell;
using Nistec;
using Nistec.Data;

using Netcell.Remoting;
using System.Threading;
using Netcell.Web;

namespace Netcell.Lib
{
   

    internal class ContactsReader : IDisposable
    {
        public const int DefaultMinContactsUploadAsync = 100;
        public static DateTime NullDate { get { return new DateTime(1900, 1, 1); } }

        #region ctor

        List<string> errs;
        public List<string> Errs
        {
            get { return errs; }
        }
        DalContacts dal = null;
        int MinContactsUploadAsync = 100;

        //public Contacts()
        //{
        //    dal = new DalContacts();
        //    dal.AutoCloseConnection = false;
        //    errs = new List<string>();
        //}
        public ContactsReader()
        {
            MinContactsUploadAsync = WebConfig.MinContactsUploadAsync;
            dal = DalContacts.Instance;
            dal.AutoCloseConnection = false;
            errs = new List<string>();
        }
        ~ContactsReader()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (dal != null)
            {
                dal.ActiveConnectionClose();
                dal.Dispose();
                dal = null;
            }
        }
        #endregion

        #region contact items

        public class KnownFields
        {
            public const string Name = "Name";

            public const string CellNumber = "CellNumber";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string BirthDate = "BirthDate";
            public const string Email = "Email";
            public const string City = "City";
            public const string Details = "Details";
            public const string Address = "Address";
            public const string Sex = "Sex";
            public const string Company = "Company";
            public const string EnableNews = "EnableNews";

            //Obsolete
            //public const string Phone = "Phone";
            //public const string Branch = "Branch";
            //public const string WeddingDate = "WeddingDate";
            //public const string PartnerBirthdate = "PartnerBirthdate";
            //public const string PatnerPhone = "PatnerPhone";
            //public const string PartnerName = "PartnerName";
            //public const string OtherDate = "OtherDate";

            public const string Country = "Country";

            public const string Phone1 = "Phone1";

            public const string ExType = "ExType";
            public const string ExKey = "ExKey";
            public const string ExLang = "ExLang";


            public const string ExDate1 = "ExDate1";
            public const string ExDate2 = "ExDate2";
            public const string ExDate3 = "ExDate3";
            public const string ExDate4 = "ExDate4";
            public const string ExDate5 = "ExDate5";

            public const string ExText1 = "ExText1";
            public const string ExText2 = "ExText2";
            public const string ExText3 = "ExText3";
            public const string ExText4 = "ExText4";
            public const string ExText5 = "ExText5";

            public const string IsActive = "IsActive";
            public const string Segments = "Segments";


            public const string GroupName = "GroupName";
            public const string GroupId = "GroupId";

            public const string Host = "Host";
            public const string FullTime = "FullTime";
            public const string Time = "Time";
            public const string Date = "Date";
        }

        internal static Dictionary<string, string> MapFields(int accountId)
        {

            var dic = new Dictionary<string, string>();
            var dt = ContactContext.ContactCategoryFields(accountId);
 
            foreach (DataRow dr in dt.Rows)
            {
                string key = dr.Get<string>("FieldName");
                string val = dr.Get<string>("Field");
                if (key == null || key=="" || val == null || val == "" || val.ToUpper() == "NA")
                    continue;
                dic[key.Replace(" ", "")] = val;
            }


           
            //var dic = ContactContext.ContactCategoryDictionary(accountId);

            dic.Add("cli", KnownFields.CellNumber);
            dic.Add("cellnumber", KnownFields.CellNumber);
            dic.Add("name", KnownFields.Name);
            dic.Add("firstname", KnownFields.FirstName);
            dic.Add("lastname", KnownFields.LastName);
            dic.Add("birthdate", KnownFields.BirthDate);
            dic.Add("email", KnownFields.Email);
            dic.Add("city", KnownFields.City);
            dic.Add("details", KnownFields.Details);
            dic.Add("phone1", KnownFields.Phone1);
            dic.Add("address", KnownFields.Address);
            dic.Add("sex", KnownFields.Sex);
            dic.Add("company", KnownFields.Company);
            dic.Add("enablenews", KnownFields.EnableNews);
            dic.Add("country", KnownFields.Country);

            dic.Add("isactive", KnownFields.IsActive);
            dic.Add("segments", KnownFields.Segments);
            dic.Add("פעיל", KnownFields.IsActive);
            dic.Add("סגמנטים", KnownFields.Segments);


            dic.Add("date1", KnownFields.ExDate1);
            dic.Add("date2", KnownFields.ExDate2);
            dic.Add("date3", KnownFields.ExDate3);
            dic.Add("date4", KnownFields.ExDate4);
            dic.Add("date5", KnownFields.ExDate5);
            dic.Add("text1", KnownFields.ExText1);
            dic.Add("text2", KnownFields.ExText2);
            dic.Add("text3", KnownFields.ExText3);
            dic.Add("text4", KnownFields.ExText4);
            dic.Add("text5", KnownFields.ExText5);

            //dic.Add("extype", KnownFields.ExType);
            dic.Add("exkey", KnownFields.ExKey);
            dic.Add("exlang", KnownFields.ExLang);


            dic.Add("טלפוןנייד", KnownFields.CellNumber);
            dic.Add("טלפוןסלואלרי", KnownFields.CellNumber);
            dic.Add("שםמלא", KnownFields.Name);
            dic.Add("שםפרטי", KnownFields.FirstName);
            dic.Add("שםמשפחה", KnownFields.LastName);
            dic.Add("תאריךלידה", KnownFields.BirthDate);
            dic.Add("דואל", KnownFields.Email);
            dic.Add("דואראלקטרוני", KnownFields.Email);
            dic.Add("עיר", KnownFields.City);
            dic.Add("פרטים", KnownFields.Details);
            dic.Add("טלפון1", KnownFields.Phone1);
            dic.Add("טלפון", KnownFields.Phone1);
            dic.Add("כתובת", KnownFields.Address);
            dic.Add("מגדר", KnownFields.Sex);
            dic.Add("מין", KnownFields.Sex);
            dic.Add("חברה", KnownFields.Company);
            dic.Add("אפשרדיוור", KnownFields.EnableNews);
            dic.Add("אפשרדיוורים", KnownFields.EnableNews);
          

            dic.Add("מדינה", KnownFields.Country);

            //dynamic
            dic.Add("תאריך1", KnownFields.ExDate1);
            dic.Add("תאריך2", KnownFields.ExDate2);
            dic.Add("תאריך3", KnownFields.ExDate3);
            dic.Add("תאריך4", KnownFields.ExDate4);
            dic.Add("תאריך5", KnownFields.ExDate5);
            dic.Add("טקסט1", KnownFields.ExText1);
            dic.Add("טקסט2", KnownFields.ExText2);
            dic.Add("טקסט3", KnownFields.ExText3);
            dic.Add("טקסט4", KnownFields.ExText4);
            dic.Add("טקסט5", KnownFields.ExText5);

            dic.Add("מזהה", KnownFields.ExKey);
            dic.Add("מפתח", KnownFields.ExKey);
            dic.Add("שפה", KnownFields.ExLang);


            var map = ContactContext.ContactCategoryMap(accountId);

            foreach(var entry in map)
            {
                dic[entry.Key.Replace(" ", "")] = entry.Value;

            }

            

            //dic.Add("תאריךטסט", KnownFields.ExDate1);
            //dic.Add("תאריךטיפול", KnownFields.ExDate2);
            //dic.Add("תאריךטיפולהבא", KnownFields.ExDate3);



            //obolete
            //dic.Add("branch", KnownFields.Branch);
            //dic.Add("otherdate", KnownFields.OtherDate);
            //dic.Add("weddingdate", KnownFields.WeddingDate);

            return dic;

        }

        
        
        public static int MapConatctsItemsUpload(DataTable dt, int accountId)
        {
            int count = dt.Columns.Count;

            Dictionary<string, string> map = MapFields(accountId);

            var dicCols = new Dictionary<string, string>();

            foreach (DataColumn col in dt.Columns)
            {
                string ctrim = col.ColumnName.Replace(" ", "");
                dicCols[ctrim] = col.ColumnName;
            }


            foreach (DataColumn col in dt.Columns)
            {
                string c = null;
                string ctrim = col.ColumnName.Replace(" ", "");
                if (map.TryGetValue(ctrim, out c))
                {
                    if (!dicCols.ContainsKey(c))
                    {
                        col.ColumnName = c;
                        dicCols[c] = col.Ordinal.ToString();
                    }
                }
            }

            return count;
        }

        public static int FormatConatctsItemsUpload(DataTable dt, int accountId)
        {
            int count = dt.Columns.Count;

            dt.Columns[0].ColumnName = "CellNumber";
            if (count > 1)
                dt.Columns[1].ColumnName = "Email";
            if (count > 2)
                dt.Columns[2].ColumnName = "BirthDate";
            if (count > 3)
                dt.Columns[3].ColumnName = "FirstName";
            if (count > 4)
                dt.Columns[4].ColumnName = "LastName";
            if (count > 5)
                dt.Columns[5].ColumnName = "Address";
            if (count > 6)
                dt.Columns[6].ColumnName = "City";
            if (count > 7)
                dt.Columns[7].ColumnName = "Sex";
            if (count > 8)
                dt.Columns[8].ColumnName = "Details";
            if (count > 9)
                dt.Columns[9].ColumnName = "Phone1";
            if (count > 10)
                dt.Columns[10].ColumnName = "Company";
            if (count > 11)
                dt.Columns[11].ColumnName = "EnableNews";
            
            int counter=12;
            var map = ContactContext.ContactCategoryMap(accountId);
            string[] cols=new string[]{"ExKey","ExLang","ExText1","ExText2","ExText3","ExText4","ExText5","ExDate1","ExDate2","ExDate3","ExDate4","ExDate5"};
            for (int i = 0; i < 12; i++)
            {
                if (count <= counter)
                    break;
                if (map.ContainsValue(cols[i]))
                    dt.Columns[counter].ColumnName = cols[i];
                else
                    dt.Columns[counter].ColumnName = string.Format("NA_{0}", counter);

                counter++;
            }

            return counter;

            //if (count > 12)
            //    dt.Columns[12].ColumnName = "ExKey";
            //if (count > 13)
            //    dt.Columns[13].ColumnName = "ExLang";
            //if (count > 14)
            //    dt.Columns[14].ColumnName = "ExText1";
            //if (count > 15)
            //    dt.Columns[15].ColumnName = "ExText2";
            //if (count > 16)
            //    dt.Columns[16].ColumnName = "ExText3";
            //if (count > 17)
            //    dt.Columns[17].ColumnName = "ExText4";
            //if (count > 18)
            //    dt.Columns[18].ColumnName = "ExText5";
            //if (count > 19)
            //    dt.Columns[19].ColumnName = "ExDate1";
            //if (count > 20)
            //    dt.Columns[20].ColumnName = "ExDate2";
            //if (count > 21)
            //    dt.Columns[21].ColumnName = "ExDate3";
            //if (count > 22)
            //    dt.Columns[22].ColumnName = "ExDate4";
            //if (count > 23)
            //    dt.Columns[23].ColumnName = "ExDate5";
            //if (count > 24)
            //    dt.Columns[24].ColumnName = "Segments";
            //return count;

        }

        public UploadSumarize ContactsItemsUpload(DataTable dtUpload, int accountId, int groupId, ContactUpdateType updateType, ContactsUploadMethod method, ContactKeyType keyType, Guid uploadKey)
        {
            UploadSumarize sumarize2;
            if ((dtUpload == null) || (dtUpload.Rows.Count == 0))
            {
                throw new UploadException("Invalid contacts");
            }

            if (keyType == ContactKeyType.None)
            {
                keyType = (ContactKeyType)this.dal.Lookup_ContactKeyType(accountId);
            }
            UploadSumarize sumarize = new UploadSumarize();
            DataTable dt = this.dal.ContactsUploadSchema().Clone();
            sumarize.WrongItems = dtUpload.Clone();
            dt.Constraints.Add("IX_Contacts_Upload", new DataColumn[] { dt.Columns["Identifier"] }, false);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            //Guid uploadKey = Guid.NewGuid();
            int count = 0;
            string identifier = "";
            //string str2 = "";
            //string str3 = "";
            int colCount = dt.Columns.Count;
            DateTime Now = DateTime.Now;
            foreach (DataRow row in dtUpload.Rows)
            {
                try
                {
                    object[] values = new object[colCount];
                    string cellNumber = ReadTextField(row, "CellNumber", null, 20);
                    string email = ReadTextField(row, "Email", null, 100);
                    string exKey = ReadTextField(row, "ExKey", null, 50);
                    ContactRule rule = ContactContext.ValidateContactRule(ref cellNumber, ref email);
                    identifier = ContactContext.GetIdentifier(keyType, accountId, cellNumber, email, exKey, rule);
                    if (identifier == null)
                    {
                        sumarize.Add(UploadState.WrongItem);
                        sumarize.WrongItems.Rows.Add(row.ItemArray);
                        continue;
                    }

                    if (dictionary.ContainsKey(identifier))
                    {
                        sumarize.Add(UploadState.Duplicate);
                        sumarize.WrongItems.Rows.Add(row.ItemArray);
                        continue;
                    }

                    count++;
                    values[0] = count;
                    values[1] = accountId;
                    values[2] = ContactContext.ValidateTarget(cellNumber);
                    values[3] = ContactContext.ValidateTarget(email);
                    values[4] = this.ReadDateField(row, KnownFields.BirthDate, null);
                    values[5] = this.ReadField(row,KnownFields.FirstName, "*");
                    values[6] = this.ReadField(row, KnownFields.LastName, "");
                    values[7] = this.ReadField(row, KnownFields.Address, null);
                    values[8] = this.ReadField(row, KnownFields.City, null);
                    values[9] = Types.ToInt(this.ReadField(row, KnownFields.Country, 0), 0);
                    values[10] = Types.NZ(this.ReadFieldSex(row, KnownFields.Sex, "U"), "U");
                    values[11] = (int)ContactSignType.Upload;
                    values[12] = this.ReadField(row, KnownFields.Details, null);
                    values[13] = this.ReadField(row, KnownFields.Phone1, null);
                    values[14] = this.ReadField(row, KnownFields.Company, null);
                    values[15] = (int)rule;
                    values[16] = "Upload";//this.ReadField(row, "Registration", "");
                    values[17] = ReadBoolField(row, KnownFields.EnableNews, true);
                    values[18] = ReadBoolField(row, KnownFields.IsActive, true);
                    values[19] = Now;
                    values[20] = Now;
                    values[21] = this.ReadField(row, KnownFields.ExText1, null);
                    values[22] = this.ReadField(row, KnownFields.ExText2, null);
                    values[23] = this.ReadField(row, KnownFields.ExText3, null);
                    values[24] = this.ReadField(row, KnownFields.ExText4, null);
                    values[25] = this.ReadField(row, KnownFields.ExText5, null);
                    values[26] = this.ReadDateField(row, KnownFields.ExDate1, null);
                    values[27] = this.ReadDateField(row, KnownFields.ExDate2, null);
                    values[28] = this.ReadDateField(row, KnownFields.ExDate3, null);
                    values[29] = this.ReadDateField(row, KnownFields.ExDate4, null);
                    values[30] = this.ReadDateField(row, KnownFields.ExDate5, null);
                    values[31] = exKey;
                    values[32] = (int)keyType;
                    values[33] = this.ReadField(row, KnownFields.ExLang, null);
                    values[34] = identifier;
                    values[35] = this.ReadField(row, KnownFields.Segments, "00000000000000000000");
                    values[36] = uploadKey;
                    //values[35] = updateExists ? 1 : 0;
                    //values[36] = groupId;


                    dt.Rows.Add(values);
                    dictionary[identifier] = identifier;
                    //dictionary2[str2] = cellNumber;
                    //dictionary3[str3] = email;
                }
                catch (Exception exception)
                {
                    sumarize.WrongItems.Rows.Add(row.ItemArray);
                    this.errs.Add(exception.Message);
                }
            }
            if ((this.errs.Count > 0) && (count <= 0))
            {
                throw new Exception("Contacts list not define correctly");
            }
            if (count <= 0)
            {
                throw new Exception("No Contacts inserted");
            }
            try
            {
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    count = dt.Rows.Count;

                   
                    //ContactUpdateType updateType = updateExists ? ContactUpdateType.UpdateFull : ContactUpdateType.InsertOnly;
                    bool updateExists = updateType != ContactUpdateType.InsertOnly;
                    if (method == ContactsUploadMethod.Auto)
                    {
                        if (!updateExists || count <= MinContactsUploadAsync)
                        {
                            // go to Contacts_Upload_Exec
                            method = ContactsUploadMethod.Sync;
                        }
                        else
                        {
                            method = ContactsUploadMethod.QueueCommand;
                        }
                    }
                    this.dal.InsertTable(dt, "Contacts_Items_Upload");
                    dal.Contacts_Upload_Mng_Insert(uploadKey.ToString(), (int)updateType, groupId, accountId, (int)method);
                    if (method == ContactsUploadMethod.Sync)
                    {
                        dal.ContactsItems_Upload_Exec(uploadKey.ToString());
                    }
                    else
                    {
                        sumarize.IsAsync = true;
                        //int asynccount = 0;
                        //while (true)
                        //{
                        //    if (asynccount<=10 && dal.Lookup_UploadExists(uploadKey.ToString()))
                        //    {
                        //        Thread.Sleep(TimeSpan.FromSeconds(10));
                        //        asynccount++;
                        //    }
                        //    else
                        //        break;
                        //}
                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                    //this.ExecUploadEx(method, uploadKey.ToString(), count, updateExists);


                }
                else
                {
                    count = 0;
                }
                sumarize.Ok = count;
                sumarize.UploadKey = uploadKey.ToString();
                sumarize2 = sumarize;
            }
            catch (Exception exception2)
            {
                throw new UploadException(exception2);
            }
            return sumarize2;
        }
        /*
         * TODO:fix this
        private void ExecUploadEx(ContactsUploadMethod method, string uploadKey, int count, bool updateExists)
        {
            //if (method == ContactsUploadMethod.AsyncDbCmd)
            //    dal.App_AsyncCmd_Insert("sp_Contacts_Upload", uploadKey.ToString());

            if (method == ContactsUploadMethod.QueueCommand)
            {
                RemoteApi.Instance.ExecuteQueueCommand("contacts_items_upload", uploadKey, 0);
            }
            else if (method == ContactsUploadMethod.Auto)
            {

                if (!updateExists || count <= MinContactsUploadAsync)
                {
                    // go to Contacts_Upload_Exec
                }
                else
                {
                    try
                    {
                        RemoteApi.Instance.ExecuteQueueCommand("contacts_items_upload", uploadKey.ToString(), 0);
                        return;
                    }
                    catch (Exception ex)
                    {
                        MsgException.Trace(AckStatus.NetworkError, ex);
                    }
                }
            }

            dal.ContactsItems_Upload_Exec(uploadKey.ToString());
        }
        */

        #endregion


        public UploadSumarize ContactsGroupListAdd(DataTable dtCustomer, int accountId, int groupType, int groupId, int personalFieldsCount, bool cleanBeforInsert)
        {
            if (dtCustomer == null || dtCustomer.Rows.Count == 0)
            {
                throw new UploadException("Invalid contacts");
            }
            UploadSumarize sum = new UploadSumarize();
            DataTable source = dal.ContactsMailingListWithSchema(accountId, groupId);
            DataTable dt = source.Clone();
            Dictionary<string, string> contacts = new Dictionary<string, string>();
            DataView dv = new DataView(source, "", "Target", DataViewRowState.CurrentRows);
            int count = 0;
            const int tableColumns = 5;

            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dtCustomer.Rows)
            {

                try
                {
                    object[] record = new object[tableColumns];
                    string cell = Types.NZ(dr[0], "");
                    string target = "";
                    string personal = "";

                    if (groupType == 2)
                    {
                        if (!Nistec.Regx.IsEmail(cell))
                        {
                            sum.Add(UploadState.WrongItem);
                            continue;
                        }
                        target = cell.Trim();

                    }
                    else
                    {
                        CLI cli = new CLI(cell.Trim());

                        if (!cli.IsValid)
                        {
                            sum.Add(UploadState.WrongItem);
                            continue;
                        }
                        target = cli.CellNumber;
                    }

                    if (contacts.ContainsKey(target))
                    {
                        sum.Add(UploadState.Duplicate);
                        continue;
                    }
                    if (dv.Find(target) > -1)
                    {
                        sum.Add(UploadState.Duplicate);
                        continue;
                    }

                    if (personalFieldsCount > 0)
                    {
                        sb = new StringBuilder();
                        for (int i = 1; i <= personalFieldsCount; i++)
                        {
                            sb.AppendFormat("{0};", Types.NZ(dr[i], ""));
                        }
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        personal = sb.ToString();
                        if (personal.Length > 250)
                            personal = "";
                    }

                    record[0] = target;
                    record[1] = groupId;
                    //record[2] = accountId;
                    record[2] = DateTime.Now;
                    record[3] = groupType;
                    record[4] = personal;// ReadTextField(dr, "Personal", "", 250);

                    dt.Rows.Add(record);
                    count++;
                    contacts[target] = target;
                }
                catch (Exception e) { errs.Add(e.Message); }
            }

            if (errs.Count > 0 && count <= 0)
            {
                throw new Exception("Contacts list not define correctly");
            }
            else if (count <= 0)
            {
                throw new Exception("No Contacts inserted");
            }
            try
            {

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (cleanBeforInsert)
                    {
                        dal.ContactsMailingList_RemoveAll(groupId);
                    }
                    dal.InsertTable(dt, "Contacts_Mailing_List");
                }
                sum.Ok = count;

                return sum;
            }
            catch (Exception ex)
            {
                throw new UploadException(ex);
            }
        }
   
        public UploadSumarize ContactsItemsBlockedAdd(DataTable dtCustomer, PlatformType platform, int accountId, int groupId, string userId, string remark, int BlockLevel)
        {
            if (platform == PlatformType.NA)
                throw new ArgumentException("Platform is required: mail or cell");
            UploadSumarize sum = new UploadSumarize();
            int TargetType = (int)platform;
            DataTable history = dal.Blocked_HistorySchema().Clone();
            DataTable source = dal.ContactsItems_BlockedWithSchema(accountId, TargetType);
            DataTable dt = source.Clone();
            DataView dv = new DataView(source, "", "Target,GroupId,TargetType", DataViewRowState.CurrentRows);
            dv.AllowEdit = true;
            List<string> contacts = new List<string>();
            int count = 0;
            bool isUpdate = false;

            foreach (DataRow dr in dtCustomer.Rows)
            {
                try
                {
                    object[] record = new object[dt.Columns.Count];
                    object[] recordH = new object[8];
                    string target = Types.NZ(dr[0], "");

                    if (platform == PlatformType.Mail)
                    {
                        if (!Nistec.Regx.IsEmail(target))
                        {
                            sum.Add(UploadState.WrongItem);
                            continue;
                        }
                    }
                    else
                    {
                        CLI cli = new CLI(target);
                        if (!cli.IsValid)
                        {
                            sum.Add(UploadState.WrongItem);
                            continue;
                        }
                        target = cli.CellNumber;

                    }
                    int row = dv.Find(new object[] { target, groupId,(int)platform });
                    if (row > -1)
                    {
                        sum.Add(UploadState.Duplicate);

                        if (Types.ToInt(dv[row]["BlockLevel"], 0) < (int)BlockLevel)
                        {
                            dv[row]["BlockLevel"] = (int)BlockLevel;
                            isUpdate = true;
                            goto Label_history;
                        }
                        continue;
                    }
                    record[0] = target;
                    record[1] = accountId;
                    record[2] = groupId;
                    record[3] = TargetType;
                    record[4] = DateTime.Now;
                    record[5] = (int)BlockLevel;
                    record[6] = 0;

                    dt.Rows.Add(record);
                    contacts.Add(target);

                Label_history:
                    //recordH[0] = count;
                    //recordH[1] = mail;
                    //recordH[2] = remark;
                    //recordH[3] = DateTime.Now;
                    //recordH[4] = accountId;
                    //recordH[5] = userId;
                    //recordH[6] = (int)BlockLevel;
                    //recordH[7] = 1;


                    recordH[1] = target;
                    record[2] = TargetType;
                    recordH[3] = remark;
                    recordH[4] = DateTime.Now;
                    recordH[5] = accountId;
                    recordH[6] = userId;
                    recordH[7] = (int)BlockLevel;
                    recordH[8] = 1;
                    recordH[9] = count;
                    history.Rows.Add(recordH);

                    //history
                    //Target	varchar(250)	Unchecked
                    //TargetType	tinyint	Unchecked
                    //Remark	nvarchar(255)	Checked
                    //CallDate	datetime	Unchecked
                    //AccountId	int	Unchecked
                    //UserName	nvarchar(50)	Unchecked
                    //BlockType	tinyint	Unchecked
                    //BlockActionType	tinyint	Unchecked
                    //Id	int	Unchecked


                    count++;
                }
                catch { }
            }
            try
            {
                if (TargetType==2)
                    dal.InsertTable(dt, "Contacts_Items_Blocked_Mail");
                else
                    dal.InsertTable(dt, "Contacts_Items_Blocked_Cli");

                //dal.InsertTable(dt, "Contacts_Items_Blocked");
                dal.InsertTable(history, "Contacts_Items_Blocked_History");
                if (isUpdate)
                {
                    try
                    {
                        if (TargetType == 2)
                            dal.UpdateTable(dt, "Contacts_Items_Blocked_Mail");
                        else
                            dal.UpdateTable(dt, "Contacts_Items_Blocked_Cli");

                        //dal.UpdateTable(source, "Contacts_Items_Blocked");
                    }
                    catch { sum.Message = "Some contacts have not been updated"; }
                }
                sum.Ok = contacts.Count;
                return sum;
            }
            catch (Exception ex)
            {
                sum.Message = ex.Message;
                throw new UploadException(ex);
            }
        }

        public object ReadDateField(DataRow dr, string field, object defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                string date = DateHelper.FormtDate(o.ToString());
                //string date = Types.FormatDate(o.ToString(), "dd/MM/yyyy", null);
                if (string.IsNullOrEmpty(date))
                {
                    double d = 0;
                    if (double.TryParse(o.ToString(), out d))
                    {
                        DateTime ndate = DateTime.FromOADate(Types.ToDouble(o, 0));
                        date = DateHelper.FormtDate(ndate);
                    }
                }
                return date;
            }
            catch
            {
                return defaultValue;
            }
        }
        public DateTime ReadDateField(DataRow dr, string field, DateTime defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                return DateHelper.ToDateTime(o.ToString(), defaultValue);

                //return Types.ToDateTime(o.ToString(), defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }
        public object ReadBoolField(DataRow dr, string field, bool defaultValue)
        {
            try
            {
                if (dr.Table.Columns.Contains(field))
                {
                    string val = Types.NZ(dr[field], "");
                    switch (val.ToLower())
                    {
                        case "כן":
                        case "yes":
                        case "true":
                        case "ok":
                        case "on":
                            return true;
                        default:
                            return false;
                    }
                }

                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        public object ReadField(DataRow dr, string field, object defaultValue)
        {
            try
            {
                if (dr.Table.Columns.Contains(field))
                    return Types.NZ(dr[field], defaultValue);
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public string ReadTextField(DataRow dr, string field, string defaultValue, int maxLength)
        {
            try
            {
                if (dr.Table.Columns.Contains(field))
                {
                    string value = Types.NZ(dr[field], "");
                    if (value.Length > maxLength)
                        return defaultValue;
                    return value;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public object ReadFieldSex(DataRow dr, string field, object defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return "U";// Types.NZ(dr[field], defaultValue);
                string val = Types.NZ(dr[field], "");
                switch (val)
                {
                    case "זכר":
                    case "ז":
                    case "m":
                    case "M":
                        return "M";
                    case "נקבה":
                    case "נ":
                    case "f":
                    case "F":
                        return "F";
                    default:
                        return "U";
                }

            }
            catch
            {
                return defaultValue;
            }
        }

        public static object ReadDateField(object value, object defaultValue)
        {
            try
            {
                if (value == null)
                    return defaultValue;
                return Types.FormatDate(value.ToString(), "d", null);
            }
            catch
            {
                return defaultValue;
            }
        }

        [Obsolete("see ContactItem")]
        public static string ReadSexField(object value, string defaultValue)
        {
            try
            {
                if (value == null)
                    return defaultValue;
                string val = Types.NZ(value, "");
                switch (val)
                {
                    case "זכר":
                    case "ז":
                    case "m":
                    case "M":
                        return "M";
                    case "נקבה":
                    case "נ":
                    case "f":
                    case "F":
                        return "F";
                    default:
                        return "U";
                }

            }
            catch
            {
                return defaultValue;
            }
        }

        //public int ContactGroupAdd(ICollection<long> contacts, int accountId, string GroupName)
        //{
        //    if (string.IsNullOrEmpty(GroupName))
        //    {
        //        throw new Exception("Invalid group name");
        //    }
        //    if (contacts == null || contacts.Count == 0)
        //    {
        //        return 0;
        //    }

        //    DataTable dtGroups = dal.Contacts_Groups(accountId,false);
        //    DataView dvGroups = new DataView(dtGroups, "", "ContactGroupName", DataViewRowState.CurrentRows);
        //    int groupId = 0;
        //    int index = dvGroups.Find(GroupName);
        //    if (index > -1)
        //    {
        //        groupId = Types.ToInt(dvGroups[index]["ContactGroupId"], 0);
        //    }
        //    if (groupId <= 0)
        //    {
        //        dal.Contacts_Groups_Insert(ref groupId, GroupName, accountId);
        //    }
        //    if (groupId <= 0)
        //    {
        //        throw new Exception("Error in group name");
        //    }

        //    return ContactGroupAdd(contacts, accountId, groupId);

        //}

       

        //public void Contacts_Relation_Remove(int groupId)
        //{
        //    dal.Contacts_Relation_Delete(groupId);
        //}


        

        
        

        //public bool ValidateGroupName(int accountId, string groupName, int groupId, ref string msg)
        //{
        //    if (groupName == "ללא" || groupName == "כללי")
        //    {
        //        msg = "שם קבוצה לא חוקי";
        //        return false;
        //    }
        //    if (groupName.Length > 50)
        //    {
        //        msg = "שם קבוצה מוגבל ל 50 תוים";
        //        return false;
        //    }
        //    if (dal.Contacts_Groups_Exists(accountId, groupName, groupId))
        //    {
        //        msg = "שם קבוצה קיים";
        //        return false;
        //    }
        //    return true;
        //}


        public static DataTable Contacts_Temp_Schema()
        {
            DataTable dt = new DataTable("Contacts_Temp");

            dt.Columns.Add("ContactId", typeof(int));
            dt.Columns.Add("AccountId", typeof(int));
            dt.Columns.Add("CellNumber");
            dt.Columns.Add("Email");
            dt.Columns.Add("GroupId", typeof(int));
            dt.Columns.Add("Creation", typeof(DateTime));
            dt.Columns.Add("UploadKey", typeof(Guid));
            return dt.Clone();
        }

        //public static void InsertContactsTemp(ICollection<int> contacts, int accountId, int groupId, string key)
        //{
        //    DalContacts.Instance.Contacts_Temp_Delete(key);

        //    DataTable dt = Contacts_Temp_Schema();
        //    Guid g = new Guid(key);
        //    DateTime creation = DateTime.Now;

        //    foreach (long l in contacts)
        //    {
        //        dt.Rows.Add(l, accountId, "", "", groupId, creation, g);
        //    }

        //    DalContacts.Instance.InsertTable(dt, "Contacts_Temp");

        //}

        
        //public static Guid InsertContactsTemp(ICollection<int> contacts, int accountId, int groupId)
        //{
        //    Guid g = Guid.NewGuid();
        //    InsertContactsTemp(contacts, accountId, groupId, g.ToString());
        //    return g;
        //}

        

    }
}
