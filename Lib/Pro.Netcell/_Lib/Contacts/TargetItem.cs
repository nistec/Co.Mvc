using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell.Lib
{
    public class ContactTarget
    {
        public readonly string Target;
        public readonly string Personal;
        public readonly int Id;
        public readonly int Rule;

        public ContactTarget(string target, string personal)
        {
            Target = target;
            Personal = personal;
            Id = 0;
        }
        public ContactTarget(string target, string personal, int id, int rule)
        {
            Target = target;
            Personal = personal;
            Id = id;
            Rule = rule;
        }
        public ContactTarget(int id, int rule)
        {
            Id = id;
            Rule = rule;
        }
    }
}
