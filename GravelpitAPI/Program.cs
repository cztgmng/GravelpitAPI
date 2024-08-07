using Microsoft.AspNetCore.Diagnostics;

namespace GravelpitAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            var configuration = builder.Configuration;

            if (configuration["ip"] is string ip)
            {
                staticVariables.ipAddress = ip;
            }
            if (configuration["dbName"] is string databaseName)
            {
                staticVariables.databaseName = databaseName;
            }
            if (configuration["dbUser"] is string databseUserID)
            {
                staticVariables.databaseUserName = databseUserID;
            }
            if (configuration["dbPass"] is string databasePassword)
            {
                staticVariables.databaseUserPassword = databasePassword;
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature.Error;

                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync($"An unexpected fault happened. Please try again later. Exception Logs: {exception.Message} {exception.StackTrace}");

                    //await LogException(exception);
                });
            });

            app.UseCors();

            app.UseRouting();
            app.UseMiddleware<ResponseMessageMiddleware>();

            app.UseDefaultFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run("http://*:5000");
        }
    }
    public class ResponseMessageMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMessageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);
            if (!httpContext.Response.HasStarted)
            {
                switch (httpContext.Response.StatusCode)
                {
                    case 404:
                        await httpContext.Response.WriteAsJsonAsync(new
                        {
                            Status = 404,
                            Title = "Not found."
                        });

                        return;
                }
            }
        }
    }

}