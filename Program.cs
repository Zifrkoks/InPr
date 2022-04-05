using InPr.Domain.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using InPr.Domain.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);
var builderConfig = new ConfigurationBuilder().AddJsonFile("config/AuthOptions.json");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
IConfiguration Appconfiguration = builderConfig.Build();
builder.Services.AddDbContext<NewsDbContext>();
builder.Services.AddDataRepositories();
builder.Services.AddDataServices();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddJwtBearer((options) =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = Appconfiguration["JwtToken:ISSUE"],
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = Appconfiguration["JwtToken:AUDIENCE"],
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Appconfiguration["JwtToken:KEY"])),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
            
         };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
