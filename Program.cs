
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KerryCoAdmin.Api.Interfaces;
using KerryCoAdmin.Api.Modules;
using KerryCoAdmin.Api.Repositories;
using KerryCoAdmin.Configurations;
using KerryCoAdmin.Data;
using KerryCoAdmin.Interfaces;
using KerryCoAdmin.Modules.Tokens;
using KerryCoAdmin.Repositories;
using KerryCoAdmint.Modules.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(builder.Configuration["VaultKey"]);
//var clientId = _configuration["Azure:ClientId"];
//var clientSecret = _configuration["Azure:ClientSecret"];
//var tenantId = _configuration["Azure:TenantId"];

//var secretClient = new SecretClient(new Uri(keyVaultEndpoint), new ClientSecretCredential(tenantId, clientId, clientSecret));
var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());

KeyVaultSecret db = secretClient.GetSecret("Kerryco-SqlDb-dev-001");
KeyVaultSecret jwt = secretClient.GetSecret("Jwt-secret-dev-001");





// for Entity Framework
//builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Kerryco-SqlDb-dev-001")));

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(db.Value));



// for Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

});


//for jwt

//builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

//var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);

var key = Encoding.ASCII.GetBytes(jwt.Value);


var tokenValidationParameter = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false, // for dev, change in production
    ValidateAudience = false, // for dev, change in production
    RequireExpirationTime = false,// for dev -- needs to be updated when refresh token is called.
    ValidateLifetime = true,

};

//token lifeSpan
builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(5));



// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParameter;
});

builder.Services.AddSingleton(tokenValidationParameter);
builder.Services.AddSingleton<PhotoService>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISecretService, SecretService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();




//register automapper
builder.Services.AddAutoMapper(typeof(Program));

// Enable cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://clothing-client.vercel.app/")
        .WithMethods("GET", "POST", "DELETE", "PUT")
        .AllowAnyHeader()
        .AllowCredentials();
    });

});


// JSON Serializer
builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
    = new DefaultContractResolver()
);




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Api", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});






var app = builder.Build();

/*

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

*/

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
