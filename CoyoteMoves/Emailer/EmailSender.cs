using System;
using System.IO;
using System.Net.Mail;
using CoyoteMoves.Models.RequestItems;
using System.Collections.ObjectModel;
using CoyoteMoves.Data_Access;

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

        string _smtp = "10.3.10.112"; //Magic numbers for coyote SMTP server, could store in DB and call that, but let's be honest here that's ridiculous
        EmailTemplate _template;
        RequestFormDB _requester;

        public EmailSender(string subject, Collection<string> to, string from, string emailBody, string pdfLocation)
        {
            _template = new EmailTemplate(subject, to, from, emailBody, pdfLocation);
            _requester = new RequestFormDB();
            
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

            return (smtp != null && _toSend != null);
        }

        public bool sendMovesRequestAndStore(RequestForm req)
        {
            if (req == null)
            {
                throw new ArgumentNullException("req");
            }

            string mapPathString = Path.GetFullPath("../../../CoyoteMoves/CoyoteMovesTemplate.pdf");

            MailMessage _toSend = _template.movesFormRequest(req);
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_smtp);
            smtp.Send(_toSend);
            bool stored = _requester.StoreRequestFormInDatabaseAsPending(req);

            return stored;
        }

      
        //public bool storeRequestInfo(RequestForm req) //TODO: store the info in DB along w/ reference number. Can generate here or have as part of the actual request object
        //{
        //    return _requester.StoreRequestFormInDatabaseAsPending(req);
        //}

        
    }
}