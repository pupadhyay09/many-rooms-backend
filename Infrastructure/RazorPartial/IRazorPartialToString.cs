namespace ManyRoomStudio.Infrastructure.RazorPartial
{
    public interface IRazorPartialToString
    {
        Task<string> Render<TModel>(string partialName, TModel model);
    }
}
