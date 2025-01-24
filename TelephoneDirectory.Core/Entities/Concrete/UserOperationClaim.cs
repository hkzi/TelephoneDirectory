using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelephoneDirectory.Core.Entities.Concrete;

public class UserOperationClaim : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }

    
    public virtual OperationClaim OperationClaim { get; set; }
   
}