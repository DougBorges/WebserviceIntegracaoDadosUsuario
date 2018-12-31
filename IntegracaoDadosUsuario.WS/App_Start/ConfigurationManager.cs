using System;

namespace IntegracaoDadosUsuario.WS {
    public class ConfigurationManager {
        private static T ObterConfiguracao<T>(String chave) {
            try {
                Object valor = System.Configuration.ConfigurationManager.AppSettings[chave];
                if (valor == null) {
                    return default(T);
                }

                var tipo = typeof(T);
                return (T) Convert.ChangeType(valor, tipo);
            } catch (Exception) {
                return default(T);
            }
        }

        public static Boolean ValidarToken {
            get { return ObterConfiguracao<Boolean>("Token.Validar"); }
        }

        public static Int32 TokenMinutosExpirar {
            get { return ObterConfiguracao<Int32>("Token.MinutosParaExpirar"); }
        }

        public static Boolean ValidarCliente {
            get { return ObterConfiguracao<Boolean>("Client.Validar"); }
        }

        public static String UsuarioCliente {
            get { return ObterConfiguracao<String>("Client.Usuario"); }
        }

        public static String SenhaCliente {
            get { return ObterConfiguracao<String>("Client.Senha"); }
        }

        public static Boolean GravarLogDeExecucao {
            get {
                var gravar = ObterConfiguracao<String>("LogExecucao.Gravar");
                if (String.IsNullOrWhiteSpace(gravar)) {
                    return true;
                }

                return gravar.ToLower().Equals("true") || gravar.Equals("1");
            }
        }
    }
}