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
    public class PreciousStoneRepository : BaseRepository<PreciousStone, int>, IPreciousStoneRepository
    {
        public PreciousStoneRepository(JewelryWorkshopContext context) : base(context)
        {
        }
    }
}
