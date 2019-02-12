using System;
using IntegracaoDadosUsuario.Dominio.Entidades;

namespace IntegracaoDadosUsuario.Infraestrutura.Interfaces {
    public interface IUsuarioRepository : IRepository<Usuario> {
        Usuario ComLogin(String login);
        Usuario ComCPF(String cpf);
        new void Salvar(Usuario usuario);
    }
}