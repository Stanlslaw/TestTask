using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services;

public class UserService:IUserService
{
    #region Fields
    private readonly ApplicationDbContext _appDbContext;
    #endregion
    
    #region Constructors
    public UserService(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    #endregion

    #region Methods
    ///<returns>
    /// Returns the user with the highest orders count (or null)
    /// </returns>
    public async Task<User?> GetUser()
    {
        /*
         * Хочется сделать замечание к данной реализации,
         * в нашем случае требуется вернуть одного пользователя,где данная реализация верна,
         * однако может возникнуть ситуация, когда будет несколько пользователей с одним количеством заказов,
         * и в данной реализации мы получим только одного пользователя, что может привести к получению неполного результата
         * для определенной цели.
         */

        return await _appDbContext.Users
            .Where(users=>users.Status!=UserStatus.Inactive)
            //.Include(users=>users.Orders)
            /*
             * Спорная ситуация возникла, по заданию просят вернуть пользователя,
             * этим я и оперировал, однако было бы полезно вернуть также и список его заказов,
             * но для этого придется создавать костыли, чтобы избежать циклической зависимости (P.S. познаю новое) :),
             * поэтому для получения списка заказов лучше создать отдельный метод 
             */
            .OrderByDescending(users => users.Orders.Count)
            .FirstOrDefaultAsync();

       
    }
    ///<returns>
    /// Returns the list of inactive users (or empty list)
    /// </returns>
    public async Task<List<User>> GetUsers()
    {
        return await _appDbContext.Users
            .Where(users => users.Status == UserStatus.Inactive).ToListAsync();
    }
    #endregion
  
}