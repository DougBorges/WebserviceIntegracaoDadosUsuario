using System;
using System.Web.Http;
using IntegracaoDadosUsuario.Infraestrutura.Interfaces;
using IntegracaoDadosUsuario.Infraestrutura.Repositorios;
using IntegracaoDadosUsuario.Query.Implementacoes;
using IntegracaoDadosUsuario.Query.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace IntegracaoDadosUsuario.WS {
    public class DependencyInjection : IDisposable {
        private static Scope scope;
        public static Container Container;

        public DependencyInjection(Boolean startScope = false) {
            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            if (startScope) {
                scope = AsyncScopedLifestyle.BeginScope(Container);
            }
        }

        public void Dispose() {
            Container?.Dispose();
            scope?.Dispose();
        }

        public T GetInstance<T>() where T : class {
            return Container.GetInstance<T>();
        }

        public static void Configure() {
            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            AddQueries();
            AddRepositories();

            Container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            Container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(Container);
        }

        private static void AddQueries() {
            Container.Register<IDadosUsuarioQuery, DadosUsuarioQuery>(Lifestyle.Scoped);
        }

        private static void AddRepositories() {
            Container.Register<IUsuarioRepository, UsuarioRepository>(Lifestyle.Scoped);
            Container.Register<ILogExecucaoAcaoRepository, LogExecucaoAcaoRepository>(Lifestyle.Scoped);
        }
    }
}