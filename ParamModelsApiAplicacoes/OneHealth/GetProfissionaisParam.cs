using Swashbuckle.AspNetCore.Annotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    public class GetProfissionaisParam : BaseParamApi
    {
        [SwaggerSchema("Tipo do medico (0 = INTERNO | 1 = EXTERNO | PADRÃO = 0) Opcional")]
        public int? tipoMedico { get; set; }

    }
}
