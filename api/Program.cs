using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using api.Data;
using api.DataStructureClasses;
using api.Interfaces;
using api.Models;
using api.Repositories;
using api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey= new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        ),
        NameClaimType = JwtRegisteredClaimNames.Name
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();

builder.Services.AddScoped<SmtpClient>(sp => new SmtpClient(emailSettings.SmtpServer)
{
    Port = emailSettings.SmtpPort, 
    Credentials = new NetworkCredential(emailSettings.EmailUsername, emailSettings.EmailPassword), 
    EnableSsl = emailSettings.EnableSsl, 
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IVerhuurVerzoekService, VerhuurVerzoekRepo>();
builder.Services.AddScoped<IWagenparkService, WagenparkRepo>();
builder.Services.AddScoped<IWagenParkUserListService, WagenParkUserListRepo>();
builder.Services.AddScoped<IVoertuigService, VoertuigServiceRepo>();
builder.Services.AddScoped<IRoleService, RoleRepo>();
builder.Services.AddScoped<IDoubleDataCheckerRepo, DoubleDataCheckerRepo>();
builder.Services.AddScoped<IReserveringService, ReserveringRepo>();
builder.Services.AddScoped<IAbonnementService, AbonnementServiceRepo>();
builder.Services.AddScoped<IEmailService, EmailRepo>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleInitializer.InitializeRolesAsync(roleManager);

    var voertuigService = scope.ServiceProvider.GetRequiredService<IVoertuigService>();
    await VoertuigInitializer.InitializeVoertuigenAsync(voertuigService);
    await VoertuigInitializer.InitializeVoertuigStatusAsync(voertuigService);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
