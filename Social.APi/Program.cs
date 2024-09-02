using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Social.APi.Data;
using Social.APi.Dtos;
using Social.APi.Endpoints;
using Social.APi.Extensions;
using Social.APi.Helpers;
using Social.APi.Repository;
using Social.APi.Services;
using Social.APi.Validators;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();
builder.Services.AddDbContext<SocialApiContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("dbconnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFriendService,FriendService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddValidatorsFromAssemblyContaining<UserSignUpValidator>();

builder.Services.AddSingleton<TokenProvider>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secrets"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapUserEndpoints();

app.UseAuthentication();
app.UseAuthorization();


app.Run();
