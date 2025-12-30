using Rakna.Middlewares.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Services;
using Rakna.BAL.Helper;
using Rakna.BAL.Interface;
using Rakna.BAL.Interfaces;
using Rakna.BAL.Service;
using Rakna.DAL;
using Rakna.DAL.Models;
using Rakna.EF;
using Serilog;
using System;
using System.Text;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using SwaggerThemes;
using System.Reflection;
using System.Text.Json.Serialization;
using Rakna.BAL.DTO.OtpsDto;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Rakna API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    option.IncludeXmlComments(xmlPath);
    var balXmlFile = "Rakna.BAL.xml";
    var balXmlPath = Path.Combine(AppContext.BaseDirectory, balXmlFile);
    option.IncludeXmlComments(balXmlPath);
});
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
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
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().WriteTo.File(new JsonFormatter(), "Log/LoggingFile-.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefuletConnection")));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentityCore<Driver>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddIdentityCore<Employee>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<ITechnicalSupportService, TechnicalSupportService>();
builder.Services.AddSingleton<IDecodeJwt, DecodeJwt>();
builder.Services.AddScoped<IGarageAdminService, GarageAdminService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IGarageStaffService, GarageStaffService>();
builder.Services.AddScoped<ImailSenderService, MailService>();
builder.Services.AddScoped<IEmailSendService, EmailSendService>();
builder.Services.AddScoped<ISystemService, SystemService>();
builder.Services.AddHostedService<CleaningService>();

builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Mail"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Garage", policy =>
    {
        policy.RequireRole("driver", "technicalsupport");
    });
    options.AddPolicy("GetAllGrageAdmins", policy =>
    {
        policy.RequireRole("customerservice", "technicalsupport");
    });
    options.AddPolicy("ReportReader", policy =>
    {
        policy.RequireRole("customerservice", "technicalsupport", "garageadmin");
    });
    options.AddPolicy("ReportWriter", policy =>
    {
        policy.RequireRole("driver", "garagestaff", "garageadmin");
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStatusCodePages();
app.UseSwagger();
app.UseSwaggerThemes(Theme.UniversalDark);
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Correct place for UseCors

app.UseAuthentication();

app.UseAuthorization();
app.UseLoggingMiddleware();

app.MapControllers();

app.Run();