using DAL.EF;
using DAL.Models;
using DAL.Repositories.Interfaces;
using JewelryWorkshop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JewelryWorkshop.Controllers
{
    public class ShoppingListController : Controller
    {
        private readonly UserManager<JewelryWorkshopUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrdersProductRepository _ordersProductRepository;

        public ShoppingListController(UserManager<JewelryWorkshopUser> userManager, 
                                IUserRepository userRepository,
                                IShoppingListRepository shoppingListRepository,
                                IOrderRepository orderRepository,
                                IOrdersProductRepository ordersProductRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _shoppingListRepository = shoppingListRepository;
            _orderRepository = orderRepository;
            _ordersProductRepository = ordersProductRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var signedInUser = await _userManager.GetUserAsync(User);

            var userRepository = _userRepository.GetAllAsQueryable();

            var user = userRepository
                .Include(u => u.ShoppingLists)
                    .ThenInclude(sh => sh.Product)
                        .ThenInclude(p => p.Size)
                .Include(p => p.ShoppingLists)
                    .ThenInclude(sh => sh.Product)
                            .ThenInclude(p => p.Material)
                .Include(p => p.ShoppingLists)
                    .ThenInclude(sh => sh.Product)
                            .ThenInclude(p => p.StoneWeight)
                .Include(p => p.ShoppingLists)
                    .ThenInclude(sh => sh.Product)
                            .ThenInclude(p => p.PreciousStone)
                .Include(p => p.ShoppingLists)
                    .ThenInclude(sh => sh.Product)
                            .ThenInclude(p => p.ProductType)
                                .ThenInclude(p=> p.Photos)
            .Where(u => u.Id == signedInUser.Id)
            .FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int shoppingListId)
        {

            var shoppingList = await _shoppingListRepository.GetAsync(shoppingListId);
            if (shoppingList == null)
            {
                return NotFound();
            }
            else
            {
                await _shoppingListRepository.DeleteAsync(shoppingListId);
                return Redirect(nameof(Index));
            }
        }

        public async Task<IActionResult> BuyAllAsync()
        {
            
            var signedInUser = await _userManager.GetUserAsync(User);

            var userRepository = _userRepository.GetAllAsQueryable();

            var user = userRepository
                .Include(u => u.ShoppingLists)
                    .ThenInclude (sh => sh.Product)
            .Where(u => u.Id == signedInUser.Id)
            .FirstOrDefault();

            if (user.ShoppingLists.Count != 0)
            {
                var order = new Order()
                {
                    OrderAmount = user.ShoppingLists.Sum(p => p.Product.Price * p.Quantity),
                    UserId = user.Id,
                    OrderDate = DateTime.Now,
                };

                await _orderRepository.CreateAsync(order);


                foreach (var item in user.ShoppingLists)
                {
                    var orderProduct = ShoppingListToOrderProduct(item, order.OrderId);
                    await _ordersProductRepository.CreateAsync(orderProduct);
                }

                await _shoppingListRepository.DeleteAllForUserAsync(user.Id);

                return RedirectToAction("Index", "Order");
            }
            else
            {
                return RedirectToAction("Index", "Product");
            }                   
        }        

        private static OrdersProduct ShoppingListToOrderProduct(ShoppingList shoppingList, int orderId)
        {
            OrdersProduct ordersProduct = new OrdersProduct()
            {
                ProductId = shoppingList.ProductId,
                Quantity = shoppingList.Quantity,   
                OrderId = orderId
            };
            return ordersProduct;
        }
    }
}
