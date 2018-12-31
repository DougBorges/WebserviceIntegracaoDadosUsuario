/* Log de execução */
CREATE TABLE Log_Integracao (
    Cd_Log                        NUMBER(5) NOT NULL CONSTRAINT PK_Log_Integracao PRIMARY KEY,
    De_Acao                       VARCHAR2(100),
    Dt_Inicio                     DATE NOT NULL,
    Dt_Fim                        DATE NOT NULL,
    Cd_Operacao_Realizada_Sucesso NUMBER(1),
    De_Parametro_Entrada          CLOB,
    De_Parametro_Saida            CLOB,
    De_Erro                       CLOB);

CREATE SEQUENCE Sq_Log_Integracao;

/* Informações de login dos usuários */
CREATE TABLE Usuario (
    Cd_Usuario    NUMBER       NOT NULL CONSTRAINT PK_Usuario PRIMARY KEY,
    Nm_Login      VARCHAR2(25) NOT NULL,
    Nm_Cpf        VARCHAR2(11) NOT NULL,
    Dt_Nascimento DATE         NOT NULL,
    Ds_Senha      VARCHAR2(32) NOT NULL);

CREATE SEQUENCE Sq_Usuario;

/* Parâmetros gerais */
INSERT INTO ParametrosSistema (Cd_Parametro, De_Parametro, Vr_Parametro, Dt_Atualizacao)
VALUES ('TAMANHO_NOVA_SENHA', 'Tamanho da nova senha gerada no método lembrar_senha do Webservice de Integração dos Dados dos Usuários', '8', SYSDATE);

/* Busca dados dos usuários com qualquer plano de saúde */
CREATE OR REPLACE VIEW vw_DadosUsuario
AS (SELECT
        B.Cd_Beneficiario AS ID,
        B.Ds_Nome AS Nome,
        B.Cd_Sexo AS Sexo,
        B.Ds_DataNascimento AS DataNascimento,
        B.Nu_Cpf AS CPF,
        B.Nu_TelefoneCelular AS TelefoneCelular,
        B.Ds_Email AS Email,
        TRIM (E.Ds_TipoLogradouro || ' ' || E.Nm_NomeLogradouro || ' ' || E.Nu_Numero || ' ' || E.Ds_Complemento) AS Endereco,
        E.Nu_Cep AS CEP,
        E.Nm_Bairro AS Bairro,
        E.Nm_CidadeMunicipio AS Cidade,
        E.Nm_Estado AS Estado
    FROM
        Beneficiario B
        LEFT JOIN Endereco E ON (E.Cd_Beneficiario = B.Cd_Beneficiario)
  WHERE B.Cd_TipoBeneficiario IN (1, 2)
        AND E.Cd_TipoEndereco IN (1, 2, 3)
        AND B.Id_Enviar = 1);

COMMIT;