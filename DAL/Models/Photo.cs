using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Photo
{
    public int PhotoId { get; set; }

    public byte[]? PhotoData { get; set; }

    public int ProductTypeId { get; set; }

    public virtual ProductsType ProductType { get; set; } = null!;
}
