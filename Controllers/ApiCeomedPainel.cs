using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;
using APICeomedAplicacoes.ParamModelsApiAplicacoes;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.Painel;
using APICeomedAplicacoes.Requests;
using APICeomedAplicacoes.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;

namespace APICeomedAplicacoes.Controllers
{
    [ApiController]
    [Route("api/v2/Painel/[action]")]
    public class ApiCeomedPainel : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Consultar paineis disponiveis por clinica")]
        [ProducesResponseType(typeof(ResponseValue<List<LocalPainelChamada>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
        public object? GetPainels([FromQuery] GetPaineisParam param)
        {
            GetRequest<GetPaineisParam, List<LocalPainelChamada>> request = new(param, Request);
            try
            {
                if (request.GetResponse().IsSuccess)
                {
                    DbHelper.SetDbClienteConection(request.Param.IdClinica.Value);
                    request.GetResponse().Value = DbHelper.Select<LocalPainelChamada>(x => x.IdEmpresa == request.param.IdEmpresa);
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
        [SwaggerOperation(Summary = "Consultar geolocalização da unidade informada")]
        [ProducesResponseType(typeof(ResponseValue<GetLocationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
        public async Task<object>? GetLocation([FromQuery] GetLocationPainelParam param)
        {
            GetRequest<GetLocationPainelParam, GetLocationResponse> request = new(param, Request);
            try
            {
                if (request.GetResponse().IsSuccess)
                {
                    DbHelper.SetDbClienteConection(request.Param.IdClinica.Value);
                    string endereco = DbHelper.GetTable("SELECT ED.Descricao + ', ' + C.Nome + ', ' + E.Sigla as Endereco " +
                        "FROM dbo.Endereco ED " +
                        "INNER JOIN dbo.Cidade C ON C.Id = ED.IdCidade " +
                        "INNER JOIN dbo.Estado E ON E.Id = C.EstadoId " +
                        "WHERE ED.id = @IdEmpresa",new Hashtable() { { "@IdEmpresa",request.param.IdEmpresa }}, false).Rows[0].ValueToString("Endereco");

                    if (endereco != null)
                    {
                        using var client = new HttpClient();

                        client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");

                        string url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(endereco)}";

                        var response = await client.GetAsync(url);
                        if (!response.IsSuccessStatusCode)
                            return request.GetResult(500);

                        var json = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<GetLocationResponse[]>(json);

                        if (resultado?.Length > 0)
                        {
                            request.GetResponse().Value = resultado[0];
                        }
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
        [SwaggerOperation(Summary = "Consultar clima pela localização")]
        [ProducesResponseType(typeof(ResponseValue<GetClimaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseValue<object>), StatusCodes.Status400BadRequest)]
        public async Task<object>? GetClima([FromQuery] GetClimaParam param)
        {
            GetRequest<GetClimaParam, GetClimaResponse> request = new(param, Request);
            try
            {
                if (request.GetResponse().IsSuccess)
                {
                   
                    using var client = new HttpClient();

                    client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");

                    string apiKey = "e2b3a1b1a7284f24b71171754251104";
                    string url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={request.param.lat},{request.param.lon}&lang=pt";

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                        return request.GetResult(500);

                    var json = await response.Content.ReadAsStringAsync();
                    request.GetResponse().Value = new GetClimaResponse();
                    request.GetResponse().Value.weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(json);

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
