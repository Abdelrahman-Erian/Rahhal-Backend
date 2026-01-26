using FluentValidation;

namespace Rahhal.Features.Post.GetByIdPost.ViewModels
{
    public class GetByIdPostRequestViewModel
    {
        public Guid PostId { get; set; }
    }

    public class GetByIdPostRequestViewModelValidator : AbstractValidator<GetByIdPostRequestViewModel>
    {
        public GetByIdPostRequestViewModelValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post id is required");
        }
    }
}