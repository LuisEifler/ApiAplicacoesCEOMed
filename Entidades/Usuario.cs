using APICeomedAplicacoes.Uteis;
using System.Text.Json.Serialization;

namespace APICeomedAplicacoes.Entidades
{
    public class Usuario
    {

        //private Boolean _lembrar;
        public Usuario() { }

        //public Int64 Id { get; set; }
        public Int64 IdUsuario { get; set; }
        public Int64 IdUsuarioClinica { get; set; }
        public Int64 IdEmpresa { get; set; }

        public String Login { get; set; }
        public String NomeMedico { get; set; }
        [JsonIgnore]
        public String Senha { get; set; }

        //public Boolean Lembrar
        //{
        //    get
        //    {
        //        return _lembrar;
        //    }
        //    set
        //    {
        //        if (value == null)
        //            _lembrar = false;
        //        else
        //            _lembrar = value;
        //    }
        //}

        public Boolean? Ativo { get; set; }
        public Byte? TipoAcesso { get; set; }
        public Int64? IdMedico { get; set; }
        public String NomeClinica { get; set; }
        [JsonIgnore]
        public String NomeEmpresa { get; set; }
        public Int64? IdClinica { get; set; }

        public List<Empresa> Empresas { get; set; }

    }
}
