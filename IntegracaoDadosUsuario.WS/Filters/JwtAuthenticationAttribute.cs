using System;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace IntegracaoDadosUsuario.WS.Filters {
    public class JwtAuthenticationAttribute : AuthorizationFilterAttribute {
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
            if (!ConfigurationManager.ValidarToken) {
                return;
            }

            var token = GetTokenFromHeader(actionContext);
            if (String.IsNullOrWhiteSpace(token)) {
                throw new SecurityException("Não foi identificado o token de autenticação na requisição.");
            }

            IPrincipal user = TokenManager.GetPrincipal(token);
            if (user == null) {
                throw new SecurityException("Token de autenticação inválido.");
            }

            Thread.CurrentPrincipal = user;
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