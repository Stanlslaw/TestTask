using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services;

public class OrderService:IOrderService
{
    #region Fields
    private readonly ApplicationDbContext _appDbContext;
    #endregion
    
    #region Constructors
    public OrderService(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    #endregion

    #region Methods

    ///<returns>
    /// Returns the order with the highest total price (Price*Quantity) (or null)
    /// </returns>
    public async Task<Order?> GetOrder()
    {
        return await _appDbContext.Orders
            .OrderByDescending(orders => orders.Price * orders.Quantity)
            .FirstOrDefaultAsync();
    }
    ///<returns>
    /// Returns the list of orders with where quantity greater then 10 (or empty list)
    /// </returns>
    public async Task<List<Order>> GetOrders()
    {
        return await _appDbContext.Orders
            .Where(orders => orders.Quantity > 10)
            .ToListAsync();
    }

    #endregion
}