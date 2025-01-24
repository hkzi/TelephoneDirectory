using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Business.Abstract;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Core.Utilities.Results;
using TelephoneDirectory.DataAccess.Abstract;
using TelephoneDirectory.DataAccess.Concrete;
using TelephoneDirectory.Entities.Concrete;

namespace TelephoneDirectory.Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private IUserOperationClaimDal _userOperationClaim;


        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaim)
        {
            _userOperationClaim = userOperationClaim;

        }

        public List<UserOperationClaim> GetAll()
        {
            return _userOperationClaim.GetAll();
        }

        public UserOperationClaim Add(UserOperationClaim userOperationClaim)
        {
            return _userOperationClaim.Add(userOperationClaim);
        }

        public UserOperationClaim Delete(UserOperationClaim userOperationClaim)
        {
            return _userOperationClaim.Delete(userOperationClaim);
        }

        public UserOperationClaim Update(UserOperationClaim userOperationClaim)
        {
            return _userOperationClaim.Update(userOperationClaim);
        }

    }

}
