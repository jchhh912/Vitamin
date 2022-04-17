
using Application.Common.Exceptions;
using Application.Presistence;
using Domain.Blog;
using MediatR;

namespace Application.Blog;

public record DeletePostCommand(Guid Id, bool SoftDelete=false):IRequest<Guid>;

public class DeletePostRequestHandler : IRequestHandler<DeletePostCommand, Guid>
{
    private readonly IRepository<Post> _postRepo;
    public DeletePostRequestHandler(IRepository<Post> postRepo)=>_postRepo=postRepo;
    public async Task<Guid> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post=await _postRepo.GetAsync(request.Id);
        if (post == null) throw new ConflictException("post.cannotbedeleted") ;
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
