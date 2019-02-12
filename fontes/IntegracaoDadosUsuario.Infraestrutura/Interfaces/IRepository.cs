namespace IntegracaoDadosUsuario.Infraestrutura.Interfaces {
    public interface IRepository<TEntidade> {
        void Salvar(TEntidade entidade);
    }
}