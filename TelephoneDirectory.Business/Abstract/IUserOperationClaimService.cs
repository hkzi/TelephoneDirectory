using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Core.Utilities.Results;
using TelephoneDirectory.Entities.Concrete;

namespace TelephoneDirectory.Business.Abstract
{
    public interface IUserOperationClaimService
    {
        List<UserOperationClaim> GetAll();
        UserOperationClaim Add(UserOperationClaim userOperationClaim);
        UserOperationClaim Delete(UserOperationClaim userOperationClaim);
        UserOperationClaim Update(UserOperationClaim userOperationClaim);
    }
}
