using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.Chat
{
    public class GetConversasParam 
    {
        [SwaggerSchema("Id do Usuario")]
        [RequiredParam]
        public Int64 IdUsuario { get; set; }

    }
}
