using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Core.Utilities.Results;
using TelephoneDirectory.Entities.Concrete;
using TelephoneDirectory.Entities.DTOs;

namespace TelephoneDirectory.Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);
        List<User> GetAll();
        User Add(User user);
        User Update(User user);
        User Delete(User user);
        User GetByMail(string email);
        List<User> GetById(int id);
        Task<bool> ChangePasswordAsync(string? identityName, string modelOldPassword, string modelNewPassword);
        Task<UserForLoginDto> GetUserByEmailAsync(string email);
        Task UpdateUserPasswordAsync(UserForLoginDto user);
        User GetUserWithClaims(string email);


    }
}
