using System.Text;
using Microsoft.EntityFrameworkCore;
// using Web_Service_.Net_Core.Models;
using Microsoft.Extensions.FileProviders;
using Web_Service_.Net_Core.Models.Common;
using Web_Service_.Net_Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Web_Service_.Net_Core.Models;



var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var myCorsPolicy = "_myCorsPolicy";


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myCorsPolicy, builder =>
    {
        builder.WithOrigins("*");
        builder.WithHeaders("*");
        builder.WithMethods("*");
    });
});

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IConceptoService, ConceptoService>();
builder.Services.AddScoped<IRolService, RolService>();

var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSetting>(appSettingsSection);

//configuracion JWT
var appSettings = appSettingsSection.Get<AppSetting>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema JWT como predeterminado
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema JWT como predeterminado
}).AddJwtBearer(options => // web api valida con token
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});


//FIN



builder.Services.AddAuthorizationBuilder()
         .AddPolicy("Empleado", policy => policy.RequireRole("Empleado", "Administrador"))
         .AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));

builder.Services.AddDbContext<DBContext>(
    options => options.UseMySql(
        configuration["AppSettings:ConnectionStrings:MySql"]
        , Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb")
    )
);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(myCorsPolicy);
app.UseHttpsRedirection();

//autentificacion para el jwt
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
