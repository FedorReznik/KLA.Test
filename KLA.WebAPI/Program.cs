using KLA.Domain.Services;
using KLA.Domain.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register domain services
builder.Services.AddSingleton<ICurrencyRangeValidator, CurrencyRangeValidator>();
builder.Services.AddSingleton<ICurrencyParser, CurrencyParser>();
builder.Services.AddSingleton<ICurrencyToTextConverterService, CurrencyToTextConverterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(swagger =>
    {
        swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "KLA Service API Version 1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();