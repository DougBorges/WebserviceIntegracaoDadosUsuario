using System;
using Dapper;
using IntegracaoDadosUsuario.Dominio.Entidades;
using IntegracaoDadosUsuario.Infraestrutura.Interfaces;

namespace IntegracaoDadosUsuario.Infraestrutura.Repositorios {
    public class UsuarioRepository : RepositoryBase, IUsuarioRepository {
        public Boolean ExisteComLogin(String login) {
            using (var conn = GetConnection()) {
                var query = " Select 1 " +
                            "   From Usuario " +
                            "  Where Nm_Login = :Login ";

                var parametros = new { Login = login };
                var resultado = conn.ExecuteScalar<Boolean>(query, parametros);

                conn.Close();
                return resultado;
            }
        }

        public Boolean ExisteComCPF(String cpf) {
            using (var conn = GetConnection()) {
                var query = " Select 1 " +
                            "   From Usuario " +
                            "  Where Nm_Cpf = :CPF ";

                var parametros = new { CPF = cpf };
                var resultado = conn.ExecuteScalar<Boolean>(query, parametros);

                conn.Close();
                return resultado;
            }
        }

        public Usuario ComLogin(String login) {
            using (var conn = GetConnection()) {
                var query = " Select Cd_Usuario Id, " +
                            "        Nm_Login Login, " +
                            "        Nm_Cpf CPF, " +
                            "        Dt_Nascimento DataNascimento, " +
                            "        Ds_Senha Senha " +
                            "   From Usuario " +
                            "  Where Nm_Login = :Login ";

                var parametros = new { Login = login };
                var resultado = conn.QueryFirstOrDefault<Usuario>(query, parametros);

                conn.Close();
                return resultado;
            }
        }

        public void Salvar(Usuario usuario) {
            if (usuario.Id.HasValue) {
                Atualizar(usuario);
                return;
            }

            using (var con = GetConnection()) {
                usuario.Id = con.ExecuteScalar<Int32>("Select Sq_Usuario.NextVal From Dual");

                var query = " Insert Into Usuario (Cd_Usuario,  Nm_Login,  Nm_Cpf,  Dt_Nascimento,    Ds_Senha) " +
                            "              Values (:Id,         :Login,    :CPF,    :DataNascimento,  :Senha) ";

                con.Execute(query, usuario);
                con.Close();
            }
        }

        private void Atualizar(Usuario usuario) {
            using (var con = GetConnection()) {
                var query = " Update Usuario " +
                            "    Set Nm_Login       = :Login, " +
                            "        Nm_Cpf         = :CPF, " +
                            "        Dt_Nascimento  = :DataNascimento, " +
                            "        Ds_Senha       = :Senha " +
                            "  Where Cd_Usuario     = :Id ";

                con.Execute(query, usuario);
                con.Close();
            }
        }
    }
}