using Microsoft.AspNetCore.Mvc;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using Rahhal.Features.Post.GetAllPosts.ViewModels;

namespace Rahhal.Features.Post.GetAllPosts
{
    public class
        GetAllPostsEndpoint(BaseEndpointParameters<GetAllPostsRequestViewModel> parameters)
        : BaseEndpoint<GetAllPostsRequestViewModel, PagingViewModel<GetAllPostsResponseViewModel>>(parameters)
    {
        [HttpGet("Post/GetAll")]
        public async Task<EndpointResponse<PagingViewModel<GetAllPostsResponseViewModel>>> HandleAsync(
            [FromQuery] GetAllPostsRequestViewModel model)
        {
            var validation = ValidateRequest(model);
            if (!validation.IsSuccess) return validation;

            var result = await _mediator.Send(new Queries.GetAllPostsQuery(model));

            if (!result.IsSuccess)
                return EndpointResponse<PagingViewModel<GetAllPostsResponseViewModel>>.Failure(result.ErrorCode,
                    result.Message);

            return EndpointResponse<PagingViewModel<GetAllPostsResponseViewModel>>.Success(result.Data);
        }
    }
}