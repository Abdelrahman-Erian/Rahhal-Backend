using FluentValidation;

namespace Rahhal.Features.Post.CreatePost.ViewModels
{
    public class CreatePostRequestViewModel
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? Files { get; set; }
    }

    public class CreatePostRequestViewModelValidator : AbstractValidator<CreatePostRequestViewModel>
    {
        public CreatePostRequestViewModelValidator()
        {
            RuleFor(x => x.UserId).NotNull().WithMessage("UserId is required");

            RuleFor(x => x)
                .Must(x =>
                {
                    var hasDesc = !string.IsNullOrWhiteSpace(x.Description);
                    var hasFiles2 = x.Files != null;

                    return hasDesc || hasFiles2;
                })
                .WithMessage("Provide either description OR at least 2 files, not both.");
        }
    }
}