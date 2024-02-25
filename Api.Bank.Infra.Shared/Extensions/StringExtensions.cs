using System.Text.RegularExpressions;

namespace Api.Bank.Infra.Shared
{
    /// <summary>
    /// Extensões para <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Extensão que valida se o valor é um CPF.
        /// </summary>
        /// <param name="value">Objeto referenciado.</param>
        /// <returns>Verdadeiro caso o valor seja um CPF e falso caso contrário.</returns>
        public static bool IsCpf(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var document = value.Replace(".", string.Empty).Replace("-", string.Empty).Trim();

            if (document.Length != 11)
            {
                return false;
            }

            if (document.ToArray().Distinct().Count() <= 1)
            {
                return false;
            }

            var firstMultiplierCollection = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var secondMultiplierCollection = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var documentNumber = document.Substring(0, 9);
            var sum = 0;

            for (var i = 0; i < 9; i++)
            {
                if (!int.TryParse(documentNumber[i].ToString(), out int intValue))
                {
                    return false;
                }

                sum += intValue * firstMultiplierCollection[i];
            }

            var rest = sum % 11;
            rest = rest < 2 ? 0 : 11 - rest;
            var digit = rest.ToString();
            documentNumber += digit;
            sum = 0;

            for (int i = 0; i < 10; i++)
            {
                if (!int.TryParse(documentNumber[i].ToString(), out int intValue))
                {
                    return false;
                }

                sum += intValue * secondMultiplierCollection[i];
            }

            rest = sum % 11;
            rest = rest < 2 ? 0 : 11 - rest;
            digit += rest.ToString();
            return value.EndsWith(digit);
        }

        /// <summary>
        /// Extensão que valida se o valor é um CNPJ.
        /// </summary>
        /// <param name="value">Objeto referenciado.</param>
        /// <returns>Verdadeiro caso o valor seja um CNPJ e falso caso contrário.</returns>
        public static bool IsCnpj(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var document = value.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Trim();

            if (document.Length != 14)
            {
                return false;
            }

            if (document.ToArray().Distinct().Count() <= 1)
            {
                return false;
            }

            var firstMultiplierCollection = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var secondMultiplierCollection = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var documentNumber = document.Substring(0, 12);

            var sum = 0;
            for (var i = 0; i < 12; i++)
            {
                if (!int.TryParse(documentNumber[i].ToString(), out int intValue))
                {
                    return false;
                }

                sum += intValue * firstMultiplierCollection[i];
            }

            var rest = sum % 11;
            rest = rest < 2 ? 0 : 11 - rest;
            var digit = rest.ToString();
            documentNumber += digit;

            sum = 0;
            for (int i = 0; i < 13; i++)
            {
                if (!int.TryParse(documentNumber[i].ToString(), out int intValue))
                {
                    return false;
                }

                sum += intValue * secondMultiplierCollection[i];
            }

            rest = sum % 11;
            rest = rest < 2 ? 0 : 11 - rest;
            digit += rest.ToString();
            return value.EndsWith(digit);
        }

        /// <summary>
        /// Extensão que valida se o valor é um CPF ou CNPJ.
        /// </summary>
        /// <param name="value">Objeto referenciado.</param>
        /// <returns>Verdadeiro caso o valor seja um CPF ou CNPJ e falso caso contrário.</returns>
        public static bool IsCpfOrCnpj(this string value)
        {
            try
            {

                var document = Regex.Replace(value, "[^0-9]", string.Empty);
                if (string.IsNullOrEmpty(document) || document.Distinct().Count() <= 1)
                {
                    return false;
                }

                return document.Count() > 11 ? document.IsCnpj() : document.IsCpf();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extensão que faz a formatação de um número de CPF.
        /// </summary>
        /// <param name="value">Objeto referenciado.</param>
        /// <returns>Valor do documento formatado.</returns>
        public static string FormatCpf(this string value)
        {
            return value.IsCpf() ? Convert.ToUInt64(value).ToString(@"000\.000\.000\-00") : "[CPF Inválido]";
        }

        /// <summary>
        /// Extensão que faz a formatação de um número de CNPJ.
        /// </summary>
        /// <param name="value">Objeto referenciado.</param>
        /// <returns>Valor do documento formatado.</returns>
        public static string FormatCnpj(this string value)
        {
            return value.IsCnpj() ? Convert.ToUInt64(value).ToString(@"00\.000\.000\/0000\-00") : "[CNPJ Inválido]";
        }

        /// <summary>
        /// Extensão que faz a formatação de um número de CPF ou CNPJ.
        /// </summary>
        /// <param name="value">Objeto referenciado.</param>
        /// <returns>Valor do documento formatado.</returns>
        public static string FormatCpfOrCnpj(this string value)
        {
            if (value.IsCpfOrCnpj())
            {
                return value.IsCpf() ? value.FormatCpf() : value.FormatCnpj();
            }
            else
            {
                return "[Documento inválido]";
            }
        }

        /// <summary>
        /// Extensão que remove formatação de documento (CPF/CNPJ).
        /// </summary>
        /// <param name="value">Objeto referenciado.</param>
        /// <returns>Número de documento sem formatação.</returns>
        public static string Unformat(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return Regex.Replace(value, "[^0-9,]", string.Empty);
        }
    }
}
