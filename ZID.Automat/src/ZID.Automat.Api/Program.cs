
#region usings
using Microsoft.AspNetCore.Http.Json;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using ZID.Automat.Application;
using ZID.Automat.Repository;
using ZID.Automat.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using ZID.Automat.DatabaseExtension;
using ZID.Automat.AutoMapper;
using ZID.Automat.Api.ExceptionFilters;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Hosting;
using System.Net.Sockets;
using ZID.Automat.Application.Admin;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Security.Policy;
#endregion

var builder = WebApplication.CreateBuilder(args);

var bui = builder.Configuration.AddEnvironmentVariables();

#region configure IP address

var urlsi = Environment.GetEnvironmentVariable("Urls")?.Split(";");
builder.WebHost.UseUrls(urlsi?.ToArray() ?? new string[] { "https://localhost:7141" });


#endregion

#region Conf Vars
var Conf = builder.Configuration;


var corsis = Environment.GetEnvironmentVariable("CorsOrigin")?.Split(";");
string[] CorsOrigins = corsis ?? Conf.GetSection("CorsOrigin").Get<string[]>();


string JWT = Environment.GetEnvironmentVariable("JWTSecret") ?? Conf.GetSection("UserLoginConf").GetSection("JWT").GetValue<string>("JWTSecret");

var DBSection = Conf.GetSection("Database");
var AuthConfSection = Conf.GetSection("UserLoginConf");
var JWTConfSection = AuthConfSection.GetSection("JWT");
var DebugConfSection = Conf.GetSection("Debug");
var TestUserConfSection = DebugConfSection.GetSection("UserAuth").GetSection("TestUser");


var je = Environment.GetEnvironmentVariable("JWTExpireTime");
int? jeint = null;
if (je != null)
{
    jeint = int.Parse(je);
}
float JWTExpireTime = jeint ?? JWTConfSection.GetValue<int>("JWTExpireTime");
string JWTSecret = Environment.GetEnvironmentVariable("JWTSecret") ?? JWTConfSection.GetValue<string>("JWTSecret");

string AutomatPassword = Environment.GetEnvironmentVariable("AutomatPassword") ?? AuthConfSection.GetValue<string>("AutomatPassword");

string AdminHall = Environment.GetEnvironmentVariable("AdminHall") ?? AuthConfSection.GetValue<string>("AdminHall");

bool UseDebug = DebugConfSection.GetValue<bool>("useDebug");
string TestUserName = TestUserConfSection.GetValue<string>("TestUserName");
string TestUserPassword = TestUserConfSection.GetValue<string>("TestUserPassword");

string UseDb = Environment.GetEnvironmentVariable("UseDatabase") ?? DBSection.GetValue<string>("UseDatabase");
string DbConnString = Environment.GetEnvironmentVariable("ConnectionString") ?? DBSection.GetSection(UseDb).GetValue<string>("ConnectionString");

var Borrow = Conf.GetSection("Borrow");

var bt = Environment.GetEnvironmentVariable("MaxBorrowTime");
int? btint = null;
if (bt != null)
{
    btint = int.Parse(bt);
}
int MaxBorrowTime = btint ?? Borrow.GetValue<int>("MaxBorrowTime");

var HttpsConf = Conf.GetSection("HttpsConf");
string CertPass = Environment.GetEnvironmentVariable("CertificatPassword") ?? HttpsConf.GetValue<string>("CertificatPassword");
string CertPath = Environment.GetEnvironmentVariable("CertificatPath") ?? HttpsConf.GetValue<string>("CertificatPath");

Console.WriteLine($"database {UseDb}");


#endregion

if (bool.Parse(Environment.GetEnvironmentVariable("UseSSL") ?? "false"))
{

    var httpsConnectionAdapterOptions = new HttpsConnectionAdapterOptions
    {
        SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
        ClientCertificateMode = ClientCertificateMode.AllowCertificate,
        ServerCertificate = new X509Certificate2(CertPath, CertPass)

    };
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ConfigureEndpointDefaults(list =>
        {
            list.UseHttps(httpsConnectionAdapterOptions);
        });
    });
}
#region ASPIntern
builder.Services.AddControllers((register) =>
{
    register.Filters.Add<ExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Configure Database
builder.Services.ConfigureDB(DbConnString, UseDb);

#endregion

#region StringEnums

builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
#endregion

#region configure Cors
builder.Services.AddCors(
    option => option.AddDefaultPolicy(
        builder => builder.WithOrigins(CorsOrigins).AllowAnyHeader().AllowAnyMethod()
    )
);
#endregion

#region jwt
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(auth =>
{
    auth.RequireHttpsMetadata = false;
    auth.SaveToken = true;
    auth.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWT)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
#endregion

#region Configuration
builder.Services.AddSingleton(new JWTCo() { JWTExpireTime = JWTExpireTime, JWTSecret = JWTSecret });
builder.Services.AddSingleton(new TestUserCo() { UseDebug = UseDebug, TestUserName = TestUserName, TestUserPassword = TestUserPassword });
builder.Services.AddSingleton(new BorrowCo() { MaxBorrowTime = MaxBorrowTime });
builder.Services.AddSingleton(new AutomatCo() { Password = AutomatPassword });
builder.Services.AddSingleton(new AdminCo() { Hall = AdminHall });
#endregion

#region
builder.Services.AddAutoMapper(c => {
    c.AddProfile<BorrowProfile>();
    c.AddProfile<ItemProfile>();
    c.AddProfile<UserProfile>();
    c.AddProfile<CategoryProfile>();
    c.AllowNullDestinationValues = true;
    c.AllowNullCollections = true;
});
#endregion

#region Repositories
builder.Services.AddScoped<IRepositoryRead, GenericRepository>();
builder.Services.AddScoped<IRepositoryWrite, GenericRepository>();

builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

#endregion

#region Services
builder.Services.AddScoped<ISeedService, SeedService>();

builder.Services.AddScoped<IUserAuthService, AuthentificationService>();
builder.Services.AddScoped<IControllerAuthService, AuthentificationService>();
builder.Services.AddScoped<IAdminAuthService, AuthentificationService>();

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();

builder.Services.AddScoped<IQrCodeCService, QrCodeService>();
builder.Services.AddScoped<IQrCodeUService, QrCodeService>();

builder.Services.AddScoped<IAutomatLoggingService, AutomatLoggingService>();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();

builder.Services.AddScoped<IAdminBorrowService, AdminBorrowService>();
builder.Services.AddScoped<IConfCategoriesService, ConfCategoriesService>();

builder.Services.AddScoped<IAdminItemService, AdminItemService>();

builder.Services.AddScoped<IAdminLogShowSerivice, AdminLogShowSerivice>();
#endregion


var app = builder.Build();

#region Development Config  
if (app.Environment.IsDevelopment() && true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

#region Exception Handling
app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()
        ?.Error;
    var response = new { error = exception?.Message ?? "Error" };
    await context.Response.WriteAsJsonAsync(response);
}));
#endregion

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.Run();
