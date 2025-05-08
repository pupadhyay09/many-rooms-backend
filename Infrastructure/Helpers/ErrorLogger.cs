using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;

namespace ManyRoomStudio.Infrastructure.Helpers
{
    public class ErrorLogger
    {
        public static async Task DoLog(string controllerName, string actionName, string? exceptionMessage,
            string exceptionType, string? ipAddress)
        {
            ManyRoomStudioContext context = ManyRoomStudioContext.Create();
            context.ErrorLogs.Add(new ErrorLog
            {
                ControllerName = controllerName,
                ActionName = actionName,
                ExceptionMessage = exceptionMessage,
                ExceptionType = exceptionType,
                Ipaddress = ipAddress,
                Timex = DateTime.Now
            });
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
