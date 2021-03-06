﻿using CoyoteMoves.Data_Access;
using CoyoteMoves.Models.RequestItems;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Services.Description;

namespace CoyoteMoves.Emailer.Models
{
    public class EmailTemplate
    {
        private string _subject;
        private Collection<string> _to;
        private string _from;
        private string _emailBody;
        private string _mappedLocation;
        private EmployeeDB _empDB;

        public EmailTemplate(string subject, Collection<string> to, string from, string emailBody, string pdfLocation)
        {
            _subject = subject;
            _to = to;
            _from = from;
            _emailBody = emailBody;
            _mappedLocation = pdfLocation;
            _empDB = new EmployeeDB();
        }

        public void addRecipient(string to)
        {
            _to.Add(to);
        }

        public MailMessage movesFormRequest(RequestForm req)
        {

            if (req == null)
            {
                throw new NullReferenceException("req");
            }
            if (string.IsNullOrEmpty(_mappedLocation))
            {
                throw new IOException("_templateLocation");
            }


            PdfReader reader = new PdfReader(_mappedLocation);
            MemoryStream memory = new MemoryStream();
            PdfStamper stamper = new PdfStamper(reader, memory);
            AcroFields form = stamper.AcroFields;

            mapFieldsFromRequest(req, form);

            stamper.FormFlattening = true;
            stamper.Writer.CloseStream = false;
            stamper.Close();

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            foreach (string entry in _to)
            {
                message.To.Add(entry);
            }
            message.Subject = _subject + " " + req.UniqueId;
            message.From = new System.Net.Mail.MailAddress(_from);
            message.Body = _emailBody;
            memory.Position = 0;

            message.Attachments.Add(new Attachment(memory, "MovesForm" + req.UniqueId + ".pdf"));
            reader.Close();

            return message;
        }

        public void mapFieldsFromRequest(RequestForm req, AcroFields form)
        {

            EmployeeDB empDB = new EmployeeDB();
            var fieldKeys = form.Fields.Keys;

            foreach (string fieldKey in fieldKeys)
            {
                if (fieldKey.Equals("Employee Name"))
                    form.SetField(fieldKey, empDB.GetFullNameById(req.EmployeeId));
                if (fieldKey.Equals("Date to occur on"))
                    form.SetField(fieldKey, (string)(DateTime.Now.AddDays(7)).ToString("d"));
                if (fieldKey.Equals("CurrentJob Title"))
                    form.SetField(fieldKey, req.Current.BazookaInfo.JobTitle);
                if (fieldKey.Equals("CurrentDepartment"))
                    form.SetField(fieldKey, req.Current.BazookaInfo.Department);
                if (fieldKey.Equals("CurrentGroup"))
                    form.SetField(fieldKey, req.Current.BazookaInfo.Group);
                if (fieldKey.Equals("CurrentManager") || fieldKey.Equals("Current Manager Name"))
                    form.SetField(fieldKey, empDB.GetFullNameById(req.Current.BazookaInfo.ManagerID));
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
                    form.SetField(fieldKey, empDB.GetFullNameById(req.Future.BazookaInfo.ManagerID));
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
                    form.SetField(fieldKey, empDB.GetFullNameById(int.Parse(req.Current.UltiproInfo.Supervisor)));
                if (fieldKey.Equals("FutureSupervisor"))
                    form.SetField(fieldKey, empDB.GetFullNameById(int.Parse(req.Future.UltiproInfo.Supervisor)));
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