using EdisonEngineering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.Common;
using EdisonEngineering.Infrastructure.Repositories;
using EdisonEngineering.Application.Services;
using EdisonEngineering.API.Middleware;
using EdisonEngineering.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Asp.Versioning;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using EdisonEngineering.API.Auth;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build())
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Errors = errors
            };

            return new BadRequestObjectResult(response);
        };
    });

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;

    options.ApiVersionReader =
        new Asp.Versioning.UrlSegmentApiVersionReader();
});

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ISolarCalculatorService, SolarCalculatorService>();
builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
builder.Services.AddScoped<ICityPricingRepository, CityPricingRepository>();
builder.Services.AddScoped<ISubsidyRepository, SubsidyRepository>();
builder.Services.AddScoped<ISlabRepository, SlabRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IRefreshTokenRepository,RefreshTokenRepository>();

// Add response compression
builder.Services.AddResponseCompression();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    // =====================================================
    // GLOBAL LIMITER
    // =====================================================

    options.GlobalLimiter =
        PartitionedRateLimiter.Create<
            HttpContext,
            string>(context =>
        {
            var ip =
                context.Connection
                    .RemoteIpAddress
                    ?.ToString()

                ?? "unknown";

            return RateLimitPartition
                .GetFixedWindowLimiter(
                    partitionKey: ip,

                    factory: _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,

                            Window =
                                TimeSpan.FromMinutes(1),

                            QueueProcessingOrder =
                                QueueProcessingOrder
                                    .OldestFirst,

                            QueueLimit = 10
                        });
        });

    options.AddPolicy(
        "login-policy",
        context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));

    // =====================================================
    // RESPONSE
    // =====================================================

    options.OnRejected =
        async (context, token) =>
        {
            context.HttpContext
                .Response.StatusCode = 429;

            context.HttpContext
                .Response.ContentType =
                    "application/json";

            await context.HttpContext
                .Response.WriteAsync(
                    """
                    {
                        "success": false,
                        "message":
                        "Too many requests. Please try again later."
                    }
                    """,
                    cancellationToken: token);
        };
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings =
            builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings["Key"]))
            };
    });

builder.Services.AddScoped<
    IAuthorizationHandler,
    PermissionHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "blog.create",
        policy =>
            policy.Requirements.Add(
                new PermissionRequirement(
                    "blog.create")));

    options.AddPolicy(
        "blog.edit",
        policy =>
            policy.Requirements.Add(
                new PermissionRequirement(
                    "blog.edit")));

    options.AddPolicy(
        "blog.delete",
        policy =>
            policy.Requirements.Add(
                new PermissionRequirement(
                    "blog.delete")));

    options.AddPolicy(
        "lead.view",
        policy =>
            policy.Requirements.Add(
                new PermissionRequirement(
                    "lead.view")));

    options.AddPolicy(
        "job.manage",
        policy =>
            policy.Requirements.Add(
                new PermissionRequirement(
                    "job.manage")));
});

// ✅ Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Edison Engineering API",
            Version = "v1"
        });

    // 🔐 JWT AUTH CONFIG

    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Enter JWT Token"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Enable response compression
app.UseResponseCompression();

// Enable CORS
app.UseCors("AllowFrontend");

// Enable static files
app.UseStaticFiles();

// Enable rate limiting
app.UseRateLimiter();

// ✅ Enable Swagger ONLY in Development
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<AuditMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();