#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities
{
    public class Requester
    {
        public int RequesterId { get; set; }

        [Required(ErrorMessage ="Please enter the Given (First) Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage ="Please enter the Family Name")]
        public string? LastName { get; set; }

        public string? CompanyName { get; set; }

        public int? AddressId { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EmailAddress { get; set; }

        [Required(ErrorMessage ="Please select a Requester Type")]
        public byte RequesterTypeId { get; set; }

        [Required(ErrorMessage ="Please select a Requester Status")]
        public string? RequesterStatusId { get; set; }

        public string? Comments { get; set; }

        public virtual Address? Address { get; set; }

        public virtual ICollection<Disclosure> Disclosures { get; set; } = new List<Disclosure>();

        public virtual RequesterStatus? RequesterStatus { get; set; }

        public virtual RequesterType? RequesterType { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
