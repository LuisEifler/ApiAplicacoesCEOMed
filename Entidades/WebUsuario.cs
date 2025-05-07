using APICeomedAplicacoes.Conexao;
using System.Text.Json.Serialization;

namespace APICeomedAplicacoes.Entidades
{
    public class WebUsuario : TableBase
    {
        public new Int64 Id { get; set; }
        public string Login { get; set; }

        [JsonIgnore]
        public string Senha { get; set; }
        public int Situacao { get; set; }
        public int TipoAcesso { get; set; }
        public Int64 IdClinica { get; set; }
        public string ConnectionID { get; set; }
        public string NomeMedico { get; set; }

        public DateTime DataUltimoAcessoChat { get; set; }

        public virtual String LetterAvatar { get { return string.IsNullOrEmpty(this.NomeMedico) ? "" : this.NomeMedico.Substring(0, 1).ToUpper(); } }


    }
}
