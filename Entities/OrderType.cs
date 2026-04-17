using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class OrderType
    {
        public OrderType()
        {
            OrderInfos = new HashSet<OrderInfo>();
        }

        public int OrderTypeId { get; set; }
        public string OrderName { get; set; }

        public virtual ICollection<OrderInfo> OrderInfos { get; set; }
    }
}
