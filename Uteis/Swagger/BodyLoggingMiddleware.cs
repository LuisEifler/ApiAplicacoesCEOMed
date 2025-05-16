using System.Text;

namespace APICeomedAplicacoes.Uteis.Swagger
{
    public class BodyLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public BodyLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            context.Items["RawBody"] = body;

            context.Request.Body.Position = 0;

            await _next(context);
        }
    }

}
