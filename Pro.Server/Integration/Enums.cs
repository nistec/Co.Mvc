using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Server.Integration
{
    public enum IntegrationSourceRule
    {
        Filter=-1,
        Ignore=0,
        Ok=1
    }

    public enum IntegrationType
    {
        Members,
        Action
    }
}
