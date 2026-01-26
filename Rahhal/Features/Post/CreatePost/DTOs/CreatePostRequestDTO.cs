namespace Rahhal.Features.Post.CreatePost.DTOs
{
    public class CreatePostRequestDTO
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}