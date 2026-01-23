using MediatR;
using Microsoft.EntityFrameworkCore;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Enum;
using Rahhal.Common.Views;
using Rahhal.Data.Repository;
using Rahhal.Features.Authenticaiton.Login.DTOs;
using Rahhal.Models.UserManagment;

namespace Rahhal.Features.Authenticaiton.Login.Query
{
    public sealed record CheckIfEmailExistQuery(string UserName) : IRequest<RequestResult<GetUserDTO>>;
    public class CheckIfEmailExistQueryHandler  : BaseRequestHandler<CheckIfEmailExistQuery, RequestResult<GetUserDTO>>
    {
        private readonly IRepository<User> _repository;

        public CheckIfEmailExistQueryHandler(BaseRequestHandlerParameter parameters, IRepository<User> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<RequestResult<GetUserDTO>> Handle(CheckIfEmailExistQuery request, CancellationToken cancellationToken)
        {
            var result =await _repository.GetAll().Where(x => x.Email == request.UserName || x.UserName == request.UserName).Select(c=> new GetUserDTO
            {
                Id = c.ID,
                Email = c.Email,
                HashedPassword = c.HashedPassword,
                SaltPassword = c.SaltPassword,
                Username = c.UserName
            }).FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                return RequestResult<GetUserDTO>.Failure(ErrorCode.UserNotFound, "User not found");
            }

            return RequestResult<GetUserDTO>.Success(result);
        }
    }

}
