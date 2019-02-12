using System;
using System.Data;
using System.Text;
using Dapper;
using IntegracaoDadosUsuario.Dominio.Entidades;
using IntegracaoDadosUsuario.Infraestrutura.Interfaces;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace IntegracaoDadosUsuario.Infraestrutura.Repositorios {
    public class LogExecucaoAcaoRepository : RepositoryBase, ILogExecucaoAcaoRepository {
        public void Salvar(LogExecucaoAcao log) {
            using (var con = GetConnection()) {
                log.Id = con.ExecuteScalar<Int32>("Select Sq_Log_Integracao.NextVal From Dual");

                var query = " Insert Into Log_Integracao (Cd_Log,                         De_Acao,               Dt_Inicio,           Dt_Fim, " +
                            "                             Cd_Operacao_Realizada_Sucesso,  De_Parametro_Entrada,  De_Parametro_Saida,  De_Erro) " +
                            "                     Values (:Id,                            :Acao,                 :DataInicio,         :DataFim, " +
                            "                             :OperacaoRealizadaSucesso,      :ParametroEntrada,     :ParametroSaida,     :DescricaoErro) ";

                var parametros = new DynamicParameters();
                parametros.Add(nameof(log.Id), log.Id);
                parametros.Add(nameof(log.Acao), log.Acao);
                parametros.Add(nameof(log.DataInicio), log.DataInicio);
                parametros.Add(nameof(log.DataFim), log.DataFim);
                parametros.Add(nameof(log.OperacaoRealizadaSucesso), log.OperacaoRealizadaSucesso);
                parametros.Add(nameof(log.ParametroEntrada), new OracleClobParameter(log.ParametroEntrada));
                parametros.Add(nameof(log.ParametroSaida), new OracleClobParameter(log.ParametroSaida));
                parametros.Add(nameof(log.DescricaoErro), new OracleClobParameter(log.DescricaoErro));

                con.Execute(query, parametros);

                con.Close();
            }
        }
    }

    internal class OracleClobParameter : SqlMapper.ICustomQueryParameter {
        private readonly String valorDoParametro;

        public OracleClobParameter(String valorDoParametro) {
            this.valorDoParametro = !String.IsNullOrEmpty(valorDoParametro) ? valorDoParametro : String.Empty;
        }

        public void AddParameter(IDbCommand command, String nomeDoParametro) {
            var clob = new OracleClob(command.Connection as OracleConnection);

            var bytes = Encoding.Unicode.GetBytes(valorDoParametro);
            var tamanhoParametro = Encoding.Unicode.GetByteCount(valorDoParametro);

            var posicaoAtual = 0;
            var tamanhoChunk = 1024; // Oracle não suporta chunks muito grandes

            while (posicaoAtual < tamanhoParametro) {
                tamanhoChunk = tamanhoChunk > tamanhoParametro - posicaoAtual ? tamanhoParametro - posicaoAtual : tamanhoChunk;
                clob.Write(bytes, posicaoAtual, tamanhoChunk);
                posicaoAtual += tamanhoChunk;
            }

            var parametro = new OracleParameter(nomeDoParametro, OracleDbType.Clob) { Value = clob };

            command.Parameters.Add(parametro);
        }
    }
}