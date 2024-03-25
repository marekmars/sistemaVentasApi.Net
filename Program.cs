using System.Text;
using Microsoft.EntityFrameworkCore;
// using Web_Service_.Net_Core.Models;
using Microsoft.Extensions.FileProviders;
using Web_Service_.Net_Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.Common;



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

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema JWT como predeterminado
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema JWT como predeterminado
// }).AddJwtBearer(options => // web api valida con token
// {
//     options.RequireHttpsMetadata = false;
//     options.SaveToken = true;
//     options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//     {
//         ValidateIssuer = false,
//         ValidateAudience = false,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Secret"]))
//     };
//     options.Events = new JwtBearerEvents
//     {
//         OnMessageReceived = context =>
//         {
//             var accessToken = context.Request.Query["access_token"];
//             var path = context.HttpContext.Request.Path;
//             if (!string.IsNullOrEmpty(accessToken))
//             {
//                 context.Token = accessToken;
//             }
//             return Task.CompletedTask;
//         }
//     };
// });
var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSetting>(appSettingsSection);
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
         .AddPolicy("Seller", policy => policy.RequireRole("Seller", "Admin"))
         .AddPolicy("Admin", policy => policy.RequireRole("Admin"));


// builder.Services.AddDbContext<DataContext>(
//     options => options.UseSqlServer(
//         configuration["AppSettings:ConnectionStrings:Some"]
//     )
// );


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DataContext>(
        options => options.UseMySql(
            configuration["AppSettings:ConnectionStrings:MySql"],
           ServerVersion.AutoDetect(configuration["AppSettings:ConnectionStrings:MySql"])
        )
    );
}
else
{
    builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(
        configuration["AppSettings:ConnectionStrings:Some"]
    )
);

}



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{

}
app.UseCors(myCorsPolicy);
app.UseHttpsRedirection();

//autentificacion para el jwt
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
