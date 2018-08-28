using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using Nistec.Data;
using Nistec.Data.SqlClient;
using System.Data.SqlClient;

using Nistec;

using System.Text;
using Netcell.Data;
using Nistec.Data.Factory;

namespace Netcell.Data.Server
{
    

    public class DalGw : Nistec.Data.SqlClient.DbCommand
    {
        public DalGw()
            : base(DBRule.ConnectionString)
        {

        }

        public static DalGw Instance
        {
            get { return new DalGw(); }
        }

        [DBCommand(DBCommandType.Insert, "Notification_CB")]
        public void Notification_CB
        (
            [DbField()]int MessageId,
            [DbField()]int OperatorId,
            [DbField()]string ConfirmId,
            [DbField()]string AckCode,
            [DbField()]string AckDescription,
            [DbField()]int Status,
            [DbField()]string FinalTime,
            [DbField()]int Completed,
            [DbField()]int FinalOperator,
            [DbField()]int Units
        )
        {
            base.Execute(MessageId, OperatorId, ConfirmId, AckCode, AckDescription, Status, FinalTime, Completed,FinalOperator,Units);
        }

        [DBCommand(DBCommandType.Insert, "Notification_RB")]
        public void Notification_RB
        (
            [DbField()]int MessageId,
            [DbField()]int OperatorId,
            [DbField()]string ConfirmId,
            [DbField()]string AckCode,
            [DbField()]string AckDescription,
            [DbField()]int Status,
            [DbField()]string FinalTime,
            [DbField()]int Completed,
            [DbField()]int FinalOperator,
            [DbField()]int Units
      )
        {
            base.Execute(MessageId, OperatorId, ConfirmId, AckCode, AckDescription, Status, FinalTime, Completed, FinalOperator, Units);
        }

        [DBCommand(DBCommandType.Insert, "Notification")]
        public void Notification
        (
            [DbField()]int MessageId,
            [DbField()]int OperatorId,
            [DbField()]string ConfirmId,
            [DbField()]string AckCode,
            [DbField()]string AckDescription,
            [DbField()]int Status,
            [DbField()]string FinalTime,
            [DbField()]int Completed,
            [DbField()]int FinalOperator,
            [DbField()]int Units,
            [DbField()]int BillingType
      )
        {
            base.Execute(MessageId, OperatorId, ConfirmId, AckCode, AckDescription, Status, FinalTime, Completed, FinalOperator, Units, BillingType);
        }


        [DBCommand(DBCommandType.Insert, "Notification_Reply")]
        public void Notification_Reply
        (
            [DbField()]int OperatorId,
            [DbField()]string ConfirmId,
            [DbField()]string AckCode,
            [DbField()]string AckDescription,
            [DbField()]int Status,
            [DbField()]string FinalTime,
            [DbField()]int Completed,
            [DbField()]int FinalOperator,
            [DbField()]int Units,
            [DbField()]int BillingType,
            [DbField()]string CellNumber
       )
        {
            base.Execute(OperatorId, ConfirmId, AckCode, AckDescription, Status, FinalTime, Completed,FinalOperator,Units, BillingType, CellNumber);
        }

        [DBCommand(DBCommandType.Insert, "Notification_Map")]
        public void Notification_Map
        (
            [DbField()]int MessageId,
            [DbField()]int OperatorId,
            [DbField()]string ConfirmId,
            [DbField()]string BillingType,
            [DbField()]string UrlNotify,
            [DbField()]int NotifState,
            [DbField()]int TransId,
            [DbField()]string CellNumber

        )
        {
            base.Execute(OperatorId, ConfirmId, MessageId, BillingType, UrlNotify, NotifState,TransId, CellNumber);
        }
   
    }
}

