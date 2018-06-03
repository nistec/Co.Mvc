using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using Nistec.Data.SqlClient;

namespace Netcell.Data.Db
{
    public class DalUpload: DbCommand
    {
         //private ActiveCampaign acl=null;

         public event SqlRowsCopiedEventHandler SqlRowsCopied;

         public DalUpload()
             : base(DBRule.CnnNetcell)
        {
            //uploaded = false;
            base.AutoCloseConnection = true;
        }

         public static DalUpload Instance
         {
             get { return new DalUpload(); }
         }

  
        //bool uploaded;
        string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
        }

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
