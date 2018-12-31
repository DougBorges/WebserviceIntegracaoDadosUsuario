using System;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace IntegracaoDadosUsuario.WS.Filters {
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute {
        public override void OnAuthorization(HttpActionContext actionContext) {
            try {
                Validate(actionContext);
            } catch (Exception ex) {
                SetAuthenticationError(actionContext, ex.Message);
                return;
            }

            base.OnAuthorization(actionContext);
        }

        protected void Validate(HttpActionContext actionContext) {
            if (!ConfigurationManager.ValidarCliente) {
                return;
            }

            var requestToken = GetTokenFromHeader(actionContext);
            if (String.IsNullOrWhiteSpace(requestToken)) {
                throw new SecurityException("Não foi identificado o token de autenticação na requisição.");
            }

            String credentials;
            try {
                credentials = TextHelper.Base64Decode(requestToken);
            } catch {
                throw new SecurityException("Token de autenticação inválido.");
            }

            var separator = credentials.IndexOf(':');
            var user = credentials.Substring(0, separator);
            var password = credentials.Substring(separator + 1);

            if (user != ConfigurationManager.UsuarioCliente || password != ConfigurationManager.SenhaCliente) {
                throw new SecurityException("O usuário e(ou) senha contidos no token de autenticação é(são) inválido(s).");
            }
        }

        protected String GetTokenFromHeader(HttpActionContext actionContext) {
            String requestToken = null;

            var authRequest = actionContext.Request.Headers.Authorization;
            if (authRequest != null) {
                requestToken = authRequest.Parameter ?? authRequest.ToString();
            }

            return requestToken;
        }

        private static void SetAuthenticationError(HttpActionContext filterContext, String error) {
            var response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { Code = 401, Message = error });
            filterContext.Response = response;
        }
    }
}