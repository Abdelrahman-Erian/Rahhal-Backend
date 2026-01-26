using MediatR;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Enum;
using Rahhal.Data.Repository;
using Rahhal.Features.Post.DeletePost.DTOs;

namespace Rahhal.Features.Post.DeletePost.Command
{
    public record DeletePostCommand(DeletePostRequestDTO DTO) : IRequest<RequestResult<DeletePostResponseDTO>>
    {
        public Guid PostId { get; init; } = DTO.PostId;
    }

    public class DeletePostCommandHandler(
        BaseRequestHandlerParameter parameters,
        IRepository<Models.PostManagement.Post> repository)
        : BaseRequestHandler<DeletePostCommand, RequestResult<DeletePostResponseDTO>>(parameters)
    {
        public override async Task<RequestResult<DeletePostResponseDTO>> Handle(DeletePostCommand request,
            CancellationToken cancellationToken)
        {
            _ = repository.SoftDeleteAsync(request.PostId);

            return RequestResult<DeletePostResponseDTO>.Success(new DeletePostResponseDTO());
        }
    }
}