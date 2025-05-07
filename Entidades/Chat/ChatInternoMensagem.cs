using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades.Chat
{
    public class ChatInternoMensagem : TableBase
    {
        public string Login { get; set; }
        public Int64 IdUsuarioOrigem { get; set; }
        public Int64 IdUsuarioDestino { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataLeitura { get; set; }
        public int Status { get; set; }
    }
}
