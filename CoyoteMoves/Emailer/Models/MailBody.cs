using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp;
using iTextSharp.text.pdf;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace CoyoteMoves.Emailer.Models
{
    public class MailBody
    {

        public MailBody()
        {

        }


        public void fillOutPdfYolo()
        {
            string mapPathString = Path.GetFullPath("../../../CoyoteMoves/CoyoteMovesTemplate.pdf");    //hack city bitch hack hack city

            /*Might need to be completely redone w/ Server.MapPath . . . depends if we're running this off of a server at some point. */

            PdfReader reader = new PdfReader(mapPathString);
    

            using (PdfStamper stamper = new PdfStamper(reader, new FileStream (Path.GetFullPath("../../../CoyoteMoves/testdoc.pdf"), FileMode.Create)))
            {             
                foreach (var field in stamper.AcroFields.Fields)
                {
                    var line = string.Format("[{0}]", field.Key);
                    Console.WriteLine(line);
                }
               
                AcroFields form = stamper.AcroFields;
                var fieldKeys = form.Fields.Keys;
                foreach (string fieldKey in fieldKeys)
                {
                    if (fieldKey.Contains("Name"))
                    {
                        form.SetField(fieldKey, "Ivanna Humpalot");
                    }
                }
            }

            reader.Close();
    
        }

        public bool sendTestEmail(string address)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(address);
            message.Subject = "ERROR: CRITICAL HULL BREACH";
            message.From = new System.Net.Mail.MailAddress("alerts@coyote.com");
            message.Body = "02 LEVELS CRITICAL\n TIME TO SELF DESTRUCT 0 SECONDS\n ALL IS LOST\n REPEAT \n ALL IS LOST";
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("10.3.10.112"); //Magic numbers for SMTP server, could store in DB . . .
            smtp.Send(message);

            return (smtp != null);
        }



    }

    
}