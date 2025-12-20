using Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers()
    .AddNewtonsoftJson();

// ✅ Swagger + Authorize Button
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Travel API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "اكتب: Bearer {token}\nمثال: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// ✅ CORS – افتحو كلياً مؤقتاً
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

// ✅ JWT Settings (من appsettings.json)
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
var issuer = builder.Configuration["JwtSettings:Issuer"];
var audience = builder.Configuration["JwtSettings:Audience"];

if (string.IsNullOrWhiteSpace(secretKey))
    throw new Exception("JwtSettings:SecretKey is missing in appsettings.json");

if (string.IsNullOrWhiteSpace(issuer))
    throw new Exception("JwtSettings:Issuer is missing in appsettings.json");

if (string.IsNullOrWhiteSpace(audience))
    throw new Exception("JwtSettings:Audience is missing in appsettings.json");

// ✅ Authentication + JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),

            // ✅ لأن التوكن عندك فيه claim اسمها "role"
            RoleClaimType = "role"
        };
    });

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Travel API v1");
});

// لا HTTPS redirection (متل ما كنت كاتب)
// app.UseHttpsRedirection();

app.UseCors("AllowAll");

// ✅ لازم قبل Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
