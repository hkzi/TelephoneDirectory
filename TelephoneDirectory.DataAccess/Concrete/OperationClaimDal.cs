using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.DataAccess;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.DataAccess.Abstract;

namespace TelephoneDirectory.DataAccess.Concrete
{
    public class OperationClaimDal:EntityRepositoryBase<OperationClaim,PhoneContext>,IOperationClaimDal
    {
      
    }
}
