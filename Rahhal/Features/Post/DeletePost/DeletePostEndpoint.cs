using Microsoft.AspNetCore.Mvc;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using Rahhal.Features.Post.DeletePost.Command;
using Rahhal.Features.Post.DeletePost.ViewModels;

namespace Rahhal.Features.Post.DeletePost
{
    public class DeletePostEndpoint : BaseEndpoint<DeletePostRequestViewModel, DeletePostResponseViewModel>
    {
        public DeletePostEndpoint(BaseEndpointParameters<DeletePostRequestViewModel> parameters) : base(parameters)
        {
        }

        [HttpDelete("Post/Delete")]
        public async Task<EndpointResponse<DeletePostResponseViewModel>> HandleAsync(
            [FromBody] DeletePostRequestViewModel model)
        {
            var validation = ValidateRequest(model);
            if (!validation.IsSuccess) return validation;

            var result = await _mediator.Send(new DeletePostOrchestrator(model));

            if (!result.IsSuccess)
                return EndpointResponse<DeletePostResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return EndpointResponse<DeletePostResponseViewModel>.Success(result.Data);
        }
    }
}