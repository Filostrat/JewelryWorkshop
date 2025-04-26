using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class WishList
{
    public int WishListId { get; set; }

    public string UserId { get; set; } = null!;

    public decimal? WishListAmount { get; set; }

    public string? WishListName { get; set; }

    public virtual AspNetUser User { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
