using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using Nistec;
using Nistec.Data.SqlClient;
using Nistec.Data;
using Nistec.Data.Factory;

namespace Netcell.Data.Client
{
    public class DalUpload: DbCommand
    {
         //private ActiveCampaign acl=null;

         public event SqlRowsCopiedEventHandler SqlRowsCopied;

         public DalUpload()
             : base(Netcell.Data.DBRule.CnnNetcell)
        {
            //uploaded = false;
            base.AutoCloseConnection = true;
        }

         public static DalUpload Instance
         {
             get { return new DalUpload(); }
         }

         //public DalUpload(string instance)
         //    : base(Netcell.Data.DBRule.GetNetcellCnn(instance))
         //{

         //}

         //public static DalUpload GetInstance(string instance)
         //{
         //    if (instance == null)
         //        return Instance;
         //    return new DalUpload(instance); 
         //}


        //bool uploaded;
        string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
        }

        //public string Result
        //{
        //    get
        //    {
        //        if (uploaded)
        //            return acl.ToString();
        //        else
        //            return errorMessage;
        //    }
        //}

        //public ActiveCampaign ActiveCampaign
        //{
        //    get
        //    {
        //        if (uploaded)
        //            return acl;
        //        else
        //            return null;
        //    }
        //}

        public static DataTable Contacts_Schema()
        {
            DataTable dt = new DataTable("Contacts");
            dt.Columns.Add("CellNumber");
            dt.Columns.Add("FirstName");
            dt.Columns.Add("LastName");
            dt.Columns.Add("BirthDate");
            dt.Columns.Add("Address");
            dt.Columns.Add("Email");
            dt.Columns.Add("Coupon");
            return dt.Clone();
        }

        public static DataTable Contacts_Lists_Schema()
        {
            DataTable dt = new DataTable("Contacts_Lists");
            dt.Columns.Add(new DataColumn("ListId", typeof(Guid)));
            dt.Columns.Add(new DataColumn("CreationDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("AccountId", typeof(int)));
            dt.Columns.Add("CellNumber");
            dt.Columns.Add("FirstName");
            dt.Columns.Add("LastName");
            dt.Columns.Add("BirthDate");
            dt.Columns.Add("Address");
            dt.Columns.Add("Email");
            dt.Columns.Add("Coupon");
            dt.Columns.Add(new DataColumn("IsTemp", typeof(bool)));
            dt.Columns.Add(new DataColumn("RowIndex", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchId", typeof(int)));
            dt.Columns.Add(new DataColumn("ExpirationDate", typeof(DateTime)));

            return dt.Clone();
        }


        /*
        public static DataTable CampaignItems_Schema()
        {
            DataTable dt = new DataTable("CampaignItems");
            dt.Columns.Add(new DataColumn("CampaignId", typeof(int)));
            dt.Columns.Add(new DataColumn("MessageId", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchId", typeof(int)));
            dt.Columns.Add(new DataColumn("SendTime", typeof(DateTime)));
            dt.Columns.Add("CellNumber");
            dt.Columns.Add(new DataColumn("State", typeof(int)));
            dt.Columns.Add(new DataColumn("Retry", typeof(int)));
            dt.Columns.Add(new DataColumn("GroupId", typeof(int)));
            dt.Columns.Add("Coupon");
            dt.Columns.Add("Personal");
            dt.Columns.Add("Sender");
            //dt.Columns.Add(new DataColumn("Id", typeof(int)));
            
            return dt.Clone();
        }

        public static DataTable CampaignMailItems_Schema()
        {
            DataTable dt = new DataTable("CampaignItems");
            dt.Columns.Add(new DataColumn("CampaignId", typeof(int)));
            dt.Columns.Add(new DataColumn("MessageId", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchId", typeof(int)));
            dt.Columns.Add(new DataColumn("SendTime", typeof(DateTime)));
            dt.Columns.Add("Email");
            dt.Columns.Add(new DataColumn("State", typeof(int)));
            dt.Columns.Add(new DataColumn("Reply", typeof(int)));
            dt.Columns.Add(new DataColumn("GroupId", typeof(int)));
            dt.Columns.Add("Coupon");
            dt.Columns.Add("Personal");
            dt.Columns.Add("Sender");
            //dt.Columns.Add(new DataColumn("Id", typeof(int)));

            return dt.Clone();
        }
        */
        public static DataTable CampaignTargets_Schema()
        {
            DataTable dt = new DataTable("CampaignTargets");
            dt.Columns.Add(new DataColumn("CampaignId", typeof(int)));
            dt.Columns.Add("Target");
            dt.Columns.Add(new DataColumn("TargetIndex", typeof(int)));
            dt.Columns.Add("Personal");
            dt.Columns.Add("Coupon");
            dt.Columns.Add("Sender");
            //dt.Columns.Add("TargetKey");
            dt.Columns.Add("GroupId", typeof(int));
            dt.Columns.Add("ContactId", typeof(long));
            dt.Columns.Add("Prefix");
            dt.Columns.Add("Date");
            return dt.Clone();
        }
        
        public static DataTable CampaignRegistry_Schema()
        {
            DataTable dt = new DataTable("CampaignItems");
            dt.Columns.Add(new DataColumn("CampaignId", typeof(int)));
            dt.Columns.Add("SourceType");
            dt.Columns.Add(new DataColumn("ContactId", typeof(Int64)));
            dt.Columns.Add(new DataColumn("GroupId", typeof(int)));
            dt.Columns.Add("Target");
            dt.Columns.Add("Personal");
            dt.Columns.Add("Date");
            dt.Columns.Add("Sender");

            return dt.Clone();
        }

        //public int Invoke(DataTable dt,ActiveCampaign acl)//, Guid listId, int accountId, bool isTemp, int BatchCount, DateTime ExpirationDate)
        //{
            
        //    int rows_up = 0;
        //    //acl = new ActiveCampaign(listId);

        //    try
        //    {
        //        if (dt == null)
        //        {
        //            return 0;
        //        }
        //        acl.TotalNumbers = dt.Rows.Count;
        //        DataTable source = Contacts_Lists_Schema();
        //        CLI cli;
        //        DateTime user_date;
        //        string cell_number;
        //        object o_date;
        //        object FirstName = null;
        //        object LastName = null;
        //        object email = "";
        //        object coupon = "";
        //        bool isCliOnly = dt.Columns.Count == 1;
        //        int batchId = 0;
        //        Dictionary<string, DataRow> list = new Dictionary<string, DataRow>();

        //        foreach (DataRow d in dt.Rows)
        //        {
        //            cli = new CLI(d[0].ToString());//"CellNumber"

        //            if (cli.IsValid)
        //            {
        //                list[cli.ToString()] = d;
        //            }
        //            else
        //            {
        //                acl.InavlidNumbers++;
        //            }
        //        }
        //        acl.ValidNumbers = list.Count;
        //        acl.CalcNumbers();
        //        int itemPerBatch=acl.BatchCount<=0?  acl.ValidNumbers:acl.ValidNumbers/acl.BatchCount;

        //        int index = 0;
        //        int batchIndex=0;

        //        foreach (KeyValuePair<string, DataRow> k in list)
        //        {
        //            try
        //            {

        //                cell_number = k.Key;
        //                o_date = null;

        //                if (!isCliOnly)
        //                {
        //                    user_date = Types.NZ(k.Value["BirthDate"], Util.NullDate);
        //                    if (user_date > Util.NullDate)
        //                        o_date = user_date;
        //                    FirstName = k.Value["FirstName"];
        //                    LastName = k.Value["LastName"];
        //                    email = k.Value["Email"];
        //                    coupon = k.Value["Coupon"];
        //                }
        //                acl.MaxPersonalLength = Math.Max(acl.MaxPersonalLength, Types.NZ(FirstName, "").ToString().Length + Types.NZ(LastName, "").ToString().Length);
        //                //add the phone
        //                //ListId	uniqueidentifier	Unchecked
        //                //CreationDate	datetime	Unchecked
        //                //AccountId	int	Unchecked
        //                //CellNumber	CLI:varchar(20)	Unchecked
        //                //FirstName	nvarchar(50)	Checked
        //                //LastName	nvarchar(50)	Checked
        //                //BirthDate	smalldatetime	Checked
        //                //Email	nvarchar(255)	Checked
        //                //IsTemp	bit	Unchecked
        //                //RowIndex	int	Unchecked
        //                //BatchId	tinyint	Unchecked
        //                //Coupon	varchar(50)	Checked
        //                //ExpirationDate	datetime	Checked

        //                batchIndex++;
        //                if(batchIndex >itemPerBatch)
        //                {
        //                    batchId++;
        //                    batchIndex=0;
        //                }

        //                object[] values = { acl.ListId, DateTime.Now, acl.AccountId, cell_number, FirstName, LastName, o_date, email,coupon, acl.IsTemp, ++index, batchId, acl.ExpirationDate };
        //                source.Rows.Add(values);
        //                rows_up++;

        //            }
        //            catch (Exception ex)
        //            {
        //                errorMessage = ex.Message;
        //            }
        //        }

        //        BulkInsert(source, "Contacts_Lists", 1000, null);

        //        uploaded = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = ex.Message;
        //        rows_up = 0;
        //    }
        //    finally
        //    {
        //        //connection close
        //    }
        //    return rows_up;
        //}

        delegate void BulkInsertDelegate(DataTable source, string destinationTableName, int batchSize, params SqlBulkCopyColumnMapping[] mapings);


        public void BulkInsertAsync(DataTable source, string destinationTableName, int batchSize, params SqlBulkCopyColumnMapping[] mapings)
        {

            BulkInsertDelegate d = new BulkInsertDelegate(BulkInsert);

            IAsyncResult ar = d.BeginInvoke(source, destinationTableName, batchSize,  mapings, null, null);

            d.EndInvoke(ar);

        }


        public void BulkInsert(DataTable source, string destinationTableName, int batchSize, params SqlBulkCopyColumnMapping[] mapings)
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)base.Connection))
            {
                try
                {
                    base.Connection.Open();
                }
                catch (Exception ex) 
                { 
                    string s = ex.Message; 
                }
                System.Data.DataTableReader reader = new System.Data.DataTableReader(source);
                bulkCopy.DestinationTableName = destinationTableName;
                bulkCopy.BatchSize = batchSize;
                // Set up the event handler to notify after x rows.
                bulkCopy.SqlRowsCopied +=
                    new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                bulkCopy.NotifyAfter = 1000;
                bulkCopy.BulkCopyTimeout = 300;
                if (mapings != null)
                {
                    foreach (SqlBulkCopyColumnMapping col in mapings)
                    {
                        bulkCopy.ColumnMappings.Add(col);
                    }
                }
                try
                {
                    // Write from the source to the destination.
                    bulkCopy.WriteToServer(reader);
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    throw ex;
                }
                finally
                {
                    reader.Close();
                    //CloseConnection();
                }
            }
        }

        private void OnSqlRowsCopied(
       object sender, SqlRowsCopiedEventArgs e)
        {
            //_procMessage = string.Format("Copied {0} so far...", e.RowsCopied);
            if (this.SqlRowsCopied != null)
                this.SqlRowsCopied(this, e);

        }


    }
}
