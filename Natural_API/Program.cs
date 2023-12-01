using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.IServices;
using Natural_Core;
using Natural_Data.Repositories;
using Natural_Data;
using Natural_Services;
using Natural_API;
using Microsoft.OpenApi.Models;
using Natural_Core.Models;
#nullable disable


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var databaseConfiguration = configuration.GetSection("Database").Get<DatabaseConfiguration>();
builder.Services.AddDbContext<NaturalsContext>(options =>
{
    options.UseMySql(databaseConfiguration.ConnectionString, new MySqlServerVersion(new Version()));
});

builder.Services.AddControllers();


builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ILoginRepository, LoginRepository>();
builder.Services.AddTransient<ILoginService, LoginService>();

builder.Services.AddTransient<IDistributorRepository, DistributorRepository>();
builder.Services.AddTransient<IDistributorService, DistributorService>();

builder.Services.AddTransient<IRetailorRepository, RetailorRepository>();
builder.Services.AddTransient<IRetailorService, RetailorService>();

builder.Services.AddTransient<IExecutiveRepository,ExecutiveRepository>();
builder.Services.AddTransient<IExecutiveService,ExecutiveService>();

builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<ICityService, CityService>();

builder.Services.AddTransient<IStateRepository, StateRepository>();
builder.Services.AddTransient<IStateService, StateService>();

builder.Services.AddTransient<IAreaRepository, AreaRepository>();
builder.Services.AddTransient<IAreaService, AreaService>();

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICategoryService, CategoryService>();




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Key Auth", Version = "v1" });
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "ApiKey",
        In = ParameterLocation.Header,
        Description = "PLEASE ENTER THE APIKEY",
        Scheme = "ApiKeyScheme"
    });
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
                        {
                             { key, new List<string>() }
                        };
    c.AddSecurityRequirement(requirement);
});


builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
