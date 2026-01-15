
using MediatR;
using Rahhal.Common.Views;
using Rahhal.Models;

namespace Rahhal.Common.BaseHandler
{
    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
       
    {
        protected readonly IMediator _mediator;

        protected readonly TokenHelper _tokenHelper;

        protected readonly UserState _userState;



        public BaseRequestHandler(BaseRequestHandlerParameter parameters)
        {
            _mediator = parameters.Mediator;
           
            _tokenHelper = parameters.TokenHelper;
            _userState = parameters.UserState;

        }
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
