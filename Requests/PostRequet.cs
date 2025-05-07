using APICeomedAplicacoes.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace APICeomedAplicacoes.Requests
{
    public class PostRequet<T> : BaseRequest<T>
    {
        public PostRequet() : base() { }
        public PostRequet(T? param, HttpRequest? defaultRequest) : base(param, defaultRequest) { this.response = base.response; }
        
        private Response response { get; set; }
        public override Response GetResponse()
        {
            return response;
        }
       
    }
}
