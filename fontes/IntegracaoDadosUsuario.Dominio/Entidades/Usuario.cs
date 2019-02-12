using System;

namespace IntegracaoDadosUsuario.Dominio.Entidades {
    public class Usuario : BaseEntity {
        public String Login { get; set; }
        public String CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public String Senha { get; set; }
    }
}