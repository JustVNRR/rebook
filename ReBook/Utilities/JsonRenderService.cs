namespace Rebook.Utilities
{
    public interface IJsonRenderService
    {
        JsonResult Render();
        JsonResult Render(JsonVM json);
        Task<JsonResult> RenderAsync(string viewName);
        Task<JsonResult> RenderAsync(string viewName, object model);
        Task<JsonResult> RenderAsync(JsonVM json, string viewName);
        Task<JsonResult> RenderAsync(JsonVM json, string viewName, object model);
    }
    public class JsonRenderService : IJsonRenderService
    {
        private readonly IViewRenderService _viewRender;

        public JsonRenderService(
            IViewRenderService viewRenderService)
        {
            _viewRender = viewRenderService;
        }

        public JsonResult Render()
        {
            return new JsonResult(new JsonVM());
        }

        public JsonResult Render(JsonVM json)
        {
            return new JsonResult(json);
        }

        public async Task<JsonResult> RenderAsync(string viewName)
        {
            JsonVM json = new()
            {
                responseText = await _viewRender.ToStringAsync(viewName, null)
            };

            return new JsonResult(json);
        }

        public async Task<JsonResult> RenderAsync(string viewName, object model)
        {
            JsonVM json = new()
            {
                responseText = await _viewRender.ToStringAsync(viewName, model)
            };

            return new JsonResult(json);
        }

        public async Task<JsonResult> RenderAsync(JsonVM json, string viewName)
        {
            json.responseText = await _viewRender.ToStringAsync(viewName, null);

            return new JsonResult(json);
        }

        public async Task<JsonResult> RenderAsync(JsonVM json, string viewName, object model)
        {
            json.responseText = await _viewRender.ToStringAsync(viewName, model);

            return new JsonResult(json);
        }
    }
}




