using System;

namespace IntegracaoDadosUsuario.DTO.Usuarios {
    public class RegistroUsuario {
        public String Login { get; set; }
        public String CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public String Senha { get; set; }
    }
}