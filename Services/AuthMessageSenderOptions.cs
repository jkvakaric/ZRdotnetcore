namespace ZRdotnetcore.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }

        // temp solution until sendgrid api gets usable - delete after
        //public string MailgunApiUri { get; set; }
        //public string MailgunApiKey { get; set; }
        //public string MailgunApiRoute { get; set; }
    }
}