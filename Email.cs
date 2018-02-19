using MySql.Data.MySqlClient;
using ServiceVerifier;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TestUsgs
{
    public class Email
    {
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
        public string EmailName{ get; set; }
        public string EmailTo { get; set; }
        public string EmailHost { get; set; }

        public Email()
        {
            MySqlCommand comm = Program.con.CreateCommand();
            comm.CommandText = "SELECT * FROM parametro WHERE id=1";
            var query = comm.ExecuteReader();

            while (query.Read())
            {
                EmailFrom = query.GetString("email_from");
                EmailPassword = query.GetString("email_pass");
                EmailName = query.GetString("email_name");
                EmailTo = query.GetString("email_to");
                EmailHost = query.GetString("email_host");
            }

            query.Close();
        }

        public void SendEmail(string title)
        {
            try
            {

                SmtpClient client = new SmtpClient
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(EmailFrom, EmailPassword),
                    Host = EmailHost
                };

                //e-mail sender
                MailAddress from = new MailAddress(EmailFrom, EmailName, Encoding.UTF8);
                //destinations for the e-mail message.
                MailAddress to = new MailAddress(EmailTo);

                //message content
                MailMessage message = new MailMessage(from, to)
                {
                    Body = "The TestUSGS have reported a new earthquake: "+title,
                    BodyEncoding = Encoding.UTF8,
                    Subject = "New USGS event",
                    SubjectEncoding = Encoding.UTF8
                };

                // Set the method that is called back when the send operation ends.
                client.SendCompleted += new
                SendCompletedEventHandler(SendCompletedCallback);

                // The userState can be any object that allows your callback 
                string userState = "test message1";
                client.SendAsync(message, userState);
                Console.WriteLine("The Email was sended to " + EmailTo, "Info");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending e-mail " + e.Message + "\n Source: " + e.Source, "Error");
            }
        }

        public void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            //Get the unique identifier for this asynchronous operation.

            if (e.Error != null)
            {
                Console.WriteLine(("Error sending e-mail " + e.Error.ToString()), "Error");
            }
            else
            {
                Console.WriteLine("Message sent", "Info");
            }
        }

    }
}
