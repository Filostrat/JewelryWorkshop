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
    public class CustomerOrderTotalModelRepository
    {
        private readonly DbSet<CustomerOrderTotalModel> _set;
        private readonly JewelryWorkshopContext _context;

        public CustomerOrderTotalModelRepository(JewelryWorkshopContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<CustomerOrderTotalModel>();
        }

        public IEnumerable<CustomerOrderTotalModel> GetAll()
        {
            return _set.ToList();
        }
    }
}
