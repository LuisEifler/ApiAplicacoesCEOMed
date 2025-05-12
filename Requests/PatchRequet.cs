using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.ParamModelsApiAplicacoes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace APICeomedAplicacoes.Requests
{
    public class PatchRequet<T> : BaseRequest<T>
    {
        public PatchRequet() : base() { }
        public PatchRequet(T param, HttpRequest? defaultRequest) : base(param, defaultRequest) { this.response = base.response;}
        
        private Response response { get; set; }

        public override Response GetResponse()
        {
            return response;
        }
        
    }
}
