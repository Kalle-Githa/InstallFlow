using InstallFlow.Data.Entities;
using InstallFlow.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InstallFlow.Data.Repos
{




    public class CartRepo : ICartRepo
    {

        private readonly InstallFlowDbContext _context;
        public CartRepo(InstallFlowDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }



        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();


            return cart;


        }



        public async Task<Cart?> GetCartByIdAsync(int id)
        {
            return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public async Task<CartItem?> GetCartItemByIdAsync(int id)
        {

            return await _context.CartItems.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task RemoveCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }








    }
}
