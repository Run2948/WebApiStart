using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Learning.WebApi.Services
{
    public class LearningControllerSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _config;
        public LearningControllerSelector(HttpConfiguration config)
            : base(config)
        {
            _config = config;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllers = GetControllerMapping(); //Will ignore any controls in same name even if they are in different namepsace

            var routeData = request.GetRouteData();
            if (string.IsNullOrWhiteSpace(routeData.Route.RouteTemplate))
            {
                return base.SelectController(request);
            }
            var controllerName = routeData.Values["controller"].ToString();

            if (controllers.TryGetValue(controllerName, out var controllerDescriptor))
            {
                //var version = GetVersionFromQueryString(request);
                //var version = GetVersionFromHeader(request);
                var version = GetVersionFromAcceptHeaderVersion(request);

                var versionedControllerName = string.Concat(controllerName, "V", version);

                return controllers.TryGetValue(versionedControllerName, out var versionedControllerDescriptor) ? versionedControllerDescriptor : controllerDescriptor;
            }

            return null;

        }

        private string GetVersionFromAcceptHeaderVersion(HttpRequestMessage request)
        {
            var acceptHeader = request.Headers.Accept;

            foreach (var mime in acceptHeader)
            {
                if (mime.MediaType == "application/json")
                {
                    var version = mime.Parameters.FirstOrDefault(v => v.Name.Equals("version", StringComparison.OrdinalIgnoreCase));
                    return version != null ? version.Value : "1";
                }
            }
            return "1";
        }

        private string GetVersionFromHeader(HttpRequestMessage request)
        {
            const string headerName = "X-Learning-Version";

            if (request.Headers.Contains(headerName))
            {
                var versionHeader = request.Headers.GetValues(headerName).FirstOrDefault();
                if (versionHeader != null)
                {
                    return versionHeader;
                }
            }

            return "1";
        }

        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);

            var version = query["v"];

            return version ?? "1";
        }
    }
}