using System.Collections;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Mvc;
using TelephoneDirectory.Business.Abstract;
using TelephoneDirectory.Business.BusinessAspects.Autofac;
using TelephoneDirectory.Entities.Concrete;
using TelephoneDirectory.MVC.Filters;
using TelephoneDirectory.MVC.Models;
using TelephoneDirectory.Core.Utilities.Results;
using X.PagedList.Extensions;
using TelephoneDirectory.Business.ValidationRules.FluentValidation;
using TelephoneDirectory.Core.Aspects.AutofacAspect;
using System;


namespace TelephoneDirectory.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = "Login")]
    [Authorize(Roles = "Moderator")]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;


        public PersonsController(IPersonService personService)
        {
            _personService = personService;

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string firstName, string lastName, string phoneNumber, int page = 1, int pageSize = 10)
        {
            IEnumerable<Person> persons = _personService.GetAll();


            if (!string.IsNullOrEmpty(firstName))
            {
                persons = persons.Where(p => p.Name.Contains(firstName, StringComparison.OrdinalIgnoreCase));
            }


            if (!string.IsNullOrEmpty(lastName))
            {
                persons = persons.Where(p => p.SurName.Contains(lastName, StringComparison.OrdinalIgnoreCase));
            }


            if (!string.IsNullOrEmpty(phoneNumber))
            {
                persons = persons.Where(p => p.PhoneNumber.Contains(phoneNumber));
            }


            var personList = persons.Select(p => new PersonListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                SurName = p.SurName,
                PhoneNumber = p.PhoneNumber
            });


            var pagedPersons = personList.ToPagedList(page, pageSize);

            return View(pagedPersons);
        }


        public IActionResult Add()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Person person)
        {
            try
            {
                _personService.Add(person);
                return RedirectToAction("Index");
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }

            return View(person);
        }


        public IActionResult Update(int id)
        {
            var result = _personService.FindById(id);
            if (result.Success && result.Data != null && result.Data.Count > 0)
            {
                var viewModel = new PersonListViewModel
                {
                    Persons = result.Data
                };
                return View(viewModel);
            }
            else
            {
                // Hata durumunu veya yönlendirmeyi kontrol edin
                return RedirectToAction("Index"); // Alternatif olarak bir hata sayfasına yönlendirin
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Update(PersonListViewModel updateViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    // updateViewModel.Persons listesini işleyin
                    foreach (var person in updateViewModel.Persons)
                    {
                        _personService.Update(person);
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }

            return View(updateViewModel);
        }

        public IActionResult Delete(Person person)
        {

            _personService.Delete(person);

            return RedirectToAction("Index");

        }
        public IActionResult Detail(int id)
        {
            var result = _personService.FindById(id);

            // Gelen sonucu doğrudan List<Person> yerine result.Data olarak atar
            if (result.Success)
            {
                var detail = new PersonListViewModel
                {
                    Persons = result.Data // result.Data ifadesi List<Person> döndürür
                };

                return View(detail);
            }
            else
            {
                // Eğer hata varsa hata mesajı gösterilebilir
                return View("Index");
            }
        }
        [AllowAnonymous]
        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404; // Set the status code to 404
            return View();
        }

    }
}
