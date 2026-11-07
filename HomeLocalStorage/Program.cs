using HomeLocalStorage.Core.Abstractions;
using HomeLocalStorage.DataAccess;
using HomeLocalStorage.DataAccess.Managers;
using HomeLocalStorage.DataAccess.Repositories;
using HomeLocalStorage.Services;
using HomeLocalStorage.Services.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;


/*static IPAddress? GetLocalNetworkIP()
{
    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
    {
        if (ni.OperationalStatus != OperationalStatus.Up)
            continue;
        if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
            ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
            continue;

        var gateway = ni.GetIPProperties().GatewayAddresses
                          .FirstOrDefault(g => g.Address.AddressFamily == AddressFamily.InterNetwork);
        if (gateway == null)
            continue;

        var ip = ni.GetIPProperties().UnicastAddresses
                   .FirstOrDefault(a => a.Address.AddressFamily == AddressFamily.InterNetwork &&
                                        !IPAddress.IsLoopback(a.Address));
        if (ip != null)
            return ip.Address;
    }
    return null;
}*/



var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024; // 100 MB
});


builder.Services.AddControllers();
builder.Services.AddCors();


builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(
/*c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT токен в формате: Bearer {ваш_токен}"
    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
}*///);

builder.Services.AddDbContext<HomeLocalStorageDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(HomeLocalStorageDbContext)));
    }
    );

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<JwtService>();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

builder.Services.AddAuth(builder.Configuration);

builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddScoped<IFolderManager, FolderManager>();








var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>() ?? [];

app.UseCors(x =>
{
    x.WithOrigins(allowedOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/debug/cors", (IConfiguration config) =>
    config.GetSection("AllowedOrigins").Get<string[]>() ?? ["EMPTY"]);
app.Run();
