using FluentValidation;

namespace Rahhal.Features.Post.UpdatePost.ViewModels
{
    public class UpdatePostRequestViewModel
    {
        public Guid ID { get; set; }

        public string? Description { get; set; }
    }

    public class UpdatePostRequestViewModelValidator : AbstractValidator<UpdatePostRequestViewModel>
    {
        public UpdatePostRequestViewModelValidator()
        {
            RuleFor(x => x.ID).NotNull().WithMessage("PostId is required");
        }
    }
}