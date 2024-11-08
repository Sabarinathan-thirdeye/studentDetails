using Microsoft.EntityFrameworkCore;
using studentDetails_Api.IRepository;
//using studentDetails_Api.Middleware;
using studentDetails_Api.Models;
using studentDetails_Api.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using studentDetails_Api.Services;
//using studentDetails_Api.Middleware;

//using studentDetails_Api.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudentDBContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("StudentCS") ??
   throw new InvalidOperationException("Connection string 'StudentCS' not found.")));
// Add CORS policy
builder.Services.AddCors(options => options.AddPolicy(name: "AllowAll",
    policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }));

// Add services to the container.
builder.Services.AddTransient<IStudentRepo, StudentRepo>();
builder.Services.AddTransient<ILogInRepo, LogInRepo>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT settings
var jwtSettings = builder.Configuration.GetSection("JWT");
var secretKey = jwtSettings["SecretKey"];

// Set up JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtSettings["Issuer"],
//        ValidAudience = jwtSettings["Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
//        ClockSkew = TimeSpan.Zero // Optional: Reduces token expiration tolerance to 0
//    };
//});

// Add authorization
builder.Services.AddAuthorization();

// Register JWT and Crypto services
//builder.Services.AddScoped<JwtServices>();
//builder.Services.AddScoped<CryptoServices>();


var app = builder.Build();

// Use the JWT Middleware
//app.UseMiddleware<JwtMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS middleware
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
