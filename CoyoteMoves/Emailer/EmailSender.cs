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
        string HRAddress = "coyotemoves@coyote.com";
        string SDAddress = "servicedesk@coyote.com";

        public EmailSender()
        {
            Collection<string> to = new Collection<string>();

            _template = new EmailTemplate(
                "New Coyote Moves Request",
                to,
                "CoyoteMovesRequest@coyote.com",
                "",
                "../../../CoyoteMoves/CoyoteMovesTemplate.pdf");

            _requester = new RequestFormDB();
        }

        public EmailSender(string subject, Collection<string> to, string from, string emailBody, string pdfLocation)
        {
            _template = new EmailTemplate(subject, to, from, emailBody, pdfLocation);
            _requester = new RequestFormDB();
        }

        public bool sendMovesRequest(RequestForm req, string sendTo)
        {
            bool isSent = false;

            if (req == null)
            {
                throw new ArgumentNullException("req");
            }

            if (sendTo == null)
            {
                throw new ArgumentNullException("sendTo");
            }

            if (sendTo == "HR")
            {
                _template.addRecipient(HRAddress);
                isSent = sendMovesRequest(req);
            }
            else if (sendTo == "SD")
            {
                _template.addRecipient(SDAddress);
                isSent = sendMovesRequest(req);
            }

            return isSent;

        }

        public bool sendMovesRequest(RequestForm req)
        {
            if (req == null)
            {
                throw new ArgumentNullException("req");
            }
            MailMessage _toSend = _template.movesFormRequest(req);
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_smtp);
            smtp.Send(_toSend);

            return (smtp != null && _toSend != null);
        }

        public bool sendMovesRequestHR(RequestForm req)
        {
            return sendMovesRequest(req, "HR");
        }

        public bool sendMovesRequestSD(RequestForm req)
        {
            return sendMovesRequest(req, "SD");
        }
    }
}