using System;
using System.Collections.Generic;
using System.Text;

namespace Netcell.Lib
{
   

 

    //public enum SchedulerDataSource
    //{
    //    Fixed,
    //    Watch,
    //    Reminder,
    //    //Alert
    //}
    public enum ServicesAlertType
    {
        SmsAlert=1,
        CellAlert = 2,
        MailAlert =3,
        CampaignStatistic = 4,
    }

    public enum DesignsType
    {
        NA=0,
        MobileMenu=1,
        MobileForm=2,
        MobileCaption=3
    }

    //public enum QueueIn
    //{
    //    NC_Quick,
    //    NC_Bulk,
    //    NC_Pending,
    //    NC_MO,
    //    NC_MT,
    //    NC_Alert,
    //}


 

    public enum Directions
    {
        MO,
        MT,
        MB
    }
 

    //public enum MediaType
    //{
    //    SMS,
    //    WAP,
    //    MMS,
    //    BIN,
    //    IVR,
    //    MAL,
    //    SWP
    //}

    //Method MT=(SMSMT|WAPMT|MMSMT|BINMT|ALERT|PASS)
    //Method MO=(MO|MB|VT)

   
    
  

    
    //public enum NotifyPlatform
    //{
    //    //NA = 0,
    //    //SmsLink = 1,
    //    SmsLink = 2,
    //    Mail=3
    //}

    public enum PageCataegory
    {
        General=0,
        Master=1,
        Home=2,
        About=3,
        Contact=4,
        Products=5,
        Partners=6,
        Regulations=7,
        Signin=8,
        Rgistry=9,
        Thanks=10,
        Map = 11,
        Pay=12,
        Shop=13,
        Form=14,
        Blog=15,
        Error=16,
        Welcome=17,
        Gallery = 18
    }

    public enum MailMode
    {
        NA = 0,
        Cell = 1,
        Mail = 2,
        Both=3
   }

  
    //public enum AccountType
    //{
    //    Customer = 0,
    //    Parent = 1,
    //    Child = 2,
    //    Manager = 3,
    //    //Channel = 4,
    //    //Affiliate = 5
    //}

    public enum AccountBillingType
    {
        Credit=0,
        Monthly=1
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

    //public enum CouponType
    //{
    //    None = 0,
    //    Text = 1,
    //    Image = 2,
    //    Barcode = 3
    //}

   
    
 
    //public enum CouponSource
    //{
    //    OneForAll = 0,
    //    List = 1,
    //    Random = 2
    //}


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

    //public enum BlockType
    //{
    //    NA = 0,
    //    Bulk = 1,
    //    Alerts = 2,
    //    Premium = 3
    //}

    public enum BlockActionType
    {
        Add = 1,
        Remove = 0
    }

   // public enum WapLinkType
   // {
   //     DownloadContentKey,
   //     DownloadWapTransKey,
   //     DownloadWapCampaignKey,
   //     DownloadWapCampaignTest
   //}

   
   
 
}
