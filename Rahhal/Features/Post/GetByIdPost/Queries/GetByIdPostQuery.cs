using MediatR;
using Microsoft.EntityFrameworkCore;
using Rahhal.Common.Views;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Enum;
using Rahhal.Data.Repository;
using Rahhal.Features.Post.GetByIdPost.ViewModels;
using Rahhal.Features.Posts.GetByIdPost.ViewModels;

namespace Rahhal.Features.Post.GetByIdPost.Queries
{
    public record GetByIdPostQuery(GetByIdPostRequestViewModel ViewModel)
        : IRequest<RequestResult<GetByIdPostResponseViewModel>>;

    public class
        GetByIdPostQueryHandler(
            BaseRequestHandlerParameter parameters,
            IRepository<Models.PostManagement.Post> repository)
        : BaseRequestHandler<GetByIdPostQuery, RequestResult<GetByIdPostResponseViewModel>>(parameters)
    {
        public override async Task<RequestResult<GetByIdPostResponseViewModel>> Handle(GetByIdPostQuery request,
            CancellationToken cancellationToken)
        {
            var baseQuery = repository.GetAll();
            baseQuery = ApplyFilters(baseQuery, request.ViewModel);


            var result = await baseQuery.Select(x => new GetByIdPostResponseViewModel
            {
                ID = x.ID,
                UserId = x.UserId,
                UserName = x.User!.UserName,
                ProfileURL = x.User.ProfilePicture,
                Description = x.Description,
                Media_URLs = x.Media != null ? x.Media.Select(y => y.Media_URL).ToList() : null
            }).FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                return RequestResult<GetByIdPostResponseViewModel>.Failure(ErrorCode.NotFound, "Not found");

            return RequestResult<GetByIdPostResponseViewModel>.Success(result!);
        }

        private static IQueryable<Models.PostManagement.Post> ApplyFilters(IQueryable<Models.PostManagement.Post> query,
            GetByIdPostRequestViewModel vm)
        {
            return query.Where(p => p.ID == vm.PostId);
        }
    }
}