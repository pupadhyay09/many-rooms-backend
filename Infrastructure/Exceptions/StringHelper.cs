namespace ManyRoomStudio.Infrastructure.Exceptions
{
    public static class StringHelper
    {
        public static string GetControllerName(this string controller)
        {
            return controller.ToLower().Replace("controller", "");
        }
    }
}
