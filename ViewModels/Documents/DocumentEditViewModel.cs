using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IS_Proj_HIT.ViewModels
{
    public class DocumentEditViewModel
    {
        [Required]
        public int DocumentId { get; set; }

        [Required]
        [Display(Name = "Patient MRN")]
        public string Mrn { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public byte DocumentTypeID { get; set; }

#nullable enable
        [Display(Name = "Description")]
        public string? DocumentDescription { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Encounter")]
        public long? EncounterId { get; set; }
        public string? Section { get; set; }
#nullable disable

        // for the “Type” dropdown
        public List<SelectListItem> DocumentTypes { get; set; }
            = new List<SelectListItem>();

        // enable re­assigning to a different encounter
        public List<SelectListItem> EncounterSelectList { get; set; }
            = new List<SelectListItem>();
            
        // For inline preview
        public string MimeType { get; set; }
        public string Base64Content { get; set; }

        public bool IsImage =>
            !string.IsNullOrWhiteSpace(MimeType) &&
            MimeType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);

        public bool IsPdf =>
            string.Equals(MimeType, "application/pdf", StringComparison.OrdinalIgnoreCase);

    }
}