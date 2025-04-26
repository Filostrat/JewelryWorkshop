using DAL.EF;
using DAL.Models;
using DAL.Repositories.Interfaces;
using JewelryWorkshop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Runtime.CompilerServices;


namespace JewelryWorkshop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productsTypeRepository;
        private readonly UserManager<JewelryWorkshopUser> _userManager;

        public ProductController(UserManager<JewelryWorkshopUser> userManager,
                                IProductTypeRepository productsTypeRepository, 
                                IShoppingListRepository shoppingListRepository, 
                                IProductRepository productRepository)
        {
            _shoppingListRepository = shoppingListRepository;
            _productRepository = productRepository;
            _productsTypeRepository = productsTypeRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync(int id)
        {
            var productType = _productsTypeRepository.GetAllAsQueryable()
                .Include(pt => pt.JewelryType)
                    .ThenInclude(jt => jt.Sizes)
                .Include(pt => pt.Photos)
                .Include(pt => pt.Materials)
                .Include(pt => pt.PreciousStones)
                .Include(pt => pt.StoneWeights)
                    .Where(pt => pt.ProductTypeId == id).FirstOrDefault();

            return View(productType);
        }

        [HttpPost]
        public async Task<IActionResult> ProductFormAsync(int productTypeId, int size, int weight, int material, int preciousStones, int quantity, int jewelryTypeId, string totalPrice)
        {
            var product = new Product
            {
                PreciousStoneId = (preciousStones == 0) ? null : preciousStones,
                StoneWeightId = (weight == 0) ? null : weight,
                MaterialId = material,
                ProductTypeId = productTypeId,
                SizeId = size,
                JewelryTypeId = jewelryTypeId,
                Price = Convert.ToDecimal(totalPrice, CultureInfo.InvariantCulture),
            };


            await _productRepository.CreateAsync(product);

            var signedInUser = await _userManager.GetUserAsync(User);


            if (signedInUser != null)
            {
                ShoppingList shoppingList = new()
                {
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    UserId = signedInUser.Id
                };

                await _shoppingListRepository.CreateAsync(shoppingList);

                return RedirectToAction("Index", "ShoppingList");
            }
            else
            {
                return RedirectToPage("/Account/Register", new { area = "Identity" });
            }                
        }
    }
}
 
 
 