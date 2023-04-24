
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
#endregion

var builder = WebApplication.CreateBuilder(args);

#region configure IP address
var ipAddresses = NetworkInterface.GetAllNetworkInterfaces()
    .Where(i => i.OperationalStatus == OperationalStatus.Up)
    .ToList();
var urls = new List<string>();
ipAddresses.ForEach(i => {
    i.GetIPProperties().UnicastAddresses
    .Where(u => u.Address.AddressFamily == AddressFamily.InterNetwork)
    .ToList().ForEach(u =>
    {
        urls.Add($"http://{u.Address}:5141");
        urls.Add($"https://{u.Address}:7141");
    });
  });
builder.WebHost.UseUrls(urls.ToArray());
#endregion

#region Conf Vars
var Conf = builder.Configuration;

string[] CorsOrigins = Conf.GetSection("CorsOrigin").Get<string[]>();
string JWT = Conf.GetSection("UserLoginConf").GetSection("JWT").GetValue<string>("JWTSecret");

var DBSection = Conf.GetSection("Database");
var AuthConfSection = Conf.GetSection("AuthConf");
var JWTConfSection = AuthConfSection.GetSection("JWT");
var DebugConfSection = Conf.GetSection("Debug");
var TestUserConfSection = DebugConfSection.GetSection("UserAuth").GetSection("TestUser");

float JWTExpireTime = JWTConfSection.GetValue<int>("JWTExpireTime");
string JWTSecret = JWTConfSection.GetValue<string>("JWTSecret");

string AutomatPassword = AuthConfSection.GetValue<string>("AutomatPassword");

bool UseDebug = DebugConfSection.GetValue<bool>("useDebug");
string TestUserName = TestUserConfSection.GetValue<string>("TestUserName");
string TestUserPassword = TestUserConfSection.GetValue<string>("TestUserPassword");

string UseDb = DBSection.GetValue<string>("UseDatabase");
string DbConnString = DBSection.GetSection(UseDb).GetValue<string>("ConnectionString");

var Borrow = Conf.GetSection("Borrow");
int MaxBorrowTime = Borrow.GetValue<int>("MaxBorrowTime");

#endregion

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
builder.Services.AddSingleton(new TestUserCo() {UseDebug = UseDebug, TestUserName = TestUserName, TestUserPassword = TestUserPassword});
builder.Services.AddSingleton(new BorrowCo() { MaxBorrowTime = MaxBorrowTime });
builder.Services.AddSingleton(new AutomatCo() { Password = AutomatPassword });
#endregion

#region
builder.Services.AddAutoMapper(c => {
    c.AddProfile<BorrowProfile>();
    c.AddProfile<ItemProfile>();
});
#endregion


#region Repositories
builder.Services.AddScoped<IRepositoryRead,GenericRepository>();
builder.Services.AddScoped<IRepositoryWrite, GenericRepository>();

#endregion

#region Services
builder.Services.AddScoped<ISeedService, SeedService>();

builder.Services.AddScoped<IUserAuthService, AuthentificationService>();
builder.Services.AddScoped<IControllerAuthService, AuthentificationService>();

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();

builder.Services.AddScoped<IQrCodeCService, QrCodeService>();
builder.Services.AddScoped<IQrCodeUService, QrCodeService>();
#endregion


var app = builder.Build();

#region Development Config  
if (app.Environment.IsDevelopment())
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
    var response = new { error = exception?.Message ??"Error" };
    await context.Response.WriteAsJsonAsync(response);
}));
#endregion

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
    