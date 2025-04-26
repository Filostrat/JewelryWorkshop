using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime? OrderDate { get; set; }

    public decimal? OrderAmount { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual AspNetUser User { get; set; } = null!;
}
