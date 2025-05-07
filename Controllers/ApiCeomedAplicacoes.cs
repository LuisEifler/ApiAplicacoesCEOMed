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

[ApiController]
[Route("api/v2/[action]")]
public class ApiCeomedAplicacoes : ControllerBase
{
    //TOKEN API
    //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJxd2VFV1ExNTApJSFzQTIiLCJuYW1lIjoiQ0VPTUVEIiwiYWRtaW4iOnRydWUsImlzcyI6ImNlb21lZGFwbGljYWNvZXMiLCJhdWQiOiJjZW9tZWRhcGxpY2Fjb2VzIiwiaWF0IjoxNzEyNDgwMDAwLCJleHAiOjE3MTI1MDAwMDB9.nLx0usBQrCil2h2EjThq6ojAFMVfyexuq1tQOtlh8Sg
    //Banco usado para teste dessa api é o tbCodigo = 98 | ceo_teste_cardiolife

    #region Metodos Get


    [Authorize]
    [HttpGet]
    [SwaggerOperation(Summary = "Consultar usuario por email e senha")]
    [ProducesResponseType(typeof(ResponseValue<Usuario>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseValue<object>),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseValue<object>),StatusCodes.Status400BadRequest)]
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

}

