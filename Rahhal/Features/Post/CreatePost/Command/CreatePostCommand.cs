using MediatR;
using Rahhal.Common.Views;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Enum;
using Rahhal.Common.Helper;
using Rahhal.Data.Repository;
using Rahhal.Features.Post.CreatePost.DTOs;
using Rahhal.Models.PostManagement;

namespace Rahhal.Features.Post.CreatePost.Command
{
    public record CreatePostCommand(CreatePostRequestDTO Dto) : IRequest<RequestResult<CreatePostResponseDTO>>
    {
        public Guid UserId { get; set; } = Dto.UserId;
        public string? Description { get; set; } = Dto.Description;
        public List<IFormFile>? Files { get; set; } = Dto.Files;
    }

    public class CreatePostCommandHandler(
        BaseRequestHandlerParameter parameters,
        IRepository<Models.PostManagement.Post> repository)
        : BaseRequestHandler<CreatePostCommand, RequestResult<CreatePostResponseDTO>>(parameters)
    {
        public override async Task<RequestResult<CreatePostResponseDTO>> Handle(CreatePostCommand request,
            CancellationToken cancellationToken)
        {
            List<string> urls = new List<string>();

            if (request.Files != null)
            {
                foreach (var file in request.Files)
                {
                    urls.Add(await UploadFileHelper.UploadFileAsync(file));
                }
            }

            var post = new Models.PostManagement.Post()
            {
                Description = request.Description,
                UserId = request.UserId,
                Media = urls.Select(u => new PostMedia
                {
                    Media_URL = u
                }).ToList()
            };

            try
            {
                await repository.AddAsync(post);
                await repository.SaveChangesAsync();

                return RequestResult<CreatePostResponseDTO>.Success(new CreatePostResponseDTO());
            }
            catch (Exception ex)
            {
                return RequestResult<CreatePostResponseDTO>.Failure(
                    ErrorCode.ServerError,
                    ex.Message
                );
            }
        }
    }
}