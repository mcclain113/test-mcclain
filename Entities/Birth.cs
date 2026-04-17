using System;
using System.Collections.Generic;
using IS_Proj_HIT.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IS_Proj_HIT.Entities
{

    public partial class Birth
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BirthId { get; set; }

        public bool? IsWctmcbirth { get; set; }

        public int? FacilityId { get; set; }

        public int? AddressId { get; set; }

        public byte? BirthPlaceTypeId { get; set; }

        public bool? IsPlannedHomeBirth { get; set; }

        public int? DeliveringAttendantId { get; set; }

        public int? CertifierOfBirthId { get; set; }

        public bool? IsMotherTransferred { get; set; }

        

        public byte? Plurality { get; set; }

        public int? DeliveryPaymentSourceId { get; set; }

        public int? BirthFatherId { get; set; }

        public bool? PaternityAcknowledgementSigned { get; set; }
        public bool? MotherMarriedInPast { get; set; }
        public bool? MotherMarriedDuringPregnancy { get; set; }
        public bool? IsFatherHusbandOfMother { get; set; }
        #nullable enable
        public string? FacilityTransferredFromName { get; set; }

        public string? CertifierSignature { get; set; }
        public DateOnly? DateCertified { get; set; }
        [Column("BirthRegistrarSignature")]
        [StringLength(100)]
        public string? RegistrarSignature { get; set; }
        [Column("BirthRegistrarSignatureDate")]
        public DateOnly? DateOfRegistrarSignature { get; set; }




#nullable enable
        public string? MotherMrn { get; set; }
        public int? MotherPersonId { get; set; }
        public virtual Person? MotherPerson { get; set; }

        /*public int? NewbornPersonId { get; set; }
        public virtual Person? NewbornPerson { get; set; }*/

        public int? FatherPersonId { get; set; }
        public virtual Person? FatherPerson { get; set; }
        
        public string? Comments { get; set; }

        public virtual Address? Address { get; set; }

        public virtual BirthFather? BirthFather { get; set; }

        public virtual BirthPlaceType? BirthPlaceType { get; set; }

        public virtual Physician? CertifierOfBirth { get; set; }

        public virtual Physician? DeliveringAttendant { get; set; }

        public virtual PaymentSource? DeliveryPaymentSource { get; set; }

        public virtual Facility? Facility { get; set; }

#nullable disable


        public virtual Patient MotherMrnNavigation { get; set; } = null!;

        public virtual ICollection<Newborn> Newborns { get; set; } = new List<Newborn>();

        public virtual ICollection<Prenatal> Prenatals { get; set; } = new List<Prenatal>();
    }
}