using System.Net.Mail;

namespace Miltha.Tech.Core.Extensions.Models
{
    public struct Email
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Full { get; set; }
        public bool IsValid { get; set; }

        public Email(string fullEmail)
        {
            if (!string.IsNullOrWhiteSpace(fullEmail) && fullEmail.Contains('@'))
            {
                Name = fullEmail.Substring(0, fullEmail.IndexOf('@'));
                Domain = fullEmail.Substring(fullEmail.IndexOf('@') + 1);
                Full = GetFullEmail(Name, Domain);
                IsValid = EmailValidate(Full);
            }
            else
            {
                Name = string.Empty;
                Domain = string.Empty;
                Full = string.Empty;
                IsValid = false;
            }
        }

        private static string GetFullEmail(string name, string domain) => $"{name}@{domain}";

        private static bool EmailValidate(string fullEmail) => MailAddress.TryCreate(fullEmail, out _);
    }
}