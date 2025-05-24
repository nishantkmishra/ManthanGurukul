namespace ManthanGurukul.Shared.DTOs
{
    public class UserDto : BaseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public long MobileNo { get; set; }

    }
}
