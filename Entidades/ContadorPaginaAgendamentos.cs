using APICeomedAplicacoes.Entidades;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace APICeomedAplicacoes.Entidades
{
    public class ContadorPaginaAgendamentos
    {
        public ContadorPaginaAgendamentos(string title, object? value)
        {
            this.title = title;
            this.value = value;
        }

        public string title { get; set; }
        public object? value { get; set; }


    };
}