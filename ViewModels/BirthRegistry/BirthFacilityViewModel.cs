using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using IS_Proj_HIT.Entities;
using System.Linq;

namespace IS_Proj_HIT.ViewModels.BirthRegistry
{
    public class BirthFacilityViewModel
    {
        public int? BirthId { get; set; }
        public bool? IsWctmcbirth { get; set; }
        public int? FacilityId { get; set; }
        public int? AddressId { get; set; }
        public byte? BirthPlaceTypeId { get; set; }
        public bool? IsPlannedHomeBirth { get; set; }
        public int? DeliveringAttendantId { get; set; }
        public int? CertifierOfBirthId { get; set; }
        public bool? IsMotherTransferred { get; set; }
        public int? FacilityTransferredFromId { get; set; }
        public string CertifierSignature { get; set; }
        public DateOnly? DateCertified { get; set; }
        public int? NonWctmcFacilityId { get; set; }
        public string FacilityTransferredFromName { get; set; }
        public string TransferFacilityCity { get; set; }
        public string TransferFacilityZip { get; set; }
        public string TransferFacilityState { get; set; }
        public string NonWctmcFacilitySummary { get; set; }

        public Facility Facility { get; set; }
        public BirthPlaceType BirthPlaceType { get; set; }
        public Physician DeliveringAttendant { get; set; }
        public Physician CertifierOfBirth { get; set; }
        public Address NonFacilityAddress { get; set; }

        public string PlaceOfBirth { get; set; } 
        public string PrintedName { get; set; } 

        public string AttendantFirst { get; set; }
        public string AttendantMiddle { get; set; }
        public string AttendantLast { get; set; }
        public string AttendantNPI { get; set; }
        public string AttendantTitle { get; set; }

        public bool IsCertifierAttendant { get; set; }
        public string CertifierFirst { get; set; }
        public string CertifierMiddle { get; set; }
        public string CertifierLast { get; set; }
        public string CertifierNPI { get; set; }
        public string CertifierTitle { get; set; }

        public string NonFacilityAddressLine1 { get; set; }
        public string NonFacilityAddressLine2 { get; set; }
        public string NonFacilityCity { get; set; }
        public string NonFacilityState { get; set; }
        public string NonFacilityZip { get; set; }

        public List<SelectListItem> Facilities { get; set; }
        public List<SelectListItem> BirthPlaceTypes { get; set; }
        public List<SelectListItem> Physicians { get; set; }
        public List<SelectListItem> AttendantTitles { get; set; }

        public bool HasFacilityInformation =>
            IsWctmcbirth == true ||
            FacilityId.HasValue ||
            NonWctmcFacilityId.HasValue ||
            !string.IsNullOrWhiteSpace(NonFacilityAddressLine1);

        public bool HasAttendantInformation =>
            !string.IsNullOrWhiteSpace(AttendantFirst) ||
            !string.IsNullOrWhiteSpace(AttendantLast) ||
            DeliveringAttendantId.HasValue;

        public bool HasCertifierInformation =>
            IsCertifierAttendant ||
            !string.IsNullOrWhiteSpace(CertifierFirst) ||
            !string.IsNullOrWhiteSpace(CertifierLast) ||
            CertifierOfBirthId.HasValue;

        public string AttendantFullName =>
            $"{AttendantFirst} {AttendantLast}".Trim().Replace("  ", " ");

        public string CertifierFullName =>
            IsCertifierAttendant ? AttendantFullName :
            $"{CertifierFirst} {CertifierLast}".Trim().Replace("  ", " ");

        public BirthFacilityViewModel()
        {
            Facilities = new List<SelectListItem>();
            BirthPlaceTypes = new List<SelectListItem>();
            Physicians = new List<SelectListItem>();
            AttendantTitles = new List<SelectListItem>();

            IsWctmcbirth = true; 
            PlaceOfBirth = "WCTMC";
            IsCertifierAttendant = true; 
        }

        public BirthFacilityViewModel(Birth birth) : this()
        {
            if (birth != null)
            {
                BirthId = birth.BirthId;
                IsWctmcbirth = birth.IsWctmcbirth;
                FacilityId = birth.FacilityId;
                AddressId = birth.AddressId;
                BirthPlaceTypeId = birth.BirthPlaceTypeId;
                IsPlannedHomeBirth = birth.IsPlannedHomeBirth;
                DeliveringAttendantId = birth.DeliveringAttendantId;
                CertifierOfBirthId = birth.CertifierOfBirthId;
                IsMotherTransferred = birth.IsMotherTransferred;
                FacilityTransferredFromName = birth.FacilityTransferredFromName;
                CertifierSignature = birth.CertifierSignature;
                DateCertified = birth.DateCertified;

                PlaceOfBirth = birth.IsWctmcbirth == true ? "WCTMC" : "NonFacility";
                if (birth.IsWctmcbirth == false && birth.FacilityId.HasValue)
                {
                    NonWctmcFacilityId = birth.FacilityId;
                }
                else
                {
                    NonWctmcFacilityId = null;
                }

                IsCertifierAttendant = birth.DeliveringAttendantId.HasValue &&
                                       birth.CertifierOfBirthId.HasValue &&
                                       birth.DeliveringAttendantId == birth.CertifierOfBirthId;


                Facility = birth.Facility;
                BirthPlaceType = birth.BirthPlaceType;
                DeliveringAttendant = birth.DeliveringAttendant;
                CertifierOfBirth = birth.CertifierOfBirth;
                NonFacilityAddress = birth.Address;

                if (birth.DeliveringAttendant != null)
                {
                    AttendantFirst = birth.DeliveringAttendant.FirstName ?? string.Empty;
                    AttendantLast = birth.DeliveringAttendant.LastName ?? string.Empty;
                    AttendantNPI = birth.DeliveringAttendant.License ?? string.Empty;
                    AttendantTitle = birth.DeliveringAttendant.Credentials ?? string.Empty;
                }

                if (birth.CertifierOfBirth != null && !IsCertifierAttendant)
                {
                    CertifierFirst = birth.CertifierOfBirth.FirstName ?? string.Empty;
                    CertifierLast = birth.CertifierOfBirth.LastName ?? string.Empty;
                    CertifierNPI = birth.CertifierOfBirth.License ?? string.Empty;
                    CertifierTitle = birth.CertifierOfBirth.Credentials ?? string.Empty;
                }

                if (birth.Address != null)
                {
                    NonFacilityAddressLine1 = birth.Address.Address1 ?? string.Empty;
                    NonFacilityAddressLine2 = birth.Address.Address2 ?? string.Empty;
                    NonFacilityCity = birth.Address.City ?? string.Empty;
                    NonFacilityZip = birth.Address.PostalCode ?? string.Empty;

                    if (birth.Address.AddressState != null)
                    {
                        NonFacilityState = birth.Address.AddressState.StateName ?? string.Empty;
                    }
                }
            }
        }

        public Birth ToEntity(Birth existingEntity = null)
        {
            var entity = existingEntity ?? new Birth();

            entity.IsWctmcbirth = PlaceOfBirth == "WCTMC";
            entity.BirthPlaceTypeId = BirthPlaceTypeId;
            entity.IsPlannedHomeBirth = IsPlannedHomeBirth;
            entity.IsMotherTransferred = IsMotherTransferred;
            entity.FacilityTransferredFromName = FacilityTransferredFromName;
            entity.DateCertified = DateCertified;

            if (PlaceOfBirth == "WCTMC")
            {
                entity.FacilityId = FacilityId;
                entity.AddressId = null; 
            }
            else
            {
                entity.FacilityId = null; 
                entity.AddressId = AddressId;
            }

            entity.DeliveringAttendantId = DeliveringAttendantId;
       
            entity.CertifierOfBirthId = IsCertifierAttendant ? DeliveringAttendantId : CertifierOfBirthId;

            return entity;
        }

        public Physician CreateAttendantPhysician(Physician existingPhysician = null)
        {
            if (string.IsNullOrWhiteSpace(AttendantFirst) && string.IsNullOrWhiteSpace(AttendantLast))
                return null;

            var physician = existingPhysician ?? new Physician();
            physician.FirstName = AttendantFirst ?? string.Empty;
            physician.LastName = AttendantLast ?? string.Empty;
            physician.License = AttendantNPI;
            physician.Credentials = AttendantTitle;
            physician.LastModified = DateTime.Now;

            return physician;
        }

        public Physician CreateCertifierPhysician(Physician existingPhysician = null)
        {
            if (IsCertifierAttendant)
                return null; // Use the attendant physician

            if (string.IsNullOrWhiteSpace(CertifierFirst) && string.IsNullOrWhiteSpace(CertifierLast))
                return null;

            var physician = existingPhysician ?? new Physician();
            physician.FirstName = CertifierFirst ?? string.Empty;
            physician.LastName = CertifierLast ?? string.Empty;
            physician.License = CertifierNPI;
            physician.Credentials = CertifierTitle;
            physician.LastModified = DateTime.Now;

            return physician;
        }
        public class FacilitySearchResultVm
        {
            public int Id { get; set; }  
            public string Name { get; set; }
            public string Description { get; set; }
            public string Phone { get; set; }

            public string Street { get; set; }
            public string Street2 { get; set; }
            public string City { get; set; }
            public string County { get; set; }
            public string Postal { get; set; }
            public string State { get; set; }
        }

    }
}