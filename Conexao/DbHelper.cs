using System.Collections;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Entidades;
using System.Drawing;
using APICeomedAplicacoes.Uteis;
using System.Diagnostics;

namespace APICeomedAplicacoes.Conexao
{
    public static class DbHelper
    {
        readonly static String connStringBancoPrincipal = "Server=ceomed.ddns.net,30232;Database=CEO_Principal;User Id=ceo_conexao;Password=qweEWQ150)%!C0n3X@o";
        private static string connStringBancoCliente = "";
        public static void SetDbClienteConection(Int64? IdClinica)
        {
            UseDbPrincipal();
            TabBancos tab = Select<TabBancos>(x => x.tbCodigo == IdClinica.Value).FirstOrDefault();
            connStringBancoCliente = tab.StringConexaoFormatada;
            UseDbCliente();
        }

        public static void SetDbConection(string connString)
        {
            connStringBancoCliente = connString;
            UseDbCliente();
        }

        public static void UseDbCliente()
        {
            Conection = new DbConection(connStringBancoCliente);

        }

        public static bool TestConection()
        {
            return Conection.TestConnection();
        }

        public static void UseDbPrincipal()
        {
            Conection = new DbConection(connStringBancoPrincipal);
        }

        public static List<T> SelectNoBancoPrincipal<T>(Expression<Func<T, bool>> where = null, params Expression<Func<T, object>>[] props)
        {
            UseDbPrincipal();
            List<T> lista = Select<T>(where, props);
            Conection = new DbConection(connStringBancoCliente);
            return lista;
        }

        public static DbConection Conection = new DbConection(connStringBancoPrincipal);

        public static object ExecuteScalar(string query, Hashtable param,bool proc = false)
        {
            return Conection.ExecuteScalar(query, param, proc);
        }

        public static List<T> GetTable<T>(string query, Hashtable param,bool procedure = false)
        {

            DataTable dt = Conection.GetDataTable(query, param, procedure);
            return Util.MontarLista<T>(dt);
        }

        public static DataTable GetTable(string query, Hashtable param, bool procedure = false)
        {

            DataTable dt = Conection.GetDataTable(query, param, procedure);
            return dt;
        }

        /// <summary>
        /// <typeparam name="T">Tipo de dado base para direcionamento da tabela que será selecionada</typeparam>
        /// <param name="where">expressões de comparação que serão convertidas em uma clausula where caso nenhuma seja informada todos os registros serão selecionados</param>
        /// <param name="props">lista de propriedades a serem selecionadas caso nenhuma seja informada todas serão selecionadas</param>
        /// <returns></returns>
        /// </summary>
        public static List<T> Select<T>(Expression<Func<T, bool>> where = null, params Expression<Func<T, object>>[] props)
        {
            if (props != null) props.ToString();

            string query = GetSelectComand<T>(props);

            if (where != null)
                query += "WHERE " + ParseExpression(where.Body, "");

            return DbHelper.GetTable<T>(query, null);
        }

        public static List<T> SelectWithPagination<T>(Expression<Func<T, bool>> where = null,int page = 0,int limitPerPage = 10,params Expression<Func<T, object>>[] props)
        {
            if (props != null) props.ToString();

            string query = GetSelectComand<T>(props);

            if (where != null)
                query += "WHERE " + ParseExpression(where.Body, "");

            query += $" ORDER BY Id DESC OFFSET @page * @limit ROWS FETCH NEXT @limit ROWS ONLY; ";

            Hashtable param = new Hashtable()
            {
                {"@page",page },
                {"@limit", limitPerPage}
            };

            return DbHelper.GetTable<T>(query, param);
        }

        public static T SelectById<T>(Int64 Id)
        {
            string query = GetSelectComand<T>() + $"WHERE Id = {Id}";

            return DbHelper.GetTable<T>(query, null).Count == 0 ? Util.CriarInstancia<T>() : DbHelper.GetTable<T>(query, null).First();
        }

        private static string GetOperator(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                default:
                    return expressionType.ToString();
            }
        }

        private static string ParseExpression(Expression expression, string str)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                if (binaryExpression.NodeType == ExpressionType.AndAlso || 
                    binaryExpression.NodeType == ExpressionType.OrElse ||
                    binaryExpression.NodeType == ExpressionType.And||
                    binaryExpression.NodeType == ExpressionType.Or

                    )
                {
                    str = ParseExpression(binaryExpression.Left, str);
                    str += binaryExpression.NodeType == ExpressionType.AndAlso || binaryExpression.NodeType == ExpressionType.And ? " AND " : " OR ";
                    str = ParseExpression(binaryExpression.Right, str);
                }
                else
                {
                    var leftMember = (binaryExpression.Left as MemberExpression)?.Member?.Name;
                    var rightValue = GetValue(binaryExpression.Right);
                    var operador = GetOperator(binaryExpression.NodeType);

                    operador = rightValue == null && operador == "=" ? " IS " :
                               rightValue == null && operador == "!=" ? " IS NOT " :
                               operador;

                    string formattedRight = rightValue == null ? "NULL" : FormatValue(rightValue);

                    str += $"{leftMember} {operador} {formattedRight}";
                }
            }
            else
            {
                throw new NotSupportedException($"Unsupported expression type: {expression.GetType().Name}");
            }

            return str;
        }


        private static object GetValue(Expression expression)
        {
            switch (expression)
            {
                case ConstantExpression constant:
                    return constant.Value;

                case MemberExpression member:
                    // Se for campo ou propriedade
                    if (member.Expression is ConstantExpression constantExpression)
                    {
                        var fieldInfo = member.Member as FieldInfo;
                        var propertyInfo = member.Member as PropertyInfo;

                        if (fieldInfo != null)
                            return fieldInfo.GetValue(constantExpression.Value);

                        if (propertyInfo != null)
                            return propertyInfo.GetValue(constantExpression.Value);
                    }
                    else
                    {
                        // Pode ser uma variável, parâmetro ou algo mais complexo
                        var lambda = Expression.Lambda(member);
                        var func = lambda.Compile();
                        return func.DynamicInvoke();
                    }

                    break;

                case UnaryExpression unary:
                    // Caso seja uma conversão, pega o operando
                    return GetValue(unary.Operand);

                default:
                    var compiled = Expression.Lambda(expression).Compile();
                    return compiled.DynamicInvoke();
            }

            throw new NotSupportedException($"Não foi possível extrair o valor da expressão: {expression}");
        }


        private static string FormatValue(object value)
        {
            if (value is string || value is DateTime)
                return $"'{value}'";
            if (value is bool b)
                return b ? "1" : "0";
            return value.ToString();
        }



        private static string ParseExpressionWithInner(Expression expression, string str)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                if (binaryExpression.NodeType == ExpressionType.AndAlso || binaryExpression.NodeType == ExpressionType.OrElse)
                {
                    str = ParseExpressionWithInner(binaryExpression.Left, str);
                    str += binaryExpression.NodeType == ExpressionType.AndAlso ? " AND " : " OR ";
                    str = ParseExpressionWithInner(binaryExpression.Right, str);
                }
                else
                {

                    var left = binaryExpression.Left.NodeType == ExpressionType.Convert ? ((UnaryExpression)binaryExpression.Left).Operand as MemberExpression : binaryExpression.Left as MemberExpression;
                    string propertyName = $"{left?.Member.ReflectedType.Name}.{left?.Member.Name}";

                    string operador = GetOperator(binaryExpression.NodeType);

                    var right = Expression.Lambda(binaryExpression.Right).Compile().DynamicInvoke();

                    operador = right == null && operador == "=" ? " IS " : right == null && operador == "!=" ? " IS NOT " : operador;
                    right = right == null ? "NULL" : "'" + right + "'";

                    str += $"{propertyName} {operador} {right}";
                }
            }
            return str;
        }

        public static (string query, Hashtable param) MontarInsert<T>(T obj)
        {
            string query = "INSERT INTO " + obj.GetType().Name + " ";
            string coloumns = "";
            string values = "";
            Hashtable param = new Hashtable();
            foreach (var item in obj.GetType().GetRuntimeProperties())
            {
                if (item.Name == "Id") continue;
                if (item.GetMethod.IsVirtual || item.SetMethod.IsNotNull() && item.SetMethod.IsVirtual) continue;

                if (coloumns == "") coloumns = "(" + item.Name;
                else coloumns += "," + item.Name;

                if (values == "") values = "(" + "@" + item.Name;
                else values += "," + "@" + item.Name;

                param.Add("@" + item.Name, item.GetValue(obj) == null ? (object)DBNull.Value : item.GetValue(obj));
            }

            coloumns += ") ";
            values += ") ";

            query += coloumns + " VALUES " + values + "; SELECT SCOPE_IDENTITY();";

            return (query, param);

        }

        public static string GetSelectComand<T>(params Expression<Func<T, object>>[] props)
        {
            string campos = "";

            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    var member = (prop.Body.NodeType == ExpressionType.Convert ? ((UnaryExpression)prop.Body).Operand as MemberExpression : prop.Body as MemberExpression).Member;

                    if (member.Name == typeof(T).Name)
                    {
                        campos = "*";
                        continue;
                    }
                    campos += campos == "" ? member.Name : $", {member.Name}";
                }
            }
            else
            {
                campos = "*";
            }


            return $"SELECT {campos} FROM {typeof(T).Name} (NOLOCK) ";
        }

        public static string GetSelectComandFromInner<TBSelect, TBInner>(params Expression<Func<TBSelect, TBInner, object>>[] props)
        {
            string campos = "";

            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    string name = "";
                    if (prop.Body.NodeType == ExpressionType.Convert)
                        name = $"{(((UnaryExpression)prop.Body).Operand as MemberExpression).Member.ReflectedType.Name}.{(((UnaryExpression)prop.Body).Operand as MemberExpression).Member.Name}";
                    else if (prop.Body.NodeType == ExpressionType.MemberAccess)
                        name = $"{(prop.Body as MemberExpression).Member.ReflectedType.Name}.{(prop.Body as MemberExpression).Member.Name}";
                    else if (prop.Body.NodeType == ExpressionType.Parameter)
                        name = (prop.Body as ParameterExpression).Type.Name;

                    if (name == typeof(TBSelect).Name)
                    {
                        campos = $"{name}.*";
                        continue;
                    }
                    campos += campos == "" ? $"{name}" : $", {name}";
                }
            }
            else
            {
                campos = "*";
            }

            return $"SELECT {campos} FROM {typeof(TBSelect).Name} (NOLOCK) ";
        }

        public static (string query, Hashtable param) MontarUpdate<T>(T obj)
        {
            string query = "UPDATE " + obj.GetType().Name + " ";
            string values = "";
            string where = "";
            Hashtable param = new Hashtable();
            foreach (var item in obj.GetType().GetRuntimeProperties())
            {
                if (item.GetMethod.IsVirtual) continue;

                param.Add("@" + item.Name, item.GetValue(obj));
                if (item.Name == "Id")
                {
                    where = $"WHERE Id = @{item.Name}";
                    continue;
                }

                if (values == "") values = $"SET {item.Name} = @{item.Name} ";
                else values += $", {item.Name} = @{item.Name} ";
            }

            query += values + where;

            return (query, param);

        }

        public static (string query, Hashtable param) MontarDelete<T>(T obj)
        {
            string query = "DELETE " + obj.GetType().Name + " ";
            string where = "WHERE Id = @Id";
            Hashtable param = new Hashtable();
            param.Add("@Id", obj.GetType().GetProperty("Id").GetValue(obj));
            query += where;
            return (query, param);

        }

        public static void UpdateWhere<T>(Expression<Func<T, bool>> where, params (Expression<Func<T, object>> propExpr, object valor)[] props)
        {
            string query = "UPDATE " + typeof(T).Name + " ";
            string values = "";
            string whereString = " WHERE ";
            Hashtable param = new Hashtable();
            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    var member = (prop.propExpr.Body.NodeType == ExpressionType.Convert ? ((UnaryExpression)prop.propExpr.Body).Operand as MemberExpression : prop.propExpr.Body as MemberExpression).Member;

                    param.Add("@" + member.Name, prop.valor);

                    if (values == "") values = $"SET {member.Name} = @{member.Name} ";
                    else values += $", {member.Name} = @{member.Name}";

                }

                whereString += ParseExpression(where.Body, "");

                query += values + whereString;

                query.ToString();

                ExecuteScalar(query, param);
            }

        }

        public static List<TypeResult> SelectWithInnerNoBancoPrincipal<TypeResult, TBSelect, TBInner>(Expression<Func<TBSelect, TBInner, bool>> on, Expression<Func<TBSelect, TBInner, bool>> where = null)
        {
            UseDbPrincipal();
            return SelectWithInner<TypeResult, TBSelect, TBInner>(on, where);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeResult">Tipo de resultado esperado</typeparam>
        /// <typeparam name="TBSelect">Classe que representa a tabela principal onde o inner sera executado</typeparam>
        /// <typeparam name="TBInner">Classe que representa a tabela secundaria que sera vinculada a tabela principal</typeparam>
        /// <param name="on">Lambda que indica o vinculo entre as duas tabelas</param>
        /// <param name="where">Lambda que indica os filtros a serem aplicados no select</param>
        /// <param name="props">Arrays de Lambdas que indica as propriedades a serem selecionadas de cada tabela</param>
        /// <returns></returns>
        public static List<TypeResult> SelectWithInner<TypeResult, TBSelect, TBInner>(Expression<Func<TBSelect, TBInner, bool>> on, 
            Expression<Func<TBSelect, TBInner, bool>> where = null, 
            params Expression<Func<TBSelect, TBInner, object>>[] props)
        {
            string query = GetSelectComandFromInner(props);
            string Inner = "";
            if (on.Body is BinaryExpression binaryExpression)
            {
                var left = binaryExpression.Left.NodeType == ExpressionType.Convert ? ((UnaryExpression)binaryExpression.Left).Operand as MemberExpression : binaryExpression.Left as MemberExpression;
                string operador = GetOperator(binaryExpression.NodeType);
                var right = binaryExpression.Right.NodeType == ExpressionType.Convert ? ((UnaryExpression)binaryExpression.Right).Operand as MemberExpression : binaryExpression.Right as MemberExpression;

                Inner += $" INNER JOIN {typeof(TBInner).Name} ON {typeof(TBSelect).Name}.{left.Member.Name} {operador} {typeof(TBInner).Name}.{right.Member.Name} ";

            }
            string Where = "";
            if (where != null)
                Where = "WHERE " + ParseExpressionWithInner(where.Body, "");

            query += Inner + Where;

            List<TypeResult> resut = GetTable<TypeResult>(query, null);

            return resut;
        }

        public static int GetCount<T>(Expression<Func<T, bool>> where = null,Expression < Func<T, object>> prop = null)
        {

            string query = GetSelectComand<T>();

            if (prop == null)
                query = query.Replace("*", "COUNT(*) Contagem");
            else
            {
                var member = (prop.Body.NodeType == ExpressionType.Convert ? ((UnaryExpression)prop.Body).Operand as MemberExpression : prop.Body as MemberExpression).Member;

                if (member.Name == typeof(T).Name)
                {
                    query = query.Replace("*", "COUNT(*) Contagem");
                }
                else
                {
                    query = query.Replace("*", $"COUNT({prop}) Contagem");
                }
            }

            if (where != null)
                query += "WHERE " + ParseExpression(where.Body, "");

            return (int)Conection.GetDataTable(query, null, false).Rows[0].ValueToInt("Contagem");
        }
    }

    public static class Util
    {

        public static List<KeyValuePair<string, int>> GetDataSource(Type enumType)
        {
            List<KeyValuePair<string, int>> lista = new List<KeyValuePair<string, int>>();
            Array EnumValues = Enum.GetValues(enumType);

            foreach (Enum en in EnumValues)
            {
                KeyValuePair<string, int> kv = new KeyValuePair<string, int>(en.Descricao(), Convert.ToInt32(en));
                lista.Add(kv);
            }
            return lista;
        }

        public static List<Tipo> MontarLista<Tipo>(DataTable dt)
        {
            List<Tipo> list = new List<Tipo>();
            if(dt is not null)
                foreach (DataRow row in dt?.Rows) list.Add(MapType<Tipo>(row));
            return list;
        }

        public static Tipo MapType<Tipo>(DataRow row)
        {
            Tipo resultObj = CriarInstancia<Tipo>();

            foreach (var item in resultObj.GetType().GetRuntimeProperties())
            {
                if (!row.ExistsAndIsNotNull(item.Name)) continue;

                Type tipo = item.PropertyType;
                Type tipoSubjacente = Nullable.GetUnderlyingType(tipo) ?? tipo;

                var valor = row[item.Name];
                if (Nullable.GetUnderlyingType(tipo) != null && tipo != typeof(string))
                    valor = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(tipoSubjacente), valor ?? GetDefaultValue(tipoSubjacente));

                if (tipo == typeof(string))
                    item.SetValue(resultObj, valor.ToString());
                else
                    item.SetValue(resultObj, valor);

            }
            return resultObj;
        }

        public static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                return null;
            }
        }

        public static Tipo CriarInstancia<Tipo>(params object[] parametros)
        {
            return (Tipo)Activator.CreateInstance(typeof(Tipo), parametros);
        }

        public static bool ExistsAndIsNotNull(this DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName))
                return false;

            return row[columnName] != DBNull.Value;
        }

        public static string ValueToString(this DataRow row, string columnName, string defaulValue)
        {
            string value = defaulValue;

            if (row.ExistsAndIsNotNull(columnName))
                value = row[columnName].ToString();

            return value;
        }

        public static string ValueToString(this DataRow row, string columnName)
        {
            return row.ValueToString(columnName, null);
        }

        public static int ValueToInt(this DataRow row, string columnName, int defaulValue)
        {
            int value = defaulValue;

            if (row.ExistsAndIsNotNull(columnName))
                int.TryParse(row[columnName].ToString(), out value);

            return value;
        }

        public static int ValueToInt(this DataRow row, string columnName)
        {
            return row.ValueToInt(columnName, 0);
        }

        public static float ValueToFloat(this DataRow row, string columnName, float defaulValue)
        {
            float value = defaulValue;

            if (row.ExistsAndIsNotNull(columnName))
                float.TryParse(row[columnName].ToString(), out value);

            return value;
        }

        public static float ValueToFloat(this DataRow row, string columnName)
        {
            return row.ValueToFloat(columnName, 0);
        }

        public static decimal ValueToDecimal(this DataRow row, string columnName, decimal defaulValue)
        {
            decimal value = defaulValue;

            if (row.ExistsAndIsNotNull(columnName))
                decimal.TryParse(row[columnName].ToString(), out value);

            return value;
        }

        public static decimal ValueToDecimal(this DataRow row, string columnName)
        {
            return row.ValueToDecimal(columnName, 0);
        }

        public static byte[] ValueToByteArray(this DataRow row, string columnName, byte[] defaulValue)
        {
            byte[] value = defaulValue;

            if (row.ExistsAndIsNotNull(columnName))
                value = (byte[])row[columnName];

            return value;
        }

        public static byte[] ValueToByteArray(this DataRow row, string columnName)
        {
            return row.ValueToByteArray(columnName, new byte[0]);
        }

        public static bool ValueToBool(this DataRow row, string columnName)
        {
            bool value = false;

            if (row.ExistsAndIsNotNull(columnName))
                bool.TryParse(row[columnName].ToString(), out value);

            return value;
        }

        public static DateTime ValueToDateTime(this DataRow row, string columnName)
        {
            //DateTime value = default;
            DateTime value = DateTime.MinValue;

            if (row.ExistsAndIsNotNull(columnName))
                DateTime.TryParse(row[columnName].ToString(), out value);

            return value;
        }

        public static bool ValidarCpf(string cpf)
        {
            if (cpf.Length != 11 || !long.TryParse(cpf, out _) || cpf == "00000000000")
            {
                return false;
            }

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }

            int resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        public static Color GetColor(string color)
        {
            Color ColorRetorno = Color.Empty;
            if (color == "") return ColorRetorno;
            if (color.Contains(","))
            {
                string[] argb = color.Split(',');
                if (argb.Count() == 3)
                {
                    int r = Convert.ToInt32(argb[0]);
                    int g = Convert.ToInt32(argb[1]);
                    int b = Convert.ToInt32(argb[2]);
                    ColorRetorno = Color.FromArgb(r, g, b);
                }
            }
            else
            {
                ColorRetorno = Color.FromName(color);
            }
            return ColorRetorno;
        }

        public static Color GetColorFromStatus(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (AttributeCorStatus)Attribute.GetCustomAttribute(field, typeof(AttributeCorStatus));
            return attribute == null ? Color.White : Util.GetColor(attribute.Cor);
        }

        public static List<ContadorPaginaAgendamentos> GetContadores(this List<Agendamento> list)
        {
            return new List<ContadorPaginaAgendamentos>()
            {
                new ("Procedimentos",
                list.Where(x => x.TipoAtendimentoPaciente == 7 || x.TipoAtendimentoPaciente == 4).Sum(x => x.QuantidadeProcedimento)),
                new ("Consultas",
                list.Where(x => x.TipoAtendimentoPaciente == 1).Count()),
                new ("Retornos",
                list.Where(x => x.TipoAtendimentoPaciente == 2 ).Count())
            };
        }

        public static void Log(string message, ConsoleColor color = ConsoleColor.White)
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;

            Debug.WriteLine(message);
        }

        public static string FristLetterLower(this string str)
        {
            str = str.Substring(0, 1).ToLower() + str.Substring(1);
            return str;
        }

        public static int ExtractIdFromUrl(this string url, int segmentIndex)
        {
            var segments = url.Split('/');
            if (segments.Length > segmentIndex && int.TryParse(segments[segmentIndex], out int id))
            {
                return id;
            }
            return -1;
        }
        internal static FormUrlEncodedContent ToEncondedContent<T>(this T obj)
        {
            return new FormUrlEncodedContent(obj.ToKeyValueList());
        }
        internal static Dictionary<string, string> ToKeyValueList<T>(this T obj)
        {
            return obj.GetType()
              .GetProperties()
              .ToDictionary(
                x => x.Name,
                x => x.GetValue(obj, null).ToString()
              );
        }

        internal static string ToJson<T>(this T obj, bool indent = false)
        {
            if (indent) return JsonConvert.SerializeObject(obj, Formatting.Indented);
            return JsonConvert.SerializeObject(obj);
        }
        internal static async Task<T> ToObject<T>(this HttpContent content)
        {
            T value = default;

            var result = await content.ReadAsStringAsync();
            try { value = JsonConvert.DeserializeObject<T>(result); }
            catch (JsonSerializationException) { }

            return value;
        }
    }

    public static class Extencions
    {
        /// <summary>
        /// Retorna o Atributo Description de um Enumerator.
        /// </summary>
        /// <param name="currentEnum">Este Enumerator.</param>
        /// <returns></returns>
        public static string Descricao(this Enum currentEnum)
        {
            if (currentEnum.Equals(null))
                return "";
            string description;

            DescriptionAttribute da;
            FieldInfo fi = currentEnum.GetType().GetField(currentEnum.ToString());

            if (fi == null) return "";

            da = (DescriptionAttribute)Attribute.GetCustomAttribute
                (fi, typeof(DescriptionAttribute));

            if (da != null)
                description = da.Description;
            else
                description = currentEnum.ToString();

            return description;
        }
        public static string DescricaoDireta(Enum currentEnum)
        {
            if (currentEnum.Equals(null))
                return "";
            string description;

            DescriptionAttribute da;
            FieldInfo fi = currentEnum.GetType().GetField(currentEnum.ToString());

            da = (DescriptionAttribute)Attribute.GetCustomAttribute
                (fi, typeof(DescriptionAttribute));

            if (da != null)
                description = da.Description;
            else
                description = currentEnum.ToString();

            return description;
        }
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return !obj.IsNull();
        }

        public static bool ValidarEmail(this String email)
        {
            if (email.Contains("@gmail.com")) return true;
            if (email.Contains("@outlook.com")) return true;
            if (email.Contains("@hotmail.com")) return true;
            return false;
        }

        public static Int32 ToInt32(this String str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {
                return -1;
            }
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static string SimNao(this bool Boolean)
        {
            return Boolean ? "Sim" : "Não";
        }

        public static decimal ToDecimal(this String str)
        {
            return Convert.ToDecimal(str);
        }

        public static double ToDouble(this String str)
        {
            return Convert.ToDouble(str);
        }

        public static DateTime FirtDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }

        public static bool Between(this DateTime dt, DateTime dt1, DateTime dt2)
        {
            return (dt >= dt1 && dt <= dt2);
        }
        
        public static List<string> GetParts(this string param, int partLength = 16)
        {

            List<string> list = new List<string>();
            string temp = "";
            for (int i = 0; i < param.Length; i++)
            {
                temp += param[i];

                if (temp.Length == partLength)
                {
                    list.Add(temp);
                    temp = "";
                }
            }

            if (temp != "")
            {
                temp += new string('$', partLength - (temp.Length % partLength));
                list.Add(temp);
            }

            return list;
        }

        public static string ToHexadecimal(this Color cor)
        {
            string hexColor = $"#{cor.R:X2}{cor.G:X2}{cor.B:X2}";

            return hexColor;
        }

        public static string GetRgbStringColor(this Color color)
        {

            if (color == Color.Empty) return "";

            string rgbString = "";

            rgbString += color.R.ToString() + ", ";
            rgbString += color.G.ToString() + ", ";
            rgbString += color.B.ToString();

            return rgbString;

        }

    }

    public class DbConection
    {
        private SqlConnection _conn;
        public bool OnTransaction = false;
        public SqlTransaction Transaction;

        public DbConection(string conString)
        {
            _conn = new SqlConnection(conString);
        }
        internal ConCeoMed RetornaConexaoCliente(string token)
        {

            var query = "dbo.PROC_AgendamentoOnline_RetornaConexaoCliente";
            Hashtable param = new Hashtable();

            param.Add("@token", token);

            DataTable dt = GetDataTable(query, param, true);

            ConCeoMed conexaoClienteDto = new ConCeoMed()
            {
                StringConexao = JsonConvert.DeserializeObject<ConectionJson>(dt.Rows[0]["StringConexao"].ToString()),
                CNPJ = dt.Rows[0]["CNPJ"].ToString()
            };

            return conexaoClienteDto;
        }
        public bool TestConnection()
        {
            try
            {
                DataTable dt;
                dt = GetDataTable("SELECT GETDATE()");
                return true;
            }
            catch
            {
                throw;
            }
        }

        public DataSet GetDataSet(SqlCommand cmd)
        {
            try
            {
                var ds = new DataSet();

                OpenConnection(cmd);

                var dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(ds);

                CloseConnection();

                dataAdapter = null;

                return ds;
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetDataTable(SqlCommand cmd)
        {
            try
            {
                DataTable dt = null;
                var ds = GetDataSet(cmd);
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];

                return dt;
            }
            catch
            {
                throw;
            }
        }

        public DataSet GetDataSet(string query, Hashtable param, bool isProcedure)
        {
            try
            {
                var cmd = FromQueryToSqlCommand(query, isProcedure);
                AddParamsToSqlCommand(cmd, param);
                return GetDataSet(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataTable(string query)
        {
            return GetDataTable(query, null, false);
        }

        public DataTable GetDataTable(string query, bool isProcedure)
        {
            return GetDataTable(query, null, isProcedure);
        }

        public DataTable GetDataTable(string query, Hashtable param, bool isProcedure)
        {
            try
            {
                var cmd = FromQueryToSqlCommand(query, isProcedure);
                AddParamsToSqlCommand(cmd, param);
                return GetDataTable(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteScalar(string query, Hashtable param)
        {
            return ExecuteScalar(query, param, false);
        }

        public object ExecuteScalar(string query, Hashtable param, bool isProcedure)
        {
            try
            {
                var cmd = FromQueryToSqlCommand(query, isProcedure);
                AddParamsToSqlCommand(cmd, param);
                return ExecuteScalar(cmd);
            }
            catch
            {
                throw;
            }
        }

        public object ExecuteScalar(SqlCommand cmd)
        {
            try
            {
                object result = null;

                OpenConnection(cmd);
                result = cmd.ExecuteScalar();
                CloseConnection();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public int ExecuteNonQueryProcedure(string query, Hashtable param)
        {
            try
            {
                var cmd = FromQueryToSqlCommand(query, true);
                AddParamsToSqlCommand(cmd, param);
                return ExecuteNonQuery(cmd);
            }
            catch
            {
                throw;
            }
        }

        private int ExecuteNonQuery(SqlCommand cmd)
        {
            try
            {
                int result = 0;

                OpenConnection(cmd);
                result = cmd.ExecuteNonQuery();
                CloseConnection();

                return result;
            }
            catch
            {
                throw;
            }
        }


        public void OpenTransaction()
        {
            try
            {
                if (_conn.State == ConnectionState.Closed) _conn.Open();
                Transaction = _conn.BeginTransaction();
                OnTransaction = true;
                // Set_xact_abort_on_database();
            }
            catch
            {
                throw;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                if (OnTransaction) Transaction.Commit();
                OnTransaction = false;
                CloseConnection();
            }
            catch
            {
                throw;
            }
        }

        public void AbortTransaction()
        {
            try
            {
                if (OnTransaction) Transaction.Rollback();
                OnTransaction = false;
                CloseConnection();
            }
            catch
            {
                throw;
            }
        }

        public void DestroyConnection()
        {
            AbortTransaction();
            _conn.Dispose();
            _conn = null;
        }

        #region Private methods

        private void CloseConnection()
        {
            if (!OnTransaction && _conn.State == ConnectionState.Open) _conn.Close();
        }

        private void OpenConnection(SqlCommand cmd)
        {
            if (cmd == null) throw new ArgumentNullException("cmd");
            if (OnTransaction) cmd.Transaction = Transaction;
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();

            cmd.Connection = _conn;
        }

        private SqlCommand FromQueryToSqlCommand(string query, bool isProc)
        {
            return isProc ? new SqlCommand(query) { CommandType = System.Data.CommandType.StoredProcedure } : new SqlCommand(query) { CommandType = System.Data.CommandType.Text };
        }

        private void AddParamsToSqlCommand(SqlCommand cmd, Hashtable param)
        {
            if (param != null && cmd != null)
            {
                foreach (DictionaryEntry item in param)
                {
                    object paramValue = item.Value;
                    if (paramValue == null)
                        paramValue = DBNull.Value;

                    cmd.Parameters.Add(new SqlParameter(item.Key.ToString(), paramValue));
                }
            }
        }

        #endregion
    }

    public class TableBase
    {
        public Int64? Id { get; set; }

        public virtual Int64 Insert()
        {
            (string, Hashtable) ObjInsert = DbHelper.MontarInsert(this);
            DataTable obj = DbHelper.Conection.GetDataTable(ObjInsert.Item1, ObjInsert.Item2, false);
            return Convert.ToInt64(obj.Rows[0][0]);
        }

        public virtual void Update()
        {
            (string, Hashtable) ObjUpdate = DbHelper.MontarUpdate(this);

            DbHelper.ExecuteScalar(ObjUpdate.Item1, ObjUpdate.Item2);
        }

        public virtual void Delete()
        {
            (string, Hashtable) ObjUpdate = DbHelper.MontarDelete(this);

            DbHelper.ExecuteScalar(ObjUpdate.Item1, ObjUpdate.Item2);
        }

    }

    class TabBancos
    {
        public Int64 tbCodigo { get; set; }
        public string tbName { get; set; }
        public string tbSituacao { get; set; }
        public string NomeClinica { get; set; }
        public string StringConexao { get; set; }

        public virtual string StringConexaoFormatada
        {
            get
            {
                if (StringConexao.IsNullOrEmpty())
                {
                    return $"Server=ceomed.ddns.net:30232;Database={tbName};User Id=ceo_sa;Password=qweEWQ150)%!sA2";
                }
                else
                {
                    Conn conn = JsonConvert.DeserializeObject<Conn>(StringConexao);
                    return $"Server={conn.ip},{conn.porta};Database={conn.banco};User Id={conn.usuario};Password={conn.senha}";
                }
            }
        }
    }

    class Conn
    {
        public string ip { get; set; }
        public string banco { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
        public string porta { get; set; }

    }
}
