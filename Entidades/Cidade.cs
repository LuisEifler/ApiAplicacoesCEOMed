using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades
{
    public class Cidade : TableBase
    {
        public Int64? EstadoId { get; set; }
        public string Nome { get; set; }
    }
}
