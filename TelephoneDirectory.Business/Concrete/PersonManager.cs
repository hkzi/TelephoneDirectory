using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Business.Abstract;
using TelephoneDirectory.Business.BusinessAspects.Autofac;
using TelephoneDirectory.Business.Constants;
using TelephoneDirectory.Business.ValidationRules.FluentValidation;
using TelephoneDirectory.Core.Aspects.AutofacAspect;
using TelephoneDirectory.Core.Utilities.Results;
using TelephoneDirectory.DataAccess.Abstract;
using TelephoneDirectory.Entities.Concrete;


namespace TelephoneDirectory.Business.Concrete
{
    public class PersonManager : IPersonService
    {
        private IPersonDal _personDal;

        public PersonManager(IPersonDal personDal)
        {
            _personDal = personDal;
        }

        public List<Person> GetAll()
        {
            return _personDal.GetAll();
        }
        public IDataResult<List<Person>> FindById(int number)
        {
            return new SuccessDataResult<List<Person>>(_personDal.GetAll(p => p.Id == number));
        }
        [ValidationAspect(typeof(PersonValidator))]
        public IResult Add(Person person)
        {
            _personDal.Add(person);
            return new SuccessResult(Messages.PersonAdded);

        }
        [ValidationAspect(typeof(PersonValidator))]
        public Person Update(Person person)
        {
            return _personDal.Update(person);

        }
        //[SecuredOperation("Admin")]
        public IResult Delete(Person person)
        {

            _personDal.Delete(person);
            return new SuccessResult(Messages.PersonDeleted);

        }


    }
}
