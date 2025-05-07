using System.ComponentModel;
using System.Globalization;

public class NullableIntConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string);

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        var str = value as string;

        if (string.IsNullOrWhiteSpace(str) || str.Trim().ToLower() == "null")
            return null;

        if (int.TryParse(str, out var result))
            return result;

        throw new FormatException($"O valor '{str}' não é um número inteiro válido.");
    }
}
