using System;
using IntegracaoDadosUsuario.DTO.Usuarios;

namespace IntegracaoDadosUsuario.Query.Interfaces {
    public interface IDadosUsuarioQuery {
        Usuario RecuperarUsuario(String matricula, String cpf, DateTime dataNascimento);
        Int32 RecuperarTamanhoSenhaPadrao();
    }
}