
using Application.Presistence;
using Domain.Blog;
using Microsoft.EntityFrameworkCore;

namespace Application.Blog.Spc;

public sealed class PostSpec:BaseSpecification<Post>
{
    public PostSpec(Guid id):base(p=>p.Id==id)
    {
        AddInclude(post => post
        .Include(pt=>pt.Tags)
        .Include(pt=>pt.PostCategory));
    }
    public PostSpec(PostStatus status)
    {
        AddInclude(post => post
       .Include(pt => pt.Tags)
       .Include(pt => pt.PostCategory));
        switch (status)
        {

            case PostStatus.Published:
                AddCriteria(p => p.IsPublished && !p.IsDeleted);
                break;
            case PostStatus.Deleted:
                AddCriteria(p => p.IsDeleted);
                break;
            case PostStatus.Default:
                AddCriteria(p => true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
    }

}
