using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades.Whatts
{
    public class ClinicasEnvioWhatsApp : TableBase
    {
        public string Chave { get;set;}
        public string NumeroTelefoneEnvio { get;set;}
    }
}
