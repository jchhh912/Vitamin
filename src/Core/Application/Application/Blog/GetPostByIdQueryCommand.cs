

using Application.Blog.Spc;
using Application.Presistence;
using Domain.Blog;
using MediatR;

namespace Application.Blog;

public record GetPostByIdQueryCommand(Guid Id) : IRequest<Post>;

public class GetPostByIdQueryCommandHandler : IRequestHandler<GetPostByIdQueryCommand, Post>
{
    private readonly IRepository<Post> _postRepo;
    public GetPostByIdQueryCommandHandler(IRepository<Post> postRepo)=>_postRepo=postRepo;

    public async Task<Post> Handle(GetPostByIdQueryCommand request, CancellationToken cancellationToken)
    {
        var spec = new PostSpec(request.Id);
        return await _postRepo.GetAsync(spec);
    }
}
