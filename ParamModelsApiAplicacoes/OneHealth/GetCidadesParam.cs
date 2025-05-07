using Swashbuckle.AspNetCore.Annotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    public class GetCidadesParam : BaseParamApi
    {
        [SwaggerSchema("Id do estado de onde deseja buscar as cidades.")]
        public Int64? idEstado { get; set; }

    }
}
