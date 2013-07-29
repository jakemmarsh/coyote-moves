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

        public EmailSender()
        {
            
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

        public bool sendTestEmail(string address)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(address);
            message.Subject = "ERROR: CRITICAL HULL BREACH";
            message.From = new System.Net.Mail.MailAddress("alerts@coyote.com");
            message.Body = "02 LEVELS CRITICAL\n TIME TO SELF DESTRUCT 0 SECONDS\n ALL IS LOST\n REPEAT \n ALL IS LOST";
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_smtp); 
            smtp.Send(message);

            return (smtp != null);
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

        public bool sendMovesRequest(RequestForm req, string address)
        {
            string mapPathString = Path.GetFullPath("../../../CoyoteMoves/CoyoteMovesTemplate.pdf");

            /*Might need to be completely redone w/ Server.MapPath . . . depends if we're running this off of a server at some point. */

            PdfReader reader = new PdfReader(mapPathString);
            MemoryStream memory = new MemoryStream();
            PdfStamper stamper = new PdfStamper(reader, memory);
            AcroFields form = stamper.AcroFields;

            mapFieldsFromRequest(req, form);

            stamper.FormFlattening = true;
            stamper.Writer.CloseStream = false;
            stamper.Close();

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(address);
            message.Subject = "Employee Move Request";
            message.From = new System.Net.Mail.MailAddress("MovesRequest@coyote.com");
            message.Body = "We have a new employee! Here's the info!";
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_smtp);
            memory.Position = 0;

            message.Attachments.Add(new Attachment(memory, "MovesForm.pdf"));
            smtp.Send(message);


            reader.Close();

            return (smtp != null);
        }

      

//[Employee Name]
//[Current Manager Name]
//[Date to occur on]
//[CurrentJob Title]
//[FutureJob Title]
//[CurrentDepartment]
//[FutureDepartment]
//[CurrentGroup]
//[FutureGroup]
//[CurrentManager]
//[FutureManager]
//[CurrentTemplate]
//[FutureTemplate]
//[CurrentSecurity ItemRights]
//[FutureSecurity ItemRights]
//[CurrentJob Title_2]
//[FutureJob Title_2]
//[CurrentDepartment_2]
//[FutureDepartment_2]
//[CurrentSupervisor]
//[FutureSupervisor]
//[CurrentOther]
//[FutureOther]
//[Current Desk Row1]
//[Future Desk Row1]
//[Current OfficeRow1]
//[Future OfficeRow1]
//[Shared Pod Line Description2nd line on phone]
//[Shared Line Employee Name3rd line on phone]
//[Shared Line Employee Name4th line on phone]
//[Shared Line Employee Name5th line on phone]
//[Shared Line Employee Name6th line on phone]
//[2nd line on phoneRow1]
//[Shared Pod Line DescriptionRow2]
//[3rd line on phoneRow1]
//[Shared Line Employee NameRow2]
//[4th line on phoneRow1]
//[Shared Line Employee NameRow2_2]
//[5th line on phoneRow1]
//[Shared Line Employee NameRow2_3]
//[6th line on phoneRow1]
//[Shared Line Employee NameRow2_4]
//[Employee Extension]
//[Need to be added to]
//[Need to be removed from]
//[Need to be added to_2]
//[Need to be removed from_2]

        public void mapFieldsFromRequest(RequestForm req, AcroFields form)
        {
            //TODO: Get First and third fields
            //TODO: Hate self more
            //TODO: Be more inefficient
            var fieldKeys = form.Fields.Keys;

            foreach (string fieldKey in fieldKeys)
            {
                    if (fieldKey.Equals("CurrentJob Title"))
                        form.SetField(fieldKey, req.Current.BazookaInfo.JobTitle);
                    if (fieldKey.Equals("CurrentDepartment"))
                        form.SetField(fieldKey, req.Current.BazookaInfo.Department);
                    if (fieldKey.Equals("CurrentGroup"))
                        form.SetField(fieldKey, req.Current.BazookaInfo.Group);
                    if (fieldKey.Equals("CurrentManager") || fieldKey.Equals("Current Manager Name"))
                        form.SetField(fieldKey, req.Current.BazookaInfo.Manager);
                    if (fieldKey.Equals("CurrentTemplate"))
                        form.SetField(fieldKey, req.Current.BazookaInfo.JobTemplate);
                    if (fieldKey.Equals("CurrentSecurity ItemRights"))
                        form.SetField(fieldKey, req.Current.BazookaInfo.SecurityItemRights);
                    if (fieldKey.Equals("FutureJob Title"))
                        form.SetField(fieldKey, req.Future.BazookaInfo.JobTitle);
                    if (fieldKey.Equals("FutureDepartment"))
                        form.SetField(fieldKey, req.Future.BazookaInfo.Department);
                    if (fieldKey.Equals("FutureGroup"))
                        form.SetField(fieldKey, req.Future.BazookaInfo.Group);
                    if (fieldKey.Equals("FutureManager"))
                        form.SetField(fieldKey, req.Future.BazookaInfo.Manager);
                    if (fieldKey.Equals("FutureTemplate"))
                        form.SetField(fieldKey, req.Future.BazookaInfo.JobTemplate);
                    if (fieldKey.Equals("FutureSecurity ItemRights"))
                        form.SetField(fieldKey, req.Future.BazookaInfo.SecurityItemRights);
                    if (fieldKey.Equals("CurrentJob Title_2"))
                        form.SetField(fieldKey, req.Current.UltiproInfo.JobTitle);
                    if (fieldKey.Equals("FutureJob Title_2"))
                        form.SetField(fieldKey, req.Future.UltiproInfo.JobTitle);
                    if (fieldKey.Equals("CurrentDepartment_2"))
                        form.SetField(fieldKey, req.Current.UltiproInfo.Department);
                    if (fieldKey.Equals("FutureDepartment_2"))
                        form.SetField(fieldKey, req.Future.UltiproInfo.Department); 
                    if (fieldKey.Equals("CurrentSupervisor"))
                        form.SetField(fieldKey, req.Current.UltiproInfo.Supervisor); 
                    if (fieldKey.Equals("FutureSupervisor"))
                        form.SetField(fieldKey, req.Future.UltiproInfo.Supervisor);
                    if (fieldKey.Equals("CurrentOther"))
                         form.SetField(fieldKey, req.Current.UltiproInfo.Other);
                    if (fieldKey.Equals("FutureOther"))
                         form.SetField(fieldKey, req.Future.UltiproInfo.Other);
                    if (fieldKey.Equals("Current Desk Row1"))
                         form.SetField(fieldKey, req.Current.DeskInfo.DeskNumber);
                    if (fieldKey.Equals("Future Desk Row1"))
                         form.SetField(fieldKey, req.Future.DeskInfo.DeskNumber);
                    if (fieldKey.Equals("Current OfficeRow1"))
                        form.SetField(fieldKey, req.Current.DeskInfo.Office);
                    if (fieldKey.Equals("Future OfficeRow1"))
                        form.SetField(fieldKey, req.Future.DeskInfo.Office);
                    if (fieldKey.Equals("Need to be added to")) 
                        form.SetField(fieldKey, String.Join(", ", req.EmailInfo.GroupsToBeAddedTo));
                    if (fieldKey.Equals("Need to be removed from")) 
                        form.SetField(fieldKey, String.Join(", ", req.EmailInfo.GroupsToBeRemovedFrom));
                    if (fieldKey.Equals("Need to be added to_2"))
                        form.SetField(fieldKey, String.Join(", ", req.ReviewInfo.FilesToBeAddedTo));
                    if (fieldKey.Equals("Need to be removed from_2"))
                        form.SetField(fieldKey, String.Join(", ", req.ReviewInfo.FilesToBeRemovedFrom));
            }

        }
        
    }
}