using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netcell
{

    public enum MsgStatus
    {

        /// <summary>
        /// Non Initiliaized
        /// </summary>
        None = 0,
        /// <summary>
        /// Pending 
        /// </summary>
        Pending = 1,
        /// <summary>
        /// In Process 
        /// </summary>
        Process = 2,
        /// <summary>
        /// CLI Not correct
        /// </summary>
        Canceled = 3,
        /// <summary>
        /// Operator changed
        /// /// </summary>
        Rejected = 4,
        /// <summary>
        /// Failed
        /// </summary>
        Failed = 5,
        /// <summary>
        /// Blocked
        /// </summary>
        Blocked = 6,
        /// <summary>
        /// Delivered
        /// </summary>
        Delivered = 7,
        /// <summary>
        /// Success/comlpleted
        /// </summary>
        Completed = 9,
        /// <summary>
        /// internal error
        /// </summary>
        ServerError=99
    }

    public enum AckService
    {
        None = 0,
        MessageResult = 5001,
        ValidateCredit = 5002,
        ValidateVersion = 5003,
        ValidateAccount = 5004,
        ActualCredit = 5005,
        ContactAdd = 5006,
        ContactBlock = 5007,
        ContactRule = 5008,
        ContactJoin = 5009,
        AlertTask = 5010
    }

    public enum AckResult
    {
        None,
        Success,
        Failed,
        Error
    }
    public enum AckStatus
    {
        None = 0,
        
        MsgPending = 1,
        MsgProcess = 2,
        MsgCanceled = 3,
        MsgRejected = 4,
        MsgFailed = 5,
        MsgBlocked = 6,
        MsgReceived = 7,
        MsgDelivered = 8,
        MsgCompleted = 9,
        MsgError = 99,

        MsgDebug = 100,
        MsgOk = 200,

        Ok = 100,
        Received = 101,
        Delivered = 102,

        //security exceptions
        AuthorizationException = 401,
        IllegalAuthentication = 402,
        AccessDenied = 403,
        SecurityException = 500,
        SecurityLoginOver = 501,
        SecurityLoginFaild = 502,
        BlockedByOperator = 503,
        RejectedByOperator = 505,
        SecurityBlockedAddress = 506,

        //account exceptions
        UnExpectedError = 1000,
        XmlParsingError = 1001,
        AccountUndefined = 1002,
        BadDestination = 1007,
        BadRequest = 1013,
        MessageUndefined = 1015,
        InvalidContent = 1021,
        NotSupportedException = 1023,
        //internal errors
        ApplicationException = 3001,
        QueueException = 3002,
        ArgumentException = 3005,
        ConfigurationException = 3007,
        SqlException = 3008,
        HttpException = 3012,
        SmtpException = 3013,
        TimeOutException = 3014,
        CarrierNotResponse=316,
        IOException = 3023,
        CacheException = 3024,
        BillingException = 3028,
        NotificationException = 3029,
        MobileException = 3030,
        WebException = 3031,
        EntityException = 3033,
        AdminException = 3037,
        SoapException = 3038,
        //Warning
        WarningAlert = 4000,
        CreditError = 4003,
        NetworkError = 4004,
        NotEnoughCredit = 4009,
        //Fatal errors
        FatalException = 5001,
        FatalCarrierException = 5002,
        FatalSchedulerException = 5007,
    }

    /*
public enum AckStatus
{
    None = 0,
    //True = 1,

    Pending = 1,
    Process = 2,
    //Canceled = 3,
    //Rejected = 4,
    //Failed = 5,
    //Blocked = 6,
        
    ////Delivered = 7,
    ////Completed = 9,
    //Sent=7,
    //Approved=9,

    BatchPending = 10,
    BatchProcess = 11,
    BatchCancled = 12,
    BatchCompleted = 13,
    BatchSentWithError = 14,

    Ok = 100,
    Received = 101,
    Delivered = 102,


    //security exceptions
    UnAuthorized = 401,
    IllegalAuthentication = 402,

    SecurityBlockedAddress = 500,
    SecurityLoginOver = 501,
    SecurityLoginFaild = 502,

    //Sender exception
    BlockedByOperator = 503,
    RejectedByOperator = 505,
    SenderException = 506,
    CarrierException = 507,

    //account exceptions
    UnExpectedError = 1000,
    XmlParsingError = 1001,
    AccountError = 1002,
    CreditError = 1003,
    NetworkError = 1004,
    TargetError = 1005,
    AccessDenied = 1006,
    BadDestination = 1007,
    ReceiveTimeout = 1008,
    NotEnoughCredit = 1009,
    ParentHasNoCredit = 1010,
    //VersionUpdate = 1011,
    VersionExpired = 1012,
    BadRequest = 1013,
    AccountNotActivated = 1014,
    MessageTextNotValid = 1015,
    EncodingNotSupported = 1016,
    PendingError = 1017,
    InvalidDestinationCount = 1018,
    InvalidServiceCode = 1019,
    InvalidAppliction = 1020,
    InvalidContent = 1021,
    InvalidAccountPrice = 1022,
    MethodNotSupported = 1023,
    AckError = 1024,
    MailServerError = 1025,

    //internal errors
    ApplicationError = 3001,
    QueueError = 3002,
    UnsupportParameter = 3003,
    InvalidCastException = 3004,
    ArgumentException = 3005,
    FormatException = 3006,
    ConfigFileException = 3007,
    DB_Error = 3008,
    DBConnectionError = 3009,
    ConnectionProviderError = 3010,
    DBUpdateError = 3011,
    HttpException = 3012,
    SmtpException = 3013,
    TimeOutException = 3014,
    DllReturnError = 3015,
    CarrierNotResponse = 3016,
    InvalidApplicationResult = 3018,
    NetworkConnectionError = 3019,
    FileLoadException = 3020,
    FileAccessException = 3021,
    ExternalException = 3022,
    IOException = 3023,
    CacheException = 3024,
    CampaignException = 3025,
    RemoteConnectionError = 3026,
    DurationWarning = 3027,
    BillingException = 3028,
    NotificationError = 3029,
    MoException = 3030,
    WapBrowsError = 3031,
    MailBrowsError = 3032,
    LoadActiveRecoredError = 3033,
    DestinationCacheError = 3304,
    MobileDeviceError = 3305,
    CmsBrowsError = 3036,
    AdminException = 3037,
    NetServicesException = 3038,

    //Warning
    WarningAlert = 4000,
    WarningWrongItemsExeeds = 4001,


 


    //Fatal errors
    FatalException = 5001,
    FatalCarrierException = 5002,
    FatalCarrierCreditException = 5003,
    FatalSecurityException = 5004,
    FatalCounterException = 5005,
    FatalDestinationException = 5006,
    FatalSchedulerException = 5007,
    FatalCampaignException = 5008
}
*/

}
