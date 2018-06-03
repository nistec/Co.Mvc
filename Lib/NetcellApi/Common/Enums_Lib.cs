using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Netcell
{

      public enum UserType
    {
        Guest=0,
        User = 1,
        Manager = 2,
        //Owner = 3,
        Admin = 9,
    }

    public enum UserLevel
    {
        DenyAll = 0,
        ReadOnly = 1,
        FullControl = 2,
    }

    public enum AccUsingType
    {
        All = 0,
        Api = 1,
        Evaluate = 2,
    }

    public enum AccountType
    {
        Customer = 0,
        Parent = 1,
        Child = 2,
        Owner = 3,
        Evaluate = 4,
        Demo = 5,
        Admin = 9
    }
    public enum BlockType
    {
        NA = 0,
        CB = 1,
        RB = 2,
        Email = 10
    }

    public enum StatisticMode
    {
        ReplyByAccount = 1,
        ReplyByCampaign = 2,
        ReplySummarize = 3,
        MailDomain = 20,
        MailDsn = 21,
        MailGeo = 22
    }

    //public enum CampaignNotifyType
    //{
    //    None = 0,
    //    OnStart = 1,
    //    OnFinished = 2,
    //    Both = 3,
    //    OnBatchBegin = 4,
    //    OnBatchEnd = 5,
    //    BothAll = 6
    //}

    //public enum MediaType
    //{
    //    SMS,
    //    WAP,
    //    MMS,
    //    BIN,
    //    IVR,
    //    MAL
    //}

    public enum WapLinkType
    {
        DownloadContentKey,
        DownloadWapTransKey,
        DownloadWapCampaignKey,
        DownloadWapCampaignTest
    }

    public enum SessionType
    {
        Init = 0,
        //SessionStart = 1,
        //SessionEnd = 2,
        Login = 3,
        Logout = 4,
        Logfailed = 5,
        StartSender = 6,
        ReloadSender = 7,
        LoadSender = 8,
        SenderSent = 9,
        SenderFailed = 10
    }

    public enum DeviceRule
    {
        None = 0,
        Mobile = 1,
        Device = 2
    }
   
    public enum CampaignSendType
    {
        Now = 0,
        Pending = 1,
        Fixed = 2,
        Batches = 3,
        Watch = 4,
        Manual = 5
        //Alert = 10,
        //Media = 11

    }
    public enum SendInterval
    {
        None = 0,
        Weekly = 1,
        FixedDayWeek = 2,
        Fixed = 3,
        Monthly = 4,
    }
    public enum CampaignStatus
    {
        Wait = 0,
        Saved = 1,
        Dtaft=2,
        Edit=8,
        Expired = 9
    }
   
    public enum ReminderType
    {
        Once = 0,
        Fixed = 1
    }

    public enum SchedulerType
    {
        D,//Daily
        M,//Monthly
        C//Cycle
    }

    public enum ServicesAlertType
    {
        SmsAlert = 1,
        CellAlert = 2,
        MailAlert = 3,
        CampaignStatistic = 4,
    }

    public enum DesignsType
    {
        NA = 0,
        MobileMenu = 1,
        MobileForm = 2,
        MobileCaption = 3
    }


    public enum Directions
    {
        MO,
        MT,
        MB
    }

    public enum PageCataegory
    {
        General = 0,
        Master = 1,
        Home = 2,
        About = 3,
        Contact = 4,
        Products = 5,
        Partners = 6,
        Regulations = 7,
        Signin = 8,
        Rgistry = 9,
        Thanks = 10,
        Map = 11,
        Pay = 12,
        Shop = 13,
        Form = 14,
        Blog = 15,
        Error = 16,
        Welcome = 17,
        Gallery = 18
    }

    public enum MailMode
    {
        NA = 0,
        Cell = 1,
        Mail = 2,
        Both = 3
    }


    public enum AccountBillingType
    {
        Credit = 0,
        Monthly = 1
    }



    public enum MeesageRuleType
    {
        Default = 0,
        RB = 1,
        Quick = 2,
        Bulk = 3,
        Pending = 4,
        Password = 5
    }

    public enum SendType
    {
        Now,
        Pending,
        Fixed
    }

   
    public enum CampaignNotifyType
    {
        None = 0,
        OnStart = 1,
        OnEnd = 2,
        Both = 3,
        //OnBatchBegin = 4,
        //OnBatchEnd = 5,
        //BothAll = 6
        OnReplyOnly = 4,
        BeginAndReply = 5,
        BothAndReply = 6,
    }

    public enum BlockActionType
    {
        Add = 1,
        Remove = 0
    }

}
