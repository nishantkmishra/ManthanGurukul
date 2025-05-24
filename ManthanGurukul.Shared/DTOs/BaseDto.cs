namespace ManthanGurukul.Shared.DTOs
{
    public class BaseDto
    {
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public long CreatedAt { get; set; }
        public Guid ModifiedBy { get; set; }
        public long ModifiedAt { get; set; }
    }
}
