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

      
        public bool storeRequestInfo(RequestForm req) //TODO: store the info in DB along w/ reference number. Can generate here or have as part of the actual request object
        {
            return false;
        }

        
    }
}