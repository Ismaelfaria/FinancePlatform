namespace FinancePlatform.API.Domain.ValueObjects
{
    public class ContactInformation
    {
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }

        public ContactInformation(string email, string phoneNumber)
        {
            if (!IsValidEmail(email))
                throw new ArgumentException("Email inválido.");

            Email = email;
            PhoneNumber = phoneNumber;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
