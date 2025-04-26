using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Size
{
    public int SizeId { get; set; }

    public string SizeName { get; set; } = null!;

    public decimal SizeVolume { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<JewelryType> JewelryTypes { get; set; } = new List<JewelryType>();
}
