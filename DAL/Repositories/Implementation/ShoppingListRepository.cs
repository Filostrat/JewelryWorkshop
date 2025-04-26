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
    public class ShoppingListRepository : BaseRepository<ShoppingList, int>, IShoppingListRepository
    {
        public ShoppingListRepository(JewelryWorkshopContext context) : base(context)
        {
        }

        public async Task DeleteAllForUserAsync(string userId)
        {
            var shoppingListsForUser = await FindAsync(sl => sl.UserId == userId);

            foreach (var shoppingList in shoppingListsForUser)
            {
                await DeleteAsync(shoppingList.ShoppingListId);
            }
        }
    }
   
}
