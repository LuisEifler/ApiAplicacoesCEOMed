using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    public class GetCidsFromParcialNameParam : BaseParamApi
    {
        //[RequiredParam(ErrorMessage = "O campo nomeCid é obrigatório.")]
        [SwaggerSchema("Nome parcial do CID")]
        public string? CidParcial { get; set; } = "";
       
    }
}
