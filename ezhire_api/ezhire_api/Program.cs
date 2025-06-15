using ezhire_api.DAL;
using ezhire_api.Repositories;
using ezhire_api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowCORS",
        policy  =>
        {
            policy.WithOrigins("http://localhost:3000");
        });
});

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<EzHireContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Candidates
builder.Services.AddScoped<ICandidatesService, CandidatesService>();
builder.Services.AddScoped<ICandidateRepository, CandidatesRepository>();

// Campaigns
builder.Services.AddScoped<IRecruitmentCampaignsRepository, RecruitmentCampaignsRepository>();
builder.Services.AddScoped<IRecruitmentCampaignsService, RecruitmentCampaignsService>();

// Postings
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowCORS");

app.UseAuthorization();

app.MapControllers();

app.Run();