using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades
{
    public class WebUsuarioClinica : TableBase
    {
        public Int64? IdUsuario { get; set; }
       public String? NomeMedico { get; set; }

    }
}
