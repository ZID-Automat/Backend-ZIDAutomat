
#region usings
using Microsoft.AspNetCore.Http.Json;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ZID.Automat.Infrastructure;
using ZID.Automat.Application;
using ZID.Automat.Repository;
using ZID.Automat.Configuration.Model;
using System.Configuration;
using ZID.Automat.Configuration;
using Microsoft.AspNetCore.Diagnostics;
#endregion

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Configure Database
builder.Services.AddDbContext<AutomatContext>(options =>
{
    if (UseDb == "MySQL")
    {
        options.UseMySQL(DbConnString);
    }
    else if (UseDb == "SQLite")
    {
        options.UseSqlite(DbConnString);
    }
    else if (UseDb == "PLSQL")
    {
        options.UseOracle(DbConnString);
    }
    else
    {
        throw new Exception("No Database selected");
    }
});
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

#region Repositories
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ISaveDBRepository, SaveRepository>();

builder.Services.AddScoped<IActiveBorrowsRepository, BorrowRepository>();
builder.Services.AddScoped<IAlllBorrowsRepository, BorrowRepository>();
builder.Services.AddScoped<IControllerQrCodeRepository, BorrowRepository>();

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
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
    