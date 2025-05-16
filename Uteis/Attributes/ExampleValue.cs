namespace APICeomedAplicacoes.Uteis.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExampleValue : Attribute
    {
        public ExampleValue(object value)
        {
            this.Value = value;
            //this.type = value.GetType();
        }

        public object Value { get; set; } = string.Empty;
        //public Type type { get; set; } = typeof(string);
    }
}
