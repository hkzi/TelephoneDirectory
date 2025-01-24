using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.Entities;
using TelephoneDirectory.Core.Entities.Concrete;

namespace TelephoneDirectory.Entities.DTOs
{
    public class UserForRegisterDto : IDto
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "E-posta gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Kullanıcı  soyadı boş geçilemez.")]
        public string? LastName { get; set; }

    }


}
