using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels
{
    public class MiscStructuredDataViewModel
    {
        public DocumentType DocumentType { get; set; }

        public MiscStructuredDataViewModel()
        {
            // add any defaults here
        }

        public MiscStructuredDataViewModel(DocumentType documentType)
        {
            this.DocumentType = documentType;
        }
    }
}