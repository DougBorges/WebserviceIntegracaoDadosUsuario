using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using IntegracaoDadosUsuario.WS.Filters;

namespace IntegracaoDadosUsuario.WS {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            var corsConfig = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsConfig);

            config.Filters.Add(new LogActionWebApiFilter());
        }
    }
}