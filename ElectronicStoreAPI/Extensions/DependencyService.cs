using API.Repository.Interfaces;
using API.Repository.Repositories;
using API.Service.Interfaces;
using API.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Reflection;
using API.BO.AutoMapperProfiles;
using API.Service.Interface;
namespace ElectronicStoreAPI.Extensions
{
    public static class DependencyServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            
            #region Service Scope
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUploadImageService,UploadImageService>();
            services.AddScoped<IImageModificationService,ImageModificationService>();
            services.AddScoped<IComboService, ComboService>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<ICategoryServices,CategoryService>();

            #endregion

            #region Repository Scope
            services.AddScoped<IComboRepository, ComboRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            #endregion

            #region Third-party Scope
            var jsonSecretKey = configuration.GetValue<string>("GoogleBucketServiceAccountKey");
            var googleCredential = GoogleCredential.FromJson(jsonSecretKey);
            var googleStorageClient = StorageClient.Create(googleCredential);
            services.AddSingleton(googleCredential);
            services.AddSingleton(googleStorageClient);
            services.AddAutoMapper(Assembly.GetAssembly(typeof(DefaultProfile)));
            
            #endregion

            return services;
        }

        public static IServiceCollection AddJwtValidation(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                };
            });
            return services;
        }

        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "ElectronicStore", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
                });
                options.EnableAnnotations();
            });
            return services;
        }
    }
}
