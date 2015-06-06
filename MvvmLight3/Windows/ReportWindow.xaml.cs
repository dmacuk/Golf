using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using GolfClub.Model;
using GolfClub.Utilities;
using Microsoft.Reporting.WinForms;

namespace GolfClub.Windows
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow
    {
        private readonly string _reportTitle;
        private readonly List<Person> _data;
        private bool _isReportLoaded;

        public ReportWindow(string reportTitle, List<Person> data)
        {
            _reportTitle = reportTitle;
            _data = data;
            InitializeComponent();
            ReportViewer.Load += ReportLoad;
            Closing += WindowClosing;
            this.LoadWindowSettings();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveWindowSettings();
        }

        private void ReportLoad(object sender, EventArgs e)
        {
            if (_isReportLoaded) return;

            var ds = BuildDataSet();
            var dataSource = new ReportDataSource("PeopleDataSet", ds.Tables[0]);
            var localReport = ReportViewer.LocalReport;
            localReport.DataSources.Add(dataSource);
            localReport.ReportPath = "../../Reports/Report1.rdlc";
            ReportParameter parameter = new ReportParameter("ReportTitle", _reportTitle);
            localReport.SetParameters(parameter);
            ReportViewer.RefreshReport();
            _isReportLoaded = true;
        }

        private DataSet BuildDataSet()
        {
            var dt = new DataTable();
            var properties = typeof (Person).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType.BaseType != null)
                    dt.Columns.Add(property.Name, property.PropertyType.BaseType);
            }
//            dt.Columns.Add("MembershipNumber", typeof(string));
//            dt.Columns.Add("Name", typeof(string));
//            dt.Columns.Add("Address", typeof(string));
//            dt.Columns.Add("MembershipNumber", typeof(string));
//            dt.Columns.Add("MembershipNumber", typeof(string));
//            dt.Columns.Add("CustomerName", typeof(string));
//            dt.Columns.Add("RegisteredAt", typeof(string));//not a typo, sadly.

            // ... lot more properties, often nested ones.

            foreach (var person in _data)
            {
                var row = dt.NewRow();
                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(person);
                }
//                row["CustomerId"] = cust.Id;
//                row["CustomerName"] = cust.At(reportDate).Name;
//                row["RegisteredAt"] = cust.RegisteredAt.ToShortDateString();
                //... lot more properties

                dt.Rows.Add(row);
            }
            var ds = new DataSet();
            ds.Tables.Add(dt);
            return ds; 
        }



//        private void ReportLoad(object sender, EventArgs e)
//        {
//            if (_isReportLoaded) return;
//
//            var src = new BindingSource();
//            ((System.ComponentModel.ISupportInitialize)(src)).BeginInit();
//
//            src.DataSource = _data;
//
//            var reportDataSource1 = new ReportDataSource();
//            reportDataSource1.Name = "People";
//            reportDataSource1.Value = src;
//
//            //            var dataset = new DataSet1();
//            //
//            //            dataset.BeginInit();
//            //
//            //            reportDataSource1.Name = "DataSet1";
//            //            //Name of the report dataset in our .RDLC file
//            //
//            //            reportDataSource1.Value = dataset.Customers;
//            reportDataSource1.Value = _data;
//            ReportViewer.LocalReport.DataSources.Add(reportDataSource1);
//
//            //                ReportDataSource items = new ReportDataSource();
//            //                _reportViewer.LocalReport.DataSources.Add(items);
//
//            ReportViewer.LocalReport.ReportPath = "../../Reports/Report3.rdlc";
//
//            ((System.ComponentModel.ISupportInitialize)(src)).EndInit();
//            //            dataset.EndInit();
//
//            //fill data into WpfApplication4DataSet
//
//            //            var accountsTableAdapter = new CustomersTableAdapter();
//            //
//            //            accountsTableAdapter.ClearBeforeFill = true;
//            //            accountsTableAdapter.Fill(dataset.Customers);
//            ReportViewer.RefreshReport();
//            _isReportLoaded = true;
//        }

        //        private void PrepareReport()
//        {
//            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
//
//            this.ProductBindingSource = new System.Windows.Forms.BindingSource(this.mform_components);
//            ((System.ComponentModel.ISupportInitialize)(this.ProductBindingSource)).BeginInit();
//
//
//
//            reportDataSource1.Name = "WpfhostReportViewer_Product";
//            reportDataSource1.Value = this.ProductBindingSource;
//
//            this.viewerInstance.LocalReport.DataSources.Add(reportDataSource1);
//            this.viewerInstance.LocalReport.ReportEmbeddedResource = "WpfhostReportViewer.MyReport.rdlc";
//            this.viewerInstance.Location = new System.Drawing.Point(89, 119);
//
//            ((System.ComponentModel.ISupportInitialize)(this.ProductBindingSource)).EndInit();
//        }   

    }
}
