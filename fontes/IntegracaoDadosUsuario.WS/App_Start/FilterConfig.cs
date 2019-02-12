using System.Web.Mvc;

namespace IntegracaoDadosUsuario.WS {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}