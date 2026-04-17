using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public partial class DocumentType
    {
        public byte DocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeLevel { get; set; }

        public DocumentType()
        {
            Documents = new HashSet<Document>();
        }

        public virtual ICollection<Document> Documents { get; set; }

    }
}