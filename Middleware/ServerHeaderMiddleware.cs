namespace vsports.Middleware
{
    public class ServerHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public ServerHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Remove("Server");
                return Task.CompletedTask;
            }, context);

            await _next(context);
        }
    }
}
