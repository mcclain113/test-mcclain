using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.PhysicianStructuredData
{
    public class PhysicianDocsStructuredDataViewModel
    {
        public Priority Priority {get; set;}
        public NoteType NoteType {get; set;}
        public VisitType VisitType {get; set;}


        public PhysicianDocsStructuredDataViewModel() {}

        public PhysicianDocsStructuredDataViewModel(Priority priority)
        {
            this.Priority = priority;
        }

        public PhysicianDocsStructuredDataViewModel(NoteType noteType)
        {
            this.NoteType = noteType;
        }

        public PhysicianDocsStructuredDataViewModel(VisitType visitType)
        {
            this.VisitType = visitType;
        }


    }
}