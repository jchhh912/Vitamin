using Application.Blog.Request;
using Application.Blog.Spc;
using Application.Presistence;
using Domain.Blog;
using Mapster;
using MediatR;

namespace Application.Blog
{
    public record SelectPostCommand(PostStatus Status) : IRequest<IReadOnlyList<PostDto>>;
    public class SelectPostCommandHandler : IRequestHandler<SelectPostCommand, IReadOnlyList<PostDto>>
    {
        private readonly IRepository<Post> _postRepo;
        public SelectPostCommandHandler(IRepository<Post> postRepo)=>_postRepo = postRepo;

        public async Task<IReadOnlyList<PostDto>> Handle(SelectPostCommand request, CancellationToken cancellationToken)
        {
            var list = await _postRepo.SelectAsync(new PostSpec(request.Status),PostDto.EntitySelector);
            return list;
        }
    }
}
