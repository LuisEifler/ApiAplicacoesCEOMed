using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades
{
    public class AgendamentoPacientes : TableBase
    {
        public int PacienteId { get; set; }
        public int FuncionarioId { get; set; }
        public TimeSpan Horario { get; set; }
        public DateTime Data { get; set; }
        public int AgendaStatusId { get; set; } = 1;
        public string TipoAtendimentoPaciente { get; set; }
        public bool Reagendou { get; set; } = false;
        public int IdUsuario { get; set; }
        public string? Procedimento { get; set; }
        public string? Observacao { get; set; }
        public bool EntregouLaudo { get; set; } = false;
        public bool ChegouAtrasado { get; set; } = false;
        public decimal ValorConsulta { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public int IdConvenio { get; set; }
        public bool TemLaudo { get; set; } = false;
        public int IdCodigoConsultaConvenioMedico { get; set; }
        public int TipoAgenda { get; set; } = 0;
        public string? TipoAcomodacao { get; set; }
        public string? NumeroQuarto { get; set; }
        public string? NumeroLeito { get; set; }
        public string? SalaCirurgia { get; set; }
        public bool Encaixe { get; set; }
        public string? WhatsAPPStatus { get; set; }
        public bool? WhatsAPPEnvia { get; set; }
        public bool Pago { get; set; } = false;
        public int IdEmpresa { get; set; }
        public int? IdNotaFiscalServico { get; set; }
        public TimeSpan? TempoDilatacaoInicial { get; set; }
        public TimeSpan? TempoDilatacaoFinal { get; set; }
        public int? IdBloqueio { get; set; }
        public int OrigemAgenda { get; set; } = 10;
        public DateTime? DataHoraPainel { get; set; }
        public int? IdContratadoExecutante { get; set; }
        public string? ContratadoExecutante { get; set; }
        public decimal? ValorRepasse { get; set; }
        public DateTime? DataLiberacaoGuia { get; set; }

    }
}
