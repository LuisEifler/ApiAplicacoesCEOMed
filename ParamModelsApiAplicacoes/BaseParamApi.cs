using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes
{
    public class BaseParamApi
    {
        [RequiredParam(ErrorMessage = "O campo IdClinica é obrigatório.")]
        [SwaggerSchema("Id da Clinica encontrado no login do usuario")]
        public Int64? IdClinica { get; set; }

    }
}
