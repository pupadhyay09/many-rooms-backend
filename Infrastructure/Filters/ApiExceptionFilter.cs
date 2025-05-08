using ManyRoomStudio.Controllers;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using ManyRoomStudio.Infrastructure.Exceptions;
using ManyRoomStudio.Infrastructure.Helpers;
using ManyRoomStudio.Gateways.Interfaces;

namespace ManyRoomStudio.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ApiExceptionFilter : Attribute, IAsyncExceptionFilter
    {
        private readonly IErrorGateway _errorGateway;

        public ApiExceptionFilter(IErrorGateway errorGateway)
        {
            _errorGateway = errorGateway;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception.GetFullMessage();
            string exceptionType = $"{context.Exception.GetType()}";

            var controllerName = $"{context.RouteData.Values["controller"]}";
            var actionName = $"{context.RouteData.Values["action"]}";
            var ipAddress = $"{context.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress}";

            await ErrorLogger.DoLog(controllerName, actionName, exception, exceptionType, ipAddress)
                .ConfigureAwait(false);

            context.ExceptionHandled = true;

            if (context.HttpContext.Request.Path.Value != null && context.HttpContext.Request.Path.Value.Contains("/api/"))
            {
                context.Result = new BadRequestObjectResult(exception);
            }
            else
            {
                var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    {
                        "Exception", context.Exception.GetFullMessage()
                    }
                };
                context.Result = new ViewResult()
                {
                    ViewData = new ViewDataDictionary(viewData),
                    ViewName = System.IO.File.Exists(@$"~\Views\Exception\{nameof(ExceptionController.Error)}.cshtml") ? actionName : "Error"
                };
            }
        }
    }
}
