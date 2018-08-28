using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Text.RegularExpressions;
using Netcell.Data.Client;

namespace Netcell.Data.DbServices.Entities
{

    [Entity("Integration_Services", "Integration_Services", "cnn_Services", EntityMode.Generic, "ServiceId")]
    public class Integration_Services : ActiveEntity
    {

        public static Integration_Services ServiceMO(string KeyCode, string SC, int OperatorId, int defaultService)
        {
            int serviceId = 0;
            using (DalServices instance = new DalServices())
            {
                serviceId = instance.Lookup_Service_MO(KeyCode, SC, OperatorId);
            }
            if (serviceId <= 0)
                serviceId = defaultService;
            if (serviceId <= 0)
            {
                throw new Exception(string.Format("ServiceId not found for KeyCode:{0}, SC:{1}, OperatorId:{2}", KeyCode, SC, OperatorId));
            }

            return new Integration_Services(serviceId);
        }

        public static Integration_Services ServiceMO(string KeyCode, string SC, string ip, int defaultService)
        {
            int serviceId = 0;
            using (DalServices instance = new DalServices())
            {
                serviceId = instance.GetService_Mo_ByIp(KeyCode, SC, ip, ref serviceId);
            }
            if (serviceId <= 0)
                serviceId = defaultService;
            if (serviceId <= 0)
            {
                throw new Exception(string.Format("ServiceId by ip not found for KeyCode:{0}, SC:{1}, OperatorId:{2}", KeyCode, SC, ip));
            }

            return new Integration_Services(serviceId);
        }

        


        #region Ctor

        public Integration_Services()
            : base()
        {

        }
        public Integration_Services(int ServiceId)
            : base(ServiceId)
        {

        }

        #endregion


        #region Properties

        [EntityProperty(EntityPropertyType.Identity)]
        public int ServiceId
        {
            get { return base.GetValue<int>(); }
        }
       
        [EntityProperty(EntityPropertyType.Default)]
        public int ServiceType
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public string Subject
        {
            get { return base.GetValue<string>(); }
            set { SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public string Sender
        {
            get { return base.GetValue<string>(); }
            set { SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public int ActionType
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int AccountId
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int CampaignId
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int RegistryForm
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        
        [EntityProperty(EntityPropertyType.Default)]
        public decimal Price
        {
            get { return base.GetValue<decimal>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public DateTime Creation
        {
            get { return base.GetValue<DateTime>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public bool IsActive
        {
            get { return base.GetValue<bool>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int PagesCount
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Destination
        {
            get { return base.GetValue<string>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string MessageReturn
        {
            get { return base.GetValue<string>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public int FailureServiceId
        {
            get { return base.GetValue<int>(); }
            set { SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public bool EnableTrace
        {
            get { return base.GetValue<bool>(); }
            set { SetValue(value); }
        }
        
        #endregion

        public void AddRegistery(string target)
        {
            //int AccountId = UserAuth.ValidateAccount(User, Password);
            //if (AccountId <= 0)
            //{
            //    throw new MsgException(AckStatus.AccessDenied, "Access is denied");
            //}

            //Log.InfoFormat("RegistryAdd: AccountId={0}, Cli={1}", AccountId, target);

            //int RegistryId = 0;
            //DalRegistry.Instance.Registry_Items_Insert(ref RegistryId, FormId, Name, target, "", "", "");

            //return new RegistryAddResponse(0, RegistryId > 0 ? "registred ok" : "item not registred", RegistryId);

        }
    }

}
