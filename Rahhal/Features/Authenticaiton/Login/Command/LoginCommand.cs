using MediatR;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Enum;
using Rahhal.Data.Repository;
using Rahhal.Features.Login.DTOs;
using Rahhal.Models.UserManagment;

namespace Rahhal.Features.Login.Command
{
    public record LoginCommand(LoginRequestDTO DTO ) : IRequest<RequestResult<LoginResponseDTO>>;

    public class LoginCommandHandler : BaseRequestHandler<LoginCommand, RequestResult<LoginResponseDTO>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly Common.Interfaces.PasswordServices.IPasswordHelper _passwordHelper;

        public LoginCommandHandler(
            BaseRequestHandlerParameter parameters, 
            IRepository<User> userRepository,
            Common.Interfaces.PasswordServices.IPasswordHelper passwordHelper) 
            : base(parameters)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
        }

        public override async Task<RequestResult<LoginResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
          

            // Verify password
            bool isPasswordValid = _passwordHelper.verifyPasswordHash(request.DTO.Password, request.DTO.HashedPassword, request.DTO.SaltPassword);

            if (!isPasswordValid)
                return RequestResult<LoginResponseDTO>.Failure(ErrorCode.Unauthorized, "Invalid email or password");

            // Generate JWT Token
            string token = await _tokenHelper.GenerateToken(request.DTO.Id.ToString(), request.DTO.Username);

            var response = new LoginResponseDTO
            {
                Token = token,
                UserId = request.DTO.Id.ToString(),
                UserName =request.DTO.Username,
                Email = request.DTO.Email
            };

            return RequestResult<LoginResponseDTO>.Success(response, "Login successful");
        }
    }
}
