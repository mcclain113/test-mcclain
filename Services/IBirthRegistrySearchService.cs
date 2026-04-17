using IS_Proj_HIT.ViewModels.BirthRegistry;
using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Services
{
    public interface IBirthRegistrySearchService
    {
        List<BirthRegistryArchiveViewModel> SearchBirthRegistries(
            string motherFirstName, 
            string motherLastName, 
            string motherMrn,
            string motherSsn, 
            DateTime? motherDateOfBirth,
            string newbornFirstName, 
            string newbornLastName, 
            string newbornMrn,
            DateTime? birthDateRangeStart, 
            DateTime? birthDateRangeEnd, 
            string registryFilterType = "all");
    }
}