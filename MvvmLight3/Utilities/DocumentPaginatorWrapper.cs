using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace GolfClub.Utilities
{
    public class DocumentPaginatorWrapper : DocumentPaginator
    {
        private readonly Size _mPageSize;
        private Size _mMargin;
        private readonly DocumentPaginator _mPaginator;
        private Typeface _mTypeface;

        public DocumentPaginatorWrapper(DocumentPaginator paginator, Size pageSize, Size margin)
        {
            _mPageSize = pageSize;
            _mMargin = margin;
            _mPaginator = paginator;
            _mPaginator.PageSize = new Size(_mPageSize.Width - margin.Width * 2, _mPageSize.Height - margin.Height * 2);
        }

        private Rect Move(Rect rect)
        {
            return rect.IsEmpty ? rect : new Rect(rect.Left + _mMargin.Width, rect.Top + _mMargin.Height, rect.Width, rect.Height);
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            var page = _mPaginator.GetPage(pageNumber);

            // Create a wrapper visual for transformation and add extras
            var newpage = new ContainerVisual();
            var title = new DrawingVisual();

            using (var ctx = title.RenderOpen())
            {
                if (_mTypeface == null)
                {
                    _mTypeface = new Typeface("Times New Roman");
                }

                var text = new FormattedText("Page " + (pageNumber + 1), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, _mTypeface, 14, Brushes.Black);
                ctx.DrawText(text, new Point(0, -96 / 4)); // 1/4 inch above page content
            }

            var background = new DrawingVisual();
            using (var ctx = background.RenderOpen())
            {
                ctx.DrawRectangle(new SolidColorBrush(Color.FromRgb(240, 240, 240)), null, page.ContentBox);
            }

            newpage.Children.Add(background); // Scale down page and center
            var smallerPage = new ContainerVisual();
            smallerPage.Children.Add(page.Visual);
            smallerPage.Transform = new MatrixTransform(0.95, 0, 0, 0.95, 0.025 * page.ContentBox.Width, 0.025 * page.ContentBox.Height);
            newpage.Children.Add(smallerPage);
            newpage.Children.Add(title);
            newpage.Transform = new TranslateTransform(_mMargin.Width, _mMargin.Height);

            return new DocumentPage(newpage, _mPageSize, Move(page.BleedBox), Move(page.ContentBox));
        }

        public override bool IsPageCountValid
        {
            get
            {
                return _mPaginator.IsPageCountValid;
            }
        }

        public override int PageCount
        {
            get
            {
                return _mPaginator.PageCount;
            }
        }

        public override Size PageSize
        {
            get
            {
                return _mPaginator.PageSize;
            }

            set
            {
                _mPaginator.PageSize = value;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                return _mPaginator.Source;
            }
        }
    }
}