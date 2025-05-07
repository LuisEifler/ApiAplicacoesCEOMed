using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    public class GetPacienteFromId : BaseParamApi
    {
        [RequiredParam(ErrorMessage = "O campo IdPaciente é obrigatório.")]
        [SwaggerSchema("IdPaciente do paciente")]
        public Int64? IdPaciente { get; set; }

    }
}
