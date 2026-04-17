using System;

public class PatientRequest
{
    // Patient Info
    public string MRN { get; set; }
    public string PatientName { get; set; }
    public string PatientFullName { get; set; }
    public string PatientFirstName { get; set; }
    public string PatientMiddleName { get; set; }
    public string PatientLastName { get; set; }
    public DateTime? PatientDOB { get; set; }

    // Patient Address
    public string PatientsAddress { get; set; }
    public string PatientMailAddress1 { get; set; }
    public string PatientMailAddress2 { get; set; }
    public string PatientMailCity { get; set; }
    public string PatientMailState { get; set; }
    public string PatientMailZip { get; set; }

    // Facility Info
    public string FacilityDesc { get; set; }
    public string FacilityPhoneNumber { get; set; }
    public string FacilityAddress1 { get; set; }
    public string FacilityCity { get; set; }
    public string FacilityState { get; set; }
    public string FacilityPostalCode { get; set; }

    // Request Info
    public int RequestID { get; set; }
    public DateTime? PatientRequestDate { get; set; }
    public string RequestorCompany { get; set; }
    public string RequestorFirstName { get; set; }
    public string RequestorLastName { get; set; }
    public string RequestorAddress1 { get; set; }
    public string RequestorAddress2 { get; set; }
    public string RequestorCity { get; set; }
    public string RequestorState { get; set; }
    public string RequestorPostalCode { get; set; }

    // Disclosure Info
    public int DisclosureID { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public DateTime? DisclosureDate { get; set; }
    public decimal? InvoiceTotal { get; set; }
    public bool? IsPaymentRequired { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? AccountingStartDate { get; set; }

    // Purpose / Legal
    public string PurposeOfDisclosure { get; set; }
    public string LegalAuthority { get; set; }

    // Items
    public bool? ItemDisclosed { get; set; }
    public string RequestedItemsComments { get; set; }
    public string ItemStatus { get; set; }
    public string ItemStatusDescription { get; set; }
    public string DisclosedItems { get; set; }

    // Misc
    public bool? IsAccountingRequired { get; set; }
    public string DocumentTypeName { get; set; }


}