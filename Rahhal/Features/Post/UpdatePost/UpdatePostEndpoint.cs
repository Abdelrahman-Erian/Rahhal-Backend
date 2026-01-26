using Microsoft.AspNetCore.Mvc;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using Rahhal.Features.Post.UpdatePost.Command;
using Rahhal.Features.Post.UpdatePost.ViewModels;

namespace Rahhal.Features.Post.UpdatePost
{
    public class UpdatePostEndpoint : BaseEndpoint<UpdatePostRequestViewModel, UpdatePostResponseViewModel>
    {
        public UpdatePostEndpoint(BaseEndpointParameters<UpdatePostRequestViewModel> parameters) : base(parameters)
        {
        }

        [HttpPatch("Post/Update")]
        public async Task<EndpointResponse<UpdatePostResponseViewModel>> HandleAsync(
            [FromBody] UpdatePostRequestViewModel model)
        {
            var validation = ValidateRequest(model);
            if (!validation.IsSuccess) return validation;

            var result = await _mediator.Send(new UpdatePostOrchestrator(model));

            if (!result.IsSuccess)
                return EndpointResponse<UpdatePostResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return EndpointResponse<UpdatePostResponseViewModel>.Success(result.Data);
        }
    }
}