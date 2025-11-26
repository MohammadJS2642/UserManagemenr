using UserManagement.Application;
using UserManagement.Application.Interfaces;
using UserManagement.Application.UseCases.RoleUseCase;
using UserManagement.Application.UseCases.User;
using UserManagement.Infrastructure;
using UserManagement.Infrastructure.Persistence;
using UserManagement.Infrastructure.Services;
using UserManagement.WebApi.Middleware;
using UserManagement.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddHttpContextAccessor();
builder.Services.ApplicationLayerInjection();

builder.Services.AddScoped<PermissionFilter>();
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
builder.Services.AddSwaggerGen();
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


app.UseAuthorization();

app.MapControllers();

app.Run();
