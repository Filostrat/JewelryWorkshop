using DAL.EF;
using DAL.Models;
using DAL.Repositories.Implementation;
using DAL.Repositories.Interfaces;
using JewelryWorkshop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JewelryWorkshop.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<JewelryWorkshopUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly OrderCountViewModelRepository _orderCountViewModelRepository;
        private readonly CustomerOrderTotalModelRepository _customerOrderTotalModelRepository;

        public OrderController(UserManager<JewelryWorkshopUser> userManager,
                                IUserRepository userRepository,
                                OrderCountViewModelRepository orderCountViewModelRepository,
                                CustomerOrderTotalModelRepository customerOrderTotalModelRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _orderCountViewModelRepository = orderCountViewModelRepository;
            _customerOrderTotalModelRepository = customerOrderTotalModelRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var signedInUser = await _userManager.GetUserAsync(User);

            var userRepository = _userRepository.GetAllAsQueryable();

            var user = userRepository
                .Include(u => u.Orders)
                    .ThenInclude(sh => sh.OrdersProducts)
                        .ThenInclude(op => op.Product)
                            .ThenInclude(p => p.ProductType)
                                .ThenInclude(pt => pt.Photos)
                .Include(u => u.Orders)
                    .ThenInclude(sh => sh.OrdersProducts)
                        .ThenInclude(op => op.Product)
                            .ThenInclude(p => p.Size)
                .Include(u => u.Orders)
                    .ThenInclude(sh => sh.OrdersProducts)
                        .ThenInclude(op => op.Product)
                            .ThenInclude(p => p.Material)
                .Include(u => u.Orders)
                    .ThenInclude(sh => sh.OrdersProducts)
                        .ThenInclude(op => op.Product)
                            .ThenInclude(p => p.StoneWeight)
                .Include(u => u.Orders)
                    .ThenInclude(sh => sh.OrdersProducts)
                        .ThenInclude(op => op.Product)
                            .ThenInclude(p => p.PreciousStone)
            .Where(u => u.Id == signedInUser.Id)
            .FirstOrDefault();



            ViewBag.OrderCount = _orderCountViewModelRepository.GetAll().Where(ovm => ovm.UserID == user.Id).FirstOrDefault().OrderCount;
            ViewBag.OrderSum = _customerOrderTotalModelRepository.GetAll().Where(ovm => ovm.CustomerID == user.Id).FirstOrDefault().TotalOrderAmount;

            return View(user);
        }
    }
}
