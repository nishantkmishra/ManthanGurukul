namespace ManthanGurukul.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        // Constructor that accepts a custom message
        public BadRequestException(string message)
            : base(message)
        {
        }

        // Constructor that accepts a custom message and an inner exception
        public BadRequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
