using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp;
using iTextSharp.text.pdf;
using System.IO;

namespace CoyoteMoves.Emailer.Models
{
    public class MailBody
    {

        public MailBody()
        {

        }


        public void fillOutPdfYolo()
        {
            var inputFile = new PdfReader("C:/BlankRequest1.pdf");
            var outputStream = new FileStream("C:/TestThisShit.pdf", FileMode.Create, FileAccess.Write);
            var pdfStamper = new PdfStamper(inputFile, outputStream);

     
            foreach (var field in pdfStamper.AcroFields.Fields)
            {
                var line = string.Format("[{0}]", field.Key);
                Console.WriteLine(line);
            }


            outputStream.Close();
            inputFile.Close();
            pdfStamper.Close();
           

        }

    }

    
}