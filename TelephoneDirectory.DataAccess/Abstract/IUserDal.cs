using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.DataAccess;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Entities.DTOs;

namespace TelephoneDirectory.DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
        Task UpdateUserPasswordAsync(UserForLoginDto user);
        Task<UserForLoginDto> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllAsync(Expression<Func<User, bool>> filter);
        Task UpdateAsync(User user);
        Task UpdateUserAsync(User user);
        User GetUserWithClaims(string email);



    }
}
