using Application.Blog.Request;
using Application.Blog.Spc;
using Application.Presistence;
using Domain.Blog;
using MediatR;

namespace Application.Blog.Admin
{
    public record UpdatePostCommand(Guid Id, CreateOrEditPostRequest Payload) : IRequest<Guid>;
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Guid>
    {
        private readonly IRepository<Post> _postRepo;
        private readonly IRepository<Tags> _tagRepo;
        private readonly IRepository<Categorys> _catRepo;
        public UpdatePostCommandHandler(IRepository<Post> postRepo, IRepository<Tags> tagRepo, IRepository<Categorys> catRepo)
        {
            _postRepo = postRepo;
            _tagRepo = tagRepo;
            _catRepo = catRepo;
        }
        public async Task<Guid> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var (guid, postEditModel) = request;
            var post = await _postRepo.GetAsync(new PostSpec(guid)); ;
            //var post = await _postRepo.GetAsync(x=>x.Id==guid);
            if (post == null)
                throw new InvalidOperationException($"Post {guid} is not found.");
            post.CommentEnabled = postEditModel.EnableComment;
            post.PostContent = postEditModel.EditorContent;
            if (request.Payload.IsPublished && !post.IsPublished)
            {
                post.IsPublished = true;
                post.PubDateUtc = DateTime.UtcNow;
            }
            post.Author = postEditModel.Author.Trim();
            post.Slug = postEditModel.Slug.Trim();
            post.Title = postEditModel.Title.Trim();
            post.LastModifiedUtc = DateTime.UtcNow;
            post.IsOriginal = postEditModel.IsOriginal;
            post.OriginLink = postEditModel.OriginLink.Trim();
            post.HeroImageUrl = postEditModel.HeroImageUrl.Trim();
            //先删除不存在的
            foreach (var item in post.Tags.Where(t => !postEditModel.Tags.Any(p => p.DisplayName == t.DisplayName)))
            {
                await _tagRepo.DeleteAsync(item);
            }
            // 再添加新添加的
            foreach (var item in postEditModel.Tags.Where(t => !post.Tags.Any(p => p.DisplayName == t.DisplayName)))
            {
                item.PostId = post.Id;
                await _tagRepo.AddAsync(item);
            }
            //同理
            foreach (var item in post.PostCategory.Where(t => !postEditModel.Categorys.Any(p => p.DisplayName == t.DisplayName)))
            {
                await _catRepo.DeleteAsync(item);
            }
            foreach (var item in postEditModel.Categorys.Where(t => !post.PostCategory.Any(p => p.DisplayName == t.DisplayName)))
            {
                item.PostId = post.Id;
                await _catRepo.AddAsync(item);
            }
            await _postRepo.UpdateAsync(post);
            return post.Id;
        }
    }
}
