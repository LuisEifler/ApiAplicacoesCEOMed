using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.Seguranca;
using APICeomedAplicacoes.Uteis;
using APICeomedAplicacoes.Uteis.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;
using System.Reflection;
using System.Text;

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

        public virtual void AddAdditionalMessage(string message)
        {
            if (this.response.additionalInfoMessages == null)
                this.response.additionalInfoMessages = new List<string>();

            this.response.additionalInfoMessages.Add(message);
        }

        public async Task<string> GetRequestBody()
        {
            this.httpRequest.EnableBuffering();

            this.httpRequest.Body.Position = 0;
            using var reader = new StreamReader(this.httpRequest.Body, Encoding.UTF8, leaveOpen: true);
            var jsonString = await reader.ReadToEndAsync();

            this.httpRequest.Body.Position = 0;

            return jsonString;
        }

        
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
            if (att.Type == ETiposParam.Cpf)
            {
                valid = (value == null || value == "" && (!Util.ValidarCpf(value.ToString())));
                return valid;
            }

            switch (value.GetType().Name)
            {
                case "String":
                    return string.IsNullOrWhiteSpace(value as string);

                case "Boolean":
                    try { Convert.ToBoolean(value); return false; }
                    catch { return true; }

                case "Int16":
                    try { Convert.ToInt16(value); return Convert.ToInt16(value) == 0 ? true : false; }
                    catch { return true; }
                case "Int32":
                    try { Convert.ToInt32(value); return Convert.ToInt32(value) == 0 ? true : false; }
                    catch { return true; }
                case "Int64":
                    try { Convert.ToInt64(value); return Convert.ToInt64(value) == 0 ? true : false; }
                    catch { return true; }

                case "DateTime":
                    try { Convert.ToDateTime(value); return false; }
                    catch { return true; }
                default:
                    {
                        return (value == null);
                    }
            }
            
        }

        virtual public void GravarLog()
        {
            this.response.traceId = LogAPIAplicacoes.GravarLog(this.response,this.httpRequest, this.httpRequest.Method is "POST" or "PATCH" ? this.GetRequestBody().Result : null).Result;
        }
    }
}
