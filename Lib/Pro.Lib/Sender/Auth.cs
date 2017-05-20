using Nistec.Data.Entities;
using Nistec.Serialization;
using Pro.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pro.Lib.Sender
{

    public class Auth : IEntityItem
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }

    }

    public class Target:IEntityItem
    {
        public string To { get; set; }
        public string Personal { get; set; }
    }

}
