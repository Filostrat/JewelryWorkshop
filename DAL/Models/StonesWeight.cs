using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class StonesWeight
{
    public int StoneWeightId { get; set; }

    public decimal StonesWeights { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<ProductsType> ProductTypes { get; set; } = new List<ProductsType>();
}
