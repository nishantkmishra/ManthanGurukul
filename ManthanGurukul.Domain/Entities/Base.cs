namespace ManthanGurukul.Domain.Entities
{
    public class Base
    {
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public long CreatedAt { get; set; }
        public Guid ModifiedBy { get; set; }
        public long ModifiedAt { get; set; }
    }
}
