
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
#endregion

var builder = WebApplication.CreateBuilder(args);

#region Conf Vars
var Conf = builder.Configuration;

string[] CorsOrigins = Conf.GetSection("CorsOrigin").Get<string[]>();
string JWT = Conf.GetSection("UserLoginConf").GetSection("JWT").GetValue<string>("JWTSecret");

var DBSection = Conf.GetSection("Database");
var JWTConf = Conf.GetSection("UserLoginConf").GetSection("JWT");
var DebugConf = Conf.GetSection("Debug");
var TestUserConf = DebugConf.GetSection("UserAuth").GetSection("TestUser");

float JWTExpireTime = JWTConf.GetValue<int>("JWTExpireTime");
string JWTSecret = JWTConf.GetValue<string>("JWTSecret");

bool UseDebug = DebugConf.GetValue<bool>("useDebug");
string TestUserName = TestUserConf.GetValue<string>("TestUserName");
string TestUserPassword = TestUserConf.GetValue<string>("TestUserPassword");

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
#endregion

#region Repositories
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
#endregion

#region Services
builder.Services.AddScoped<ISeedService, SeedService>();
builder.Services.AddScoped<IUserAuth, AuthentificationService>();
builder.Services.AddScoped<IItemService, ItemService>();
#endregion

var app = builder.Build();

#region Development Config  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
    