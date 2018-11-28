using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using System.Collections.Generic;

namespace EuropeanParliamentTracker.Pdf
{
    class TextAndLocationExtractionStrategy : LocationTextExtractionStrategy
    {
        public List<TextWithLocation> TextsWithLocations { get; set; }
        public System.Globalization.CompareOptions CompareOptions { get; set; }

        private int _currentPdfPosition { get; set; }

        public TextAndLocationExtractionStrategy(System.Globalization.CompareOptions compareOptions = System.Globalization.CompareOptions.None)
        {
            CompareOptions = compareOptions;
            TextsWithLocations = new List<TextWithLocation>();
        }

        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);

            var rectangle = GetRectangleFromReaderInfo(renderInfo);
            var text = renderInfo.GetText();

            TextsWithLocations.Add(new TextWithLocation(rectangle, text, _currentPdfPosition));
            _currentPdfPosition += text.Length;
        }

        public Rectangle GetRectangleAtPdfPosition(int position)
        {
            foreach(var textWithLocation in TextsWithLocations)
            {
                if(textWithLocation.PositionInDocument >= position)
                {
                    return textWithLocation.Rect;
                }
            }
            return null;
        }

        private Rectangle GetRectangleFromReaderInfo(TextRenderInfo renderInfo)
        {
            var bottomLeft = renderInfo.GetDescentLine().GetStartPoint();
            var topRight = renderInfo.GetAscentLine().GetEndPoint();

            return new Rectangle(bottomLeft[Vector.I1], bottomLeft[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);
        }
    }
}
