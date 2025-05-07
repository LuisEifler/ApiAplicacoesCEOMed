using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades
{
    public class LocalPainelChamada : TableBase
    {
        public String ConnectionID { get; set; }
        public String Descricao { get; set; }
        public Int64 IdEmpresa { get; set; }

    }
}
