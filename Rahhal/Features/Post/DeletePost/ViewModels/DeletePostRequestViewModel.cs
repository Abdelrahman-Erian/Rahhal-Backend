using FluentValidation;

namespace Rahhal.Features.Post.DeletePost.ViewModels
{
    public class DeletePostRequestViewModel
    {
        public Guid PostId { get; set; }
    }

    public class DeletePostRequestViewModelValidator : AbstractValidator<DeletePostRequestViewModel>
    {
        public DeletePostRequestViewModelValidator()
        {
            RuleFor(x => x.PostId).NotNull().WithMessage("PostId is required");
        }
    }
}