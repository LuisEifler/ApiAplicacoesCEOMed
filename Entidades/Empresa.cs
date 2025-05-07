using System.Text.Json.Serialization;
using System.Data;

namespace APICeomedAplicacoes.Entidades
{
    public class Empresa
    {
        public Empresa() 
        {
            UrlFTP = "";
            UsuarioFTP = "";
            SenhaFTP = "";
            CaminhoFTP = "";
            RazaoSocial = "";
            CpfCnpj = "";
        }    
        public Int64? Id { get; set; }
        public string RazaoSocial { get; set; }

        [JsonIgnore]
        public string UrlFTP { get; set; }
        [JsonIgnore]
        public string UsuarioFTP { get; set; }
        [JsonIgnore]
        public string SenhaFTP { get; set; }
        [JsonIgnore]
        public string CaminhoFTP { get; set; }
        [JsonIgnore]
        public string CpfCnpj { get; set; }

    }
}
