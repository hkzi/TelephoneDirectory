using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Entities.DTOs;

namespace TelephoneDirectory.MVC.Models
{
    public class ListUserViewRegisterModel
    {

        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string RolString { get; set; }
        public List<UserRoleViewModel> SelectedRoles { get; set; } // Rollerin detaylı listesi



    }
}
