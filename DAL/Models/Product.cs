using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public decimal? Price { get; set; }

    public int ProductTypeId { get; set; }

    public int MaterialId { get; set; }

    public int? PreciousStoneId { get; set; }

    public int? StoneWeightId { get; set; }

    public int JewelryTypeId { get; set; }

    public int SizeId { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual PreciousStone? PreciousStone { get; set; }

    public virtual ProductsType ProductType { get; set; } = null!;

    public virtual ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();

    public virtual Size Size { get; set; } = null!;

    public virtual StonesWeight? StoneWeight { get; set; }

    public virtual ICollection<WishList> WishLists { get; set; } = new List<WishList>();
}
