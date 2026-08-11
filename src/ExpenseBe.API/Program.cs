using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Options;
using ExpenseBe.Core.Services;
using ExpenseBe.Data.Context;
using ExpenseBe.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS (AllowAnyOrigin for dev)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
    policy =>
    {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add MongoDB Context
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.Configure<GeminiOptions>(options =>
{
    builder.Configuration.GetSection(GeminiOptions.SectionName).Bind(options);

    // Render / host secrets: prefer Gemini__ApiKey, fallback GEMINI_API_KEY
    if (string.IsNullOrWhiteSpace(options.ApiKey))
        options.ApiKey = builder.Configuration["GEMINI_API_KEY"] ?? string.Empty;

    // Optional override only — keep Model from appsettings (Gemini:Model) by default
    var modelOverride = builder.Configuration["GEMINI_MODEL"];
    if (!string.IsNullOrWhiteSpace(modelOverride))
        options.Model = modelOverride;
});

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<WordRepository>();

// Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<WordService>();
builder.Services.AddHttpClient<IAiWordGenerator, GeminiWordGenerator>((sp, client) =>
{
    var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<GeminiOptions>>().Value;
    var baseUrl = string.IsNullOrWhiteSpace(options.BaseUrl)
        ? "https://generativelanguage.googleapis.com/v1beta/"
        : options.BaseUrl;
    if (!baseUrl.EndsWith('/'))
        baseUrl += "/";
    client.BaseAddress = new Uri(baseUrl);
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

// Enable CORS
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();