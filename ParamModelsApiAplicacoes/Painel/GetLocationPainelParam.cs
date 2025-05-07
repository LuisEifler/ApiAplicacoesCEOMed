using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.Painel
{
    public class GetLocationPainelParam : BaseParamApi
    {
        [RequiredParam(ErrorMessage = "O campo IdEmpresa é obrigatório.")]
        [SwaggerSchema("Id da unidade onde o painel sera utilizado")]
        public Int64 IdEmpresa { get; set; }

    }
}
