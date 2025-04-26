using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class PreciousStone
{
    public int PreciousStoneId { get; set; }

    public string PreciousStoneName { get; set; } = null!;

    public decimal PriceForCarat { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<ProductsType> ProductTypes { get; set; } = new List<ProductsType>();
}
