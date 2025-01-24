using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TelephoneDirectory.Core.DataAccess;
using TelephoneDirectory.DataAccess.Abstract;
using TelephoneDirectory.Entities.Concrete;

namespace TelephoneDirectory.DataAccess.Concrete
{
    public class PersonDal:EntityRepositoryBase<Person,PhoneContext>,IPersonDal
    {
      
    }
}
