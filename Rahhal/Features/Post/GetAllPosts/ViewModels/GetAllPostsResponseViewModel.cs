namespace Rahhal.Features.Post.GetAllPosts.ViewModels
{
    public class GetAllPostsResponseViewModel
    {
        public Guid ID { get; set; }

        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? ProfileUrl { get; set; }

        public string? Description { get; set; }
        public List<string?>? Media_URLs { get; set; }
    }
}