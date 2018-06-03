using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Remoting;


namespace Netcell.Lib.View
{

    public struct CampaignPath
    {

        public string UploadLocalPath;
        public string UploadVirtualPath;
        public string WebClientLocalPath;
        public string WebClientVirtualPath;
        public bool IsQuiz;

        public static CampaignPath Empaty
        {
            get { return new CampaignPath(); }
        }

        public bool IsEmpaty
        {
            get { return UploadVirtualPath == null; }
        }


        public CampaignPath(string uploadLocalPath, string uploadVirtualPath,
               string webClientLocalPath, string webClientVirtualPath, bool isQuiz)
        {
            UploadLocalPath = uploadLocalPath;
            UploadVirtualPath = uploadVirtualPath;
            WebClientLocalPath = webClientLocalPath;
            WebClientVirtualPath = webClientVirtualPath;
            IsQuiz = isQuiz;
        }

    }

}