using Mapster;
using MediatR;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Enum;
using Rahhal.Common.Views;
using Rahhal.Features.Authenticaiton.Login.Query;
using Rahhal.Features.Login.DTOs;
using Rahhal.Features.Login.ViewModels;

namespace Rahhal.Features.Login.Command
{
    public record LoginOrchestrator(LoginRequestViewModel ViewModel) : IRequest<RequestResult<LoginResponseViewModel>>;

    public class LoginOrchestratorHandler : BaseRequestHandler<LoginOrchestrator, RequestResult<LoginResponseViewModel>>
    {
        public LoginOrchestratorHandler(BaseRequestHandlerParameter parameters) : base(parameters) { }

        public override async Task<RequestResult<LoginResponseViewModel>> Handle(LoginOrchestrator request, CancellationToken cancellationToken)
        {
            // check the user exist Or not 
            var GetUser =await _mediator.Send(new CheckIfEmailExistQuery(request.ViewModel.Email));
            if(!GetUser.IsSuccess)
                return RequestResult<LoginResponseViewModel>.Failure(GetUser.ErrorCode, "UserNotFound");
     

           var dto = GetUser.Data.Adapt<LoginRequestDTO>();
            dto.Password = request.ViewModel.Password;

            var result = await _mediator.Send(new LoginCommand(dto), cancellationToken);

            if (!result.IsSuccess)
                return RequestResult<LoginResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return RequestResult<LoginResponseViewModel>.Success(result.Data.Adapt<LoginResponseViewModel>(), result.Message);
        }

       
    }
}
