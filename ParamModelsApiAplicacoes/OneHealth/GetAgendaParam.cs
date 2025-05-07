using APICeomedAplicacoes.Uteis;
using Swashbuckle.AspNetCore.Annotations;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth
{
    public class GetAgendaParam : BaseParamApi
    {
        [RequiredParam(ErrorMessage = "O campo dataInicial é obrigatório.")]
        [SwaggerSchema("Data inicial da consulta de agenda")]
        public DateTime? dataInicial { get; set; }

        [SwaggerSchema("Data final da consulta de agenda, caso seja nulo o valor sera igual a dataInicial")]
        public DateTime? dataFinal { get; set; }

        [RequiredParam(ErrorMessage = "O campo idProfissional é obrigatório.")]
        [SwaggerSchema("Id do profissional da agenda")]
        public long? idProfissional { get; set; }

        [RequiredParam(ErrorMessage = "O campo idEmpresa é obrigatório.")]
        [SwaggerSchema("Id da empresa")]
        public long? idEmpresa { get; set; }

    }
}