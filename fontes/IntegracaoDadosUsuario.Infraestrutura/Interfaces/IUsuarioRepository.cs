using System;
using IntegracaoDadosUsuario.Dominio.Entidades;

namespace IntegracaoDadosUsuario.Infraestrutura.Interfaces {
    public interface IUsuarioRepository : IRepository<Usuario> {
        Boolean ExisteComLogin(String login);
        Boolean ExisteComCPF(String cpf);
        Usuario ComLogin(String login);
        new void Salvar(Usuario usuario);
    }
}