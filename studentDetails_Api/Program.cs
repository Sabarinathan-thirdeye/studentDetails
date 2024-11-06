using Microsoft.EntityFrameworkCore;
using UsersDetailsApi.IRepository;
//using UsersDetailsApi.Services;
using UsersDetailsApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersDBContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("UsersCS") ??
   throw new InvalidOperationException("Connection string 'UsersCS' not found.")));
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") // Allow your frontend origin
                   .AllowAnyHeader()                     // Allow any headers
                   .AllowAnyMethod();                    // Allow any HTTP methods (GET, POST, etc.)
        });
});

// Add services to the container.
//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor(); // Required for IHttpContextAccessor
                                           //builder.Services.AddScoped<JwtServices>(); // Register JwtServices

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS middleware
app.UseCors("AllowFrontendOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Use authentication and authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();