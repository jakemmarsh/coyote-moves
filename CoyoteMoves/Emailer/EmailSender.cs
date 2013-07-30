using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp;
using iTextSharp.text.pdf;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Net.Mail;
using CoyoteMoves.Models.RequestItems;
using System.Collections.ObjectModel;

/*To TRACK:
 * - Group movement by date
 * - Desk Movement by date
 * - Count of each group
 * - Dynamic Counts of each group
 */ 

namespace CoyoteMoves.Emailer.Models
{
    public class EmailSender
    {

        string _smtp = "10.3.10.112";
        EmailTemplate _template;

        public EmailSender(string subject, Collection<string> to, string from, string emailBody, string pdfLocation)
        {
            _template = new EmailTemplate(subject, to, from, emailBody, pdfLocation);
            
        }

        public void getFieldNames(string pdfPath)
        {
            string mapPathString = Path.GetFullPath(pdfPath);
            PdfReader reader = new PdfReader(mapPathString);
            MemoryStream memory = new MemoryStream();
            PdfStamper stamper = new PdfStamper(reader, memory);

            AcroFields form = stamper.AcroFields;

            foreach (var field in stamper.AcroFields.Fields)
            {
                var line = string.Format("[{0}]", field.Key);
                Console.WriteLine(line);
            } 
            stamper.Close();  
            memory.Close();
            reader.Close();       

        }


        public bool sendTestAttachment(string address)
        {
            string mapPathString = Path.GetFullPath("../../../CoyoteMoves/testdoc.pdf");    

            /*Might need to be completely redone w/ Server.MapPath . . . depends if we're running this off of a server at some point. */

            PdfReader reader = new PdfReader(mapPathString);
            MemoryStream memory = new MemoryStream(); 
            PdfStamper stamper = new PdfStamper(reader, memory);   
               
            AcroFields form = stamper.AcroFields;
               
            var fieldKeys = form.Fields.Keys;
            
            foreach (string fieldKey in fieldKeys)
            {
                if (fieldKey.Contains("Name"))
                {
                    form.SetField(fieldKey, "Ivanna Humpalot");
                }
            }
  
            stamper.FormFlattening = true;
            stamper.Writer.CloseStream = false;
            stamper.Close();
       
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(address);
            message.Subject = "A new PDF for you!";
            message.From = new System.Net.Mail.MailAddress("MovesRequest@coyote.com");
            message.Body = "We have a new employee! Here's the info!";
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_smtp); 
            memory.Position = 0;
          
            message.Attachments.Add(new Attachment(memory, "MovesForm.pdf"));
            smtp.Send(message);

           
            reader.Close();

            return (smtp != null);

        }

        public bool sendMovesRequest(RequestForm req)
        {
            if (req == null)
            {
                throw new ArgumentNullException("req");
            }

            string mapPathString = Path.GetFullPath("../../../CoyoteMoves/CoyoteMovesTemplate.pdf");

            MailMessage _toSend = _template.movesFormRequest(req); 
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_smtp);
            smtp.Send(_toSend);

            return (smtp != null);
        }

      
        public bool storeRequestInfo(RequestForm req)
        {
            return false;
        }

        
    }
}