using APICeomedAplicacoes.Uteis.Enum;
using System.ComponentModel.DataAnnotations;

namespace APICeomedAplicacoes.Uteis
{
    public class RequiredParam : Attribute
    {
        public bool? Required { get; set; }
        public ETiposParam? Type { get; set; }
        public String? ErrorMessage { get; set; }

        public RequiredParam(string ErrorMessage = "O campo @CAMPO é inválido.", ETiposParam type = ETiposParam.String) 
        {
            this.Required = true;
            this.Type = type;
            this.ErrorMessage = ErrorMessage;
        }
    }
    public class ErrorMessage : Attribute
    {
        public String? message { get; set; }
        public ErrorMessage(string msg)
        {
            message = msg;
        }
    }
    public class SqlParametrer : Attribute
    {
        public string? ParamName { get; set; }
        public SqlParametrer(string param)
        {
            ParamName = param;
        }
    }
}
