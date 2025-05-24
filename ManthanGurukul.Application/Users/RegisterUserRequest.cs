namespace ManthanGurukul.Application.Users
{
    public class RegisterUserRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public long MobileNo { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public long CreatedAt { get; set; }
        public Guid ModifiedBy { get; set; }
        public long ModifiedAt { get; set; }

    }
}
