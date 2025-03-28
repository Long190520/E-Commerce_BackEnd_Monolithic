using E_Commerce_BackEnd.Models;
using E_Commerce_BackEnd.Startup;
using E_Commerce_BackEnd.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Memory cache
builder.Services.AddMemoryCache();

// Configure JSON serialization
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Configure CORS
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

// Configure DbContext
builder.Services.AddDbContext<MyDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ECommerce"));
});
builder.Services.AddIdentity<Users, IdentityRole<Guid>>().AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();

// Register app services
builder.Services.RegisterAppService();

// Configure JWT
var tokenOptions = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:ValidAudience"],
    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? "")),
    ValidateIssuerSigningKey = true,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddSingleton(tokenOptions);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = true;
    option.TokenValidationParameters = tokenOptions;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
