namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    public class PostAgendamentoParam : BaseParamApi
    {
        public Int64 idPaciente { get; set; }
        public Int64 idProfissional { get; set; }
        public DateOnly data { get; set; }
        public TimeSpan horario{ get; set; }
        public String observacao { get; set; }
        public int tipoAtendimento { get; set; }
        public bool encaixe { get; set; }
        public Int64 idEmpresa { get; set; }
        public String nomePaciente { get; set; }
        public String telefonePaciente { get; set; }
        public Int64 idConvenio { get; set; }
        public Int64 idCodigoConsulta { get; set; }

    }
}
