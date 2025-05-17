using Domain.Models;

namespace Business.Models;

public class UserResult<T> : ServiceResult
{
    public T? Result { get; set; }
}

public class UserResult : ServiceResult
{
}