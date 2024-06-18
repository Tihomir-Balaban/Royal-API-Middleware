using Royal.Models.Dtos.Base;

namespace Royal.Models.Dtos
{
    public sealed class UserDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
