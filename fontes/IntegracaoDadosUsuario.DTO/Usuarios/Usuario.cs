using System;

namespace IntegracaoDadosUsuario.DTO.Usuarios {
    public class Usuario {
        public String ID { get; set; }
        public String Nome { get; set; }
        public String Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public String CPF { get; set; }
        public String TelefoneCelular { get; set; }
        public String Email { get; set; }
        public String Endereco { get; set; }
        public String CEP { get; set; }
        public String Bairro { get; set; }
        public String Cidade { get; set; }
        public String Estado { get; set; }
    }
}