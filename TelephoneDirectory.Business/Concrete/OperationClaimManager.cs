using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Business.Abstract;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.DataAccess.Abstract;

namespace TelephoneDirectory.Business.Concrete
{
    public class OperationClaimManager:IOperationClaimService
    {
        private IOperationClaimDal _operationClaimDal;

        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        public List<OperationClaim> GetAll()
        {
            return _operationClaimDal.GetAll().ToList();
        }

    }
}
