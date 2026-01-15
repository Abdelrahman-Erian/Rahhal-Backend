using Rahhal.Common.Enum;

namespace Rahhal.Common.Views
{
    public record RequestResult<T>(T Data, bool IsSuccess, string Message, ErrorCode ErrorCode)
    {

        public static RequestResult<T> Success(T Data, string Message = "")
        {
            return new RequestResult<T>(Data, true, Message, ErrorCode.None);
        }

        public static RequestResult<T> Failure(ErrorCode ErrorCode, string Message = "")
        {
            return new RequestResult<T>(default, false, Message, ErrorCode);
        }
    }
}
