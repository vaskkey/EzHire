using ezhire_api.DAL;
using ezhire_api.Middlewares;
using ezhire_api.Models;
using ezhire_api.Repositories;
using ezhire_api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCORS",
        policy => { policy.WithOrigins("http://localhost:3000"); });
});

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<EzHireContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// AUTH
// In Program.cs or Startup.cs
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
// Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
// User settings
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<EzHireContext>()
    .AddDefaultTokenProviders();
// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// Candidates
builder.Services.AddScoped<ICandidatesService, CandidatesService>();
builder.Services.AddScoped<ICandidateRepository, CandidatesRepository>();

// Campaigns
builder.Services.AddScoped<IRecruitmentCampaignsRepository, RecruitmentCampaignsRepository>();
builder.Services.AddScoped<IRecruitmentCampaignsService, RecruitmentCampaignsService>();

// Postings
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
builder.Services.AddScoped<IJobPostingsService, JobPostingsService>();

// Job Applications
builder.Services.AddScoped<IJobApplicationsService, JobApplicationsService>();
builder.Services.AddScoped<IJobApplicationsRepository, JobApplicationsRepository>();

// Recruitment stages
builder.Services.AddScoped<IRecruitmentStagesService, RecruitmentStagesService>();
builder.Services.AddScoped<IRecruitmentStagesRepository, RecruitmentStagesRepository>();

// AUTH Services
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseCors("AllowCORS");

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();