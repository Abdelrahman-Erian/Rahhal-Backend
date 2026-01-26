namespace Rahhal.Features.Posts.GetByIdPost.ViewModels
{
    public class GetByIdPostResponseViewModel
    {
        public Guid ID { get; set; }

        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? ProfileURL { get; set; }

        public string? Description { get; set; }
        public List<string?>? Media_URLs { get; set; }
    }
}