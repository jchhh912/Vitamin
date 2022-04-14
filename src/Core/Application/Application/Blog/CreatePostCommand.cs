using Application.Blog.Request;
using Application.Presistence;
using Domain.Blog;
using MediatR;

namespace Application.Blog;

public record CreatePostCommand(CreatePostRequest Payload) : IRequest<Guid>;
public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IRepository<Post> _postRepo;
    public CreatePostCommandHandler(
        IRepository<Post> postRepo)
    {
        _postRepo = postRepo;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post
        {
            CommentEnabled = request.Payload.EnableComment,
            Id = Guid.NewGuid(),
            PostContent = request.Payload.EditorContent,
            CreateTimeUtc = DateTime.UtcNow,
            LastModifiedUtc = DateTime.UtcNow,
            PubDateUtc = DateTime.UtcNow,
            Slug = request.Payload.Slug,
            Author = request.Payload.Author,
            Title = request.Payload.Title,
            IsDeleted = false,
            IsPublished = request.Payload.IsPublished,
            IsOriginal = request.Payload.IsOriginal,
            OriginLink = request.Payload.OriginLink,
            HeroImageUrl = request.Payload.HeroImageUrl
        };
        await _postRepo.AddAsync(post);
        return post.Id;
    }
}
