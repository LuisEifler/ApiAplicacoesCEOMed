using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Uteis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace APICeomedAplicacoes.Requests
{
    public class GetRequest<T,TR> : BaseRequest<T>
    {
        public GetRequest() : base() { }
        public GetRequest(T? param, HttpRequest? defaultRequest = null) : base(param, defaultRequest)
        {
            this.responseValue = base.response.ToResponseValue<TR>();
        }
        public T Param { get { return this.param; } }

        private ResponseValue<TR> responseValue { get; set; }

        public override ResponseValue<TR> GetResponse()
        {
            return responseValue;
        }

        public bool VerifyParam()
        {
            if (this.param == null)
                response.AddError("param nulo.");
            else
                responseValue = responseValue.VerifyCampos(param);
            return responseValue.IsSuccess;
        }

        public override ObjectResult GetResult(int? code = null)
        {
            this.responseValue.Code = code.IsNull() ? this.responseValue.Code : code.Value;

            ObjectResult result = new ObjectResult(this.GetResponse())
            {
                StatusCode = this.responseValue.Code
            };
            base.GetResult();
            return result;
        }
        public override void GravarLog()
        {
            base.GravarLog();
            this.responseValue.traceId = this.response.traceId;
        }

        //public bool VerifyTokens()
        //{
        //    this.responseValue = new ResponseValue(base.VerifyTokens());
        //    return this.responseValue.IsSuccess;
        //}
    }
}
