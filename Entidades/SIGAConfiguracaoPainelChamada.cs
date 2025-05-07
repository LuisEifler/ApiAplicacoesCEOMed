using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades
{
    public class SIGAConfiguracaoPainelChamada : TableBase
    {
        public String IPServidor { get; set; }
        public Int64? IdProfissionalAgenda { get; set; }
        public String Profissional { get; set; }
        public String LocalAtendimento { get; set; }
        public String FormatoFrase { get; set; }
        public Int64? IdLocalPainelChamada { get; set; }

    }
}
