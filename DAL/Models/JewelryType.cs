using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class JewelryType
{
    public int JewelryTypeId { get; set; }

    public string JewelryTypeName { get; set; } = null!;

    public string? JewelryTypeDescription { get; set; }

    public virtual ICollection<ProductsType> ProductsTypes { get; set; } = new List<ProductsType>();

    public virtual ICollection<Size> Sizes { get; set; } = new List<Size>();
}
