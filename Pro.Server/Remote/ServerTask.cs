using Nistec.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Server
{
    public class ServerTask:IEntityItem
    {
        public string CommandText
        {
            get ;set;
        }
        public string CommandName
        {
            get;
            set;
        }
        public string Arguments
        {
            get;
            set;
        }
        public int CommandType
        {
            get;
            set;
        }
        public int CommandId
        {
            get;
            set;
        }
        public int SchedulerId
        {
            get;
            set;
        }
        public int Timeout
        {
            get;
            set;
        }
        public bool Async
        {
            get;
            set;
        }
    }
}
