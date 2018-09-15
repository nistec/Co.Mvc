using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pro.Upload
{

    [Serializable]
    public class UploadException : ApplicationException
    {

        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="msg"></param>
        public UploadException(string msg)
            : base(msg)
        {
            OnException(msg);
        }
        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="ex"></param>
        public UploadException(Exception ex)
            : base(ex.Message)
        {
            OnException(ex.Message);
        }

        /// <summary>
        /// MessageException
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public UploadException(string msg, Exception ex)
            : base(msg, ex)
        {
            OnException(msg);
        }


        protected void OnException(string message)
        {
            //Log.ErrorFormat("Error:{0} {1}",_AckStatus, message);

        }

    }
}
