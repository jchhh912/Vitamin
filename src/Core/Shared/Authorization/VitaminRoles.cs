

using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class VitaminRoles
{
    public const string Administrators = nameof(Administrators);
    public const string Visitor = nameof(Visitor);
    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Administrators,
        Visitor
    });
    public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
}
