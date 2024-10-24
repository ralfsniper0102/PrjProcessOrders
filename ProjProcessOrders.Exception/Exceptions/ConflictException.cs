namespace ProjProcessOrders.Exception.Exceptions
{
    public class ConflictException : System.Exception
    {
        public ConflictException(string message, System.Exception exception = default) : base(message, exception) { }
    }
}
