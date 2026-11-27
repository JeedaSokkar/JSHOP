
namespace JSHOP.BLL.Common.Results
{
    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(bool isSuccess, T? data, Error? error)
            : base(isSuccess, error)
        {
            Data = data;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, null);
        }

        public new static Result<T> Failure(Error error)
        {
            return new Result<T>(false, default, error);
        }
    }
}
