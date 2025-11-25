using AuthenticationSystemAPI.Services.Interfaces;
using EducationalSystem.Application.Features.User.Commands.AuthCommands.Register;
using EducationalSystem.Domain.Entities;
using EducationalSystem.Domain.Interfaces;
using EducationalSystem.Domain.Interfaces.EducationalSystem.Domain.Interfaces;
using EducationalSystem.Infrastructure.Models;
using EducationalSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Add enum converter
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // Handle circular references
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context
builder.Services.AddDbContext<EducationalSystemApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<EducationalSystemApiContext>()
    .AddDefaultTokenProviders();

// Repository Registration
builder.Services.AddScoped<IAuth, AuthRepo>();
builder.Services.AddScoped<IGroup, GroupRepo>();
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<IPayment, PaymentRepo>();
builder.Services.AddScoped<IAttendance, AttendanceRepo>();
builder.Services.AddScoped<IExpense, ExpenseRepo>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();
builder.Services.AddScoped<IStudentExamRepository, StudentExamRepository>();
builder.Services.AddScoped<IStudentAnswerRepository, StudentAnswerRepository>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

// JWT Configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtConfig"));

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json; charset=utf-8";

            var result = System.Text.Json.JsonSerializer.Serialize(
                new { success = false, message = "ليس مسموح لك بالوصول." },
                new System.Text.Json.JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                });

            return context.Response.WriteAsync(result);
        }
    };
});

// Authorization
builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Custom 403 Forbidden handler
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var result = System.Text.Json.JsonSerializer.Serialize(
            new
            {
                success = false,
                message = "ليس لديك صلاحية للوصول إلى هذا الجزء من النظام."
            },
            new System.Text.Json.JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            });

        await context.Response.WriteAsync(result);
    }
});

app.MapControllers();

app.Run();