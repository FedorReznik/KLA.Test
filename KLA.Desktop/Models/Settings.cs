using JetBrains.Annotations;

namespace KLA.Desktop.Models;

[PublicAPI]
public record Settings
{
    public string ServerAddress { get; set; } = null!;
}