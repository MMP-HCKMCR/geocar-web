using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using GeoCar.WcfService.Requests;
using GeoCar.WcfService.Responses;

namespace GeoCar.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IWebApiService
    {

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedResponse, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        MarcoResponse Marco();

        #region Session management
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedResponse, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        LoginResponse Login(LoginRequest request);
        #endregion

    }
}
