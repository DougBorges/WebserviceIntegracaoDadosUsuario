using System;

namespace IntegracaoDadosUsuario.DTO.Usuarios {
    public class ResultadoRegistroUsuario {
        public Boolean Status { get; set; }
        public String MotivoCritica { get; set; }
        public Usuario UsuarioLogado { get; set; }
    }
}