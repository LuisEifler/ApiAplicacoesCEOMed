namespace APICeomedAplicacoes.Base
{
    public class ValueError
    {
        public ValueError() { }
        public ValueError(string message) 
        { 
            this.message = message;
            //this.value = value;
        }
        public string? message { get; set; }
        //public string? value { get; set; }

    }
}
