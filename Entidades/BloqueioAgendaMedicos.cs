using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth;

namespace APICeomedAplicacoes.Entidades
{
    public class BloqueioAgendaMedicos : TableBase
    {

        public BloqueioAgendaMedicos() { }
        public BloqueioAgendaMedicos(PostBloqueioAgendaParam param) 
        {
            this.IdFuncionario = param.IdProfissional;
            this.IdUsuario = param.IdUsuario;
            this.HorarioInicio = param.HorarioInicio;
            this.HorarioTermino = param.HorarioFim.Value;
            this.Data = param.Data.ToDateTime(new TimeOnly(0,0,0));
            this.TipoBloqueio = 1;
            this.IdEmpresa = param.IdEmpresa;
            this.Observacao = param.Observacao;
        }

        public Int64 IdFuncionario { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioTermino { get; set; }
        public DateTime Data { get; set; }
        public Int64 IdUsuario { get; set; }
        public DateTime DataHoraInclusao { get { return DateTime.Now; }}
        public int TipoBloqueio { get; set; }
        public Int64 IdEmpresa { get; set; }
        public String Observacao { get; set; }

    }
}
