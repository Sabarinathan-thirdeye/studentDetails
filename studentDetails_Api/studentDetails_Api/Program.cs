using Microsoft.EntityFrameworkCore;
using studentDetails_Api.IRepository;
//using studentDetails_Api.Middleware;
using studentDetails_Api.Models;
using studentDetails_Api.Repository;
//using studentDetails_Api.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudentDBContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("StudentCS") ??
   throw new InvalidOperationException("Connection string 'StudentCS' not found.")));
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Allow your frontend origin
                   .AllowAnyHeader()                     // Allow any headers
                   .AllowAnyMethod();                    // Allow any HTTP methods (GET, POST, etc.)
        });
});

// Add services to the container.
builder.Services.AddTransient<IStudentRepo, StudentRepo>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<JwtServices>(); // Add your JwtServices here
// Use the JWT middleware
//app.UseMiddleware<JwtMiddleware>();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
