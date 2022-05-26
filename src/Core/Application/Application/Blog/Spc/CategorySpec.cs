
using Application.Presistence;
using Domain.Blog;

namespace Application.Blog.Spc;

public sealed class CategorySpec:BaseSpecification<Categorys>
{
    public CategorySpec(string name) 
    {
        AddCriteria(c => c.DisplayName == name);
    }
}
