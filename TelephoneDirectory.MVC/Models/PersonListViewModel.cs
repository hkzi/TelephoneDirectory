using System.Net.NetworkInformation;
using TelephoneDirectory.Core.Utilities.Results;
using TelephoneDirectory.Entities.Concrete;
using X.PagedList;


namespace TelephoneDirectory.MVC.Models
{
    public class PersonListViewModel
    {
        public List<Person> Persons { get; set; }
        public int Id { get; set; }      // Kişinin id
        public string? Name { get; set; }      // Kişinin adı
        public string? SurName { get; set; }   // Kişinin soyadı
        public string? PhoneNumber { get; set; } // Kişinin telefon numarası
        
    }
}