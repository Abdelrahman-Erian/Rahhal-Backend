
using MediatR;
using Rahhal.Common.Views;
using Rahhal.Models;

namespace Rahhal.Common.BaseHandler
{
    public class BaseRequestHandlerParameter
    {
        private readonly IMediator _mediator;
        protected readonly TokenHelper _tokenHelper;
        private readonly UserState _userState;


        public IMediator Mediator => _mediator;
        public TokenHelper TokenHelper => _tokenHelper;
        public UserState UserState => _userState;



        public BaseRequestHandlerParameter(IMediator mediator, UserInfoProvider userInfoProvider, TokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
            _mediator = mediator;
            _userState = userInfoProvider.userState;
        }
    }
}
