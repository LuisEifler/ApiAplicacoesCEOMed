using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades
{
    public class ApiAccessTokens : TableBase
    {

        public string Nome { get; set; }
        public string ApiToken { get; set; }
        public DateTime DataCriacao { get; set; }

    }
}
