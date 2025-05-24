namespace ManthanGurukul.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        // Constructor that accepts a custom message
        public NotFoundException(string message)
            : base(message)
        {
        }

        // Constructor that accepts a custom message and an inner exception
        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
