﻿
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace ManyRoomStudio.Infrastructure.RazorPartial
{
    public class RazorPartialToString : IRazorPartialToString
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        public RazorPartialToString(
           IRazorViewEngine viewEngine,
           ITempDataProvider tempDataProvider,
           IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }
        public async Task<string> Render<TModel>(string partialName, TModel model)
        {
            var actionContext = GetActionContext();
            var partial = FindView(actionContext, partialName);
            await using var output = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                partial,
                new ViewDataDictionary<TModel>(
                    metadataProvider: new EmptyModelMetadataProvider(),
                    modelState: new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(
                    actionContext.HttpContext,
                    _tempDataProvider),
                output,
                new HtmlHelperOptions()
            );
            await partial.RenderAsync(viewContext);
            return output.ToString();
        }
        private IView FindView(ActionContext actionContext, string partialName)
        {
            var getPartialResult = _viewEngine.GetView(null, partialName, false);
            if (getPartialResult.Success)
            {
                return getPartialResult.View;
            }
            var findPartialResult = _viewEngine.FindView(actionContext, partialName, false);
            if (findPartialResult.Success)
            {
                return findPartialResult.View;
            }
            var searchedLocations = getPartialResult.SearchedLocations.Concat(findPartialResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find partial '{partialName}'. The following locations were searched:" }.Concat(searchedLocations)); ;
            throw new InvalidOperationException(errorMessage);
        }
        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}
