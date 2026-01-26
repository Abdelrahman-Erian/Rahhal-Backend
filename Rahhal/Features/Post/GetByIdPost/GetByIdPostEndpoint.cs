using Microsoft.AspNetCore.Mvc;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using Rahhal.Features.Post.GetByIdPost.ViewModels;
using Rahhal.Features.Posts.GetByIdPost.ViewModels;

namespace Rahhal.Features.Post.GetByIdPost
{
    public class GetByIdPostEndpoint(BaseEndpointParameters<GetByIdPostRequestViewModel> parameters)
        : BaseEndpoint<GetByIdPostRequestViewModel, GetByIdPostResponseViewModel>(parameters)
    {
        [HttpGet("Post/GetById")]
        public async Task<EndpointResponse<GetByIdPostResponseViewModel>> HandleAsync(
            [FromQuery] GetByIdPostRequestViewModel model)
        {
            var validation = ValidateRequest(model);
            if (!validation.IsSuccess) return validation;

            var result = await _mediator.Send(new Queries.GetByIdPostQuery(model));

            if (!result.IsSuccess)
                return EndpointResponse<GetByIdPostResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return EndpointResponse<GetByIdPostResponseViewModel>.Success(result.Data);
        }
    }
}