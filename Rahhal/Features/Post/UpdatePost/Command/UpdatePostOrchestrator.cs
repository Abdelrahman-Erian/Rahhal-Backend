using MediatR;
        using Mapster;
        using Rahhal.Common.BaseHandler;
        using Rahhal.Common.Views;
        using Rahhal.Features.Post.UpdatePost.ViewModels;
        using Rahhal.Features.Post.UpdatePost.DTOs;

namespace Rahhal.Features.Post.UpdatePost.Command {
    public record UpdatePostOrchestrator(UpdatePostRequestViewModel ViewModel) : IRequest<RequestResult<UpdatePostResponseViewModel>>;

    public class UpdatePostOrchestratorHandler : BaseRequestHandler<UpdatePostOrchestrator, RequestResult<UpdatePostResponseViewModel>> {
        public UpdatePostOrchestratorHandler(BaseRequestHandlerParameter parameters) : base(parameters) { }

        public override async Task<RequestResult<UpdatePostResponseViewModel>> Handle(UpdatePostOrchestrator request, CancellationToken cancellationToken) {
            var dto = request.ViewModel.Adapt<UpdatePostRequestDTO>();
            var result = await _mediator.Send(new UpdatePostCommand(dto), cancellationToken);

            if (!result.IsSuccess)
                return RequestResult<UpdatePostResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return RequestResult<UpdatePostResponseViewModel>.Success(result.Data.Adapt<UpdatePostResponseViewModel>());
        }
    }
}