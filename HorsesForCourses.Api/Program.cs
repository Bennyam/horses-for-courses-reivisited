using HorsesForCourses.Application.Coaches;
using HorsesForCourses.Application.Courses;
using HorsesForCourses.Infrastructure.Courses;
using HorsesForCourses.Infrastructure.Coaches;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
builder.Services.AddSingleton<ICoachRepository, InMemoryCoachRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICoachService, CoachService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
