
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

}
