
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Uteis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public string? message { get; set; }

        public string? traceId { get; set; }

        public int Code { get; set; } = 200;

        public string codeMessage { get { return ((EHttpCode)this.Code).Descricao(); } }

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
