

using System.Text.Json.Serialization;

namespace Domain.Blog;

public class Categorys
{
    [JsonIgnore]
    public int CategoryId { get; set; }
    [JsonIgnore]
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
