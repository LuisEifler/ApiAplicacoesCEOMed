using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.Chat
{
    public class GetMensagensParam : BaseParamApi
    {
        [SwaggerSchema("Id do Usuario de origem")]
        [RequiredParam]
        public Int64 IdUsuario { get; set; }

        [SwaggerSchema("Id do Usuario de destino")]
        [RequiredParam]
        public Int64 IdUsuarioDestino { get; set; }

        [SwaggerSchema("Paginação")]
        [RequiredParam]
        public int page { get; set; }

    }
}
