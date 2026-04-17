using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Data;
using IS_Proj_HIT.ViewModels.BirthRegistry;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IS_Proj_HIT.Services
{
    public class BirthRegistrySearchService(IWCTCHealthSystemRepository repository, IBirthRegistryValidationService validationService) : IBirthRegistrySearchService
    {
        private readonly IWCTCHealthSystemRepository _repository = repository;
        private readonly IBirthRegistryValidationService _validationService = validationService;

        public List<BirthRegistryArchiveViewModel> SearchBirthRegistries(
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
            string registryStatus = "all")
        {
            try
            {
                var query = _repository.Births
                    .Include(b => b.MotherMrnNavigation)
                    .Include(b => b.Facility)
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.NewbornMrnNavigation)
                    .Include(b => b.Newborns)
                        .ThenInclude(n => n.LaborAndDeliveries)
                    .Include(b => b.Prenatals)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(motherFirstName))
                    query = query.Where(b => EF.Functions.Like(b.MotherMrnNavigation.FirstName, $"%{motherFirstName}%"));

                if (!string.IsNullOrWhiteSpace(motherLastName))
                    query = query.Where(b => EF.Functions.Like(b.MotherMrnNavigation.LastName, $"%{motherLastName}%"));

                if (!string.IsNullOrWhiteSpace(motherMrn))
                    query = query.Where(b => EF.Functions.Like(b.MotherMrn, $"%{motherMrn.Trim()}%"));

                if (!string.IsNullOrWhiteSpace(motherSsn))
                {
                    var normalizedSsn = motherSsn.Trim().Replace("-", "");
                    query = query.Where(b => EF.Functions.Like(b.MotherMrnNavigation.Ssn, $"%{normalizedSsn}%"));
                }

                if (motherDateOfBirth.HasValue)
                    query = query.Where(b => b.MotherMrnNavigation.Dob.HasValue && 
                                           b.MotherMrnNavigation.Dob.Value.Date == motherDateOfBirth.Value.Date);
                
                var births = query.ToList();
                var results = new List<BirthRegistryArchiveViewModel>();

                bool hasNewbornCriteria = !string.IsNullOrWhiteSpace(newbornFirstName) || 
                                          !string.IsNullOrWhiteSpace(newbornLastName) || 
                                          !string.IsNullOrWhiteSpace(newbornMrn) ||
                                          birthDateRangeStart.HasValue || 
                                          birthDateRangeEnd.HasValue;

                foreach (var birth in births)
                {
                    var newborns = birth.Newborns?.ToList() ?? new List<Newborn>();

                    if (newborns.Count == 0)
                    {
                        if (!hasNewbornCriteria)
                        {
                            results.Add(MapBirthToViewModel(birth, birth.MotherMrnNavigation, null));
                        }
                        continue;
                    }

                    foreach (var newborn in newborns)
                    {
                        if (!hasNewbornCriteria || NewbornMatchesCriteria(newborn, newbornFirstName, newbornLastName, 
                                                                       newbornMrn, birthDateRangeStart, birthDateRangeEnd))
                        {
                            results.Add(MapBirthToViewModel(birth, birth.MotherMrnNavigation, newborn));
                        }
                    }
                }

                if (registryStatus == "complete")
                {
                    results = results.Where(r => r.IsComplete).ToList();
                }
                else if (registryStatus == "incomplete")
                {
                    results = results.Where(r => !r.IsComplete).ToList();
                }

                return results
                    .OrderBy(r => r.MotherLastName)
                    .ThenBy(r => r.MotherFirstName)
                    .ThenByDescending(r => r.BirthDate)
                    .ToList();
            }
            catch
            {
                return new List<BirthRegistryArchiveViewModel>();
            }
        }

        private bool NewbornMatchesCriteria(
            Newborn newborn,
            string firstName, 
            string lastName, 
            string mrn,
            DateTime? birthDateRangeStart, 
            DateTime? birthDateRangeEnd)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                if (string.IsNullOrEmpty(newborn.NewbornMrnNavigation?.FirstName) ||
                    !newborn.NewbornMrnNavigation.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                if (string.IsNullOrEmpty(newborn.NewbornMrnNavigation?.LastName) ||
                    !newborn.NewbornMrnNavigation.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(mrn))
            {
                if (string.IsNullOrEmpty(newborn.NewbornMrn) ||
                    !newborn.NewbornMrn.Contains(mrn, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            if (birthDateRangeStart.HasValue)
            {
                if (!newborn.DateAndTimeOfBirth.HasValue ||
                    newborn.DateAndTimeOfBirth.Value.Date < birthDateRangeStart.Value.Date)
                {
                    return false;
                }
            }

            if (birthDateRangeEnd.HasValue)
            {
                if (!newborn.DateAndTimeOfBirth.HasValue ||
                    newborn.DateAndTimeOfBirth.Value.Date > birthDateRangeEnd.Value.Date)
                {
                    return false;
                }
            }

            return true;
        }

        private BirthRegistryArchiveViewModel MapBirthToViewModel(Birth birth, Patient mother, Newborn newborn)
        {
            return new BirthRegistryArchiveViewModel
            {
                BirthId = birth.BirthId,
                MotherMrn = mother?.Mrn,
                MotherFirstName = mother?.FirstName,
                MotherMiddleName = mother?.MiddleName,
                MotherLastName = mother?.LastName,
                MotherDob = mother?.Dob,
                NewbornMrn = newborn?.NewbornMrn,
                NewbornFirstName = newborn?.NewbornMrnNavigation?.FirstName,
                NewbornMiddleName = newborn?.NewbornMrnNavigation?.MiddleName,
                NewbornLastName = newborn?.NewbornMrnNavigation?.LastName,
                NewbornDob = newborn?.DateAndTimeOfBirth,
                FacilityName = birth.Facility?.Name ?? "Unknown",
                BirthDate = newborn?.DateAndTimeOfBirth,
                CertifierSignature = birth.CertifierSignature,
                BirthCount = mother?.Births?.Count ?? 0,
                IsComplete = _validationService.IsRegistryComplete(birth)
            };
        }
    }
}