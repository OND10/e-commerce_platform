namespace Order.API.Common.Exceptions
{
    public class IdNullException : SystemException
    {
        public IdNullException(string? message) : base(message)
        {

        }
    }
}
