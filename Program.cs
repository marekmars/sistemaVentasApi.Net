using Microsoft.EntityFrameworkCore;
// using Web_Service_.Net_Core.Models;
using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var myCorsPolicy = "_myCorsPolicy";


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options=>{
    options.AddPolicy(name:myCorsPolicy, builder=>{
        builder.WithOrigins("*");
        builder.WithHeaders("*");
        builder.WithMethods("*");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(myCorsPolicy);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
