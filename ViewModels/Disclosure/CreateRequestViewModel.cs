using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using IS_Proj_HIT.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

#nullable enable

namespace IS_Proj_HIT.ViewModels.Disclosure
{
    public class CreateRequestViewModel
    {
        public Request Request { get; set; } = new Request();

        public bool IsRequestForPersonalAppearanceChecked
        {
            get => Request.IsRequestForPersonalAppearance ?? false;
            set => Request.IsRequestForPersonalAppearance = value;
        }

        public bool IsRequestForOriginalRecordsChecked
        {
            get => Request.IsRequestForOriginalRecords ?? false;
            set => Request.IsRequestForOriginalRecords = value;
        }

        public bool IsRequestForCertifiedRecordsChecked
        {
            get => Request.IsRequestForCertifiedRecords ?? false;
            set => Request.IsRequestForCertifiedRecords = value;
        }

        public bool IsRiskManagementAlertChecked
        {
            get => Request.IsRiskManagementAlert ?? false;
            set => Request.IsRiskManagementAlert = value;
        }

        public bool IsAuthorizationValidChecked
        {
            get => Request.IsAuthorizationValid ?? false;
            set => Request.IsAuthorizationValid = value;
        }

        public bool IsSpecialAuthorizationRequiredChecked
        {
            get => Request.IsSpecialAuthorizationRequired ?? false;
            set => Request.IsSpecialAuthorizationRequired = value;
        }

        public bool IsAttestationRequiredChecked
        {
            get => Request.IsAttestationRequired ?? false;
            set => Request.IsAttestationRequired = value;
        }

        public bool IsTpoDisclosureChecked
        {
            get => Request.IsTpodisclosure ?? false;
            set => Request.IsTpodisclosure = value;
        }

        public bool IsSubpoenaChecked
        {
            get => Request.IsSubpoena ?? false;
            set => Request.IsSubpoena = value;
        }

        public bool IsCourtOrderChecked
        {
            get => Request.IsCourtOrder ?? false;
            set => Request.IsCourtOrder = value;
        }

        public Facility? Facility { get; set; }

        public Requester? Requester { get; set; }

        public Address? Address { get; set; }

        public Country? Country { get; set; }

        public AddressState? AddressState {get; set;}

        public Patient? PatientMrnNavigation { get; set; }

        public PatientRepresentative? PatientRepresentative { get; set; }
        public int? PatientRepresentativeId { get; set; }

        public UserTable? EnteredByNavigation { get; set; }

        public UserTable? CompletedByNavigation { get; set; }

        public RequestPriority? RequestPriority { get; set; }

        public RequestPurpose? RequestPurpose { get; set; }

        public RequestReleaseFormat? RequestReleaseFormat { get; set; }

        public RequestStatus? RequestStatus { get; set; }
        public RequestedItem RequestedItem { get; set; } = null!;
        public DocumentRequested? DocumentRequested { get; set; }

        public RequestStatusReason? RequestStatusReason { get; set; }

        public ICollection<RequestedItem> RequestedItems { get; set; } = new List<RequestedItem>();

        public ICollection<Entities.Disclosure> Disclosures { get; set; } = new List<Entities.Disclosure>();

        // Select lists for dropdowns
        public List<SelectListItem> Requesters { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequestPurposes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequestPriorities { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequestReleaseFormats { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequestStatuses { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequestStatusReasons { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> CountryOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AddressStateOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequesterTypeOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequesterStatusOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequestPurposeOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequestPriorityOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RequestedItemsList { get; set; } = new List<SelectListItem>();

        public List<int> SelectedRequestedItemIds { get; set; } = new List<int>();
        public List<SelectListItem> RequestStatusOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusReasonOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> UserOptions { get; set; } = new List<SelectListItem>();

        // properties for Requested Item Other input
        public string? RequestedItemOther { get; set; }

        // properties for Release Format Other input
        public string? ReleaseFormatOther { get; set; }

        // TODO: Determin if these can be deleted
        // Patient details properties for when the patient is not in the system 
        public string? PatientFirstName { get; set; }
        public string? PatientMiddleName { get; set; }
        public string? PatientLastName { get; set; }
        public DateOnly? PatientDob { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public string? PatientEmailAddress { get; set; }

        // New properties for "Entered By" fields
        public int? EnteredByUserId { get; set; }
        public string? EnteredByFullName { get; set; }

        // New properties for "Completed By" fields
        public int? CompletedByUserId { get; set; }
        public string? CompletedByFullName { get; set; }

        // New properties for Request Purpose Other
        public string? RequestPurposeOther { get; set; }

        // New propery for Status Reason Other input
        public string? StatusReasonOther { get; set; }
    }
}