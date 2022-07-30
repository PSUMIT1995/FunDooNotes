using Experimental.System.Messaging;
using System;

using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Modal
{
    public class MSMQmodel
    {
        MessageQueue messageQueue = new MessageQueue();

        public void sendData2Queue(string Token)
        {

            messageQueue.Path = @".\private$\Token";

            if(!MessageQueue.Exists(messageQueue.Path))
            {
                
                MessageQueue.Create(messageQueue.Path);
            }
            

            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
            messageQueue.Send(Token);
            messageQueue.BeginReceive();
            messageQueue.Close();
        }

        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = messageQueue.EndReceive(e.AsyncResult);
            string Token = msg.Body.ToString();
            string subject = "FundooNotes Reset Link";
            string body = "Hi Sumit,\nToken Genrated To Reset Password\n\n" + "Session Token : " + Token;
            var SMTP = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("patelsumit1995.sp@gmail.com", "dcmmajrljgfyxqhx"),
                EnableSsl = true

            };

            SMTP.Send("patelsumit1995.sp@gmail.com", "patelsumit1995.sp@gmail.com", subject, body);

            messageQueue.BeginReceive();

        }
    }
}
