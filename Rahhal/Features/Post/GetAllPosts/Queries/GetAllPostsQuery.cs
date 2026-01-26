using MediatR;
using Rahhal.Common.Views;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Helper;
using Rahhal.Data.Repository;
using Rahhal.Features.Post.GetAllPosts.ViewModels;

namespace Rahhal.Features.Post.GetAllPosts.Queries
{
    public record GetAllPostsQuery(GetAllPostsRequestViewModel ViewModel)
        : IRequest<RequestResult<PagingViewModel<GetAllPostsResponseViewModel>>>;

    public class GetAllPostsQueryHandler(
        BaseRequestHandlerParameter parameters,
        IRepository<Models.PostManagement.Post> repository)
        : BaseRequestHandler<GetAllPostsQuery,
            RequestResult<PagingViewModel<GetAllPostsResponseViewModel>>>(parameters)
    {
        public override async Task<RequestResult<PagingViewModel<GetAllPostsResponseViewModel>>> Handle(
            GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            
            var baseQuery = repository.GetAll();
            baseQuery = ApplyFilters(baseQuery, request.ViewModel);

            var projection = baseQuery.Select(x => new GetAllPostsResponseViewModel
            {
                ID = x.ID,
                UserId = x.UserId,
                UserName = x.User!.UserName,
                ProfileUrl = x.User.ProfilePicture,
                Description = x.Description,
                Media_URLs = x.Media != null ? x.Media.Select(y => y.Media_URL).ToList() : null
            });
            var result =
                await PagingHelper.CreateAsync(projection, request.ViewModel.PageNumber, request.ViewModel.PageSize);
            return RequestResult<PagingViewModel<GetAllPostsResponseViewModel>>.Success(result);
        }

        private static IQueryable<Models.PostManagement.Post> ApplyFilters(IQueryable<Models.PostManagement.Post> query,
            GetAllPostsRequestViewModel vm)
        {
            return query;
        }
    }
}