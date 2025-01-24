using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.Core.DataAccess;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.DataAccess.Abstract;

namespace TelephoneDirectory.DataAccess.Concrete
{
    public class UserOperationClaimDal : EntityRepositoryBase<UserOperationClaim, PhoneContext>, IUserOperationClaimDal
    {
        public IEnumerable<UserOperationClaim> GetAllRole(int id)
        {
            using (var context = new PhoneContext())
            {
                try
                {
                    var userOperationClaims = context.UserOperationClaims
                        .Where(uoc => uoc.OperationClaimId == id)
                        .ToList();

                    // Log the number of items retrieved
                    Console.WriteLine($"Retrieved {userOperationClaims.Count} user operation claims with OperationClaimId {id}.");

                    return userOperationClaims;
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"Error occurred while fetching UserOperationClaims: {ex.Message}");
                    return new List<UserOperationClaim>(); // Return an empty list if there was an error
                }
            }
        }


    }
}
