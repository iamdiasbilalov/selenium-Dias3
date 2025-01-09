using System.Text.Json.Serialization;

namespace SeleniumWebDriverPracticeWithDotNet.Models;

public sealed class UserData
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }
}