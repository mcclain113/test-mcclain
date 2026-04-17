using System;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.BirthRegistry
{
    #nullable enable
    public class FinalizeViewModel
    {
        public int? BirthId { get; set; }
        
        public string? RegistrarSignature { get; set; }
        
        public DateOnly? DateOfRegistrarSignature { get; set; }

        // Parameterless constructor
        public FinalizeViewModel()
        {
            DateOfRegistrarSignature = DateOnly.FromDateTime(DateTime.Today);
        }

        // Constructor from Birth entity
        public FinalizeViewModel(Birth birth) : this()
        {
            if (birth != null)
            {
                BirthId = birth.BirthId;
                RegistrarSignature = birth.RegistrarSignature;
                DateOfRegistrarSignature = birth.DateOfRegistrarSignature ?? DateOnly.FromDateTime(DateTime.Today);
            }
        }

        public void UpdateBirthRecord(Birth birth)
        {
            if (birth == null)
            {
                throw new ArgumentNullException(nameof(birth));
            }
            
            birth.RegistrarSignature = this.RegistrarSignature?.Trim();
            birth.DateOfRegistrarSignature = this.DateOfRegistrarSignature;
        }
    }
    #nullable disable
}