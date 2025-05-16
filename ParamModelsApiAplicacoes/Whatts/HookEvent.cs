using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data;

namespace APICeomedAplicacoes.ParamModelsApiAplicacoes.Whatts
{
    public class HookEvent 
    {
        public HookEvent() { }

        public static HookEvent Parse(JObject json)
        {
            var hookEvent = json?.ToObject<HookEvent>();
            if (hookEvent != null)
            {
                if (hookEvent.ContactPhoneNumber is null)
                {
                    hookEvent.Contact = new Contato
                    {
                        Number = json["contact[number]"]?.ToString(),
                        Name = json["contact[name]"]?.ToString(),
                        Server = json["contact[server]"]?.ToString()
                    };
                }
                if (hookEvent.MessageWhatsUniqueId is null)
                {
                    hookEvent.Chat = new Chat()
                    {
                        Dtm = json["chat[dtm]"]?.ToString(),
                        WhatsUniqueId = json["chat[uid]"]?.ToString(),
                        Dir = json["chat[dir]"]?.ToString(),
                        Type = json["chat[type]"]?.ToString(),
                        BodyMessage = json["chat[body]"]?.ToString()
                    };
                }
            }
            return hookEvent;
        }

        public string Event { get; set; }
        public string Token { get; set; }
        /// <summary>
        /// Número de telefone da sua conta WhatsApp com código internacional
        /// </summary>
        [JsonProperty("user")]
        public string MyPhoneNumber { get; set; }
        public string Operador { get; set; }
        public Contato Contact { get; set; }
        public Chat Chat { get; set; }
        public string Acknowledge { get; set; }
        public string ContactPhoneNumber => Contact?.Number;
        public string MessageWhatsUniqueId => Chat?.WhatsUniqueId;
        public string MessageBody => Chat?.BodyMessage;

        public Answer GetAnswerForUser()
        {
            var answer = new MessageData().GetAnswerForUser(this);
            return answer;
        }

        public async Task SendMessageBack()
        {
            Answer answer = GetAnswerForUser();
            if (answer != null && answer.HasMessage())
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded");

                var msg = new
                {
                    cmd = "chat",
                    id = answer.CustomID,
                    to = $"{this.ContactPhoneNumber}@c.us",
                    msg = answer.MessageReturn
                };


                var response = await client.PostAsync($"https://v1.utalk.chat/send/{this.Token}/", msg.ToEncondedContent());
                var content = await response.Content.ToObject<UTalkResponse>();

                if (!response.IsSuccessStatusCode || content?.Status == "offline")
                {
                    //Se o 'content?.Status' for offline significa que mandamos o token errado, provavelmente
                    var ex = new MessageNotSentException($"Utalk HTTP Status Response is {(int)response.StatusCode}");
                    ex.Data.Add("Status telefone (utalk)", content?.Status);
                    ex.Data.Add("Mensagem potencialmente não entregue", msg.ToJson());
                    ex.Data.Add("Resposta da API uTalk", await response.Content.ReadAsStringAsync());
                    throw ex;
                }
            }
                
        }

    }

    public class MessageNotSentException : ApplicationException
    {
        public MessageNotSentException(string message) : base(message) { }
    }

    public class UTalkResponse
    {
        public string Type { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
        public string Servidor { get; set; }
    }

    public class MessageData 
    {
        public Answer GetAnswerForUser(HookEvent hook)
        {
            var query = "dbo.ap_Whatts_InsertMessage";
            Hashtable param = new Hashtable();
            param.Add("@SourcePhone", hook.MyPhoneNumber);
            param.Add("@DestinationPhone", hook.ContactPhoneNumber);
            param.Add("@WhatsUniqueId", hook.MessageWhatsUniqueId);
            param.Add("@BodyText", hook.MessageBody);
            DbHelper.UseDbPrincipal();
            return DbHelper.GetTable<Answer>(query, param, true).FirstOrDefault();
        }
    }

    public class Answer
    {
        public Answer(){}

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

    public class Contato
    {
        public string Number { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Tipo de chat. c.us = Contato e g.us = Grupo
        /// </summary>
        public string Server { get; set; }
    }

    public class Chat
    {
        /// <summary>
        /// Mensagem enviada. timestamp
        /// </summary>
        public string Dtm { get; set; }
        /// <summary>
        /// MSG ID exclusiva do WhatsApp para esta mensagem: 62397B58E3E0B
        /// </summary>
        [JsonProperty("uid")]
        public string WhatsUniqueId { get; set; }
        /// <summary>
        /// Direção da mensagem Ex: i (msg recebida) Ex: o (msg enviada)
        /// </summary>
        public string Dir { get; set; }
        /// <summary>
        /// chat, image, video, audio, document, vcard, location
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Conteúdo da mensagem
        /// </summary>
        [JsonProperty("body")]
        public string BodyMessage { get; set; }
    }

    public class EventType
    {
        public const string Chat = "chat";
        public const string Message = "Message";
        public const string StatusChange = "ack";
    }
}
