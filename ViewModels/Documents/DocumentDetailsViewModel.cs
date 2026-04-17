using System;
using System.Linq;

namespace IS_Proj_HIT.ViewModels
{
    public class DocumentDetailsViewModel
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }    //this is the same as FileType, like "image/png" or "application/pdf"
        public string Base64Content { get; set; }   // the document content, already encoded file bytes
        public string DocumentTypeName { get; set; }
        public string Mrn { get; set; }

#nullable enable
        public string? DocumentDescription { get; set; }
        public string? Notes { get; set; }
        public long? EncounterId { get; set; }
#nullable disable

        // these booleans are calculated in the view, mapping in the controller method is not required
        public bool IsImage =>
            !string.IsNullOrWhiteSpace(MimeType) &&
            MimeType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);

        public bool IsPdf =>
            string.Equals(MimeType, "application/pdf", StringComparison.OrdinalIgnoreCase);

    }
}