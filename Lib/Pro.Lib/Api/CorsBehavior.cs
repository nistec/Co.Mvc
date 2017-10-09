using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace Pro.Lib.Api
{
    public class CorsBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public void AddBindingParameters(
          ServiceEndpoint endpoint,
          BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
              new CorsHeaderInjectingMessageInspector()
            );
        }

        public void Validate(ServiceEndpoint endpoint) { }

        public override Type BehaviorType { get { return typeof(CorsBehavior); } }

        protected override object CreateBehavior() { return new CorsBehavior(); }

        private class CorsHeaderInjectingMessageInspector : IDispatchMessageInspector
        {
            public object AfterReceiveRequest(
              ref Message request,
              IClientChannel channel,
              InstanceContext instanceContext)
            {
                return null;
            }

      private static IDictionary<string, string> _headersToInject = new Dictionary<string, string>
      {
        { "Access-Control-Allow-Origin", "http(s)?://(www\\.)?(my-t.co.il)$" },
        { "Access-Control-Request-Method", "POST,GET,PUT,DELETE,OPTIONS" },
        { "Access-Control-Allow-Headers", "X-Requested-With,Content-Type" }
      };

            public void BeforeSendReply(ref Message reply, object correlationState)
            {
                var httpHeader = reply.Properties["httpResponse"] as HttpResponseMessageProperty;
                foreach (var item in _headersToInject)
                    httpHeader.Headers.Add(item.Key, item.Value);
            }
        }
    }
}