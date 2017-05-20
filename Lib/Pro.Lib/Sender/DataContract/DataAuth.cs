using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Nistec.Data;
using Nistec;
using Nistec.Serialization;

namespace RestApi.DataContracts
{
    public class DataAuth
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public int UserId { get; private set; }
    }
  
}