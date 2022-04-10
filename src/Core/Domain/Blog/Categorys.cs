

namespace Domain.Blog;

public class Categorys
{
    public Categorys(string displayName,string note) 
    {
        if (string.IsNullOrEmpty(displayName))
        {
            DisplayName = displayName;
        }
        if (string.IsNullOrEmpty(note))
        {
            Note = note;
        }
    }
    public Guid Id { get; set; }
    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Note { get; set; }
}
