using Rahhal.Common.Enum;

namespace Rahhal.Common.Expections
{

    public class BusinessLogicException : Exception
    {
        public ErrorCode ErrorCode { get; set; }

        public BusinessLogicException(string message, ErrorCode errorCode = ErrorCode.Unknown)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
