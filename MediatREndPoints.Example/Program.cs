using MediatR;
using MediatREndPoints.Contracts;
using MediatREndPoints.Contracts.Models;
using MediatREndPoints.ServicePipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatREndPoints(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddAuthentication()
    .AddJwtBearer()
    .AddJwtBearer("LocalAuthIssuer");

builder.Services.AddAuthorization();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseMediatREndPointFor<WeatherForecast>();


app.Run();

public record WeatherForecast(DateTime Date) : IRequestEndPoint
{
    public EndPointModel SetUpEndPointModel()
    {
        var endPointBuilder = new EndPointBuilder()
            .WithEndPointAddress("/api/v1/GetWeatherInfo")
            .WithEndPointName("GetWeatherInfo")
            .WithEndPointType(EndPointTypes.Get)
            .WithOpenApiOperation(operation =>
            {
                operation.Description = "Gets the weather for authenticated user";
                return operation;
            })
            .WithAuthorizationPolicy(builder =>
            {
                builder.RequireAuthenticatedUser();
                builder.RequireRole("mediatREndpoint");
                builder.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };

            }).WithEndPointFilter(new CustomEndPointFilter());

        return endPointBuilder.Build();
    }
}

public record WeatherForCastResult(string WeatherType, int Temperature, string Message);


public class WeatherForCastHandler : IRequestEndPointHandler<WeatherForecast>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WeatherForCastHandler(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResult> Handle(WeatherForecast request, CancellationToken cancellationToken)
    {
        if (request.Date < DateTime.Now.Date)
            return Results.BadRequest("I can't predict weather from past");

        var temperature = new Random().Next(10, 40);

        await Task.CompletedTask;

        return Results.Ok(temperature < 20 ? new WeatherForCastResult("Cold", temperature, $"Hello {_httpContextAccessor.HttpContext.User.Identity.Name}") : new WeatherForCastResult("Warm", temperature, $"Hello {_httpContextAccessor.HttpContext.User.Identity.Name}"));
    }
}

public class CustomEndPointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        var executionResult = await next(context);

        if (executionResult is null)
            return Results.Empty;

        if (executionResult is BadRequest<string> badRequest)
            return Results.BadRequest(new
            {
                ErrorMessage=badRequest.Value,
                ErrorCode=badRequest.StatusCode,
                IsSuccess=false
            });

        return Results.Ok(new
        {
            IsSuccess=true,
            StatusCode=StatusCodes.Status200OK,
            executionResult
        });
    }
}
