using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.Entities.Concrete;

namespace TelephoneDirectory.Business.Abstract
{
    public interface IOperationClaimService
    {
        List<OperationClaim> GetAll();
    }
}
