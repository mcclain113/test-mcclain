#nullable enable
using System.Collections.Generic;
using System;

namespace IS_Proj_HIT.Entities
{
    public class Request
    {
        public int RequestId { get; set; }

        public int? FacilityId { get; set; }

        public int RequesterId { get; set; }

        public string? PatientMrn { get; set; }

        public DateOnly DateCreated { get; set; }

        public int EnteredBy { get; set; }

        public byte RequestPurposeId { get; set; }

        public string RequestPriorityId { get; set; } = null!;

        public DateOnly? DateNeeded { get; set; }

        public byte? RequestReleaseFormatId { get; set; }

        public DateOnly? RequestDate { get; set; }

        public DateOnly? AuthorizationExpirationDate { get; set; }

        public bool? IsAuthorizationValid { get; set; }

        public bool? IsSpecialAuthorizationRequired { get; set; }
        
        public bool? IsAttestationRequired { get; set; }

        public bool? IsTpodisclosure { get; set; }

        public bool? IsSubpoena { get; set; }
        public bool? IsCourtOrder { get; set; }

        public DateOnly? CourtOrderIssueDate { get; set; }

        public DateOnly? HearingDate { get; set; }

        public string? CaseNumber { get; set; }

        public string? Plaintiff { get; set; }

        public string? Defendant { get; set; }

        public bool? IsRequestForPersonalAppearance { get; set; }

        public bool? IsRequestForOriginalRecords { get; set; }

        public bool? IsRequestForCertifiedRecords { get; set; }

        public bool? IsRiskManagementAlert { get; set; }

        public string? Comments { get; set; }

        public string? RequestStatusId { get; set; }

        public byte? RequestStatusReasonId { get; set; }

        public int? CompletedBy { get; set; }

        public DateOnly? DateCompleted { get; set; }

        public bool? IsPatientInSystem { get; set; }

        public int? PatientRepresentativeId { get; set; }

        public string PatientFirstName { get; set; } = null!;

        public string? PatientMiddleName { get; set; }

        public string PatientLastName { get; set; } = null!;

        public DateOnly? PatientDob { get; set; }

        public string? PatientPhoneNumber { get; set; }

        public string? PatientEmailAddress { get; set; }

        public virtual UserTable? CompletedByNavigation { get; set; }

        public virtual ICollection<Disclosure> Disclosures { get; set; } = new List<Disclosure>();

        public virtual UserTable EnteredByNavigation { get; set; } = null!;

        public virtual Facility? Facility { get; set; }

        public virtual Patient? PatientMrnNavigation { get; set; }

        public virtual PatientRepresentative? PatientRepresentative { get; set; }

        public virtual RequestPriority RequestPriority { get; set; } = null!;

        public virtual RequestPurpose RequestPurpose { get; set; } = null!;

        public virtual RequestReleaseFormat? RequestReleaseFormat { get; set; }

        public virtual RequestStatus? RequestStatus { get; set; }

        public virtual RequestStatusReason? RequestStatusReason { get; set; }

        public virtual Requester Requester { get; set; } = null!;

        public virtual ICollection<RequestedItem> RequestedItems { get; set; } = new List<RequestedItem>();

    }
}
