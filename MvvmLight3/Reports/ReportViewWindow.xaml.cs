using System;
using GolfClub.Model;
using GolfClub.Properties;
using GolfClub.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GolfClub.Reports
{
    /// <summary>
    /// Description for ReportViewWindow.
    /// </summary>
    public partial class ReportViewWindow : Window
    {
        private readonly string _reportTitle;
        private readonly LocalPrintServer _local;

        /// <summary>
        /// Initializes a new instance of the ReportViewWindow class.
        /// </summary>
        public ReportViewWindow()
        {
            InitializeComponent();
            this.LoadWindowSettings();
            Closing += WindowClosing;

            _local = new LocalPrintServer();
            Printers.ItemsSource = GetPrinters(_local);

            if (string.IsNullOrEmpty(Settings.Default.LastPrinter))
            {
                Printers.SelectedItem = _local.DefaultPrintQueue.Name;
            }
            else
            {
                Printers.SelectedItem = Settings.Default.LastPrinter;
            }
        }

        private static IEnumerable<string> GetPrinters(LocalPrintServer local)
        {
            var myPrintQueues = local.GetPrintQueues();
            var printers = myPrintQueues.Select(printQueue => printQueue.Name).ToList();
            return printers;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            Settings.Default.LastPrinter = (string)Printers.SelectedItem;
            this.SaveWindowSettings();
        }

        public ReportViewWindow(string reportTitle, IEnumerable<Person> data)
            : this()
        {
            _reportTitle = reportTitle;
            ReportTile.Inlines.Clear();
            ReportTile.Inlines.Add(_reportTitle);
            ListView.ItemsSource = data;
        }

        private void PrintReport(object sender, RoutedEventArgs e)
        {
//            DoThePrint(ReportDocument);
            Print2();
            return;
            var pq = _local.GetPrintQueue((string)Printers.SelectedItem);

            // Create a PrintDialog
            var printDlg = new PrintDialog { PrintQueue = pq };
            DocumentPaginator paginator = ((IDocumentPaginatorSource)ReportDocument).DocumentPaginator;
            DocumentPaginatorWrapper wrapper = new DocumentPaginatorWrapper(paginator, new Size(768, 676), new Size(48, 48));
//            IDocumentPaginatorSource idpSource = ReportDocument;
//            idpSource.DocumentPaginator.PageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);

            // Call PrintDocument method to send document to printer
            printDlg.PrintDocument(wrapper, _reportTitle);
        }

        private void Print2()
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {

                FlowDocument flowDoc = ReportDocument;

                flowDoc.PageHeight = printDialog.PrintableAreaHeight;
                flowDoc.PageWidth = printDialog.PrintableAreaWidth;
                flowDoc.PagePadding = new Thickness(25);

                flowDoc.ColumnGap = 0;

                flowDoc.ColumnWidth = (flowDoc.PageWidth -
                                       flowDoc.ColumnGap -
                                       flowDoc.PagePadding.Left -
                                       flowDoc.PagePadding.Right);

                printDialog.PrintDocument(((IDocumentPaginatorSource)flowDoc)
                                         .DocumentPaginator,
                                         "Task Manager Print Job");

            }
        }

        private void DoThePrint(FlowDocument document)
        {
            // Clone the source document's content into a new FlowDocument.
            // This is because the pagination for the printer needs to be
            // done differently than the pagination for the displayed page.
            // We print the copy, rather that the original FlowDocument.
            var s = new System.IO.MemoryStream();
            var source = new TextRange(document.ContentStart, document.ContentEnd);
            source.Save(s, DataFormats.Xaml);
            var copy = new FlowDocument();
            var dest = new TextRange(copy.ContentStart, copy.ContentEnd);
            dest.Load(s, DataFormats.Xaml);

            // Create a XpsDocumentWriter object, implicitly opening a Windows common print dialog,
            // and allowing the user to select a printer.

            // get information about the dimensions of the seleted printer+media.
            PrintDocumentImageableArea ia = null;
            var docWriter = PrintQueue.CreateXpsDocumentWriter(ref ia);

            if (docWriter == null || ia == null) return;
            var paginator = ((IDocumentPaginatorSource)copy).DocumentPaginator;

            // Change the PageSize and PagePadding for the document to match the CanvasSize for the printer device.
            paginator.PageSize = new Size(ia.MediaSizeWidth, ia.MediaSizeHeight);
            var t = new Thickness(72);  // copy.PagePadding;
            copy.PagePadding = new Thickness(
                Math.Max(ia.OriginWidth, t.Left),
                Math.Max(ia.OriginHeight, t.Top),
                Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), t.Right),
                Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), t.Bottom));

            copy.ColumnWidth = double.PositiveInfinity;
            //copy.PageWidth = 528; // allow the page to be the natural with of the output device

            // Send content to the printer.
            docWriter.Write(paginator);
        }
    }
}