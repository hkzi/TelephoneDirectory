using TelephoneDirectory.Core.Entities.Concrete;

namespace TelephoneDirectory.Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User  user,List<OperationClaim> operationClaims);
    }
}
