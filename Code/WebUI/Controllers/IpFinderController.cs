using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Service;
using Service.Models;

namespace WebUI.Controllers
{
    public class IpFinderController : ApiController
    {
        [ActionName("GetIpInformation")]
        public IpDto GetIpInformation(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress) || ipAddress.Equals("::1"))
                ipAddress = "201.202.108.21";
            ipAddress = ipAddress.Trim();
            var ipService = new IpProvider();
            var result = ipService.GetIpData(ipAddress);
            result.IpAddress = HttpUtility.HtmlEncode(result.IpAddress);
            return ipService.GetIpData(ipAddress);
        }

        [ActionName("GetLocalIpInformation")]
        public IpDto GetLocalIpInformation()
        {
            return GetIpInformation(GetClientIp());
        }

        private string GetClientIp()
        {
            var request = Request;
            return request.Properties.ContainsKey("MS_HttpContext") ? ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress : null;
        }
    }
}
