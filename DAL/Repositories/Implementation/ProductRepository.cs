using DAL.EF;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public class ProductRepository: BaseRepository<Product, int>, IProductRepository
    {
        public ProductRepository(JewelryWorkshopContext context) : base(context)
        {
        }
    }
}
