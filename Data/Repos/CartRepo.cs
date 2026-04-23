using InstallFlow.Data.Entities;
using InstallFlow.Data.Enums;
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

            return cartItem;
        }



        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);

            return cart;


        }

        public async Task<List<Cart>> GetAllCartsAsync()
        {
            return await _context.Carts
                .AsNoTracking()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ToListAsync();
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
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == CartStatus.Active);
        }


        public async Task<CartItem?> GetCartItemByIdAsync(int id)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }


        public async Task<List<Cart>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Carts
                .AsNoTracking()
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }





        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
        }

        public async Task RemoveCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);

        }
        public async Task DeleteCartAsync(Cart cart)
        {
            _context.Carts.Remove(cart);

        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }








    }
}
