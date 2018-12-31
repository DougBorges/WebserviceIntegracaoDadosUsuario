using System;
using Dapper;
using IntegracaoDadosUsuario.DTO.Usuarios;
using IntegracaoDadosUsuario.Query.Interfaces;

namespace IntegracaoDadosUsuario.Query.Implementacoes {
    public class DadosUsuarioQuery : QueryBase, IDadosUsuarioQuery {
        public Usuario RecuperarUsuario(String matricula, String cpf, DateTime dataNascimento) {
            using (var conn = GetConnection()) {
                var query = " Select ID, " +
                            "        Nome, " +
                            "        Sexo, " +
                            "        DataNascimento, " +
                            "        CPF, " +
                            "        TelefoneCelular, " +
                            "        Email, " +
                            "        Endereco, " +
                            "        CEP, " +
                            "        Bairro, " +
                            "        Cidade, " +
                            "        Estado " +
                            "   From vw_DadosUsuario " +
                            "  Where CPF = :CPF " +
                            "        and DataNascimento = :DataNascimento " +
                            "        and Matricula = :Matricula ";

                var parametros = new { CPF = cpf, DataNascimento = dataNascimento.Date, Matricula = matricula };
                var resultado = conn.QueryFirstOrDefault<Usuario>(query, parametros);

                conn.Close();
                return resultado;
            }
        }

        public Int32 RecuperarTamanhoSenhaPadrao() {
            using (var conn = GetConnection()) {
                var query = " Select Vr_Parametro " +
                            "   From ParametroSistema " +
                            "  Where Cd_Parametro = :Parametro ";

                var parametros = new { Parametro = "TAMANHO_NOVA_SENHA" };
                var resultado = conn.QueryFirstOrDefault<Int32?>(query, parametros);

                conn.Close();

                return resultado ?? 8;
            }
        }
    }
}