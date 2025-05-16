namespace APICeomedAplicacoes.Uteis
{
    public class ColumnInfo : Attribute
    {

        public ColumnInfo(string name,ETipoUpdate tipoUpdate = ETipoUpdate.Override)
        {
            this.Name = name;
            this.UpdateType = tipoUpdate;
        }

        public string Name { get; set; }
        public ETipoUpdate UpdateType { get; set; }

    }
}
