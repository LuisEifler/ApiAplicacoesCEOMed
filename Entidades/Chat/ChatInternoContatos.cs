using APICeomedAplicacoes.Conexao;

namespace APICeomedAplicacoes.Entidades.Chat
{
    public class ChatInternoContatos : TableBase
    {
        private string _ultimaMensagem = "";
        private string _usuarioDestino = "";

        public ChatInternoContatos() { }

        public Int64 IdUsuario { get; set; }
        public Int64 IdUsuarioDestino { get; set; }
        public DateTime UltimaInteracao { get; set; }
        public int QuantidadeNovasMensagens { get; set; }
        public Int64 IdUltimaMensagem { get; set; }
        public String UsuarioDestino
        { 
            get {
                if (this._usuarioDestino.IsNullOrEmpty()) return "";
                int index = this._usuarioDestino.IndexOf(" ");
                if (index == -1) return this._usuarioDestino;
                index = index == this._usuarioDestino.LastIndexOf(" ") ? index : index + 1;
                index = this._usuarioDestino.IndexOf(" ", index);
                string name = this._usuarioDestino.Substring(0, index);
                if (name.EndsWith(" DA") || name.EndsWith(" DE") || name.EndsWith(" DO") || name.EndsWith(" DI"))
                {
                    index = index == this._usuarioDestino.LastIndexOf(" ") ? index : index + 1;
                    index = this._usuarioDestino.IndexOf(" ", index);
                    name = this._usuarioDestino.Substring(0, index);
                }
                return name;
            } set { _usuarioDestino = value; } }
       
        public String LetterAvatar { get { return string.IsNullOrEmpty(this.UsuarioDestino) ? "" : this.UsuarioDestino.Substring(0, 1).ToUpper();  } }
        public Int64 IdUsuarioUltimaMensage { get; set; }
        public String UltimaMensagem { get; set; }
        public String ConnectionID { get; set; }

        //private WebUsuario UsuarioDestino { get; set; }
        //private ChatInternoMensagem UltimaMensagem { get; set; }
    }
}
