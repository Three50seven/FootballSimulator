using System.Net.Mail;

namespace Common.Core.Services
{
    public class MessageSmtpSettings
    {
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 25;
        public bool Anonymous { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }

        public SmtpClient ToSmtpClient()
        {
            var client = new SmtpClient(Host, Port);

            client.EnableSsl = UseSSL;

            if (Anonymous || (string.IsNullOrWhiteSpace(UserName) && string.IsNullOrWhiteSpace(Password)))
            {
                client.UseDefaultCredentials = false;
            }
            else
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(UserName, Password);
            }

            return client;
        }
    }
}
