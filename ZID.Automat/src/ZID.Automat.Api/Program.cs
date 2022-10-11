using Microsoft.AspNetCore.Authentication.Cookies;

using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);


string[] CorsOrigins = builder.Configuration.GetSection("CorsOrigin").Get<string[]>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(
         option => option.AddDefaultPolicy(
             builder => builder.WithOrigins(CorsOrigins).AllowAnyHeader().AllowAnyMethod()
             )
         ); ;

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
    {
        config.Cookie.Name = "UserLoginCookie";
    });



builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddCors(
    option => option.AddDefaultPolicy(
        builder => builder.WithOrigins(CorsOrigins).AllowAnyHeader().AllowAnyMethod()
    )
); 


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors();

    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();
}


app.UseCors();
app.UseHttpsRedirection();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();
app.Run();
