using LoyaltyModels;
using LoyaltyServices.Contracts.Redis;
using LoyaltyServices.Contracts.TransactionPoint;
using LoyaltyServices.Contracts.User;
using LoyaltyServices.Data;
using LoyaltyServices.Mapping;
using LoyaltyServices.Services.RedisService;
using LoyaltyServices.Services.TransactionPointService;
using LoyaltyServices.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.MaxDepth = 64; // Adjust max depth as needed
}); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


DateTime dateTimeFile = DateTime.Now;
string fileName = dateTimeFile.Year.ToString() + dateTimeFile.Month.ToString() + dateTimeFile.Day.ToString();

Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Debug()
   .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
   .Enrich.FromLogContext()
   .WriteTo.Console()
   .WriteTo.File("logs/" + fileName + ".txt", rollingInterval: RollingInterval.Day)
   .CreateLogger();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    ));

builder.Services.AddDistributedRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "redisIns";
}
);

builder.Services.AddTransient<IUser, UserSrv>();

builder.Services.AddTransient<ITransactionPoint, TransactionPointSrv>();

builder.Services.AddTransient<IRedisService, RedisSrv>();

builder.Services.AddAutoMapper(typeof(Maps));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080/auth/realms/master";
        options.Audience = "loyaltyapi";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:8080/realms/master",
            ValidateAudience = true,
            ValidAudiences = new[] { "loyaltyapi", "account" },
            ValidateLifetime = true,
            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
            {
                var client = new HttpClient();
                var keySet = client.GetStringAsync("http://host.docker.internal:8080/realms/master/protocol/openid-connect/certs").Result;
                return new JsonWebKeySet(keySet).GetSigningKeys();
            }
        };
    });




builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Loyalty System API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();


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
