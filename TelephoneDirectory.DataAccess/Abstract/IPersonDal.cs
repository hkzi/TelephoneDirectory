using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.DataAccess;
using TelephoneDirectory.Entities.Concrete;

namespace TelephoneDirectory.DataAccess.Abstract
{
    public interface IPersonDal:IEntityRepository<Person>
    {
       
    }
}
