using Infrastructure.Configurations;
using Newtonsoft.Json;
using AutoMapper;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

//Add custom services
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomLogging(builder.Configuration);
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddAutoMapper();
builder.Services.AddDomainServices();


//clear the default logging providers and use Serilog
builder.Logging.ClearProviders();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();