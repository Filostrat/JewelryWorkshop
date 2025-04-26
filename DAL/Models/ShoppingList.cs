using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ShoppingList
{
    public int ShoppingListId { get; set; }

    public int ProductId { get; set; }

    public string UserId { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
