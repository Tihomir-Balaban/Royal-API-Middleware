using Royal.Common.Enum;
using Royal.Models.Dtos.Base;
using System.Text.Json.Serialization;

namespace Royal.Models.Dtos;

public sealed class UserDto : BaseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }

    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }
}
