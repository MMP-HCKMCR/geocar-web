using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Runtime.Serialization.Json;

using GeoCar.WcfService.Responses;

namespace GeoCar.WcfService
{
    internal class WcfErrorHandler : IErrorHandler
    {
        bool IErrorHandler.HandleError(Exception error)
        {
            return true;
        }

        void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (!(error is FaultException))
            {
                return;
            }

            var result = new ApiResult()
            {
                Success = false,
                ErrorMessage = $"ServiceFault: {error.Message}"
            };

            fault = Message.CreateMessage(MessageVersion.None, "Fault", result,
                new DataContractJsonSerializer(typeof(ApiResult)));
        }

        internal class WebApiServiceBehaviour : IEndpointBehavior
        {
            public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
                return;
            }

            public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                return;
            }

            public void Validate(ServiceEndpoint endpoint)
            {
                return;
            }

            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher dispatcher)
            {
                dispatcher.ChannelDispatcher.ErrorHandlers.Add(new WcfErrorHandler());
            }
        }
    }

    public class GeoCarServiceFactory : WebServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = base.CreateServiceHost(serviceType, baseAddresses);
            foreach (var endpoint in host.Description.Endpoints)
            {
                endpoint.EndpointBehaviors.Add(new WcfErrorHandler.WebApiServiceBehaviour());
            }

            return host;
        }
    }
}