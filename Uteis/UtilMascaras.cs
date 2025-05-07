namespace APICeomedAplicacoes.Uteis
{
    public static class UtilMascaras
    {
        /// <summary>
        /// Gera a mascara de Telefone para um string contendo os
        /// números correspondentes a um dos dois formatos:
        /// 0 - Celulares de SP (incluindo o digito 9)
        /// 1 - Demais números de telefone
        /// </summary>
        /// <param name="texto">
        /// Texto que contem a sequência de números correspondentes
        /// a um telefone
        /// </param>
        /// <returns>Campo mascarado com telefone</returns>
        public static string GeraMascaraTelefone(string texto)
        {
            if (String.IsNullOrEmpty(texto) || texto == null)
                return string.Empty;

            texto = RemoveMascara(texto);

            // (99) 99999-9999 - celulares do estado de SP
            if (texto.Length > 10)
                return texto.Insert(0, "(").Insert(3, ")").Insert(4, " ").Insert(6, " ").Insert(11, "-");
            //(99) 9999-9999 - Demais números telefonicos
            else
                return texto.Insert(0, "(").Insert(3, ")").Insert(4, " ").Insert(9, "-");
        }

        /// <summary>
        /// Gera a mascara de CEP para um string.
        /// </summary>
        /// <param name="texto">
        /// Texto que contem a sequência de números correspondentes
        /// a um CEP
        /// </param>
        /// <returns>Campo mascarado com CEP</returns>
        public static string GeraMascaraCep(string texto)
        {
            texto = RemoveMascara(texto);

            if (!String.IsNullOrEmpty(texto))
                return texto.Insert(5, "-");
            else
                return String.Empty;
        }

        /// <summary>
        /// Gera a mascara de CPF ou CNPJ para um string contendo os
        /// números correspondentes a um dos dois formatos.
        /// </summary>
        /// <param name="texto">Texto que contem a sequência de números correspondentes
        /// a um CPF ou CNPJ</param>
        /// <returns>Campo mascarado com CPF ou CNPJ</returns>
        public static string GeraMascaraCpfOuCnpj(string texto)
        {
            //limpa máscara do campo
            texto = RemoveMascara(texto);

            //999.999.999-99 - Máscara para CPF
            if (texto.Length.Equals(11))
                texto = texto
                    .Insert(3, ".")
                    .Insert(7, ".")
                    .Insert(11, "-");

            //99.999.999/9999-99 - Máscara para CNPJ
            else if (texto.Length.Equals(14))
                texto = texto.Replace(".", "").Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");

            return texto;
        }

        /// <summary>
        /// Gera a mascara de CNPJ para um string contendo os
        /// números correspondentes a um dos dois formatos.
        /// </summary>
        /// <param name="texto">Texto que contem a sequência de números correspondentes
        /// a um CNPJ</param>
        /// <returns>Campo mascarado com CNPJ</returns>
        public static string GeraMascaraCnpj(string texto)
        {
            //limpa máscara do campo
            texto = RemoveMascara(texto);

            //99.999.999/9999-99 - Máscara para CNPJ
            if (texto.Length.Equals(14))
                texto = texto.Replace(".", "").Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");

            return texto;
        }

        /// <summary>
        /// Gera a mascara de CPF para um string contendo os
        /// números correspondentes a um dos dois formatos.
        /// </summary>
        /// <param name="texto">Texto que contem a sequência de números correspondentes
        /// a um CPF</param>
        /// <returns>Campo mascarado com CPF</returns>
        public static string GeraMascaraCpf(string texto)
        {
            //limpa máscara do campo
            texto = RemoveMascara(texto);

            //999.999.999-99 - Máscara para CPF
            if (texto.Length.Equals(11))
                texto = texto.Insert(3, ".").Insert(7, ".").Insert(11, "-");

            return texto;
        }

        /// <summary>
        /// Gera a mascara do Codigo CNAE.
        /// </summary>
        /// <param name="texto">Texto que contem a sequência de números correspondentes
        /// a um Codigo CNAE</param>
        /// <returns>Campo mascarado com Codigo CNAE</returns>
        public static string GeraMascaraCodigoCNAE(string texto)
        {
            //limpa máscara do campo
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            texto = RemoveMascara(texto);
            texto = texto.Insert(4, "-").Insert(6, "/");
            return texto;
        }

        public static string GeraMascaraPlaca(string texto)
        {
            //limpa máscara do campo
            texto = RemoveMascara(texto);

            //XXX-9999 - Máscara para Placa
            if (!String.IsNullOrEmpty(texto) && texto.Length.Equals(7))
                texto = texto.Insert(3, "-");

            return texto;
        }

        /// <summary>
        /// Gera a mascara de Classificação Fiscal NCM para um string.
        /// </summary>
        /// <param name="texto">Texto que contem a sequência de números correspondentes ao NCM.</param>
        /// <returns>Campo mascarado NCM</returns>
        public static string GeraMascaraNcm(string texto)
        {
            //limpa máscara do campo
            texto = RemoveMascara(texto);

            //0000.00.00 - Máscara para NCM
            if (texto.Length.Equals(8))
                texto = texto
                    .Insert(4, ".")
                    .Insert(7, ".");

            return texto;
        }

        /// <summary>
        /// Remove caracteres de mascaras (".","-","/","\","(",")"), 
        /// e espaços vazios no final da string.
        /// </summary>
        /// <param name="texto">Texto que contém </param>
        /// <returns></returns>
        public static string RemoveMascara(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;

            string retorno =
            texto
               .Trim()
               .Replace(".", "")
               .Replace(",", "")
               .Replace("-", "")
               .Replace("/", "")
               .Replace("(", "")
               .Replace(")", "")
               .Replace(" ", "")
               .Replace(@"\", "");

            return retorno;
        }
    }
}
