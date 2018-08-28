using System;
using System.Data;

namespace Netcell.Remoting
{

   
     public enum MobileOs
    {
        NA=0,
        Smartphone=1,
        IPhone=2,
        IPod=3,
        Android=4
   }

     public class MobileDevice
     {
         public MobileOs OsType;
         public string Version;
         public int Width;

         public MobileDevice(string ua)
         {
             OsType = DetectMobileDevice(ua, out Version,out Width);
         }

         public bool IsSupportViewPort
         {
             get
             {
                 switch (OsType)
                 {
                     case MobileOs.NA:
                         return false;
                     default:
                         return true;
                 }
             }
         }

          
         public string GetDoctype()
         {
             switch (OsType)
             {
                 case MobileOs.Smartphone:
                     return "<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.0//EN\" \"http://www.wapforum.org/DTD/xhtml-mobile10.dtd\">";

                 default:
                     return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
             }
         }

         public string GetViewPort(int width, bool user_scalable)
         {
             switch (OsType)
             {
                 case MobileOs.NA:
                     return "";
                 default:
                     return string.Format("<meta name=\"viewport\" content=\"width={0}; initial-scale=1.0; maximum-scale=1.0;user-scalable={1};\">", width > 0 ? width.ToString() : "device-width", user_scalable ? "yes" : "no");
             }
             //sbHeader.Append("<meta name=\"viewport\" content=\"width=device-width; initial-scale=1.0; maximum-scale=1.0;\">");
             //sbHeader.Append("<meta name=\"viewport\" content=\"width=320; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;\" />");
         }

         public static MobileDevice Empty
         {
             get{ return new MobileDevice("NA");}

         }
         public static MobileDevice GetMobileDevice(string ua)
         {
             MobileDevice device = new MobileDevice(ua);

             return device;
         }


         public static MobileOs DetectMobileDevice(string ua, out string version, out int width)
         {
             if (ua.ToUpper()=="NA")
             {
                 version = "";
                 width = 0;
                 return MobileOs.NA;
             }
             if (ua.ToLower().Contains("iphone"))
             {
                 version = "";
                 width = 320;
                 return MobileOs.IPhone;
             }
             else if (ua.ToLower().Contains("ipod"))
             {
                 version = "";
                 width = 320;
                 return MobileOs.IPod;
             }
             else if (ua.ToLower().Contains("android"))
             {
                 version = "";
                 width = 320;
                 return MobileOs.Android;
             }
             else
             {
                 version = "";
                 width = 320;
                 return MobileOs.Smartphone;
             }
         }
     }
 

}