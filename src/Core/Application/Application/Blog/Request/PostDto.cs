
using Domain.Blog;
using System.Linq.Expressions;

namespace Application.Blog.Request;

public class PostDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 自定义文章Slug链接 https://zhuanlan.zhihu.com/p/126112777
    /// </summary>
    public string Slug { get; set; }
    /// <summary>
    /// 作者
    /// </summary>
    public string Author { get; set; }
    /// <summary>
    /// 文章类容
    /// </summary>
    public string PostContent { get; set; }
    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime PubDateUtc { get; set; }
    /// <summary>
    /// 是否原创
    /// </summary>
    public bool IsOriginal { get; set; }
    /// <summary>
    /// 文章封面图片
    /// </summary>
    public string? HeroImageUrl { get; set; }
    /// <summary>
    /// 标签列表
    /// </summary>
    public virtual ICollection<Tags> Tags { get; set; }
    /// <summary>
    /// 分类
    /// </summary>
    public virtual ICollection<Categorys> PostCategory { get; set; }

    public static readonly Expression<Func<Post, PostDto>> EntitySelector = p => new()
    {
        Id = p.Id,
        Title = p.Title,
        Slug = p.Slug,
        Author = p.Author,
        PostContent = p.PostContent,
        PubDateUtc = p.PubDateUtc,
        IsOriginal = p.IsOriginal,
        HeroImageUrl = p.HeroImageUrl,
        Tags=p.Tags,
        PostCategory = p.PostCategory
    };
}
