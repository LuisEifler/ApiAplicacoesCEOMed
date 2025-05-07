using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Uteis;
using System.Data;

namespace APICeomedAplicacoes.Entidades
{
    public class Paciente : TableBase
    {
        public String Nome { get; set; }
        public String CpfCnpj { get; set; }
        public Byte? Sexo { get; set; }
        public String StringSexo { get { return this.Sexo == 1 ? "Feminino" : this.Sexo == 0 ? "Masculino" : ""; } }
        public DateTime? DataNascimento { get; set; }
        public String Profissao { get; set; }
        public String Idade
        {
            get
            {
                string Stringidade = "";
                if (this.DataNascimento.HasValue)
                {
                    Stringidade = this.DataNascimento.Value.CalculaIdade();
                }
                return Stringidade;
            }
        }
        public Int64 IdEndereco { get; set; }
        public String Cep { get; set; }
        public Int64? IdEstado { get; set; }
        public String UF { get; set; }
        public String EstadoDescricao { get; set; }
        public Int64? IdCidade { get; set; }
        public String Cidade { get; set; }
        public String Endereco { get; set; }
        public String Bairro { get; set; }
        public String Numero { get; set; }
        public String Complemento { get; set; }

        public String TelefoneResidencial { get; set; }
        public String TelefoneCelular { get; set; }
        public bool WhatsApp { get; set; }
        public String Email { get; set; }
        public String Alerta { get; set; }
        public String Observacao { get; set; }
        public String NomeCompleto { get; set; }
        public Int64? IdConvenio { get; set; }
        public String DescricaoConvenio { get; set; }
        public String Profissional { get; set; }
        public Int64? FuncionarioId { get; set; }
        public String NumeroCarteirinha { get; set; }
        public DateTime? DataCadastro { get; set; }
        public String Titulo
        {
            get
            {
                string titulo = "";
                if (this.Id != 0)
                {
                    titulo += this.Nome;
                    titulo += this.DataNascimento == null ? "" : ", " + this.Idade;
                    titulo += this.Sexo == null? "" : ", " + this.StringSexo;
                    titulo += UtilMascaras.GeraMascaraTelefone(this.TelefoneCelular) == "" ? "" : ", " + UtilMascaras.GeraMascaraTelefone(this.TelefoneCelular);
                    titulo += this.DescricaoConvenio == "" || this.DescricaoConvenio == null ? "" : ", " + this.DescricaoConvenio;
                }
                return titulo;

            }
        }

        public DateTime? UltimoAtendimento { get; set; }
    }

 }
