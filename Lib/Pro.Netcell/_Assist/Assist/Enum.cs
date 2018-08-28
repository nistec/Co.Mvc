using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell
{

    public enum Lang
    {
        He = 0,
        En = 1
    }

    //public enum PlatformSite
    //{
    //    NA = 0,
    //    Cell = 1,
    //    Web = 2,
    //    Multimedia=3
    //}

    public enum PreviewMode
    {
        NA = 0,
        Html = 1,
        Body = 2,
        Preview = 3,
        Sms = 4
    }

    public enum CreditBillingType
    {
        Credit=0,
        Monthly=1,
        NoBilling=2
    }

    public enum BillingType
    {
        NA = 0,
        /// <summary>
        /// Credit billing
        /// </summary>
        CB = 1,
        /// <summary>
        /// Reverse billing
        /// </summary>
        RB = 2,
        /// <summary>
        /// Lab test no billing
        /// </summary>
        LB = 3
    }
    public enum BillingMethodTypes
    {
        None = 0,
        Unit = 1,
        Campaign = 2,
        Monthly = 3,
        Manual = 4

    }
    public enum CreditState
    {
        InvalidItems = -2,
        InvalidBillingPrice = -1,
        NotEnoughCredit = 0,
        OkMonthlyBilling = 2,
        OkAlertFlag = 3,
        Ok = 1
    }
    public enum MethodCategory
    {
        NA = 0,
        SMS = 1,
        //WAP = 2,
        SWP = 2,
        MAL = 3,
    }
    public enum TransModule
    {
        TransCB = 0,
        CmapaignCell = 1,
        CampaignMail = 2
    }

    public enum TransTypes
    {
        /// <summary>
        /// MO-MT
        /// </summary>
        MO = 1,
        /// <summary>
        /// MT Push
        /// </summary>
        MT = 2,
        /// <summary>
        /// MO billing
        /// </summary>
        MB = 3,
        /// <summary>
        /// Alert MT
        /// </summary>
        AT = 4,
        /// <summary>
        /// Voting MT
        /// </summary>
        VT = 5,
        /// <summary>
        /// Password PS
        /// </summary>
        PS = 6
    }

    public enum ContentTypes
    {
        NA = 0,
        Text = 1,
        RingSms = 2,
        PictureSms = 3,
        LogoSms = 4,
        RingTone = 5,
        TrueTone = 6,
        Image = 7,
        Animation = 8,
        Java = 9,
        Video = 10,
        Link = 11,
        Mms = 12,
        Empty = 13
    }
}
