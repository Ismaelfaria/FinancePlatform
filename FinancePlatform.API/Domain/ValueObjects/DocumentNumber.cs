namespace FinancePlatform.API.Domain.ValueObjects
{
    public class DocumentNumber
    {
        public string FormatCPForCNPJ(string num)
        {
            if (string.IsNullOrWhiteSpace(num))
                return "Inválido";

            string digits = new string(num.Where(char.IsDigit).ToArray());

            if (digits.Length == 11)
                return FormatCPF(digits);
            else if (digits.Length == 14)
                return FormatCNPJ(digits);
            else
                return "Inválido";
        }

        private string FormatCPF(string cpf)
        {
            string digits = new string(cpf.Where(char.IsDigit).ToArray());

            if (digits.Length != 11)
                throw new ArgumentException("CPF inválido");

            return Convert.ToUInt64(digits).ToString(@"000\.000\.000\-00");
        }

        private string FormatCNPJ(string cnpj)
        {
            string digits = new string(cnpj.Where(char.IsDigit).ToArray());

            if (digits.Length != 14)
                throw new ArgumentException("CNPJ inválido");

            return Convert.ToUInt64(digits).ToString(@"00\.000\.000\/0000\-00");
        }
    }
}
