using DAL.EF;
using DAL.Models;
using DAL.Repositories.Interfaces;
using JewelryWorkshop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;

namespace JewelryWorkshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductTypeRepository _productsTypeRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IStonesWeightRepository _stonesWeightRepository;
        private readonly IPreciousStoneRepository _preciousStoneRepository;

        public HomeController(ILogger<HomeController> logger,
                              IProductTypeRepository productsTypeRepository,
                              IMaterialRepository materialRepository,
                              IStonesWeightRepository stonesWeightRepository,
                              IPreciousStoneRepository preciousStoneRepository)
        {
            _logger = logger;
            _productsTypeRepository = productsTypeRepository;
            _materialRepository = materialRepository;
            _stonesWeightRepository = stonesWeightRepository;
            _preciousStoneRepository = preciousStoneRepository;
        }

        public async Task<IActionResult> Index(string material = "",
                                               string stonesWeight = "",
                                               string preciousStone = "",
                                               double minPrice = 0,
                                               double maxPrice = 10000)
        {
            ViewBag.SelectStonesWeight = "Select Stone Weight";
            ViewBag.SelectPreciousStone = "Select Precious Stone";
            ViewBag.SelectMaterial = "Select Material";
            ViewBag.SelectMinPrice = 0;
            ViewBag.SelectMaxPrice = 100000;

            IQueryable <ProductsType> filteredProductTypes = _productsTypeRepository.GetAllAsQueryable()
                .Include(pt => pt.JewelryType)
                    .ThenInclude(jt => jt.Sizes)
                .Include(pt => pt.Photos)
                .Include(pt => pt.Materials)
                .Include(pt => pt.PreciousStones)
                .Include(pt => pt.StoneWeights);

            if (!string.IsNullOrEmpty(material) && material != ViewBag.SelectMaterial)
            {
                filteredProductTypes = filteredProductTypes
                    .Where(pt => pt.Materials.Any(m => m.MaterialName.Contains(material)));
                ViewBag.SelectMaterial = material;
            }

            if (!string.IsNullOrEmpty(preciousStone) && preciousStone != ViewBag.SelectPreciousStone)
            {
                filteredProductTypes = filteredProductTypes
                    .Where(pt => pt.PreciousStones.Any(s => s.PreciousStoneName.Contains(preciousStone)));
                ViewBag.SelectPreciousStone = preciousStone;
            }

            if (decimal.TryParse(stonesWeight, out decimal stonesWeightDecimal) && stonesWeight != ViewBag.SelectStonesWeight)
            {
                filteredProductTypes = filteredProductTypes
                    .Where(pt => pt.StoneWeights.Any(s => s.StonesWeights == stonesWeightDecimal));
                ViewBag.SelectStonesWeight = stonesWeight;

            }         

            if (minPrice != 0 || maxPrice != 0)
            {
                var minPriceDecimal = Convert.ToDecimal(minPrice, CultureInfo.InvariantCulture);
                var maxPriceDecimal = Convert.ToDecimal(maxPrice, CultureInfo.InvariantCulture);
                filteredProductTypes = filteredProductTypes.Where(pt => pt.MinPrice > minPriceDecimal && pt.MinPrice < maxPriceDecimal);
                ViewBag.SelectMinPrice = minPriceDecimal;
                ViewBag.SelectMaxPrice = maxPriceDecimal;
            }

            ViewBag.StonesWeight = _stonesWeightRepository.GetAllAsQueryable().OrderBy(s => s.StonesWeights).ToList();
            ViewBag.PreciousStones = _preciousStoneRepository.GetAllAsQueryable().OrderBy(ps => ps.PriceForCarat).ToList();
            ViewBag.Materials = _materialRepository.GetAllAsQueryable().OrderBy(m => m.PriceForGram).ToList();

            return View(filteredProductTypes.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}