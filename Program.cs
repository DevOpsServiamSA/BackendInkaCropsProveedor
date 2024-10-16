using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProveedorApi;
using ProveedorApi.Data;
using ProveedorApi.Models;
using ProveedorApi.Services;

var builder = WebApplication.CreateBuilder(args);

string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
string[] cors = builder.Configuration["Cors"].Split(";");

/*Se recupera la información del AppConfig*/

/*Configuracion*/
AppConfig.Configuracion.Website = builder.Configuration["appConfig:Configuracion:Website"];
AppConfig.Configuracion.CarpetaArchivos = builder.Configuration["appConfig:Configuracion:CarpetaArchivos"];
AppConfig.Configuracion.CarpetaArchivosBCTS = builder.Configuration["appConfig:Configuracion:CarpetaArchivosBCTS"];
AppConfig.Configuracion.DestinoRobotMail = builder.Configuration["appConfig:Configuracion:DestinoRobotMail"];
AppConfig.Configuracion.DestinoCompraMail = builder.Configuration["appConfig:Configuracion:DestinoCompraMail"];
AppConfig.Configuracion.EnableSSLMail = Convert.ToBoolean(builder.Configuration["appConfig:Configuracion:EnableSSLMail"]);
AppConfig.Configuracion.PasswordMail = builder.Configuration["appConfig:Configuracion:PasswordMail"];
AppConfig.Configuracion.PuertoMail = Convert.ToInt32(builder.Configuration["appConfig:Configuracion:PuertoMail"]);
AppConfig.Configuracion.ServidorMail = builder.Configuration["appConfig:Configuracion:ServidorMail"];
AppConfig.Configuracion.UserMail = builder.Configuration["appConfig:Configuracion:UserMail"];

/*Mensajes*/
AppConfig.Mensajes.AsuntoSolicitudAcceso = builder.Configuration["appConfig:Mensajes:AsuntoSolicitudAcceso"];
AppConfig.Mensajes.MensajeSolicitudAcceso = builder.Configuration["appConfig:Mensajes:MensajeSolicitudAcceso"];
AppConfig.Mensajes.AsuntoBloqueoPorDeuda = builder.Configuration["appConfig:Mensajes:AsuntoBloqueoPorDeuda"];
AppConfig.Mensajes.MensajeBloqueoPorDeuda = builder.Configuration["appConfig:Mensajes:MensajeBloqueoPorDeuda"];
AppConfig.Mensajes.AsuntoDesbloqueoPorDeuda = builder.Configuration["appConfig:Mensajes:AsuntoDesbloqueoPorDeuda"];
AppConfig.Mensajes.MensajeDesbloqueoPorDeuda = builder.Configuration["appConfig:Mensajes:MensajeDesbloqueoPorDeuda"];
AppConfig.Mensajes.AsuntoRechazoRequisitos = builder.Configuration["appConfig:Mensajes:AsuntoRechazoRequisitos"];
AppConfig.Mensajes.AsuntoReseteoClave = builder.Configuration["appConfig:Mensajes:AsuntoReseteoClave"];
AppConfig.Mensajes.MensajeReseteoClave = builder.Configuration["appConfig:Mensajes:MensajeReseteoClave"];
AppConfig.Mensajes.AsuntoRechazoSustento = builder.Configuration["appConfig:Mensajes:AsuntoRechazoSustento"];
AppConfig.Mensajes.MensajeRechazoRequisitos = builder.Configuration["appConfig:Mensajes:MensajeRechazoRequisitos"];
AppConfig.Mensajes.MensajeRechazoSustento = builder.Configuration["appConfig:Mensajes:MensajeRechazoSustento"];
AppConfig.Mensajes.MensajeRechazoRetencion = builder.Configuration["appConfig:Mensajes:MensajeRechazoRetencion"];
AppConfig.Mensajes.MensajeRechazoDetraccion = builder.Configuration["appConfig:Mensajes:MensajeRechazoDetraccion"];
AppConfig.Mensajes.MensajeSolicitudAccesoAdmin = builder.Configuration["appConfig:Mensajes:MensajeSolicitudAccesoAdmin"];
AppConfig.Mensajes.AsuntoSolicitudAccesoAdmin = builder.Configuration["appConfig:Mensajes:AsuntoSolicitudAccesoAdmin"];

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
                    builder => builder
                    .WithOrigins(cors)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((x) => true)
                    .AllowCredentials()
                );
});

builder.Services.AddDbContext<ProveedorContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Proveedor")));
builder.Services.AddDbContext<TransporteContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Transporte")));
builder.Services.AddDbContext<ExactusExtContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ExactusExt")));
builder.Services.AddSingleton<RecaptchaService>(sp 
    => new RecaptchaService(sp.GetRequiredService<IConfiguration>().GetValue<string>("AppConfig:Recaptcha:Secret"), 
                                 sp.GetRequiredService<IConfiguration>().GetValue<string>("AppConfig:Recaptcha:Url")));
builder.Services.AddSingleton<ProveedorBCTSService>(sp =>
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        var webServiceBCTSConfig = configuration.GetSection("AppConfig:WebServiceBCTS").Get<WebServiceBCTSConfig>();
        return new ProveedorBCTSService(webServiceBCTSConfig);
    }
);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt.Issuer"],
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(myAllowSpecificOrigins);

if (app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders();

app.UseDefaultFiles();
app.UseStaticFiles();

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
