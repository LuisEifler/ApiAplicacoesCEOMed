using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;

namespace APICeomedAplicacoes.Responses
{
    public class GetPacientesFromNameResponse 
    {
        public Int64? Id { get; set; }
        public string? Nome { get; set; }
        public string Cpf { get; set; }
        public string Convenio { get; set; }
        public string Telefone { get; set; }
        public Int64 ConvenioId { get; set; }

    }
}
