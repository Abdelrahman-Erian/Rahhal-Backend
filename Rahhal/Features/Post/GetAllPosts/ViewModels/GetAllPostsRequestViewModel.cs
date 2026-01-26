using FluentValidation;

namespace Rahhal.Features.Post.GetAllPosts.ViewModels
{
    public class GetAllPostsRequestViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public bool SortByLastAdded { get; set; } = false;
    }

    public class GetAllPostsRequestViewModelValidator : AbstractValidator<GetAllPostsRequestViewModel>
    {
        public GetAllPostsRequestViewModelValidator()
        {
        }
    }
}