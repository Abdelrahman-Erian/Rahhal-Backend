using MediatR;
        using Mapster;
        using Rahhal.Common.BaseEndpoint;
        using Rahhal.Common.BaseHandler;
        using Rahhal.Common.Views;
        using Rahhal.Features.Post.DeletePost.ViewModels;
        using Rahhal.Features.Post.DeletePost.DTOs;

namespace Rahhal.Features.Post.DeletePost.Command {
    public record DeletePostOrchestrator(DeletePostRequestViewModel ViewModel) : IRequest<RequestResult<DeletePostResponseViewModel>>;

    public class DeletePostOrchestratorHandler : BaseRequestHandler<DeletePostOrchestrator, RequestResult<DeletePostResponseViewModel>> {
        public DeletePostOrchestratorHandler(BaseRequestHandlerParameter parameters) : base(parameters) { }

        public override async Task<RequestResult<DeletePostResponseViewModel>> Handle(DeletePostOrchestrator request, CancellationToken cancellationToken) {
            var dto = request.ViewModel.Adapt<DeletePostRequestDTO>();
            var result = await _mediator.Send(new DeletePostCommand(dto), cancellationToken);

            if (!result.IsSuccess)
                return RequestResult<DeletePostResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return RequestResult<DeletePostResponseViewModel>.Success(result.Data.Adapt<DeletePostResponseViewModel>());
        }
    }
}