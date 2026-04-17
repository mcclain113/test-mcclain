using System.Collections.Generic;

    public interface IDisclosureReportService
{
    List<PatientRequest> getDisclosureReport(bool isDisclosed, string firstName, string lastName);
}
