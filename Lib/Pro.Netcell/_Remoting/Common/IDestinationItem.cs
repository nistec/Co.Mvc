using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netcell.Data;
using Netcell;

namespace Netcell.Remoting
{
    public interface IDestinationItem
    {
        string Key { get; }
        string Target { get; }
        string Personal { get; }
        string Sender { get; }
        string Coupon { get; }
        int GroupId { get; }
        int ContactId { get; }
        string Prefix { get; }//u=upload,g=group,q=query,t=target
        string Date { get; }

        PlatformType Platform { get; }

        //object[] ItemArray(int campaignId,int batchId,DateTime timeSend,int index, int batchIndex);
    }
}
