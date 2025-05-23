using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.ParamModelsApiAplicacoes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace APICeomedAplicacoes.Requests
{
    public class DeleteRequet : BaseRequest<BaseParamApi>
    {
        public DeleteRequet() : base() { }
        public DeleteRequet(Int64? IdClinica, Int64? Id, HttpRequest? defaultRequest, HttpContext context = null) : base(new BaseParamApi() { IdClinica = IdClinica }, defaultRequest, context) { this.response = base.response;this.Id = Id; }
        
        private Response response { get; set; }

        private Int64? Id { get; set; }

        public override Response GetResponse()
        {
            return response;
        }
        
        public bool Execute<TR>()
        {
            try
            {
                TR table = Util.CriarInstancia<TR>();
                if (!typeof(TR).BaseType.Equals(typeof(TableBase))) throw new Exception("Tipo não é uma tabela.");

                DbHelper.SetDbClienteConection(param.IdClinica);
                table.GetType().GetProperty("Id").SetValue(table, this.Id);
                table.GetType().GetMethod("Delete").Invoke(table, null);

                return true;
            }
            catch(Exception ex)
            {
                response.AddError(ex.Message);
                return false;
                throw;
            }
            finally
            {
                DbHelper.UseDbPrincipal();
            }
        }

    }
}
