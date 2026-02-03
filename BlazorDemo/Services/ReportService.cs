using DevExpress.XtraReports.Web.ReportDesigner.Native;
using Microsoft.Reporting.NETCore;
using System.Data;
using System.Data.SqlClient;
using System.IO;


public class ReportService
{
    public byte[] GenerateReport(string reportPath, DataTable dataSource, string datasetName)
    {
        using var report = new LocalReport();
        report.ReportPath = reportPath;

        report.DataSources.Add(new ReportDataSource(datasetName, dataSource));

        // Export to PDF
        return report.Render("PDF");
    }

    private readonly SqlConnectionConfiguration _configuration;

    public ReportService(SqlConnectionConfiguration configuration)
    {
        _configuration = configuration;
    }



    public byte[] GenerateReportFromDatabase(string reportPath)
    {
        // Step 1: Load data from database
        var dataTable = new DataTable("Party");

        using (SqlConnection conn = new SqlConnection(_configuration.Value))
        {
            conn.Open();
            using var cmd = new SqlCommand("select a.*,b.cname,b.addr from party a,company b  where a.PartyAsPartytype=b.cid", conn);
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);
        }

        // Step 2: Load and render RDLC report
        var report = new LocalReport();
        report.ReportPath = reportPath;

        report.DataSources.Add(new ReportDataSource("Party", dataTable));

        return report.Render("PDF");
    }
    




}
