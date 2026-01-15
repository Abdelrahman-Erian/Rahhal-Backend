
using Rahhal.Common.Enum;

namespace Rahhal.Common.Views
{
    public record EndpointResponse<T>(T Data, bool IsSuccess, string Message, ErrorCode ErrorCode)
    {
        public static EndpointResponse<T> Success(T Data, string Message = "")
        {
            return new EndpointResponse<T>(Data, true, Message, ErrorCode.None);
        }

        public static EndpointResponse<T> Failure(ErrorCode ErrorCode, string Message = "")
        {
            return new EndpointResponse<T>(default, false, Message, ErrorCode);
        }
    }
}
