using APICeomedAplicacoes.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace APICeomedAplicacoes.Requests
{
    public class PostRequest<T> : BaseRequest<T>
    {
        public PostRequest() : base() { }
        public PostRequest(T? param, HttpRequest? defaultRequest) : base(param, defaultRequest)
        {
            httpRequest.EnableBuffering();
            this.response = base.response; }
        
        private Response response { get; set; }
        public override Response GetResponse()
        {
            return response;
        }
       
    }
}
