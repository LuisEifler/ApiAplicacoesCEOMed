using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades.Whatts
{
    public class CelularesEnvioWhatsApp : TableBase
    {
        public string Token { get;set;}
        public string NumeroEnvio { get;set;}
    }
}
