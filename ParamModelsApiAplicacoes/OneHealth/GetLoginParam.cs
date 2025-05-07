using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    public class GetLoginParam
    {
        [SwaggerSchema("Login/Email do usuario")]
        [RequiredParam("O campo login é obrigatório.")]
        public string? login { get; set; }

        [SwaggerSchema("Senha do usuario")] 
        [RequiredParam("O campo senha é obrigatório.")]
        public string? senha { get; set; }

        [SwaggerSchema("Tipo Acesso de usuario (0 = Web_Chat | 1 = Web | 2 = Chat | 3 = Painel), PADRÃO = \"0,1\"")]
        //[RequiredParam("O campo senha é obrigatório.")]
        public string? tipoAcesso { get; set; } = "0,1";
    }
}
