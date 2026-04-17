using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public partial class OrderInfo
    {
        public OrderInfo()
        {
            AdmitOrders = new HashSet<AdmitOrder>();
        }

        public long OrderInfoId { get; set; }
        public long EncounterId { get; set; }
        public int OrderTypeId { get; set; }
        public int OrderingProviderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int PriorityId { get; set; }
        public bool? CoAuthorApproved { get; set; } = false;
        public DateTime? ProviderSignedDate {get; set;}
        public string Notes { get; set; }
        public bool IsOrderComplete { get; set; } = false;
        public DateTime? OrderCompletedDateTime { get; set; } = null;
        public int? OrderCompletedByID { get; set; }
        public bool IsVerbalOrder { get; set; } = false;
        public int? AuthenticatingProviderID { get; set; }
        public string AuthorESignature { get; set; }
        public string AuthenticatingProviderESignature { get; set; }
        public string AdmittingDiagnosis {get; set;}
        public DateTime? AuthenticatingProviderSignedDate {get; set;}
        public int? OrderedItemChargeID {get; set;}
        public bool? IsFasting {get; set;} = false;
        public int? HoursFasting {get; set;}
        public string TestReason {get; set;}
        public int? AuthorId { get; set; } 
        public DateTime? AuthorSignedDate {get; set;}
        public string ProviderESignature { get; set; }

        public virtual Physician Author {get; set;}
        public virtual Physician OrderingProvider { get; set; }
        public virtual Physician AuthenticatingProvider { get; set; }
        public virtual Physician CompletedByProvider { get; set; }
        public virtual Encounter Encounter { get; set; }
        public virtual OrderType OrderType { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual ChargeDefinition ChargeDefinition {get; set;}

        public virtual ICollection<AdmitOrder>AdmitOrders { get; set; }
    }
}