using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserManagement.Application;
using UserManagement.Application.Contracts.Auth;
using UserManagement.Application.Interfaces;
using UserManagement.Application.UseCases.Role;
using UserManagement.Application.UseCases.User;
using UserManagement.Infrastructure;
using UserManagement.Infrastructure.Persistence;
using UserManagement.Infrastructure.Services;
using UserManagement.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection")!);


builder.Services.AddScoped<PermissionFilter>();

var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);

var key = Encoding.UTF8.GetBytes(jwtSection.Get<JwtSettings>()!.SecretKey);

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
            ValidIssuer = jwtSection.Get<JwtSettings>()!.Issuer,

            ValidateAudience = false,
            ValidAudience = jwtSection.Get<JwtSettings>()!.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT FAILED");
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                Console.WriteLine("TOKEN RECIVED: " + context.Token);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("TOKEN VALIDATED SUCCESSFULLY");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

//builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
builder.Services.ApplicationLayerInjection();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
//builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPermissionSyncService, PermissionSyncService>();

builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<DisableUserUseCase>();
builder.Services.AddScoped<AssignRoleToUserUseCase>();
builder.Services.AddScoped<GetUsersUseCase>();

builder.Services.AddScoped<CreateRoleUseCase>();
builder.Services.AddScoped<DeleteRoleUseCase>();
builder.Services.AddScoped<GetRoleUseCase>();
builder.Services.AddScoped<GetAllRolesUseCase>();


var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });

    c.AddPolicy("ProductionClient", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? [])
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


builder.Services.AddControllers(options =>
{
    options.Filters.Add<PermissionFilter>();
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "ضعف Bearer را وارد کن. مثال:\nBearer eyJhbGciOiJIUzI1NiIs...",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    var sync = scope.ServiceProvider.GetRequiredService<IPermissionSyncService>();
    await sync.SyncPermissionAsync();
}


app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
    app.UseCors("AllowAngularClient");
else
    app.UseCors("ProductionClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();