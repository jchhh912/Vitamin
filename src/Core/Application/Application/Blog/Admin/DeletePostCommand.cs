using Application.Blog.Spc;
using Application.Common.Exceptions;
using Application.Presistence;
using Domain.Blog;
using MediatR;

namespace Application.Blog.Admin;

public record DeletePostCommand(Guid Id, bool SoftDelete = false) : IRequest<Guid>;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Guid>
{
    private readonly IRepository<Post> _postRepo;
    public DeletePostCommandHandler(IRepository<Post> postRepo) => _postRepo = postRepo;
    public async Task<Guid> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepo.GetAsync(new PostSpec(request.Id)); ;
        if (post == null) throw new InvalidOperationException($"Post {request.Id} is not found.");
        if (request.SoftDelete)
        {
            post.IsDeleted = true;
            await _postRepo.UpdateAsync(post);
        }
        else
        {
            await _postRepo.DeleteAsync(post);
        }
        return post.Id;
    }
}
