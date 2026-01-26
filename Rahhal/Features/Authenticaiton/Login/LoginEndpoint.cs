using Microsoft.AspNetCore.Mvc;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using Rahhal.Features.Login.ViewModels;

namespace Rahhal.Features.Login
{
    public class LoginEndpoint : BaseEndpoint<LoginRequestViewModel, LoginResponseViewModel>
    {
        public LoginEndpoint(BaseEndpointParameters<LoginRequestViewModel> parameters) : base(parameters) { }

        [HttpPost("Auth/Login")]
        public async Task<EndpointResponse<LoginResponseViewModel>> HandleAsync([FromBody] LoginRequestViewModel model)
        {
            var validation = ValidateRequest(model);
            if (!validation.IsSuccess) return validation;

            var result = await _mediator.Send(new Command.LoginOrchestrator(model));

            if (!result.IsSuccess)
                return EndpointResponse<LoginResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return EndpointResponse<LoginResponseViewModel>.Success(result.Data);
        }
    }
}
