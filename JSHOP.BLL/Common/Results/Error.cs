
namespace JSHOP.BLL.Common.Results
{
    public sealed record Error
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException(
                    "Error code cannot be empty.",
                    nameof(code)
                );

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(
                    "Error message cannot be empty.",
                    nameof(message)
                );

            Code = code;
            Message = message;
        }
    }
}