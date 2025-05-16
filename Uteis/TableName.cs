namespace APICeomedAplicacoes.Uteis
{
    public class TableName : Attribute
    {

        public TableName(string name)
        {
            this.Name = name;
        }

        public string Name { get;set; } = "";

    }
}
