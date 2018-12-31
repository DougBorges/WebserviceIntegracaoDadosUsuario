using System;
using System.IO;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using IntegracaoDadosUsuario.Dominio.Entidades;
using IntegracaoDadosUsuario.Infraestrutura.Interfaces;

namespace IntegracaoDadosUsuario.WS.Filters {
    public class LogActionWebApiFilter : ActionFilterAttribute {
        private LogExecucaoAcao log;
        private readonly Boolean gravarLog = ConfigurationManager.GravarLogDeExecucao;

        public override void OnActionExecuting(HttpActionContext actionContext) {
            log = new LogExecucaoAcao {
                DataInicio = DateTime.Now,
                Acao = actionContext.ActionDescriptor.ActionName,
                ParametroEntrada = GetStringContent(actionContext.Request.Content)
            };

            if (String.IsNullOrWhiteSpace(log.ParametroEntrada)) {
                log.ParametroEntrada = actionContext.Request.RequestUri.Query;
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext) {
            log.DataFim = DateTime.Now;

            if (actionExecutedContext.Response != null) {
                log.OperacaoRealizadaSucesso = 1;
                log.ParametroSaida = GetStringContent(actionExecutedContext.Response.Content);
            } else if (actionExecutedContext.Exception != null) {
                log.OperacaoRealizadaSucesso = 0;
                log.DescricaoErro = actionExecutedContext.Exception.Message;
            }

            var repositoryRegistrarLog = DependencyInjection.Container.GetInstance<ILogExecucaoAcaoRepository>();

            if (gravarLog) {
                repositoryRegistrarLog.Salvar(log);
            }
        }

        private String GetStringContent(HttpContent httpContent) {
            var stream = new StreamReader(httpContent.ReadAsStreamAsync().Result);

            stream.BaseStream.Position = 0;
            var content = stream.ReadToEnd();

            return content;
        }
    }
}