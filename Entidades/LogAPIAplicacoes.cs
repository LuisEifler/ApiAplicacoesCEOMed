using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Base;
using Newtonsoft.Json;
using APICeomedAplicacoes.ParamModelsApiAplicacoes;
using System.Collections;
using System.Data;
using APICeomedAplicacoes.Seguranca;
using System.Text;

namespace APICeomedAplicacoes.Entidades
{
    public class LogAPIAplicacoes : TableBase
    {
        public Int64? IdClinica{ get; set; }
        public Int64? IdAplicacao { get; set; }
        public DateTime Data { get; set; }
        public string RequestPath { get; set; }
        public String Params { get; set; }
        public int RequestStatus { get; set; }
        public String Response { get; set; }
        public String Message{ get; set; }
        public int Ambiente { get; set; }
        public String RequestBody { get; set; }
        
        protected virtual HttpRequest httpRequest { get; set; }

        public static async Task<string> GravarLog(Response response,HttpRequest httpRequest,string body = null)
        {
            DbHelper.UseDbPrincipal();
            string mensagem = "";

            if (!response.IsSuccess)
                foreach (var item in response.errors) mensagem += mensagem == "" ? item.message : ", " + item.message;
            else
                mensagem = response.message;

            string ClinicaId = httpRequest.Query.Where(x => x.Key == "IdClinica").FirstOrDefault().Value;


            string token = httpRequest.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Int64? IdAplicacao = DbHelper.Select<ApiAccessTokens>(x => x.ApiToken == token).FirstOrDefault()?.Id;

            Int64 idClinica = 0;

            Int64.TryParse(ClinicaId, out idClinica);
           
            LogAPIAplicacoes log = new LogAPIAplicacoes()
            {
                IdAplicacao = IdAplicacao,
                IdClinica = idClinica == 0 ? null : idClinica,
                RequestStatus = 0,
                Response = JsonConvert.SerializeObject(response),
                Message = mensagem,
                Data = DateTime.Now,
                Ambiente = httpRequest == null ? 0 : httpRequest.Host.Host.Contains("localhost") || httpRequest.Path.Value.Contains("v2") ? 1 : 0,
                httpRequest = httpRequest,
                RequestPath = httpRequest.Scheme + "://" + httpRequest.Host.Value + httpRequest.Path.Value,
                Params = httpRequest?.QueryString.Value?.ToString(),
                RequestBody = body
            };

            return log.Insert().ToString();

        }

        public override Int64 Insert()
        {
            (string, Hashtable) ObjInsert = DbHelper.MontarInsert(this);

            DataTable obj = DbHelper.Conection.GetDataTable(ObjInsert.Item1, ObjInsert.Item2, false);
            return Convert.ToInt64(obj.Rows[0][0]);
        }

    }
}
