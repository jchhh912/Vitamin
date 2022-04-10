

namespace Domain.Blog;

public class Tags
{
    public Tags(string displayName) 
    {
        if (string.IsNullOrEmpty(displayName))
        {
            DisplayName = displayName;
        }
    }
    public int Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string DisplayName { get; set; }

}
