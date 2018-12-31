using System;
using IntegracaoDadosUsuario.DTO.Usuarios;

namespace IntegracaoDadosUsuario.DTO.Seguranca {
    public class ResultadoAutenticacao {
        public Boolean Status { get; set; }
        public String Token { get; set; }
        public String MotivoCritica { get; set; }
        public Usuario UsuarioLogado { get; set; }
    }
}