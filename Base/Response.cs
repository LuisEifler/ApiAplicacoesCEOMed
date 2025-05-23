
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Uteis.Attributes;
using APICeomedAplicacoes.Uteis.Enum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

namespace APICeomedAplicacoes.Base
{
    public class Response
    {
        public Response(object value, bool isSuccess = true)
        {
            this.value = value;
            this.IsSuccess = isSuccess;
        }

        public Response(string message, bool isSuccess = false)
        {
            this.message = message;
            this.IsSuccess = isSuccess;
        }
        
        private object value;

        public Response() { }

        public Response(object value)
        {
            this.value = value;
        }

        public bool IsSuccess { get; set; }

        [ExampleValue("Sucesso.")]
        public string? message { get; set; }

        [ExampleValue("6546")]
        public string? traceId { get; set; }
        
        [ExampleValue(200)]
        public int Code { get; set; } = 200;
        
        [ExampleValue("OK: A solicitação foi bem-sucedida.")]
        public string codeMessage { get { return ((EHttpCode)this.Code).Descricao(); } }

        [SwaggerSchema("Informações adicionais da requisição. (Só retorna quando necessário.)")]
        [ExampleValue(null)]
        public List<string>? additionalInfoMessages { get; set; } = null;

        [ExampleValue(null)]
        public List<ValueError>? errors { get; set; }

        public ResponseValue<T> ToResponseValue<T>()
        {
            return new()
            {
                IsSuccess = this.IsSuccess,
                message = this.message,
                errors = this.errors,
                Code = this.Code,
                Value = default(T)
            };
        }

        public virtual void AddError(string Mensagem,string ResponseMessage = "Algo deu errado.")
        {
            this.IsSuccess = false;
            message = ResponseMessage;
            this.Code = 400;
            if(this.errors == null) this.errors = new();
            this.errors.Add(new(Mensagem));
        }

        public static Response Error(string message = "Algo deu errado.")
        {
            return new() { IsSuccess = false, message = message, Code = 400 };
        }

        public static Response Success()
        {
            return new() { IsSuccess = true, message = "Sucesso.", Code = 200 };
        }
        
    }
}
