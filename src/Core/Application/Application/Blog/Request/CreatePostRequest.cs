

using System.ComponentModel.DataAnnotations;

namespace Application.Blog.Request;

public class CreatePostRequest
{
    [Required]
    [MaxLength(128)]
    public string Title { get; set; }

    [Required]
    [RegularExpression(@"[a-z0-9\-]+")]
    [MaxLength(128)]
    public string Slug { get; set; }

    [Display(Name = "Author")]
    [MaxLength(64)]
    public string Author { get; set; }

    [Required]
    [Display(Name = "Enable Comment")]
    public bool EnableComment { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    public string EditorContent { get; set; }

    [Required]
    [Display(Name = "Publish Now")]
    public bool IsPublished { get; set; }

    [Display(Name = "Tags")]
    [MaxLength(128)]
    public List<string> Tags { get; set; }

    [Display(Name = "Original")]
    public bool IsOriginal { get; set; }

    [Display(Name = "Origin Link")]
    [DataType(DataType.Url)]
    public string OriginLink { get; set; }

    [Display(Name = "Hero Image")]
    [DataType(DataType.Url)]
    public string HeroImageUrl { get; set; }

}
