using System.Collections.Generic;
using IS_Proj_HIT.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IS_Proj_HIT.ViewModels
{
    public class DocWrapperViewModel
    {
        // The patient entity or a dedicated PatientViewModel
        public Patient Patient { get; set; }
        public Encounter Encounter { get; set; }

        // The list of documents to render
        public IEnumerable<DocumentViewModel> Documents { get; set; }

        // Document-type dropdown items
        public IEnumerable<SelectListItem> DocumentTypes { get; set; }

        public int? DocType { get; set; }

        // The views are used in multiple scenarios so calculate the header text here
        public string HeaderText
        {
            get
            {
                if (Encounter == null) return "Patient Documents";
                if (!DocType.HasValue) return "Encounter Documents";
                return DocType.Value switch
                {
                    4 => "Laboratory & Pathology Reports",
                    5 => "Radiology Reports",
                    12 => "Cardiology Reports",
                    13 => "Other Diagnostic Reports",
                    _ => "Encounter Documents"
                };
            }
        }
    }
}