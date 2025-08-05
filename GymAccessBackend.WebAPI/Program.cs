using GymAccessBackend.Core.Interfaces;
using GymAccessBackend.WebAPI;
using GymAccessBackend.WebAPI.Request;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });

    app.UseReDoc(options =>
    {
        options.SpecUrl("/openapi/v1.json");
    });

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapPost("/payment", async (PurchaseRequest request, IPurchaseLogic purchaseLogic) =>
{
    if (string.IsNullOrWhiteSpace(request.CardNumber) ||
        string.IsNullOrWhiteSpace(request.CardHolder) ||
        string.IsNullOrWhiteSpace(request.Email) ||
        request.DayRequested == null)
    {
        return Results.BadRequest("Invalid payment request.");
    }

    var response = await purchaseLogic.ProcessPurchaseAsync(
        request.CardHolder,
        request.CardNumber,
        request.Email,
        request.DayRequested.GetValueOrDefault());

    return Results.Ok(response);
})
.WithName("DoPayment");

app.Run();
