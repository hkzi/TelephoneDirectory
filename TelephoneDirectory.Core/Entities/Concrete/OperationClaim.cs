namespace TelephoneDirectory.Core.Entities.Concrete;

public class OperationClaim : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual UserOperationClaim UserOperationClaims { get; set; }

}