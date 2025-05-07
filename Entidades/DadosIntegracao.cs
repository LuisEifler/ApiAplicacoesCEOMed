namespace APICeomedAplicacoes.Entidades
{
    public class DadosIntegracao
    {
        public Int64? Id { get; set; }
        public String RazaoSocial{ get; set; }
        public String Cnpj { get; set; }
        public String ApiToken { get; set; }
        public bool Ativo { get; set; }

    }
}
