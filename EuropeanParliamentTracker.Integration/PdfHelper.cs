using System;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace EuropeanParliamentTracker.Pdf
{
    public class PdfHelper
    {
        public static string GetTextFromPDF(string url)
        {
            StringBuilder text = new StringBuilder();
            using (PdfReader reader = new PdfReader(new Uri(url)))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }

            return text.ToString();
        }

    }
}
