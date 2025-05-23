﻿using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.ParamModelsApiAplicacoes;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.Painel;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.Whatts;
using APICeomedAplicacoes.Requests;
using APICeomedAplicacoes.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.OpenApi.Expressions;
using APICeomedAplicacoes.Models.Whatts;

namespace APICeomedAplicacoes.Controllers
{
    [ApiController]
    public class ApiCeomedWhatts : ControllerBase
    {
        [HttpPost]
        [SwaggerOperation(Summary = "Receber uma mensagem de um paciente para a confirmação pelo Utalk 1.0")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [Route("api/utalk")]
        public async Task<object?> Utalk([FromBody] HookEvent param)
        {
            PostRequest<HookEvent> request = new(param, Request);
            try
            {
                switch (param.Event)
                {
                    case EventType.Chat:
                        await param.SendMessageBack();
                    break;

                    case EventType.StatusChange:
                        // Aqui chega o evento de leitura da mensagem do Whats
                        // O objeto 'hook' contém as informações que chegou
                        break;
                }

                return request.GetResult();
            }
            catch (Exception ex)
            {
                request.GetResponse().AddError(ex.Message);
                return request.GetResult(500);
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Receber uma mensagem de um paciente para a confirmação pelo Utalk 2.0")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [Route("api/utalkv2")]
        public async Task<object?> UtalkV2()
        {

            PostRequest<string> request = new("", Request);
            UtalkParam param = JsonConvert.DeserializeObject<UtalkParam>(await request.GetRequestBody());
            try
            {
                switch (param.Type)
                {
                    case EventType.Message:
                        DbMessageResponse messageResponse = param.GetDbResponse();
                        if (messageResponse != null && messageResponse.HasMessage())
                        {
                            SentMessage msg = new SentMessage(param, messageResponse.MessageReturn);
                            var response = await param.SendMessageResponse(msg);
                            var content = await response.Content.ToObject<UTalkResponse>();

                            if (!response.IsSuccessStatusCode || content?.Status == "offline")
                            {
                                request.GetResponse().AddError($"Utalk HTTP Status Response is {(int)response.StatusCode}");
                                request.GetResponse().AddError("Mensagem potencialmente não entregue");
                                request.GetResponse().AddError(msg.ToJson());
                                request.GetResponse().AddError("Resposta da API uTalk:\n" + content.ToJson());
                            }
                        }
                        break;

                    case EventType.StatusChange:
                        // Aqui chega o evento de leitura da mensagem do Whats
                        // O objeto 'hook' contém as informações que chegou
                        break;
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
