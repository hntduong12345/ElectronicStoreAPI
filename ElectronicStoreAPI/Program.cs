using API.BO.Models;
using API.Service;
using ElectronicStoreAPI.Constants;
using ElectronicStoreAPI.Extensions;
using ElectronicStoreAPI.Middlewares;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsConstant.PolicyName,
        policy => { policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod(); });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddServices(builder.Configuration);
//builder.Services.AddJwtValidation(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddConfigSwagger();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.Configure<MongoDBContext>(builder.Configuration.GetSection("MongoDbSection"));
builder.Services.AddScoped<IMongoClient>(sp =>
{
    var getContext = sp.GetRequiredService<IOptions<MongoDBContext>>();
    IMongoClient client = new MongoClient(getContext.Value.ConnectionURI);
    return client;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(CorsConstant.PolicyName);
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
