using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ProductsType
{
    public int ProductTypeId { get; set; }

    public int JewelryTypeId { get; set; }

    public string ProductTypeName { get; set; } = null!;

    public string? ProductTypeDescription { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal AdditionalSizeCost { get; set; }

    public decimal AdditionalWorkCost { get; set; }

    public virtual JewelryType JewelryType { get; set; } = null!;

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    public virtual ICollection<PreciousStone> PreciousStones { get; set; } = new List<PreciousStone>();

    public virtual ICollection<StonesWeight> StoneWeights { get; set; } = new List<StonesWeight>();
}
