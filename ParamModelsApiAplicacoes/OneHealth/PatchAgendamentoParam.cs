using APICeomedAplicacoes.Uteis;
using APICeomedAplicacoes.Uteis.Enum;
using Swashbuckle.AspNetCore.Annotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    [TableName("AgendamentoPacientes")]
    public class PatchAgendamentoParam 
    {
        private Int64 IdClinica { get; set; }
        public void SetIdClinica(Int64 value) => this.IdClinica = value;

        private Int64 IdAgendamento { get; set; }
        public void SetIdAgendamento(Int64 value) => this.IdAgendamento = value;

        [SwaggerSchema("Observação do agendamento.")]
        [ColumnInfo("Observacao",ETipoUpdate.Concat)]
        public string? Observacao { get; set; } = string.Empty;

        [SwaggerSchema("Situação do agendamento.")]
        [ColumnInfo("AgendaStatusId")]
        public int? Status { get; set; } = null;

        [SwaggerSchema("Data do agendamento. (yyyy-MM-dd)")]
        [ColumnInfo("Data")]
        public DateOnly? Data { get; set; } = null;

        [SwaggerSchema("Horario do agendamento. (hh:mm:ss)")]
        [ColumnInfo("Horario")]
        public TimeSpan? Horario { get; set; } = null;

        [SwaggerSchema("Profissional da agenda.")]
        [ColumnInfo("FuncionarioId")]
        public Int64? IdProfissional { get; set; } = null;

    }
}
