using System;

namespace IntegracaoDadosUsuario.DTO.Seguranca {
    public class CredencialTrocarSenha {
        public String Login { get; set; }
        public String SenhaAtual { get; set; }
        public String NovaSenha { get; set; }
    }
}