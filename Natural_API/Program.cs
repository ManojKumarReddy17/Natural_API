
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
using Natural_Data.Models;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.Util;
using Amazon;
using Natural_Core.S3Models;
using Natural_API.Resources;
#nullable disable


var builder = WebApplication.CreateBuilder(args);

var CorsPolicy = "CorsPolicy";
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

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

//configure AWS s3 client
var awssettings = configuration.GetSection(key: "AWS");
var cradentials = new BasicAWSCredentials(awssettings[key: "AccessKey"], awssettings[key: "SecretKey"]);

//configure AWS Options
var awsoptions = configuration.GetAWSOptions();
awsoptions.Credentials = cradentials;
awsoptions.Region = RegionEndpoint.APSouth1;

builder.Services.AddDefaultAWSOptions(awsoptions);
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.Configure<S3Config>(builder.Configuration.GetSection("S3Config"));

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ILoginRepository, LoginRepository>();
builder.Services.AddTransient<ILoginService, LoginService>();

builder.Services.AddTransient<IDistributorRepository, DistributorRepository>();
builder.Services.AddTransient<IDistributorService, DistributorService>();


builder.Services.AddTransient<IRetailorRepository, RetailorRepository>();
builder.Services.AddTransient<IRetailorService, RetailorService>();

builder.Services.AddTransient<IExecutiveRepository,ExecutiveRepository>();
builder.Services.AddTransient<IExecutiveService,ExecutiveService>();
builder.Services.AddTransient<IExecutiveAreaRepository, ExecutiveAreaRepository>();

builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<ICityService, CityService>();

builder.Services.AddTransient<IStateRepository, StateRepository>();
builder.Services.AddTransient<IStateService, StateService>();

builder.Services.AddTransient<IAreaRepository, AreaRepository>();
builder.Services.AddTransient<IAreaService, AreaService>();

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddTransient<IDsrRepository,DsrRepository>(); 
builder.Services.AddTransient<IDsrService, DsrService>();

builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddTransient<IAssignDistributorToExecutiveRepository, AssignDistributorToExecutiveRepository>();
builder.Services.AddTransient<IAssignDistributorToExecutiveService, AssignDistributorToExecutiveService>();


builder.Services.AddTransient<IAssignRetailorToDistributorService, AssignRetailorToDistributorService>();
builder.Services.AddTransient<IAssignRetailorToDistributorRepository, AssignRetailorToDistributorRepository>();


builder.Services.AddTransient<IDsrRepository, DsrRepository>();
builder.Services.AddTransient<IDsrService,DsrService>();

builder.Services.AddTransient<IDsrdetailRepository, DsrdetailRepository>();
builder.Services.AddTransient<IDsrdetailService, DsrdetailService>();


builder.Services.AddTransient<IDistributorSalesRepository, DistributorSalesRepository>();
builder.Services.AddTransient<IDistributorSalesService, DistributorSalesService>();



builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddTransient<INotificationExecutiveRepository, NotificationExecutiveRepository>();
builder.Services.AddTransient<INotificationDistributorRepository, NotificationDistributorRepository>();
builder.Services.AddTransient<INotificationService, NotificationService>();

builder.Services.AddTransient<IExecutiveGpsRepository, ExecutiveGpsRepository>();
builder.Services.AddTransient<IExecutiveGpsService, ExecutiveGpsService>();
builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection("Pagination"));

builder.Services.AddEndpointsApiExplorer();


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

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().
     AllowAnyHeader());
});

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
//app.Urls.Add("http://10.0.0.4:5024");
//app.Urls.Add("http://localhost:5024");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowOrigin");
app.UseCors(CorsPolicy);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
