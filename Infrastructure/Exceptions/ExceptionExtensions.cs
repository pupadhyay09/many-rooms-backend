namespace ManyRoomStudio.Infrastructure.Exceptions
{
    public static class ExceptionExtensions
    {
        public static string GetFullMessage(this Exception ex)
        {
            if (ex == null)
            {
                return "Exception message is empty";
            }

            var innerException = ex.InnerException?.GetFullMessage();

            if (ex.Message.Contains("DELETE statement conflicted with the REFERENCE constraint") ||
                ((innerException ?? string.Empty).Contains("DELETE statement conflicted with the REFERENCE constraint")))
                return $"This item is already in use";

            return
                string.IsNullOrEmpty(innerException) ?
                    $"{ex.Message}" :
                    $"{innerException}";
        }
    }
}
