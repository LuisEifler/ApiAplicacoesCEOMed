using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.Requests;
using APICeomedAplicacoes.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using APICeomedAplicacoes.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.Painel;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth;
using APICeomedAplicacoes.Uteis;
using APICeomedAplicacoes.ParamModelsApiAplicacoes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

[ApiController]
[Route("api/v2/[action]")]
public class ApiCeomedAplicacoes : ControllerBase
{
    //TOKEN API
    //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJxd2VFV1ExNTApJSFzQTIiLCJuYW1lIjoiT05FSEVBTFRIIiwiYWRtaW4iOiJ0cnVlIiwiaXNzIjoiY2VvbWVkYXBsaWNhY29lcyIsImF1ZCI6WyJjZW9tZWRhcGxpY2Fjb2VzIiwiY2VvbWVkYXBsaWNhY29lcyJdfQ.UUSV6fpPtNZhmMo_YJv35nmKGpCLoNQehi5OBPu0N90
    //Banco usado para teste dessa api é o tbCodigo = 98 | ceo_teste_cardiolife

    #region Metodos Get


    [Authorize]
    [HttpGet]
    [SwaggerOperation(Summary = "Consultar usuario por email e senha")]
    [ProducesResponseType(typeof(ResponseValue<Usuario>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseValue<object>),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseValue<object>),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status500InternalServerError)]
    public object? Login([FromQuery] GetLoginParam param)
    {
        GetRequest<GetLoginParam, Usuario> request = new(param, Request);
        try
        {
            Usuario usuarioEncontrado = null;
            bool usuarioNaoEncontrado = false;
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.UseDbPrincipal();
                Hashtable hashtable = new Hashtable() { { "@Login", request.Param.login }, { "@Senha", request.Param.senha } , { "@TipoWebUsuario", request.param.tipoAcesso } };
                List<Usuario> list = DbHelper.GetTable<Usuario>("Proc_APIAplicacao_ConsultarUsuarioSenhaMultiClinicas", hashtable, true);

                if (list.IsNotNull() && list.Count > 0)
                {
                    if (usuarioEncontrado == null)
                    {
                        usuarioEncontrado = list.First();
                        usuarioEncontrado.Empresas = new List<Empresa>();

                        foreach (var emp in list)
                        {
                            usuarioEncontrado.Empresas.Add(new Empresa()
                            {
                                Id = emp.IdEmpresa,
                                RazaoSocial = emp.NomeEmpresa
                            });
                        }
                        request.GetResponse().Value = usuarioEncontrado;
                        request.GetResponse().ValidResultValue();

                    }
                }
                else
                    usuarioNaoEncontrado = true;

                if (usuarioNaoEncontrado)
                {
                    request.GetResponse().AddError("Usuário e/ou Senha incorretos.");
                }
            }
            return request.GetResult();
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Consultar profissionais")]
    [ProducesResponseType(typeof(ResponseValue<List<Profissional>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status500InternalServerError)]
    public object? GetProfissionais([FromQuery] GetProfissionaisParam param)
    {

        GetRequest<GetProfissionaisParam, List<Profissional>> request = new(param, Request);
        try
        {
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.SetDbClienteConection(request.Param.IdClinica.Value);
                Hashtable hashtable = new Hashtable() { { "@TipoMedico", request.Param.tipoMedico }};
                List<Profissional> list = DbHelper.GetTable<Profissional>("Proc_APIAplicacao_ConsultaProfissional", hashtable, true);
                request.GetResponse().Value = list;
                request.GetResponse().ValidResultValue();

            }

            return request.GetResult();
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }
    }


    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Consultar agendamentos do profissional")]
    [ProducesResponseType(typeof(ResponseValue<GetAgendaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status500InternalServerError)]
    public object? GetAgenda([FromQuery] GetAgendaParam parametro)
    {

        GetRequest<GetAgendaParam, GetAgendaResponse> request = new(parametro, Request);
        try
        {
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.SetDbClienteConection(request.Param.IdClinica.Value);
                Hashtable hashtable = new Hashtable()
                {
                    { "@Data", Convert.ToDateTime(request.Param.dataInicial) },
                    { "@DataFim", request.Param.dataFinal != null ? Convert.ToDateTime(request.Param.dataFinal) : Convert.ToDateTime(request.Param.dataInicial)},
                    { "@IdProfissional", request.Param.idProfissional },
                    { "@IdEmpresa", request.Param.idEmpresa }
                };
                List<Agendamento> list = DbHelper.GetTable<Agendamento>("Proc_APIAplicacao_ConsultarAgendamento", hashtable, true);
                GetAgendaResponse response = new GetAgendaResponse(list);
                request.GetResponse().Value = response;
                request.GetResponse().ValidResultValue();

            }

            return request.GetResult();        
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }
    }


    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Consultar paciente por nome parcial")]
    [ProducesResponseType(typeof(ResponseValue<List<GetPacientesFromNameResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status500InternalServerError)]
    public object? GetPacientesFromParcialName([FromQuery] GetPacienteFromNameParam param)
    {
        GetRequest<GetPacienteFromNameParam, List<GetPacientesFromNameResponse>> request = new(param, Request);
        try
        {
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.SetDbClienteConection(request.Param.IdClinica.Value);

                Hashtable hashtable = new Hashtable()
                {
                    { "@NomeParcial", request.Param.partialName }
                };

                List<GetPacientesFromNameResponse> list = DbHelper.GetTable<GetPacientesFromNameResponse>("Proc_APIAplicacao_ConsultarPacientes", hashtable, true);
                request.GetResponse().Value = list;
                request.GetResponse().ValidResultValue();

            }
            return request.GetResult();
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }
    }


    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Consultar paciente por id")]
    [ProducesResponseType(typeof(ResponseValue<Paciente>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status500InternalServerError)]
    public object? GetPacienteFromId([FromQuery] GetPacienteFromId param)
    {
        GetRequest<GetPacienteFromId, Paciente> request = new(param, Request);
        try
        {
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.SetDbClienteConection(request.Param.IdClinica.Value);

                Hashtable hashtable = new Hashtable()
                {
                    { "@pacienteId", request.Param.IdPaciente }
                };

                Paciente pacient = DbHelper.GetTable<Paciente>("Proc_APIAplicacao_ConsultarDadosPaciente", hashtable, true).FirstOrDefault();
                request.GetResponse().Value = pacient;
                request.GetResponse().ValidResultValue();
            }
            else
            {
                return request.GetResult();
            }
            return request.GetResult();
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }
    }


    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Consultar cidades por estado")]
    [ProducesResponseType(typeof(ResponseValue<List<Cidade>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status500InternalServerError)]
    public object? GetCidadesFromEstado([FromQuery] GetCidadesParam param)
    {
        GetRequest<GetCidadesParam, List<Cidade>> request = new(param, Request);
        try
        {
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.SetDbClienteConection(request.Param.IdClinica.Value);

                List<Cidade> cidades = DbHelper.Select<Cidade>(x => x.EstadoId == request.param.idEstado);
                request.GetResponse().Value = cidades;
                request.GetResponse().ValidResultValue();
            }
            else
            {
                return request.GetResult();
            }
            return request.GetResult();
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }
    }


    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Consultar convênios medicos")]
    [ProducesResponseType(typeof(ResponseValue<List<ConvenioMedico>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status500InternalServerError)]
    public object? GetConveniosMedicos([FromQuery] BaseParamApi param)
    {
        GetRequest<BaseParamApi, List<ConvenioMedico>> request = new(param, Request);
        try
        {
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.SetDbClienteConection(request.Param.IdClinica.Value);

                List<ConvenioMedico> convenios = DbHelper.Select<ConvenioMedico>(x => x.Situacao == true);
                request.GetResponse().Value = convenios;
                request.GetResponse().ValidResultValue();
            }
            else
            {
                return request.GetResult();
            }
            return request.GetResult();
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }
    }
    

    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Consultar CIDs por nome parcial ou Codigo")]
    [ProducesResponseType(typeof(ResponseValue<List<CIDCategoriaSub>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status500InternalServerError)]
    public object? GetCidsFromParcialName([FromQuery] GetCidsFromParcialNameParam param)
    {
        GetRequest<GetCidsFromParcialNameParam, List<CIDCategoriaSub>> request = new(param, Request);
        try
        {
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.SetDbClienteConection(request.Param.IdClinica.Value);

                Hashtable hashtable = new Hashtable()
                {
                    { "@ParcialCid", request.Param.CidParcial }
                };

                string json = DbHelper.GetTable<ModelJsonFromDB>("Proc_APIAplicacao_ConsultaCidParcial", hashtable, true).FirstOrDefault().Json;

                List<CIDCategoriaSub> Cids = JsonConvert.DeserializeObject<List<CIDCategoriaSub>>(json);

                request.GetResponse().Value = Cids;
                request.GetResponse().ValidResultValue();
            }
            else
            {
                return request.GetResult();
            }
            return request.GetResult();
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }
    }

    #endregion

    #region Metodos Post

    [Authorize]
    [HttpPost]
    [SwaggerOperation(Summary = "Blqueia um determinado horario.")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public object? Bloqueio([FromBody] PostBloqueioAgendaParam param)
    {
        PostRequest<PostBloqueioAgendaParam> request = new(param,Request);

        try
        {
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.SetDbClienteConection(request.param.IdClinica);
                BloqueioAgendaMedicos bloqueio = new(request.param);
                Int64 id = bloqueio.Insert();
                if (id != -1) request.GetResponse().Code = 201;
            }
            return request.GetResult();
        }
        catch(Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }

    }

    #endregion

    #region Metodos Delete

    [Authorize]
    [HttpDelete]
    [SwaggerOperation(Summary = "Remove um bloqueio de um horario.")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    [Route("/api/v2/[action]/{IdClinica}/{Id}")]
    public object? Bloqueio(Int64 IdClinica,Int64 Id)
    {
        Id = Id - 9000000;
        DeleteRequet request = new(IdClinica,Id,Request);

        try
        {
            if (request.GetResponse().IsSuccess)
            {
                if (!request.Execute<BloqueioAgendaMedicos>()) 
                    request.GetResponse().AddError("Erro ao remover o bloqueio.");
            }
            return request.GetResult();
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }

    }

    #endregion

    #region Metodos Patch

    [Authorize]
    [HttpPatch]
    [SwaggerOperation(Summary = "Altera dados do Agendamento.")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    [Route("/api/v2/[action]/{IdClinica}/{Id}")]
    public object? Agendamento([SwaggerSchema("Id da Clinica encontrado no login do usuario")]
                                [RequiredParam(ErrorMessage = "O campo IdClinica é obrigatório.")]
                                Int64 IdClinica,
                                [SwaggerSchema("Id do agendamento.")]
                                [RequiredParam(ErrorMessage = "O campo Id é obrigatório.")]
                                Int64 Id, 
                                [FromBody] PatchAgendamentoParam param )
    {

        param.SetIdClinica(IdClinica);
        param.SetIdAgendamento(Id);
        PatchRequet<PatchAgendamentoParam> request = new(param, Request);

        try
        {
            if (request.GetResponse().IsSuccess)
            {
                DbHelper.SetDbClienteConection(IdClinica);
                request.paramsFromDb.Add("@Id", Id);
                DbHelper.ExecuteScalar(request.updateCommand, request.paramsFromDb);

            }
            return request.GetResult();
        }
        catch (Exception ex)
        {
            request.GetResponse().AddError(ex.Message);
            return request.GetResult(500);
        }

    }

    #endregion

}

