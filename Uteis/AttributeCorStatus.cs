using APICeomedAplicacoes.Conexao;
using System.Drawing;

namespace APICeomedAplicacoes.Uteis
{
    public class AttributeCorStatus : Attribute
    {
        private Color Color { get; set; }
        public string Cor { get { return this.Color.GetRgbStringColor(); } set { this.Color = Util.GetColor(value); } }

        public AttributeCorStatus(string cor) { this.Cor = cor; }

    }
}
