using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    public class PostBloqueioAgendaParam : BaseParamApi
    {
        private TimeSpan? _horarioFim = null;
        [RequiredParam(ErrorMessage = "O campo IdProfissional é obrigatório.")]
        [SwaggerSchema("Id do profissional da agenda.")]
        public Int64 IdProfissional { get; set; }

        [RequiredParam(ErrorMessage = "O campo IdEmpresa é obrigatório.")]
        [SwaggerSchema("Id da empresa.")]
        public Int64 IdEmpresa { get; set; }

        [RequiredParam(ErrorMessage = "O campo Data é obrigatório.")]
        [SwaggerSchema("Data do bloqueio (yyyy-MM-dd)", Format = "date")]
        public DateOnly Data { get; set; }

        [RequiredParam(ErrorMessage = "O campo HorarioInicio é obrigatório.")]
        [SwaggerSchema("Horario inicial do bloqueio (hh:mm:ss)", Format = "time")]
        public TimeSpan HorarioInicio { get; set; }

        [SwaggerSchema("Horario final do bloqueio (hh:mm:ss) (PADRÃO = horarioInicio)", Format = "time")]
        public TimeSpan? HorarioFim { get { return _horarioFim.IsNull() ? this.HorarioInicio.Add(new TimeSpan(0, 1, 0)) : this._horarioFim; } set { _horarioFim = value; } }

        [RequiredParam(ErrorMessage = "O campo IdUsuario é obrigatório.")]
        [SwaggerSchema("Id do usuario que esta executando o bloqueio.")]
        public Int64 IdUsuario { get; set; }

        [SwaggerSchema("Observação do bloqueio.")]
        public String? Observacao { get; set; } 
        

    }
}
