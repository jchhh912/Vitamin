

namespace Domain.Blog;

public class Categorys
{
    public Categorys(string displayName,string note) 
    {
            DisplayName = displayName;
            Note = note;
    }
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Note { get; set; }


}
