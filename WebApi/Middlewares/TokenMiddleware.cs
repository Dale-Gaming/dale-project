using Domain.Interfaces.Services;
using Newtonsoft.Json;
using System.Net;

namespace WebApi.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _provider;
        public TokenMiddleware(RequestDelegate next, IServiceProvider provider)
        {
            _next = next;
            _provider = provider;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Method == "OPTIONS")
            {
                await _next(httpContext);
            }
            else
            {
                string token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Replace("bearer ", "");
                if (!string.IsNullOrEmpty(token))
                {

                    using var scope = _provider.CreateScope();
                    var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
                    if (tokenService.TokenValido(token))
                    {
                        await _next(httpContext);
                    }
                    else
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        httpContext.Response.ContentType = "application/json; charset=utf-8";
                        httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            success = false,
                            message = "Token invalido",
                            statusCode = (int)HttpStatusCode.Unauthorized
                        }));
                    }
                }
                else
                {
                    await _next(httpContext);
                }
            }
        }
    }
}
