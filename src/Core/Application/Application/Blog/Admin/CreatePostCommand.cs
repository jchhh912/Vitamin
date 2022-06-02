using Application.Blog.Request;
using Application.Common.FileStorage;
using Application.Presistence;
using Domain.Blog;
using Domain.Common;
using MediatR;

namespace Application.Blog.Admin;

public record CreatePostCommand(CreateOrEditPostRequest Payload) : IRequest<Guid>;
public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IRepository<Post> _postRepo;
    private readonly IFileStorageService _file;
    public CreatePostCommandHandler(
        IRepository<Post> postRepo,
        IFileStorageService file)
    {
        _postRepo = postRepo;
        _file = file;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        string ImageUrl = await _file.UploadAsync<Post>(request.Payload.HeroImageUrl, FileType.Image, cancellationToken);
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
            HeroImageUrl = ImageUrl

        };
        post.Tags = request.Payload.Tags;
        post.PostCategory = request.Payload.Categorys;
        await _postRepo.AddAsync(post);
        return post.Id;
    }
}
