using System;

namespace IntegracaoDadosUsuario.Dominio.Entidades {
    public class LogExecucaoAcao : BaseEntity {
        public String Acao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public Int32 OperacaoRealizadaSucesso { get; set; }
        public String ParametroEntrada { get; set; }
        public String ParametroSaida { get; set; }
        public String DescricaoErro { get; set; }
    }
}