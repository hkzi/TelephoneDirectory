using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.DataAccess;
using TelephoneDirectory.Core.Entities.Concrete;

namespace TelephoneDirectory.DataAccess.Abstract
{
    public interface IOperationClaimDal: IEntityRepository<OperationClaim>
    {
        
    }
}
