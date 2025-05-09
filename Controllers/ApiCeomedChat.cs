using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.Entidades.Chat;
using APICeomedAplicacoes.ParamModelsApiAplicacoes;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.Chat;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.Painel;
using APICeomedAplicacoes.Requests;
using APICeomedAplicacoes.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;

namespace APICeomedAplicacoes.Controllers
{
    [ApiController]
    [Route("api/v2/Chat/[action]")]
    public class ApiCeomedChat : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Consultar as conversas por clinica")]
        [ProducesResponseType(typeof(ResponseValue<List<ChatInternoContatos>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public object? GetConversas([FromQuery] GetConversasParam param)
        {
            GetRequest<GetConversasParam, List<ChatInternoContatos>> request = new(param, Request);
            try
            {
                if (request.GetResponse().IsSuccess)
                {
                    DbHelper.UseDbPrincipal();
                    request.GetResponse().Value = DbHelper.GetTable<ChatInternoContatos>("PROC_ApiAplicacao_ConsultarConversasChat",new Hashtable() { { "@IdUsuario",request.param.IdUsuario } },true);
                    
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
        [SwaggerOperation(Summary = "Consultar as mansagem da conversa")]
        [ProducesResponseType(typeof(ResponseValue<GetMensagensResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public object? GetMensagens([FromQuery] GetMensagensParam param)
        {
            GetRequest<GetMensagensParam, GetMensagensResponse> request = new(param, Request);
            try
            {
                if (request.GetResponse().IsSuccess)
                {
                    DbHelper.SetDbClienteConection(request.param.IdClinica.Value);
                    GetMensagensResponse response = new GetMensagensResponse();
                    var idUsuario = request.param.IdUsuario;
                    var idUsuarioDestino = request.param.IdUsuarioDestino;

                    response.mensagens = DbHelper.SelectWithPagination<ChatInternoMensagem>(x =>
                    (x.IdUsuarioOrigem == idUsuario &&
                    x.IdUsuarioDestino == idUsuarioDestino) ||
                    (x.IdUsuarioOrigem == idUsuarioDestino &&
                    x.IdUsuarioDestino == idUsuario), request.param.page, 20);

                    response.totalPaginas = DbHelper.GetCount<ChatInternoMensagem>(x =>
                    (x.IdUsuarioOrigem == idUsuario &&
                    x.IdUsuarioDestino == idUsuarioDestino) ||
                    (x.IdUsuarioOrigem == idUsuarioDestino &&
                    x.IdUsuarioDestino == idUsuario)) / 20;

                    request.GetResponse().Value = response;
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
        [SwaggerOperation(Summary = "Consultar todos os contatos por clinica")]
        [ProducesResponseType(typeof(ResponseValue<List<WebUsuario>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public object? GetContatos([FromQuery] BaseParamApi param)
        {
            GetRequest<BaseParamApi, List<WebUsuario>> request = new(param, Request);
            try
            {
                if (request.GetResponse().IsSuccess)
                {
                    DbHelper.UseDbPrincipal();
                    request.GetResponse().Value = 
                        DbHelper.SelectWithInner<WebUsuario, WebUsuario, WebUsuarioClinica>(
                        (WU, WUC ) => WU.Id == WUC.IdUsuario ,
                        (WU, WUC) => WU.IdClinica == request.param.IdClinica,
                        (WU, WUC) => WU, (WU, WUC) => WUC.NomeMedico);
                }

                return request.GetResult();
            }
            catch (Exception ex)
            {
                request.GetResponse().AddError(ex.Message);
                return request.GetResult(500);
            }
        }

    }
}
