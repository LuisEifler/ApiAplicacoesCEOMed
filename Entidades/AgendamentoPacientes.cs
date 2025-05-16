using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades
{
    public class AgendamentoPacientes : TableBase
    {
        public Int64? FuncionarioId { get; set; }
        public DateTime? Data { get; set; }
        public TimeSpan? Horario { get; set; }
        public String Observacao { get; set; }
        public int? AgendaStatusId { get; set; }

    }
}
