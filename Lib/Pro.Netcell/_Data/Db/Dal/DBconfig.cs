#define Netcell//WL//Netcell

using System;
using Nistec;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Netcell.Data
{
	/// <summary>
    /// Summary description for DBconfig.
	/// </summary>
	public class DBRule
	{

         public const string AddPool = ";Max Pool Size =250;Min Pool Size=5;Pooling=true";

        /*      
        static string _cnn;
        static string _trace;
        static string _services;
        static string _media;
        static string _app;
        static string _cms;
        static string _crm;
        

        static DBRule()
        {
#if (WL)
            _cnn = System.Configuration.ConfigurationManager.ConnectionStrings["NetcellCnn"].ConnectionString;
            _trace = System.Configuration.ConfigurationManager.ConnectionStrings["NetcellTrace"].ConnectionString;
            _media = System.Configuration.ConfigurationManager.ConnectionStrings["MediaCnn"].ConnectionString;
            _app = System.Configuration.ConfigurationManager.ConnectionStrings["AppCnn"].ConnectionString;
            _cms = System.Configuration.ConfigurationManager.ConnectionStrings["CmsCnn"].ConnectionString;
            //_auth = System.Configuration.ConfigurationManager.ConnectionStrings["NetcellAuoth"].ConnectionString;
            //_wsurl = System.Configuration.ConfigurationManager.ConnectionStrings["NetcellUrl"].ConnectionString;

#else

            //_cnn = System.Configuration.ConfigurationManager.ConnectionStrings["NetcellCnn"].ConnectionString;
            //_trace = System.Configuration.ConfigurationManager.ConnectionStrings["NetcellTrace"].ConnectionString;
            //_media = System.Configuration.ConfigurationManager.ConnectionStrings["MediaCnn"].ConnectionString;
            //_app = System.Configuration.ConfigurationManager.ConnectionStrings["AppCnn"].ConnectionString;
            //_cms = System.Configuration.ConfigurationManager.ConnectionStrings["CmsCnn"].ConnectionString;



            //if (string.IsNullOrEmpty(_cnn))
            //    _cnn = "Data Source=62.219.21.26; Initial Catalog=NetcellDB; User ID=sa; Password=tishma567; Connection Timeout=30";
            //if (string.IsNullOrEmpty(_trace))
            //    _trace = "Data Source=62.219.21.26; Initial Catalog=Netcell_Trace; User ID=sa; Password=tishma567; Connection Timeout=30";
            //if (string.IsNullOrEmpty(_app))
            //    _app = "Data Source=62.219.21.26; Initial Catalog=NetcellApp; User ID=sa; Password=tishma567; Connection Timeout=30";
            //if (string.IsNullOrEmpty(_media))
            //    _media = "Data Source=62.219.21.26; Initial Catalog=MyMediaDB; User ID=sa; Password=tishma567; Connection Timeout=30";
            //if (string.IsNullOrEmpty(_cms))
            //    _cms = "Data Source=62.219.21.26; Initial Catalog=MyCrm; User ID=sa; Password=tishma567; Connection Timeout=30";
            //_cms = "Data Source=62.219.21.26; Initial Catalog=Netcell_Cms; User ID=sa; Password=tishma567; Connection Timeout=30";

#endif


            _cnn = CnnNetcell;
            _trace = CnnTrace;
            _services = CnnServices;
            _media = CnnMedia;
            _app = CnnApp;
            _cms = CnnCms;
            _crm = CnnCrm;
        }

*/

        #region Connection

        

        public static string CnnEphone
        {
            get { return ConfigurationManager.AppSettings["cnn_ephone"]; }
        }

        public static string CnnNetcell
        {
            get { return ConfigurationManager.AppSettings["cnn_Netcell"]; }
        }
        public static string CnnTrace
        {
            get { return ConfigurationManager.AppSettings["cnn_Trace"]; }
        }
        public static string CnnServices
        {
            get { return ConfigurationManager.AppSettings["cnn_Services"]; }
        }
        public static string CnnCo
        {
            get { return ConfigurationManager.AppSettings["cnn_Co"]; }
        }
        public static string CnnWeb
        {
            get { return ConfigurationManager.AppSettings["cnn_Web"]; }
        }
        public static string CnnAuto
        {
            get { return ConfigurationManager.AppSettings["cnn_Auto"]; }
        }
        public static string CnnMedia
        {
            get { return ConfigurationManager.AppSettings["cnn_Media"]; }
        }
        public static string CnnApp
        {
            get { return ConfigurationManager.AppSettings["cnn_App"]; }
        }
        public static string CnnCms
        {
            get { return ConfigurationManager.AppSettings["cnn_Cms"]; }
        }
        public static string CnnCrm
        {
            get { return ConfigurationManager.AppSettings["cnn_Crm"]; }
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


        /*
 public static string NetcellCnn
 {
     get {
         if(_cnn==null)
             _cnn = ConfigurationManager.ConnectionStrings["NetcellCnn"].ConnectionString;

         return _cnn; 
     }
 }

 public static string NetcellTrace
 {
     get
     {
         if (_trace == null)
             _trace = ConfigurationManager.ConnectionStrings["NetcellTrace"].ConnectionString;

         return _trace;
     }
 }
 public static string ServicesCnn
 {
     get
     {
         if (_services == null)
             _services = CnnServices;

         return _services;
     }
 }

 public static string MediaCnn
 {
     get {
         if (_media == null)
             _media = ConfigurationManager.ConnectionStrings["MediaCnn"].ConnectionString;
         return _media; 
     }
 }

 public static string AppCnn
 {
     get {
         if (_app == null)
             _app = System.Configuration.ConfigurationManager.ConnectionStrings["AppCnn"].ConnectionString;
         return _app; 
     }
 }
 public static string CmsCnn
 {
     get {
         if (_cms == null)
             _cms = ConfigurationManager.ConnectionStrings["CmsCnn"].ConnectionString;
         return _cms; 
     }
 }
 public static string CrmCnn
 {
     get
     {
         if (_crm == null)
             _crm = ConfigurationManager.ConnectionStrings["CrmCnn"].ConnectionString;
         return _cms;
     }
 }
 */

        public static string GetNetcellCnn(string instance)
        {
            if (string.IsNullOrEmpty(instance) || instance == "default")
                return CnnNetcell;
            return ConfigurationManager.ConnectionStrings[instance].ConnectionString;
        }
        public static IDbConnection GetNetcellConnection(string instance)
        {
            if (string.IsNullOrEmpty(instance) || instance == "default")
                return NetcellConnection;
            return new SqlConnection(GetNetcellCnn(instance)); ;
        }
   	}
}
