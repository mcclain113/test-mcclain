using IS_Proj_HIT.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IS_Proj_HIT.ViewModels.Disclosure
{
    public class RequestDetailsViewModel
    {
        public Request Request { get; set; }
        public Requester Requester { get; set; }
        public RequestReleaseFormat RequestReleaseFormat { get; set; }
        public Patient Patient { get; set; }
        public PatientRepresentative PatientRepresentative { get; set; }
        public IS_Proj_HIT.Entities.Disclosure Disclosure { get; set; }
        public List<DisclosureFeeViewModel> DisclosureFeeDetails {get; set;} = new List<DisclosureFeeViewModel>();
        public RequestedItem RequestedItem { get; set; }
        public DocumentRequested DocumentRequested { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public List<ExtendedSelectListItem> DocumentRequestedOptions { get; set; }
        public List<ExtendedSelectListItem> ItemStatusOptions { get; set; }
        public int? RequestedItemBeingEditedId { get; set; }
        public RequestedItem RequestedItemBeingEdited { get; set; } = new RequestedItem();
        public int? DisclosurePaymentBeingEditedId { get; set; }
        public DisclosurePayment DisclosurePaymentBeingEdited { get; set; } = new DisclosurePayment();
        public Request RequestBeingEdited { get; set; } = new Request();
        public PatientRepresentative PatientRepresentativeBeingEdited { get; set; } = new PatientRepresentative();
        public IS_Proj_HIT.Entities.Disclosure DisclosureBeingEdited { get; set; } = new Entities.Disclosure();
        public DisclosurePayment DisclosurePayment { get; set; }
        public UserTable CompletedBy { get; set; }
        public UserTable EnteredBy { get; set; }
        public List<SelectListItem> UserTableList { get; set; }
        public List<SelectListItem> Requesters { get; set; }
        public List<SelectListItem> RequestPurposes { get; set; }
        public List<SelectListItem> RequestPriorities { get; set; }
        public List<SelectListItem> RequestStatuses { get; set; }
        public List<SelectListItem> RequestStatusReasons { get; set; }
        public List<SelectListItem> RequesterTypes { get; set; }
        public List<SelectListItem> RequesterStatuses { get; set; }
        public List<SelectListItem> RequestReleaseFormats { get; set; }
        public List<SelectListItem> PatientRepresentatives { get; set; }
        public List<SelectListItem> PaymentTypes { get; set; }
        public List<SelectListItem> DisclosureFeeAmounts { get; set; }
        public List<SelectListItem> DisclosureFeeDescriptions { get; set; }
        public List<SelectListItem> DisclosureFeeComments { get; set; }
        public List<SelectListItem> DisclosureFeeEffectiveDates { get; set; }
        public List<SelectListItem> DisclosureFeeExpirationDates { get; set; }
        public List<SelectListItem> AddressStates { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public DisclosureFeeType DisclosureFeeType { get; set; } = new DisclosureFeeType();

    }
}
