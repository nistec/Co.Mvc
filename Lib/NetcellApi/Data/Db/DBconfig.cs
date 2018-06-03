#define Netcell//WL//Netcell

using System;
using MControl;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Nistec.Generic;

namespace Netcell.Data.Db
{
	/// <summary>
    /// Summary description for DBconfig.
	/// </summary>
	public class DBRule
	{

         public const string AddPool = ";Max Pool Size =250;Min Pool Size=5;Pooling=true";

      
        #region Connection

        

        public static string CnnEphone
        {
            get { return NetConfig.AppSettings["cnn_ephone"]; }
        }

        public static string CnnNetcell
        {
            get { return NetConfig.AppSettings["cnn_Netcell"]; }
        }
        public static string CnnTrace
        {
            get { return NetConfig.AppSettings["cnn_Trace"]; }
        }
        public static string CnnServices
        {
            get { return NetConfig.AppSettings["cnn_Services"]; }
        }
        public static string CnnCo
        {
            get { return NetConfig.AppSettings["cnn_Co"]; }
        }
        public static string CnnWeb
        {
            get { return NetConfig.AppSettings["cnn_Web"]; }
        }
        public static string CnnAuto
        {
            get { return NetConfig.AppSettings["cnn_Auto"]; }
        }
        public static string CnnMedia
        {
            get { return NetConfig.AppSettings["cnn_Media"]; }
        }
        public static string CnnApp
        {
            get { return NetConfig.AppSettings["cnn_App"]; }
        }
        public static string CnnCms
        {
            get { return NetConfig.AppSettings["cnn_Cms"]; }
        }
        public static string CnnCrm
        {
            get { return NetConfig.AppSettings["cnn_Crm"]; }
        }
        #endregion

        /// <summary>
        /// Get Netcell Connection string with max pool
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return /*_cnn*/ CnnNetcell + AddPool;
            }
        }
         
        public static IDbConnection NetcellConnection
        {
            get { return new SqlConnection(CnnNetcell); }
        }

        public static IDbConnection CmsConnection
        {
            get { return new SqlConnection(CnnCms); }
        }


        public static string GetNetcellCnn(string instance)
        {
            if (string.IsNullOrEmpty(instance) || instance == "default")
                return CnnNetcell;
            return NetConfig.ConnectionStrings[instance].ConnectionString;
        }
        public static IDbConnection GetNetcellConnection(string instance)
        {
            if (string.IsNullOrEmpty(instance) || instance == "default")
                return NetcellConnection;
            return new SqlConnection(GetNetcellCnn(instance)); ;
        }
   	}
}
