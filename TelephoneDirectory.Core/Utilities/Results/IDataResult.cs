using System.Collections;

namespace TelephoneDirectory.Core.Utilities.Results
{
    public interface IDataResult<T>:IResult
    {

        T Data { get; }
    }
}
