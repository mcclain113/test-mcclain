using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using IS_Proj_HIT.Controllers;
using ReportViewerCore;
namespace IS_Proj_HIT.Controllers;

using IS_Proj_HIT.ViewModels.Reports;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

public class ReportHubController : BaseController
{
    private IDisclosureReportService disclosureReportService;

    public ReportHubController(IDisclosureReportService reportService)
    {
        disclosureReportService = reportService;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CategoryView(string category)
    {
        return View("Category", category);
    }

    public IActionResult ReportView(string report)
    {
        ReportDisclosureDisplay reportDisplay = new ReportDisclosureDisplay();
        reportDisplay.ReportName = report;
        reportDisplay.ReportFile = null;
        reportDisplay.ErrorMessage = "Enter First and Last Name.";
        return View("ReportView", reportDisplay);
    }

    [HttpPost("/reports/disclosure/pdf")]
    public IActionResult DisclosurePdf(string firstName, string lastName)
    {
        ReportDisclosureDisplay reportDisplay = new ReportDisclosureDisplay();
        //Name will come from the database, This is a placeholder
        reportDisplay.ReportName = "Disclosure Report";
        //Creates new LocalReport
        var report = new LocalReport();

        //Finds the path to rdlc file
        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Reports",
            "HipaaAccounting.rdlc");

        //Turns rdlc file into a file stream
        using var stream = System.IO.File.OpenRead(path);

        //Loads the rdlc report definition into report
        report.LoadReportDefinition(stream);

        List<PatientRequest> reportData = disclosureReportService.getDisclosureReport(true, firstName, lastName);

        if (reportData.Count == 0)
        {
            reportDisplay.ErrorMessage = "No data found for the " + firstName + " " + lastName + ". Please check the spelling and try again.";
            return View("ReportView", reportDisplay);
        }
        //Adds data to the dataset in the report
        //IMPORTANT: The dataset name must match the name passed to the new ReportDataSource
        report.DataSources.Add(
            new ReportDataSource("PatientRequest", reportData)
        );

        var parameters = new List<ReportParameter>
        {
            new ReportParameter("FirstName", reportData[0].PatientFirstName),
            new ReportParameter("LastName", reportData[0].PatientFirstName),
            new ReportParameter("IsDisclosed", "1"),
        };

        report.SetParameters(parameters);

        string deviceInfo = @"
        <DeviceInfo>
            <OutputFormat>PDF</OutputFormat>
            <PageWidth>11in</PageWidth>
            <PageHeight>8.5in</PageHeight>
            <MarginTop>0in</MarginTop>
            <MarginLeft>0in</MarginLeft>
            <MarginRight>0in</MarginRight>
            <MarginBottom>0in</MarginBottom>
        </DeviceInfo>";

        //Renders the report into a PDF
        var result = report.Render("PDF", deviceInfo);

        var file = File(result, "application/pdf", "DisclosureReport.pdf");

        reportDisplay.ReportMessage = "Report for " + firstName + " " + lastName;
        reportDisplay.ReportFile = Url.Action("DisclosurePdfView", "ReportHub", new { firstName, lastName });
        //Returns report to user
        return View("ReportView", reportDisplay);
    }

    public IActionResult DisclosurePdfView(string firstName, string lastName)
    {
        //Creates new LocalReport
        var report = new LocalReport();

        //Finds the path to rdlc file
        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Reports",
            "HipaaAccounting.rdlc");

        //Turns rdlc file into a file stream
        using var stream = System.IO.File.OpenRead(path);

        //Loads the rdlc report definition into report
        report.LoadReportDefinition(stream);

        List<PatientRequest> reportData = disclosureReportService.getDisclosureReport(true, firstName, lastName);

        //Adds data to the dataset in the report
        //IMPORTANT: The dataset name must match the name passed to the new ReportDataSource
        report.DataSources.Add(
            new ReportDataSource("PatientRequest", reportData)
        );

        var parameters = new List<ReportParameter>
        {
            new ReportParameter("FirstName", reportData[0].PatientFirstName),
            new ReportParameter("LastName", reportData[0].PatientFirstName),
            new ReportParameter("IsDisclosed", "1"),
        };

        report.SetParameters(parameters);

        string deviceInfo = @"
        <DeviceInfo>
            <OutputFormat>PDF</OutputFormat>
            <PageWidth>11in</PageWidth>
            <PageHeight>8.5in</PageHeight>
            <MarginTop>0in</MarginTop>
            <MarginLeft>0in</MarginLeft>
            <MarginRight>0in</MarginRight>
            <MarginBottom>0in</MarginBottom>
        </DeviceInfo>";

        //Renders the report into a PDF
        var result = report.Render("PDF", deviceInfo);

        //Returns report to user
        return File(result, "application/pdf");
    }
}