using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public decimal Density { get; set; }

    public decimal PriceForGram { get; set; }

    public string MaterialName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<ProductsType> ProductTypes { get; set; } = new List<ProductsType>();
}
