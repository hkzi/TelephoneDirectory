using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.Utilities.Results;
using TelephoneDirectory.Entities.Concrete;

namespace TelephoneDirectory.Business.Abstract
{
    public interface IPersonService
    {
        List<Person> GetAll();
        IDataResult<List<Person>> FindById(int number);
        IResult Add(Person person);
        IResult Delete(Person person);
        Person Update(Person person);
    }
}
