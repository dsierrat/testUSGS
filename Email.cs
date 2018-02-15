//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Text;
//using System.Threading.Tasks;

//namespace TestUsgs
//{
//    public class Email
//    {
//        public static void SendEmail(bool isRestarted)
//        {
//            try
//            {
//                SmtpClient client = new SmtpClient
//                {
//                    Port = 587,
//                    EnableSsl = true,
//                    Credentials = new NetworkCredential(config.EmailFrom, config.EmailPassword),
//                    Host = config.EmailHost
//                };

//                //e-mail sender
//                MailAddress from = new MailAddress(config.EmailFrom, config.EmailName, System.Text.Encoding.UTF8);
//                //destinations for the e-mail message.
//                MailAddress to = new MailAddress(config.EmailTo);

//                //message content
//                MailMessage message = new MailMessage(from, to)
//                {
//                    Body = "The service " + config.ServiceName + " has stopped",
//                    BodyEncoding = System.Text.Encoding.UTF8,
//                    Subject = config.ServiceName + " Service down",
//                    SubjectEncoding = System.Text.Encoding.UTF8
//                };
//                message.Body += isRestarted ? ", but it was recovered by the recovery program.\n\n" : " and it couldn't be recovered please restart it.\n\n";
//                message.Body += GetLastServiceError();

//                // Set the method that is called back when the send operation ends.
//                client.SendCompleted += new
//                SendCompletedEventHandler(SendCompletedCallback);

//                // The userState can be any object that allows your callback 
//                string userState = "test message1";
//                client.SendAsync(message, userState);
//                log.LogServiceWrite("The Email was sended to " + config.EmailTo, "Info");
//            }
//            catch (Exception e)
//            {
//                log.LogServiceWrite("Error sending e-mail " + e.Message + "\n Source: " + e.Source, "Error");
//            }
//        }

//        public static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
//        {
//            //Get the unique identifier for this asynchronous operation.

//            if (e.Error != null)
//            {
//                log.LogServiceWrite(("Error sending e-mail " + e.Error.ToString()), "Error");
//            }
//            else
//            {
//                log.LogServiceWrite("Message sent", "Info");
//            }
//        }

//    }
//}
