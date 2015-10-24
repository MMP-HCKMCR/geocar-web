using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GeoCar.WcfService
{
    public class WebApiService : IWebApiService
    {
        [WebGet]
        public string marco()
        {
            return "polo";
        }
    }
}
