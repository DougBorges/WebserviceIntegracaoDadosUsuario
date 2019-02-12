using System;
using System.Configuration;
using Oracle.DataAccess.Client;

namespace IntegracaoDadosUsuario.Query {
    public abstract class QueryBase {
        protected readonly String ConnectionString;

        protected QueryBase() {
            ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public virtual OracleConnection GetConnection() {
            var con = new OracleConnection(ConnectionString);
            con.Open();
            return con;
        }
    }
}