using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICeomedAplicacoes.Uteis.Enum
{
    public enum ETipoAtendimentoPaciente
    {
        [Description("CONSULTA")]
        Consulta = 1,
        [Description("RETORNO")]
        Retorno = 2,
        [Description("CIRURGIA")]
        Cirurgia = 3,
        [Description("PACOTE DE PROCEDIMENTOS")]
        PacoteProcedimento = 4,
        [Description("SEDAÇÃO")]
        Sedacao = 5,
        [Description("EXAMES/ PROCEDIMENTOS")]
        ExamesProcedimentos = 7,
        [Description("CURATIVO")]
        Curativo = 8,
        [Description("RECEITA MÉDICA")]
        ReceitaMedica = 9,
        [Description("REPRESENTANTE")]
        Representante = 10,
        [Description("RECADO")]
        Recado = 11,
        [Description("PEDIDO EXAMES")]
        PedidoExames = 12,
        [Description(" ")]
        Livre = 13
          
    }
}
