using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Core.Utilities.Results;
using TelephoneDirectory.Core.Utilities.Security.JWT;
using TelephoneDirectory.Entities.Concrete;
using TelephoneDirectory.Entities.DTOs;

namespace TelephoneDirectory.Business.Abstract;

public interface IAuthService
{
    IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);
    IDataResult<User> Login(string email, string password);
    IResult UserExists(string email);
    IDataResult<AccessToken> CreateAccessToken(User user);
    List<User> GetAll();

}