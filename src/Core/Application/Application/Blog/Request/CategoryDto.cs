

using Domain.Blog;
using System.Linq.Expressions;

namespace Application.Blog.Request;

public class CategoryDto
{
    public CategoryDto(string displayName)
    {
        DisplayName = displayName;
    }
    public string DisplayName { get; set; }
    public static readonly Expression<Func<IGrouping<CategoryDto, Categorys>, CategoryDto>> EntitySelector =
        c => new(c.Key.DisplayName);

  
}
