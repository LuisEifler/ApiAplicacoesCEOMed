using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace APICeomedAplicacoes.Uteis
{
    public enum EStatusAgendamento : long
    {
        [Description("BLOQUEIO AGENDA")]
        BloqueioAgenda = 0,
        [Description("AGENDADO")]
        [AttributeCorStatus("SkyBlue")]
        Agendado = 1,
        [Description("CONFIRMADO")]
        [AttributeCorStatus("192, 255, 192")]
        Confirmado = 2,
        [Description("CANCELADO")]
        [AttributeCorStatus("255, 128, 128")]
        Cancelado = 3,
        [Description("ATRASADO")]
        [AttributeCorStatus("255, 192, 128")]
        Atrasado = 4,
        [Description("EM ATENDIMENTO")]
        [AttributeCorStatus("255, 255, 128")]
        EmAtendimento = 5,
        [Description("NÃO CONFIRMADO")]
        [AttributeCorStatus("Gainsboro")]
        AgendamentoNaoConfirmado = 6,
        [Description("FINALIZADO")]
        [AttributeCorStatus("Turquoise")]
        Finalizado = 7,
        [Description("AGUARDANDO")]
        [AttributeCorStatus("Plum")]
        Aguardando = 8,
        [Description("REAGENDAMENTO")]
        [AttributeCorStatus("White")]
        Reagendado = 9,
        [Description("FALTOU")]
        [AttributeCorStatus("White")]
        Faltou = 10,
        [Description("PENDENTE LIBERAÇÃO/PAGAMENTO")]
        [AttributeCorStatus("LightPink")]
        PendenteLiberacao = 11,
        [Description("DILATANDO")]
        Dilatando = 12,
        [Description(" ")]
        HorarioLivre = 13,
        [Description("PENDENTE EXAMES")]
        [AttributeCorStatus("255, 136, 98")]
        PendenteExames = 14,
        [Description("RETORNO EXAMES")]
        [AttributeCorStatus("Plum")]
        RetornoExames = 15,
        [Description("TRIADO")]
        Triado = 16,
        [Description("REAVALIAÇÃO")]
        Reavaliacao = 17,
        [Description("MEDICAÇÃO")]
        Medicacao = 18,

    }
}
