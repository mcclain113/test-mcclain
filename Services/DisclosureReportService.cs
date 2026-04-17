using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

    public class DisclosureReportService : IDisclosureReportService
{
    private WCTCHealthSystemContext _context;

    public DisclosureReportService(WCTCHealthSystemContext context)
    {
        _context = context;
    }

    public List<PatientRequest> getDisclosureReport(bool isDisclosed, string firstName, string lastName)
    {
        return _context.Set<PatientRequest>()
        .FromSqlRaw("EXEC Rpt_HIPAA_AccountingDiscloures @IsDisclosed, @FirstName, @LastName",
            new SqlParameter("@IsDisclosed", isDisclosed),
            new SqlParameter("@FirstName", firstName),
            new SqlParameter("@LastName", lastName))
        .ToList();
    }
}

