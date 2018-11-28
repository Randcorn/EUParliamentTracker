using iTextSharp.text;

namespace EuropeanParliamentTracker.Pdf
{
    public class TextWithLocation
    {
        public Rectangle Rect { get; set; }
        public string Text { get; set; }
        public int PositionInDocument { get; set; }
        public TextWithLocation(Rectangle rect, string text, int positionInDocument)
        {
            Rect = rect;
            Text = text;
            PositionInDocument = positionInDocument;
        }
    }
}
