using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Uteis;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Reflection;

namespace APICeomedAplicacoes.Base
{
    public class ResponseValue<T> : Response 
    {
        private T? _value;
        public ResponseValue(Response responseBase) 
        { 
            this.Value = default(T); 
            this.IsSuccess = responseBase.IsSuccess;
            this.message = responseBase.message;
            this.errors = responseBase.errors;
            this.additionalInfoMessages = responseBase.additionalInfoMessages;
        }
        public ResponseValue() { this.Value = default(T); }

        public ResponseValue(T? value) { Value = value; }
        public List<object> ConverterParaLista<T>(T valor)
        {
            if (valor is IEnumerable<object> lista)
                return lista.ToList();

            return new List<object> { valor };
        }
        public T? Value { get { return _value == null ? default(T) : _value; } 
            set 
            {
                _value = value;
            }
        }

        public void ValidResultValue()
        {
            this.Code = ConverterParaLista(Value).Count == 0 || ConverterParaLista(Value)[0] == null ? 204 : 200;
        }

        public override void AddError(string Mensagem,string ResponseMessage = "Algo deu errado.")
        {
            this.Value = default(T);
            base.AddError(Mensagem, ResponseMessage);
        }

        public static ResponseValue<T> Error()
        {
            return new() { IsSuccess = false, message = "Algo deu errado."};
        }

        public static ResponseValue<T> Success()
        {
            return new() { IsSuccess = true, message = "Sucesso." };
        }

        public ResponseValue<T> VerifyCampos<TP>(TP param)
        {
            ResponseValue<T> response = ResponseValue<T>.Success();
            foreach (var item in param.GetType().GetRuntimeProperties())
            {
                var attribute = (RequiredParam)Attribute.GetCustomAttribute(item, typeof(RequiredParam));

                if (attribute?.Required == true && !BaseRequest<TP>.ValidParam(attribute,item.GetValue(param)))
                {
                    var attribute2 = (ErrorMessage)Attribute.GetCustomAttribute(item, typeof(ErrorMessage));
                    if (attribute2 != null && !string.IsNullOrEmpty(attribute2?.message))
                    {
                        response.AddError(attribute2.message);
                    }
                    else
                    {
                        response.AddError($"Valor invalido em {item.Name}");
                    }
                    this.Value = default(T);
                }

            }
            return response;
        }

       
    }
}
