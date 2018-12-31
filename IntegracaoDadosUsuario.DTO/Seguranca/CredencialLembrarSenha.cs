using System;

namespace IntegracaoDadosUsuario.DTO.Seguranca {
    public class CredencialLembrarSenha {
        public String Login { get; set; }
        public String CPF { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}