using Server.Services;
using Microsoft.EntityFrameworkCore;
using Server.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

//Add Database service 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<EmailAuthentication>();

builder.Services.AddScoped<PasswordHashService>();

builder.Services.AddScoped<ExpenseServices>();

builder.Services.AddScoped<JwtToken>();

builder.Services.AddScoped<SplitService>();

builder.Services.AddAuthentication(options =>{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>{

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
       ValidateIssuer = true,
       ValidateAudience = true,
       ValidateLifetime = true,
       ValidateIssuerSigningKey = true,
       ValidIssuer = builder.Configuration["Jwt:Issuer"],
       ValidAudience = builder.Configuration["Jwt:Audience"],
       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Enable routing and map controllers
app.MapControllers();

app.Run();
