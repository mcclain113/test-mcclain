#nullable enable
using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public class RequestedItem
    {
        public int RequestedItemId { get; set; }
        public int RequestId { get; set; }

        public int DocumentRequestedId { get; set; }

        public string ItemStatusId { get; set; } = null!;

        public bool? IsDisclosed { get; set; }

        public string? Comments { get; set; }

        public virtual Request Request { get; set; } = null!;
        public virtual DocumentRequested DocumentRequested { get; set; } = null!;

        public virtual ItemStatus ItemStatus { get; set; } = null!;

    }
}
