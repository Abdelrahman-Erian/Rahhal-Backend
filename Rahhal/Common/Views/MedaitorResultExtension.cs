

namespace  Rahhal.Common.Views
{
    public static class MediatorResultExtensions
    {
        public static EndpointResponse<TResponse> ToEndpointResponse<TResponse, TData>(
            this RequestResult<TData> result,
            Func<TData, TResponse> onSuccess)
        {
            if (!result.IsSuccess)
            {
                return EndpointResponse<TResponse>.Failure(result.ErrorCode, result.Message);
            }

            return EndpointResponse<TResponse>.Success(onSuccess(result.Data),result.Message);
        }
    }

}
