
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Domain.Blog;

public class Tags
{
    [JsonIgnore]
    public int TagId { get; set; }
    [JsonIgnore]
    public Guid PostId { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string DisplayName { get; set; }
    public static bool ValidateName(string tagDisplayName)
    {
        if (string.IsNullOrWhiteSpace(tagDisplayName)) return false;

        // Regex performance best practice
        // See https://docs.microsoft.com/en-us/dotnet/standard/base-types/best-practices

        const string pattern = @"^[a-zA-Z 0-9\.\-\+\#\s]*$";
        var isEng = Regex.IsMatch(tagDisplayName, pattern);
        if (isEng) return true;

        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-classes-in-regular-expressions#supported-named-blocks
        const string chsPattern = @"\p{IsCJKUnifiedIdeographs}";
        var isChs = Regex.IsMatch(tagDisplayName, chsPattern);

        return isChs;
    }
}
