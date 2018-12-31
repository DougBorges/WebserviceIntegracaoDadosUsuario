using System;
using System.Configuration;
using Oracle.DataAccess.Client;

namespace IntegracaoDadosUsuario.Infraestrutura.Repositorios {
    public abstract class RepositoryBase {
        protected readonly String ConnectionString;

        protected RepositoryBase() {
            ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        protected OracleConnection GetConnection() {
            var conn = new OracleConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}