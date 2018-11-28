using System;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace EuropeanParliamentTracker.Pdf
{
    public class PdfHelper
    {
        public static string GetTextFromPDF(string url, int startPage = 1)
        {
            var text = new StringBuilder();
            using (var reader = new PdfReader(new Uri(url)))
            {
                var strategy = new TextAndLocationExtractionStrategy();
                for (int i = startPage; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i, strategy));
                }
            }

            return text.ToString();
        }

    }
}
