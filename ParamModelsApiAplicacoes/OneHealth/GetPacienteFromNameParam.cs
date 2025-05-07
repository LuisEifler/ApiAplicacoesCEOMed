using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    public class GetPacienteFromNameParam : BaseParamApi
    {
        [RequiredParam(ErrorMessage = "O campo partialName é obrigatório.")]
        [SwaggerSchema("Nome parcial do paciente")]
        public string partialName { get; set; }

    }
}
