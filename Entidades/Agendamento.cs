using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Uteis.Enum;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace APICeomedAplicacoes.Entidades
{
    public class Agendamento
    {
        private string _horario;
        private string _paciente;
        public Agendamento() { }
        public Int64? Id { get; set; }
        public Int64? PacienteId { get; set; }
        public Int64? FuncionarioId { get; set; }
        public String Horario { get { return  _horario.Length > 5 ? _horario.Substring(0, 5) : _horario; } set { _horario = value; } }
        public DateTime? Data { get; set; }
        public Int64? AgendaStatusId { get; set; }
        public String TipoAtendimentoDescription { get { return this.TipoAtendimentoPaciente != null && this.TipoAtendimentoPaciente != 0 ? ((ETipoAtendimentoPaciente)this.TipoAtendimentoPaciente).Descricao() : ""; } }
        public int? TipoAtendimentoPaciente { get; set; }
        public String Profissional { get; set; }
        public String Paciente 
        { 
            get 
            {
                string desc = _paciente;
                if(this.AgendaStatusId == 0)
                {
                    desc = "BLOQUEIO " + _paciente.Substring(20).Replace(" ","");
                }
                return desc;
            } 
            set { _paciente = value; } }
        public DateTime? DataInicialAguardando { get; set; }
        public String Procedimento { get; set; }
        public String ObservacaoAgenda { get; set; }
        public Int64? IdUsuario { get; set; }
        public Int64? IdConvenio { get; set; }
        public String Exames { get; set; }
        public String Status { get; set; }
        public String Convenio { get; set; }
        public bool? ChegouAtrasado { get; set; }
        public bool? TemProcedimento { get; set; }
        public decimal? ValorConsulta { get; set; }
        public DateTime? DataCriacao { get; set; }
        public String ObservacaoExames { get; set; }
        public bool GeraLaudo { get; set; }
        public Int64? IdCodigoConsultaConvenioMedico { get; set; }
        public String TelefoneCelular { get; set; }
        public Int64? EnderecoId { get; set; }
        public bool? Encaixe { get; set; }
        public Int64? IdEmpresa { get; set; }
        public String CPFPaciente { get; set; }
        public int QuantidadeProcedimento { get; set; }

        public String CorAgendaStatus { get
            {
                if(AgendaStatusId != null)
                {
                    return Util.GetColorFromStatus((EStatusAgendamento)AgendaStatusId).ToHexadecimal();
                }
                return Color.White.ToHexadecimal();
            } 
        }
        public string PacienteSimplificado { get
            {
                if (this.Paciente.IsNullOrEmpty()) return "";
                int index = this.Paciente.IndexOf(" ");
                index = index == this.Paciente.LastIndexOf(" ") ? index : index + 1;
                index = this.Paciente.IndexOf(" ", index);
                string name = this.Paciente.Substring(0, index);
                if(name.EndsWith(" DA") || name.EndsWith(" DE") || name.EndsWith(" DO") || name.EndsWith(" DI"))
                {
                    index = index == this.Paciente.LastIndexOf(" ") ? index : index + 1;
                    index = this.Paciente.IndexOf(" ", index);
                    name = this.Paciente.Substring(0, index);
                }
                return name;
            } 
        }
    }
}
