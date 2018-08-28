using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using Nistec;
using Nistec.Data.SqlClient;
using Nistec.Data;

namespace Netcell.Data.Server
{
    public class DalUploadDB: DbCommand
    {
         //private ActiveCampaign acl=null;

         public event SqlRowsCopiedEventHandler SqlRowsCopied;

         public DalUploadDB()
             : base(DBRule.ConnectionString)
        {
            //uploaded = false;
            base.AutoCloseConnection = true;
        }

        //bool uploaded;
        string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
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
                bulkCopy.NotifyAfter = 50;
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
