using System.Runtime.Serialization;

namespace Royal.Common.Enum;

public enum UserRole
{
    None,
    [EnumMember(Value = "admin")]
    Admin,
    [EnumMember(Value = "moderator")]
    Moderator,
    [EnumMember(Value = "user")]
    User
}
