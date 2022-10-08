var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string[] CorsOrigins = builder.Configuration.GetSection("CorsOrigin").Get<string[]>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(
         option => option.AddDefaultPolicy(
             builder => builder.WithOrigins(CorsOrigins).AllowAnyHeader().AllowAnyMethod()
             )
         ); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
