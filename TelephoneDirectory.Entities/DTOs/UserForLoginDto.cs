using TelephoneDirectory.Core.Entities;
using TelephoneDirectory.Core.Entities.Concrete;

namespace TelephoneDirectory.Entities.DTOs;

public class UserForLoginDto : IDto
{
    public UserForLoginDto()
    {
    }

    public string Email { get; set; }
    public string Password { get; set; }
}