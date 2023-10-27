using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using WebApplication.Constants;
using WebApplication.Data;
using WebApplication.Mappers;
using WebApplication.MiddleWares;
using WebApplication.Services;
using WebApplication.Services.Impl;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter()); 
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSingleton<DatabaseMigrator>();
builder.Services.AddScoped<Tokens>();


var configuration = builder.Configuration;

builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:AccessKey").Value!))
    };
});


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


var app = builder.Build();

app.Services.GetRequiredService<DatabaseMigrator>().MigrateDatabase();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandlingMiddlewares();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();