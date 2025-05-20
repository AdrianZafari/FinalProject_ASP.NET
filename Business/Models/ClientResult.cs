using Domain.Models;

namespace Business.Models;

public class ClientResult<T> : ServiceResult
{
    public T? Result { get; set; }
}

public class ClientResult : ServiceResult
{

}