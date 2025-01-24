using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TelephoneDirectory.Core.Entities.Concrete;

namespace TelephoneDirectory.MVC.Models
{
    public class UserRoleViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Kullanıcı  soyadı boş geçilemez.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "E-posta gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        public string Email { get; set; }
        public int RoleId { get; set; }  // Seçilen rol ID'si

        public int SelectedRoleId { get; set; }
        public List<SelectListItem> OperationClaim { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }


    }

}

