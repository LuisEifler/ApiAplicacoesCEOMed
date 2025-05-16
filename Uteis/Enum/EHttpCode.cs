using System.ComponentModel;

namespace APICeomedAplicacoes.Uteis.Enum
{
    public enum EHttpCode
    {
        // 1xx Informativo
        [Description("Continue: O servidor recebeu a solicitação e o cliente pode continuar.")]
        Continue_100 = 100,

        [Description("Switching Protocols: O servidor concorda em mudar o protocolo.")]
        TrocarProtocolos_101 = 101,

        [Description("Processing: O servidor está processando a solicitação.")]
        Processando_102 = 102,

        // 2xx Sucesso
        [Description("OK: A solicitação foi bem-sucedida.")]
        OK_200 = 200,

        [Description("Created: O recurso foi criado com sucesso.")]
        Criado_201 = 201,

        [Description("Accepted: A solicitação foi aceita para processamento.")]
        Aceito_202 = 202,

        [Description("Non-Authoritative Information: A resposta é de uma fonte não autoritativa.")]
        InformacaoNaoAutoritativa_203 = 203,

        [Description("No Content: A solicitação foi bem-sucedida, mas não há conteúdo para retornar.")]
        SemConteudo_204 = 204,

        [Description("Reset Content: A solicitação foi bem-sucedida, redefina o conteúdo.")]
        RedefinirConteudo_205 = 205,

        [Description("Partial Content: A resposta contém apenas parte do recurso.")]
        ConteudoParcial_206 = 206,

        [Description("Multi-Status: A resposta fornece informações sobre múltiplos recursos.")]
        MultiStatus_207 = 207,

        [Description("Already Reported: O recurso já foi reportado.")]
        JaReportado_208 = 208,

        [Description("IM Used: O servidor usou uma extensão para retornar a resposta.")]
        IMUsado_226 = 226,

        // 3xx Redirecionamento
        [Description("Multiple Choices: Existem múltiplas opções para o recurso.")]
        MultiplasEscolhas_300 = 300,

        [Description("Moved Permanently: O recurso foi movido permanentemente.")]
        MovidoPermanentemente_301 = 301,

        [Description("Found: O recurso foi encontrado em outro local.")]
        Encontrado_302 = 302,

        [Description("See Other: Veja outro recurso.")]
        VerOutro_303 = 303,

        [Description("Not Modified: O recurso não foi modificado.")]
        NaoModificado_304 = 304,

        [Description("Use Proxy: Use um proxy para acessar o recurso.")]
        UsarProxy_305 = 305,

        [Description("Temporary Redirect: Redirecionamento temporário.")]
        RedirecionamentoTemporario_307 = 307,

        [Description("Permanent Redirect: Redirecionamento permanente.")]
        RedirecionamentoPermanente_308 = 308,

        // 4xx Erro do Cliente
        [Description("Bad Request: A solicitação é inválida.")]
        RequisicaoRuim_400 = 400,

        [Description("Unauthorized: Autenticação necessária.")]
        NaoAutorizado_401 = 401,

        [Description("Payment Required: Pagamento necessário.")]
        PagamentoNecessario_402 = 402,

        [Description("Forbidden: Acesso proibido.")]
        Proibido_403 = 403,

        [Description("Not Found: Recurso não encontrado.")]
        NaoEncontrado_404 = 404,

        [Description("Method Not Allowed: Método não permitido.")]
        MetodoNaoPermitido_405 = 405,

        [Description("Not Acceptable: Recurso não aceitável.")]
        NaoAceitavel_406 = 406,

        [Description("Proxy Authentication Required: Autenticação do proxy necessária.")]
        AutenticacaoProxyNecessaria_407 = 407,

        [Description("Request Timeout: Tempo de solicitação esgotado.")]
        TempoEsgotado_408 = 408,

        [Description("Conflict: Conflito na solicitação.")]
        Conflito_409 = 409,

        [Description("Gone: Recurso não está mais disponível.")]
        RecursoIndisponivel_410 = 410,

        [Description("Length Required: Comprimento necessário.")]
        ComprimentoNecessario_411 = 411,

        [Description("Precondition Failed: Pré-condição falhou.")]
        PreCondicaoFalhou_412 = 412,

        [Description("Payload Too Large: Carga útil muito grande.")]
        CargaUtilMuitoGrande_413 = 413,

        [Description("URI Too Long: URI muito longa.")]
        URIMuitoLonga_414 = 414,

        [Description("Unsupported Media Type: Tipo de mídia não suportado.")]
        TipoMidiaNaoSuportado_415 = 415,

        [Description("Range Not Satisfiable: Intervalo não satisfatório.")]
        IntervaloNaoSatisfatorio_416 = 416,

        [Description("Expectation Failed: Expectativa falhou.")]
        ExpectativaFalhou_417 = 417,

        [Description("I'm a teapot: Sou um bule de chá.")]
        SouUmBuleDeCha_418 = 418,

        [Description("Misdirected Request: Solicitação mal direcionada.")]
        SolicitacaoMalDirecionada_421 = 421,

        [Description("Unprocessable Entity: Entidade não processável.")]
        EntidadeNaoProcessavel_422 = 422,

        [Description("Locked: Recurso bloqueado.")]
        RecursoBloqueado_423 = 423,

        [Description("Failed Dependency: Dependência falhou.")]
        DependenciaFalhou_424 = 424,

        [Description("Upgrade Required: Atualização necessária.")]
        AtualizacaoNecessaria_426 = 426,

        [Description("Precondition Required: Pré-condição necessária.")]
        PreCondicaoNecessaria_428 = 428,

        [Description("Too Many Requests: Muitas solicitações.")]
        MuitasSolicitacoes_429 = 429,

        [Description("Request Header Fields Too Large: Campos de cabeçalho muito grandes.")]
        CamposCabecalhoMuitoGrandes_431 = 431,

        [Description("Unavailable For Legal Reasons: Indisponível por razões legais.")]
        IndisponivelPorRazoesLegais_451 = 451,

        // 5xx Erro do Servidor
        [Description("Internal Server Error: Erro interno do servidor.")]
        ErroInternoServidor_500 = 500,

        [Description("Not Implemented: Não implementado.")]
        NaoImplementado_501 = 501,

        [Description("Bad Gateway: Gateway inválido.")]
        GatewayInvalido_502 = 502,

        [Description("Service Unavailable: Serviço indisponível.")]
        ServicoIndisponivel_503 = 503,

        [Description("Gateway Timeout: Tempo de gateway esgotado.")]
        TempoEsgotadoGateway_504 = 504,

        [Description("HTTP Version Not Supported: Versão HTTP não suportada.")]
        VersaoHTTPNaoSuportada_505 = 505,

        [Description("Variant Also Negotiates: A escolha de representação é configurada incorretamente.")]
        VariacaoTambemNegocia_506 = 506,

        [Description("Insufficient Storage: O servidor não pode armazenar a representação necessária.")]
        ArmazenamentoInsuficiente_507 = 507,

        [Description("Loop Detected: Um loop infinito foi detectado durante o processamento da solicitação.")]
        LoopDetectado_508 = 508,

        [Description("Not Extended: Extensões adicionais à solicitação são necessárias.")]
        NaoEstendido_510 = 510,

        [Description("Network Authentication Required: A autenticação de rede é necessária.")]
        AutenticacaoRedeNecessaria_511 = 511,

        [Description("Network Connect Timeout Error: Erro de tempo esgotado ao conectar à rede.")]
        ErroTimeoutConexaoRede_599 = 599
    }
}
