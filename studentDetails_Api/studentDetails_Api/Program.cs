using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using studentDetails_Api.Extensions;
using System.Reflection;
using System.Text;
using studentDetails_Api.Utilities;
using studentDetails_Api.Models;
using studentDetails_Api.Services;
using studentDetails_Api.Middleware;
using Microsoft.AspNetCore.Mvc;
using studentDetails_Api.IRepository;
using studentDetails_Api.NonEntity;
using studentDetails_Api.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext for database connection
builder.Services.AddDbContext<StudentDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentCS") ??
    throw new InvalidOperationException("Connection string 'StudentCS' not found.")));

// Add CORS policy (allow all for now, restrict in production)
// It is recommended to restrict CORS to specific origins in production for security purposes
builder.Services.AddCors(options => options.AddPolicy(name: "ApiCorsPolicy",
    policy =>
    {
        policy.WithOrigins("*") // Use the correct frontend URL
              .AllowAnyMethod()
              .AllowAnyHeader();
    }));

builder.Services.AddHttpContextAccessor();

// Add controllers and specify JSON serializer options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // This is to keep camelCase naming
        options.JsonSerializerOptions.DictionaryKeyPolicy = null; // This ensures dictionary keys are preserved as is
    });

// Add services to the container
builder.Services.AddTransient<JwtServices>();
builder.Services.AddTransient<CryptoServices>();
builder.Services.APIServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
SwaggerControllerOrder<ControllerBase> swaggerControllerOrder = new SwaggerControllerOrder<ControllerBase>(Assembly.GetEntryAssembly()!);
builder.Services.AddSwaggerGen(option =>
{
    option.OrderActionsBy((apiDesc) => $"{swaggerControllerOrder.SortKey(apiDesc.ActionDescriptor.RouteValues["controller"]!)}");
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "studentDetails_Api", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
    });
});

// Get keys for JWT signing and encryption
var signingKey = Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!);
var encKey = Encoding.UTF8.GetBytes(builder.Configuration["JWT:EncryptionKey"]!);
var symmetricSigningKey = new SymmetricSecurityKey(signingKey);
var symmetricEncKey = new SymmetricSecurityKey(encKey);

// Add authentication schema
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = symmetricSigningKey,
                TokenDecryptionKey = symmetricEncKey,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

var app = builder.Build();

// Swagger UI and Docs setup
// In a production environment, you can disable Swagger UI for security reasons
app.UseSwagger();
app.UseSwaggerUI();

// JWT middleware for checking authentication
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseRouting();

app.UseCors("ApiCorsPolicy"); // Use the named policy

app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
