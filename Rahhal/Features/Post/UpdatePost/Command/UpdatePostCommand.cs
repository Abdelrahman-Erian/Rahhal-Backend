using MediatR;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Enum;
using Rahhal.Data.Repository;
using Rahhal.Features.Post.UpdatePost.DTOs;

namespace Rahhal.Features.Post.UpdatePost.Command
{
    public record UpdatePostCommand(UpdatePostRequestDTO DTO) : IRequest<RequestResult<UpdatePostResponseDTO>>;

    public class UpdatePostCommandHandler(
        BaseRequestHandlerParameter parameters,
        IRepository<Models.PostManagement.Post> repository)
        : BaseRequestHandler<UpdatePostCommand, RequestResult<UpdatePostResponseDTO>>(parameters)
    {
        public override async Task<RequestResult<UpdatePostResponseDTO>> Handle(UpdatePostCommand request,
            CancellationToken cancellationToken)
        {
            var post = await repository.GetByIDAsync(request.DTO.ID);
            if (post is null)
                return RequestResult<UpdatePostResponseDTO>.Failure(ErrorCode.NotFound, "Post not found");

            post.Description = request.DTO.Description;

            await repository.Update(post,
                nameof(post.Description));

            await repository.SaveChangesAsync();

            return RequestResult<UpdatePostResponseDTO>.Success(new UpdatePostResponseDTO());
        }
    }
}