using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace ManyRoomStudio.Infrastructure.Exceptions
{
    public static class ModelStateExtension
    {
        public static string? GetErrorMessages(this ModelStateDictionary modelState)
        {
            return
                string.Join(",", modelState.Where(x => x.Value != null && x.Value.Errors != null).SelectMany(e => e.Value.Errors.Select(s => s.ErrorMessage)));
        }

        public static string? GetLastErrorMessage(this ModelStateDictionary modelState)
        {
            return
                modelState?.Values.LastOrDefault(x => x.ValidationState == ModelValidationState.Invalid)?.Errors.LastOrDefault()?.ErrorMessage;
        }

        public static void RemoveModel(this ModelStateDictionary modelState, string modelName)
        {
            foreach (var s in modelState.Keys.Where(p => p.ToLower().StartsWith(modelName.ToLower())))
            {
                modelState.Remove(s);
            }
        }
        public static void RemoveAll(this ModelStateDictionary modelState, string regex)
        {
            foreach (var s in modelState.Keys.Where(p => new Regex(regex, RegexOptions.IgnoreCase).IsMatch(p)))
            {
                modelState.Remove(s);
            }
        }
    }
}
