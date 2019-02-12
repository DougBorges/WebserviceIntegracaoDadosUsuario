using System;

namespace IntegracaoDadosUsuario.DTO.Seguranca {
    public class ResultadoLembrarSenha {
        public Boolean Status { get; set; }
        public String MotivoCritica { get; set; }
        public String Login { get; set; }
        public String Senha { get; set; }
        public String Email { get; set; }
    }
}