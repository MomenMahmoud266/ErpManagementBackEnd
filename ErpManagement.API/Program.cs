using ErpManagement.API.Middlewares;
using ErpManagement.Domain.Options;
using ErpManagement.Domain.SwaggerFilter;
using ErpManagement.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ErpManagement.API.Utilities;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ErpManagement.API.Utilities.SignalRConfig;
using ErpManagement.Services.Services.WebSocket;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.SwaggerUI;
using ErpManagement.WebApi.Services;
using ErpManagement.DataAccess.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(Shared.CorsPolicy,
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});



builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<ErpManagementDbContext>();

builder.Services.AddDbContext<ErpManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(Shared.ErpManagementConnection),
    b => b.MigrationsAssembly(typeof(ErpManagementDbContext).Assembly.FullName)));

#region JWT config

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(Shared.JwtSettings));

var jwtSettings = new JwtSettings();
builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    //o.SecurityTokenValidators.Clear();
    //o.SecurityTokenValidators.Add(new CustomJwtSecurityTokenHandler());
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidIssuer = jwtSettings.Issuer,
        ClockSkew = TimeSpan.Zero
    };
    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query[Shared.AccessToken];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments(Shared.RealimeViewData))
            {
                // Read the token out of the query string
                context.Request.Headers.Authorization = accessToken;
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

#endregion

#region SignalR config

builder.Services.AddSignalR();

builder.Services.TryAddEnumerable(
    ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
        ConfigureJwtBearerOptions>());
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RequestClaims.DomainRestricted, policy =>
    {
        policy.Requirements.Add(new DomainRestrictedRequirement());
    });
});

#endregion


#region Localization config

builder.Services.AddLocalization(options => options.ResourcesPath = Shared.Resources);

var supportedCultures = Shared.Cultures;
var localizationOptions =
    new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[1])
.AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

builder.Services.AddControllers().AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(DataAnnotationValidation));
});

#endregion

#region Swagger config

//services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "dnndon API", Version = "v1" });
//    // add JWT Authentication
//    var securityScheme = new OpenApiSecurityScheme
//    {
//        Name = "JWT Authentication",
//        Description = "Enter JWT Bearer token **_only_**",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = "bearer", // must be lower case
//        BearerFormat = "JWT",
//        Reference = new OpenApiReference
//        {
//            Id = JwtBearerDefaults.AuthenticationScheme,
//            Type = ReferenceType.SecurityScheme
//        }
//    };
//    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                          {
//                             {securityScheme, new string[] { }}
//                          });
//});




builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc(Modules.Auth, new OpenApiInfo
    {
        Title = $"{Shared.ErpManagement} {Modules.Auth}",
        Version = Modules.V1
    });

    x.SwaggerDoc(Modules.Shared, new OpenApiInfo
    {
        Title = $"{Shared.ErpManagement} {Modules.Shared}",
        Version = Modules.V1
    });

    x.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Please insert JWT with Bearer token into field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = Modules.Bearer,
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id =  JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
  });
    x.SchemaFilter<SwaggerTest>();
});


#endregion 

#region DI

builder.Services.InjectedDependencies();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentTenant, HttpContextCurrentTenant>();

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();

# region To take an instance from specific repository

var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
using var scope = scopedFactory!.CreateScope();
ILoggingRepository loggingRepository = scope.ServiceProvider.GetService<ILoggingRepository>()!;

#endregion

#region Seed cliams

SeedData(app);

static async void SeedData(IHost app) //can be placed at the very bottom under app.Run()
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopedFactory!.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetService<IDbInitSeedsService>();
    await dbInitializer!.SeedClaimsForSuperAdmin();
}

#endregion


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsEnvironment(Shared.Local))
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint($"/swagger/{Modules.Auth}/swagger.json", "Auth_Management v1");
        x.SwaggerEndpoint($"/swagger/{Modules.Shared}/swagger.json", "Shared_Management v1");
        x.DocExpansion(DocExpansion.None);
        x.EnableValidator();
    });
}

//app.ConfigureExceptionHandler(loggingRepository);    // custom as a global exception
app.UseHttpsRedirection();
app.UseRouting();
app.ConfigureStaticFilesHandler();                   // custom as Static files
app.UseRequestLocalization(localizationOptions);
app.UseCors(Shared.CorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TenantSubscriptionMiddleware>();  // subscription gate — after auth
app.MapHub<BroadcastHub>(Shared.RealimeViewData);
app.UseExceptionHandler();
app.MapControllers();

app.Run();
