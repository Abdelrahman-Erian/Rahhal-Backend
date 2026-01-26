using MediatR;
using Mapster;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Views;
using Rahhal.Features.Post.CreatePost.ViewModels;

namespace Rahhal.Features.Post.CreatePost.Command
{
    public record CreatePostOrchestrator(CreatePostRequestViewModel ViewModel)
        : IRequest<RequestResult<CreatePostResponseViewModel>>;

    public class CreatePostOrchestratorHandler(BaseRequestHandlerParameter parameters)
        : BaseRequestHandler<CreatePostOrchestrator, RequestResult<CreatePostResponseViewModel>>(parameters)
    {
        public override async Task<RequestResult<CreatePostResponseViewModel>> Handle(CreatePostOrchestrator request,
            CancellationToken cancellationToken)
        {
            var dto = new DTOs.CreatePostRequestDTO
            {
                UserId = request.ViewModel.UserId,
                Description = request.ViewModel.Description,
                Files = request.ViewModel.Files // Direct assignment, no deep mapping
            };

            var result = await _mediator.Send(new CreatePostCommand(dto), cancellationToken);

            if (!result.IsSuccess)
                return RequestResult<CreatePostResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return RequestResult<CreatePostResponseViewModel>.Success(
                result.Data.Adapt<CreatePostResponseViewModel>()
            );
        }
    }
}