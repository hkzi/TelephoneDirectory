using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Entities.Concrete;
using TelephoneDirectory.Entities.DTOs;

namespace TelephoneDirectory.MVC.Models
{
    public class UserListViewLoginModel
    {

        public UserForLoginDto UserForLoginDtos { get; set; }
     
    }
}
