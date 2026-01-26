using FluentValidation;

namespace Rahhal.Features.Login.ViewModels
{
    public class LoginRequestViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequestViewModelValidator : AbstractValidator<LoginRequestViewModel>
    {
        public LoginRequestViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}
