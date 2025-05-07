using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.Seguranca;
using APICeomedAplicacoes.Uteis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;
using System.Reflection;

namespace APICeomedAplicacoes.Base
{
    public class BaseRequest<T>
    {

        public BaseRequest() { response = Response.Success(); }
        public BaseRequest(T? param, HttpRequest? defaultRequest = null)
        {
            this.param = param;
            this.httpRequest = defaultRequest;
            response = Response.Success();
            VerifyParam();
        }

        public T? param { get; set; }
        public HttpRequest httpRequest { get; set; }
        public int ambiente { get { return httpRequest == null ? 0 : httpRequest.Host.Host.Contains("localhost") || httpRequest.Path.Value.Contains("v2") ? 1 : 0; } }

        protected Response response { get; set; }

        public virtual Response GetResponse() { return response; }

        public virtual ObjectResult GetResult(int? code = null)
        {
            this.response.Code = code.IsNull() ? this.response.Code : code.Value;
            ObjectResult result = new ObjectResult(this.GetResponse())
            {
                StatusCode = this.response.Code
            };
            this.GravarLog();
            return result;
        }
        public bool VerifyParam()
        {
            VerifyCampos(param);
            return response.IsSuccess;
        }

        public void VerifyCampos<TP>(TP param)
        {
            foreach (var item in param.GetType().GetRuntimeProperties())
            {
                var attribute = (RequiredParam)Attribute.GetCustomAttribute(item, typeof(RequiredParam));

                if (attribute?.Required == true && BaseRequest<TP>.ValidParam(attribute, item.GetValue(param)))
                {
                    response.AddError(attribute.ErrorMessage.Replace("@CAMPO", item.Name), "Parâmetros obrigatórios não foram informados.");
                    this.response.Code = (int)EHttpCode.RequisicaoRuim_400;
                }
            }
        }

        public static bool ValidParam(RequiredParam att, object value)
        {
            bool valid = true;
            switch (att.Type)
            {
                case ETiposParam.String:
                    {
                        return (value == null || value == "");
                    }
                case ETiposParam.Bool:
                    {
                        valid = (value == null);
                        try { Convert.ToBoolean(value); valid = true; } catch { valid = false; }
                        return !valid;
                    }
                case ETiposParam.Int:
                    {
                        valid = (value == null);
                        try { Convert.ToInt32(value); valid = true; } catch { valid = false; }
                        return !valid;
                    }
                case ETiposParam.DateTime:
                    {
                        valid = (value == null || value == "");
                        try { Convert.ToDateTime(value); valid = true; } catch { valid = false; }
                        return !valid;
                    }
                case ETiposParam.Cpf:
                    {
                        valid = (value == null || value == "" && (!Util.ValidarCpf(value.ToString())));
                        return valid;
                    }
                default:
                    {
                        return (value == null);
                    }
            }
        }

        virtual public void GravarLog()
        {
            this.response.traceId = LogAPIAplicacoes.GetLog(this);
        }

    }
}
