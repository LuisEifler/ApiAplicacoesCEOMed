using APICeomedAplicacoes.Entidades.Chat;

namespace APICeomedAplicacoes.Responses
{
    public class GetMensagensResponse
    {

        public List<ChatInternoMensagem> mensagens { get; set; }

        public int totalPaginas { get; set; }

    }
}
