using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using IntegracaoDadosUsuario.DTO.Seguranca;
using IntegracaoDadosUsuario.DTO.Usuarios;
using IntegracaoDadosUsuario.Infraestrutura.Interfaces;
using IntegracaoDadosUsuario.Query.Interfaces;
using IntegracaoDadosUsuario.WS.Filters;

namespace IntegracaoDadosUsuario.WS.Controllers {
    [RoutePrefix("webservice/integracao_dados_usuario")]
    public class IntegracaoController : ApiController {
        private readonly IDadosUsuarioQuery dadosUsuarioQuery;
        private readonly IUsuarioRepository usuarios;

        public IntegracaoController(IDadosUsuarioQuery dadosUsuarioQuery, IUsuarioRepository usuarios) {
            this.dadosUsuarioQuery = dadosUsuarioQuery;
            this.usuarios = usuarios;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("novo_usuario")]
        public ResultadoRegistroUsuario Registrar([FromBody] RegistroUsuario usuario) {
            try {
                GerarErroSeNaoInformado(usuario.Senha, "senha");
                GerarErroSeNaoInformado(usuario.CPF, "CPF");
                GerarErroSeNaoInformado(usuario.Login, "login");
                GerarErroSeNaoInformado(usuario.DataNascimento, "data de nascimento");

                if (usuarios.ComLogin(usuario.Login)?.Id != null) {
                    throw new Exception("Já existe um usuário cadastrado com esse login");
                }

                if (usuarios.ComCPF(usuario.CPF)?.Id != null) {
                    throw new Exception("Já existe um usuário cadastrado com esse CPF");
                }

                var dadosUsuario = dadosUsuarioQuery.RecuperarUsuario(usuario.Login, usuario.CPF, usuario.DataNascimento);
                if (dadosUsuario == null) {
                    throw new Exception("Não foram encontrados dados deste usuário no sistema");
                }

                var senhaCriptografada = CalcularHash(usuario.Senha);

                var novoUsuario = new Dominio.Entidades.Usuario {
                    Login = usuario.Login,
                    CPF = usuario.CPF,
                    DataNascimento = usuario.DataNascimento,
                    Senha = senhaCriptografada
                };

                try {
                    usuarios.Salvar(novoUsuario);
                } catch (Exception e) {
                    throw new Exception($"Não foi possível criar um novo usuário com os dados enviados: {e.Message}");
                }

                return new ResultadoRegistroUsuario {
                    Status = true,
                    MotivoCritica = String.Empty,
                    UsuarioLogado = dadosUsuario
                };
            } catch (Exception e) {
                return new ResultadoRegistroUsuario { Status = false, MotivoCritica = $"Ocorreu um erro ao criar um novo usuário: {e.Message}" };
            }
        }

        [HttpPost]
        [BasicAuthentication]
        [Route("login")]
        public ResultadoAutenticacao Autenticar([FromBody] CredencialAutenticacao credencial) {
            try {
                GerarErroSeNaoInformado(credencial.Login, "login");
                GerarErroSeNaoInformado(credencial.Senha, "senha");

                var usuarioExistente = usuarios.ComLogin(credencial.Login);
                if (usuarioExistente?.Id == null) {
                    throw new Exception("O usuário não está cadastrado");
                }

                var senhaCriptografada = CalcularHash(credencial.Senha);
                if (!usuarioExistente.Senha.Equals(senhaCriptografada)) {
                    throw new Exception("A senha informada está incorreta");
                }

                var dadosUsuario = dadosUsuarioQuery.RecuperarUsuario(usuarioExistente.Login, usuarioExistente.CPF, usuarioExistente.DataNascimento);
                if (dadosUsuario == null) {
                    throw new Exception("Não foram encontrados dados deste usuário no sistema");
                }

                return new ResultadoAutenticacao {
                    Status = true,
                    Token = TokenManager.GenerateToken(credencial.Login),
                    MotivoCritica = String.Empty,
                    UsuarioLogado = dadosUsuario
                };
            } catch (Exception e) {
                return new ResultadoAutenticacao { Status = false, MotivoCritica = $"Ocorreu um erro ao fazer login do usuário: {e.Message}" };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("lembrar_senha")]
        public ResultadoLembrarSenha LembrarSenha([FromBody] CredencialLembrarSenha credencial) {
            try {
                GerarErroSeNaoInformado(credencial.Login, "login");
                GerarErroSeNaoInformado(credencial.CPF, "CPF");
                GerarErroSeNaoInformado(credencial.DataNascimento, "data de nascimento");

                var usuarioExistente = usuarios.ComLogin(credencial.Login);
                if (usuarioExistente?.Id == null) {
                    throw new Exception("O usuário não está cadastrado");
                }

                if (!usuarioExistente.CPF.Equals(credencial.CPF)) {
                    throw new Exception("O CPF informado está incorreto");
                }

                if (!usuarioExistente.DataNascimento.Equals(credencial.DataNascimento)) {
                    throw new Exception("A data de nascimento informada está incorreta");
                }

                var dadosUsuario = dadosUsuarioQuery.RecuperarUsuario(usuarioExistente.Login, usuarioExistente.CPF, usuarioExistente.DataNascimento);
                if (dadosUsuario == null) {
                    throw new Exception("Não foram encontrados dados deste usuário no sistema");
                }

                var caracteresLetraSenha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var caracteresNumeroSenha = "0123456789";
                var random = new Random();
                var tamanhoDaSenha = dadosUsuarioQuery.RecuperarTamanhoSenhaPadrao();
                var novaSenha = String.Empty;

                if (tamanhoDaSenha % 2 != 0) {
                    novaSenha += caracteresLetraSenha[random.Next(caracteresLetraSenha.Length)];
                }

                while (novaSenha.Length < tamanhoDaSenha) {
                    novaSenha += caracteresLetraSenha[random.Next(caracteresLetraSenha.Length)];
                    novaSenha += caracteresNumeroSenha[random.Next(caracteresNumeroSenha.Length)];
                }

                var novaSenhaCriptografada = CalcularHash(novaSenha);
                usuarioExistente.Senha = novaSenhaCriptografada;

                try {
                    usuarios.Salvar(usuarioExistente);
                } catch (Exception e) {
                    throw new Exception($"A senha do usuário não pôde ser alterada: {e.Message}");
                }

                return new ResultadoLembrarSenha {
                    Status = true,
                    Email = dadosUsuario.Email,
                    Login = credencial.Login,
                    Senha = novaSenha
                };
            } catch (Exception e) {
                return new ResultadoLembrarSenha { Status = false, MotivoCritica = $"Ocorreu um erro ao gerar nova senha: {e.Message}" };
            }
        }

        [HttpPost]
        [JwtAuthentication]
        [Route("trocar_senha")]
        public ResultadoTrocarSenha TrocarSenha([FromBody] CredencialTrocarSenha credencial) {
            try {
                GerarErroSeNaoInformado(credencial.Login, "login");
                GerarErroSeNaoInformado(credencial.SenhaAtual, "senha atual");
                GerarErroSeNaoInformado(credencial.NovaSenha, "nova senha");

                var usuarioExistente = usuarios.ComLogin(credencial.Login);
                if (usuarioExistente?.Id == null) {
                    throw new Exception("O usuário não está cadastrado");
                }

                var senhaAtualCriptografada = CalcularHash(credencial.SenhaAtual);
                if (!usuarioExistente.Senha.Equals(senhaAtualCriptografada)) {
                    throw new Exception("A senha atual informada está incorreta");
                }

                var novaSenhaCriptografada = CalcularHash(credencial.NovaSenha);
                usuarioExistente.Senha = novaSenhaCriptografada;

                try {
                    usuarios.Salvar(usuarioExistente);
                } catch (Exception e) {
                    throw new Exception($"A senha do usuário não pôde ser alterada: {e.Message}");
                }

                return new ResultadoTrocarSenha { Status = true };
            } catch (Exception e) {
                return new ResultadoTrocarSenha { Status = false, MotivoCritica = $"Ocorreu um erro ao alterar a senha: {e.Message}" };
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("validar_token")]
        public ResultadoValidarToken ValidarToken(String token) {
            var tokenValido = TokenManager.TokenValid(token);
            return new ResultadoValidarToken { TokenValido = tokenValido };
        }

        private void GerarErroSeNaoInformado(Object valor, String nomeParametro) {
            if (valor.Equals(default(Object))) {
                throw new Exception($"Campo \"{nomeParametro}\" não informado");
            }
        }

        private String CalcularHash(String valor) {
            var bytesSenhaCriptografada = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(valor));
            var stringSenhaCriptografada = new StringBuilder();

            foreach (var byteSenha in bytesSenhaCriptografada) {
                stringSenhaCriptografada.Append(byteSenha.ToString("x2"));
            }

            return stringSenhaCriptografada.ToString();
        }
    }
}