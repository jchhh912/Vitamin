

namespace Domain.Blog;

public class Post
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
    /// 评论开关
    /// </summary>
    public bool CommentEnabled { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTimeUtc { get; set; }
    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PubDateUtc { get; set; }
    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? LastModifiedUtc { get; set; }
    /// <summary>
    /// 是否公开
    /// </summary>
    public bool IsPublished { get; set; }
    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }
    /// <summary>
    /// 是否原创
    /// </summary>
    public bool IsOriginal { get; set; }
    /// <summary>
    /// 原创链接
    /// </summary>
    public string OriginLink { get; set; }
    /// <summary>
    /// 文章封面图片
    /// </summary>
    public string HeroImageUrl { get; set; }
    /// <summary>
    /// hash检查
    /// </summary>
    public int HashCheckSum { get; set; }
    /// <summary>
    /// 标签列表
    /// </summary>
    public virtual ICollection<Tags> Tags { get; set; }
    /// <summary>
    /// 分类
    /// </summary>
    public virtual Categorys Category { get; set; }

}

