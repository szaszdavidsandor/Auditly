using Domain.Interface;
using Infrastructure.Repository;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//==================================================================================================
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Traverse parent directories to ensure .env is found regardless of execution path
DotNetEnv.Env.TraversePath().Load();

// Load Infrastructure dynamically at runtime
var infrastructureAssembly = Assembly.Load("Infrastructure");
var type = infrastructureAssembly.GetType("Infrastructure.DependencyInjection");
var method = type?.GetMethod("RegisterInfrastructure");
method?.Invoke(null, new object[] { builder.Services, builder.Configuration });

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        policy.WithOrigins("")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        string clientKey = httpContext.User.Identity?.Name
            ?? httpContext.Connection.RemoteIpAddress?.ToString()
            ?? "anonymous";

        string endpointhKey = httpContext.Request.Path.ToString().ToLower();

        // Unique key for each user + endpoint combination
        string fullKey = $"{clientKey}:{endpointhKey}";

        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: fullKey,
            factory: partition => new SlidingWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10, //blocked after hit this number of requests
                Window = TimeSpan.FromSeconds(10), // time window for the rate limit
                SegmentsPerWindow = 10 //blocked for this time(sec)
            });
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//==================================================================================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");

app.UseStaticFiles();

app.UseAuthentication();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
