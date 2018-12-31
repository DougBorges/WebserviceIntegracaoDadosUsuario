using System;
using System.Web.Http;
using IntegracaoDadosUsuario.WS;
using Swashbuckle.Application;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace IntegracaoDadosUsuario.WS {
    public class SwaggerConfig {
        public static void Register() {
            GlobalConfiguration.Configuration
                               .EnableSwagger(c => {
                                   c.SingleApiVersion("v1", "Webservice de Integração dos Dados dos Usuários");
                                   c.IncludeXmlComments(GetXmlCommentsPath());
                               })
                               .EnableSwaggerUi(c => { });
        }

        protected static String GetXmlCommentsPath() {
            return $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\SwaggerDoc.xml";
        }
    }
}