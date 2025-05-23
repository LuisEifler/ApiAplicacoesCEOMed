using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Controllers;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.Whatts;
using Newtonsoft.Json;
using System.Collections;
using System.Text.Json.Nodes;
using System.Text;
using System.Data.SqlClient;
using System.Net;
using APICeomedAplicacoes.Entidades.Whatts;
using APICeomedAplicacoes.Entidades;

namespace APICeomedAplicacoes.Models.Whatts
{
    public class UtalkParam
    {
        public string Type { get; set; }
        public DateTime EventDate { get; set; }
        public Payload Payload { get; set; }
        public string EventId { get; set; }

        public virtual string MyPhoneNumber { get { return Payload.Content.Channel.PhoneNumber; } }
        public virtual string ContactPhoneNumber { get { return Payload.Content.Contact.PhoneNumber; } }
        public virtual string ContactName { get { return Payload.Content.Contact.Name; } }
        public virtual string MessageWhatsUniqueId { get { return Payload.Content.LastMessage.Id; } }
        public virtual string Message { get { return Payload.Content.LastMessage.Content; } }
        public virtual string OrganizationId { get { return Payload.Content.Organization.Id; } }

        public DbMessageResponse GetDbResponse()
        {
            var query = "dbo.ap_Whatts_InsertMessage";
            Hashtable param = new Hashtable()
            {
                {"@SourcePhone", MyPhoneNumber },
                {"@DestinationPhone", ContactPhoneNumber },
                {"@WhatsUniqueId", MessageWhatsUniqueId },
                {"@BodyText", Message },
            };

            DbHelper.UseDbPrincipal();
            return DbHelper.GetTable<DbMessageResponse>(query, param, true).FirstOrDefault();
        }

        public bool Requestduplicate(string eventId)
        {
            DbHelper.UseDbPrincipal();
            List<LogAPIAplicacoes> logs = DbHelper.GetTable<LogAPIAplicacoes>($"SELECT * FROM dbo.LogAPIAplicacoes WHERE RequestBody LIKE '%{eventId}%'",null,false);
            return logs != null && logs.Count > 0;
        }

        public async Task<HttpResponseMessage> SendMessageResponse(SentMessage mensagem)
        {

            string telefoneFormatado = mensagem.fromPhone.Replace("+", "");
            telefoneFormatado = telefoneFormatado.Length == 13 ? telefoneFormatado.Remove(5,1) : telefoneFormatado;

            string tokenEnvio = DbHelper.Select<CelularesEnvioWhatsApp>(x => x.NumeroEnvio == telefoneFormatado)?.FirstOrDefault().Token;

            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenEnvio);
                client.DefaultRequestHeaders.Add("ContentType", "application/json");

                var content = new StringContent(mensagem.ToJson(), Encoding.UTF8, "application/json");

                await Task.Delay(new TimeSpan(0, 0, 20));

                var response = await client.PostAsync($"https://app-utalk.umbler.com/api/v1/messages/simplified", content);

                return response;
            }
        }
    }


    public class SentMessage
    {
        public SentMessage (UtalkParam param,string mensagem)
        {
            this.toPhone = param.ContactPhoneNumber;
            this.fromPhone = param.MyPhoneNumber;
            this.organizationId = param.OrganizationId;
            this.message = mensagem;
            this.file = null;
            this.skipReassign = false;
            this.contactName = param.ContactName;
        }

        public string toPhone { get; set; }
        public string fromPhone { get; set; }
        public string organizationId { get; set; }
        public string message { get; set; }
        public object file { get; set; }
        public bool skipReassign { get; set; }
        public string contactName { get; set; }
    }

    public class DbMessageResponse
    {
        public DbMessageResponse() { }

        public string Id { get; set; }
        public string CustomID { get; set; }
        public string MessageReturn { get; set; }
        public string Template { get; set; }
        public string StatusConfirmacao { get; set; }
        public string DataAgenda { get; set; }
        public string DadosClinica { get; set; }
        public string Rodape { get; set; }

        internal bool HasMessage()
        {
            return !CustomID.IsNullOrEmpty() && !MessageReturn.IsNullOrEmpty();
        }
    }

    public class Payload
    {
        public string Type { get; set; }
        public Content Content { get; set; }
    }

    public class Content
    {
        public string _t { get; set; }
        public string Id { get; set; }
        public Contact Contact { get; set; }
        public Channel Channel { get; set; }
        public LastMessage LastMessage { get; set; }
        public Organization Organization { get; set; }
        public DateTime CreatedAtUTC { get; set; }
    }

    public class Organization
    {
        public string Id { get; set; }
    }

    public class Contact
    {
        public string PhoneNumber { get; set; }
        public string ContactType { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class Channel
    {
        public string _t { get; set; }
        public string ChannelType { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class LastMessage
    {
        public string Content { get; set; }
        public string MessageType { get; set; }
        public string Source { get; set; }
        public string MessageState { get; set; }
        public DateTime EventAtUTC { get; set; }
        public string Id { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public SentByOrganizationMember? SentByOrganizationMember { get; set; }
    }

    public class SentByOrganizationMember 
    {
        public string Id { get; set; }
    }

}
