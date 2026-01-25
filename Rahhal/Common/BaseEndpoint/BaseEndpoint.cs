using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rahhal.Common.Enum;
using Rahhal.Common.Views;

namespace Rahhal.Common.BaseEndpoint
{
    [ApiController]
    public class BaseEndpoint<TRequest, TResponse> : ControllerBase
    {
        protected IMediator _mediator;
        protected IValidator<TRequest> _validator;

        public BaseEndpoint(BaseEndpointParameters<TRequest> parameters)
        {
            _mediator = parameters.Mediator;
            _validator = parameters.Validator;
        }
        protected EndpointResponse<TResponse> ValidateRequest(TRequest request)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var validationError = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

                return EndpointResponse<TResponse>.Failure(ErrorCode.InvalidData, validationError);
            }

            return EndpointResponse<TResponse>.Success(default);
        }
    }
}
