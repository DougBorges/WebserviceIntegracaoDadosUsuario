using IntegracaoDadosUsuario.Dominio.Entidades;

namespace IntegracaoDadosUsuario.Infraestrutura.Interfaces {
    public interface ILogExecucaoAcaoRepository : IRepository<LogExecucaoAcao> {
        new void Salvar(LogExecucaoAcao entidade);
    }
}