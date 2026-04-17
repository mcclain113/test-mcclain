using System;
using System.Linq;

namespace IS_Proj_HIT.ViewModels
{
    public class DocumentViewModel
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string DocumentTypeName { get; set; }
        public DateTime CreatedAt { get; set; }

#nullable enable
        public string? DocumentDescription { get; set; }
        public string? Notes { get; set; }
        public long? EncounterId { get; set; }
#nullable disable


                public bool IsImage =>
                    new[] { "jpg", "jpeg", "png", "gif" }
                    .Contains(FileType.ToLower());
    }
}
