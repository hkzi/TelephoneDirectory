using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.Business.Abstract;
using TelephoneDirectory.Core.Entities;
using TelephoneDirectory.Entities.Concrete;
using TelephoneDirectory.MVC.Models;

namespace TelephoneDirectory.MVC.Controllers
{
    public class PersonsController : Controller
    {
        
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        public ActionResult Index()
        {
            var model = new PersonListViewModel()
            {
                Persons = _personService.GetAll()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult GetByName(string name)
        {

            var search = new PersonListViewModel()
            {
                Persons = _personService.GetByName(name)
            };

            return View(search);
            

        }

        [HttpGet] 
        public IActionResult FindByNumber(string number)
        {

            var result = _personService.FindByNumber(number);
            return View(result);
        }
        [HttpPost]
        public IActionResult Add(Person person)
        {
            return View();
        }
        [HttpGet]
        public IActionResult Update()
        {
          
            return View();
        }
        [HttpDelete]
        public IActionResult Delete()
        {
            
         return  View();
        }


    }
}
