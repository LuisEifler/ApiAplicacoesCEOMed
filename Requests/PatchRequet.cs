using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.ParamModelsApiAplicacoes;
using APICeomedAplicacoes.Uteis;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace APICeomedAplicacoes.Requests
{
    public class PatchRequet<T> : BaseRequest<T>
    {
        public PatchRequet() : base() { }
        public PatchRequet(T param,HttpRequest? defaultRequest) : base(param, defaultRequest) 
        { 
            this.response = base.response;
            this.CreateHashTable();
        }

        public Hashtable paramsFromDb { get; set; } = new Hashtable();

        private Response response { get; set; }

        public string updateCommand { get; set; } = "";

        public override Response GetResponse()
        {
            return response;
        }

        private async void CreateHashTable()
        {
            string jsonString = this.GetRequestBody().Result;
            JObject json = JObject.Parse(jsonString);
            TableName attrTable = (TableName)Attribute.GetCustomAttribute(typeof(T), typeof(TableName));

            this.updateCommand = $"UPDATE dbo.{attrTable.Name} ";

            foreach (var prop in param.GetType().GetRuntimeProperties())
            {
                string name = prop.Name.FristLetterLower();
                if (json.ContainsKey(name))
                {
                    ColumnInfo attr = (ColumnInfo)Attribute.GetCustomAttribute(prop,typeof(ColumnInfo));

                    this.paramsFromDb.Add($"@{attr.Name}", attr.Name.Contains("Observacao") ? "\n" + prop.GetValue(param) : prop.GetValue(param));

                    string SetCommand = "";

                    if(attr.UpdateType == ETipoUpdate.Concat)
                        SetCommand = this.updateCommand == $"UPDATE dbo.{attrTable.Name} " ? $"SET {name} = CONCAT({name} ,@{name}) " : $", {name} = @{name} ";
                    else
                        SetCommand = this.updateCommand == $"UPDATE dbo.{attrTable.Name} " ? $"SET {name} = @{name} " : $", {name} = @{name} ";

                    this.updateCommand += SetCommand;

                }
            }

            this.updateCommand += $"WHERE Id = @Id";
        }

    }
}
