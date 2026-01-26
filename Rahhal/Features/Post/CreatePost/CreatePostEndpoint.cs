using Microsoft.AspNetCore.Mvc;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using Rahhal.Features.Post.CreatePost.Command;
using Rahhal.Features.Post.CreatePost.ViewModels;

namespace Rahhal.Features.Post.CreatePost
{
    public class CreatePostEndpoint : BaseEndpoint<CreatePostRequestViewModel, CreatePostResponseViewModel>
    {
        public CreatePostEndpoint(BaseEndpointParameters<CreatePostRequestViewModel> parameters) : base(parameters)
        {
        }

        [HttpPost("Post/Create")]
        public async Task<EndpointResponse<CreatePostResponseViewModel>> HandleAsync(
            CreatePostRequestViewModel model)
        {
            var validation = ValidateRequest(model);
            if (!validation.IsSuccess) return validation;

            var result = await _mediator.Send(new CreatePostOrchestrator(model));

            if (!result.IsSuccess)
                return EndpointResponse<CreatePostResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return EndpointResponse<CreatePostResponseViewModel>.Success(result.Data);
        }
    }
}