using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;

namespace APICeomedAplicacoes.Responses
{
    public class GetAgendaResponse
    {
        public GetAgendaResponse(List<Agendamento> agendamentos)
        {
            this.agendamentos = agendamentos;
            this.contadores = agendamentos.GetContadores();
        }
        public List<Agendamento> agendamentos { get; set; }
        public List<ContadorPaginaAgendamentos> contadores { get; set; }

    }
}
