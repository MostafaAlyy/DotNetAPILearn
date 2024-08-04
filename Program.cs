using System.Text;
using DotNetAPILearn.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepo, UserRepo>();


string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    tokenKeyString ?? ""
                )),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.MapUserEndPoints();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

