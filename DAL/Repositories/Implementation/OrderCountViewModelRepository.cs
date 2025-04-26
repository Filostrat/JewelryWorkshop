using DAL.EF;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public class OrderCountViewModelRepository
    {
        private readonly DbSet<OrderCountViewModel> _set;
        private readonly JewelryWorkshopContext _context;

        public OrderCountViewModelRepository(JewelryWorkshopContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<OrderCountViewModel>();
        }

        public IEnumerable<OrderCountViewModel> GetAll()
        {
            return _set.ToList();
        }
    }
}
