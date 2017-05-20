using System;
using Nistec;

namespace Pro.Server.Data
{
	/// <summary>
	/// Summary description for NativeMethods.
	/// </summary>
	public class DBconfig
	{
		
        
 
        static string _cnnAdmin;

        static DBconfig()
        {
            _cnnAdmin = System.Configuration.ConfigurationManager.ConnectionStrings["AdminCnn"].ConnectionString;

        }


        public static string CnnServices
		{
			get
			{

                return System.Configuration.ConfigurationManager.ConnectionStrings["Netcell_Services"].ConnectionString;
			}
		}

        public static string CnnCo
        {
            get
            {

                return System.Configuration.ConfigurationManager.ConnectionStrings["Netcell_SB"].ConnectionString;
            }
        }
        public static string CnnNetcell
        {
            get
            {

                return System.Configuration.ConfigurationManager.ConnectionStrings["NetcellDB"].ConnectionString;
            }
        }
		public static string ConnectionString
		{
			get
			{
               
                return _cnnAdmin;
			}
		}

   
 
	}
}
