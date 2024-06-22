using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Royal.API.Middleware.JWT;
using Royal.Service.ProductService;
using Royal.Service.Security;
using Royal.Service.UserService;
using System.Net.Http.Headers;
using System.Text;

namespace Royal.API.Middleware;

public sealed class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddLogging();

        // Configure JWT Authentication
        var jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
        //services.AddAutoMapper(typeof(Startup).Assembly);

        var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });


        // Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISecurityService, SecurityService>();

        // Repositories


        //services.AddDbContext<DbContext>(options =>
        //options.UseSqlServer(
        //.GetConnectionString("DefaultConnection")));

        services.AddHttpClient("DummyApiClient", client =>
        {
            client.BaseAddress = new Uri("https://dummyjson.com/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        services.AddEndpointsApiExplorer();

        // Swagger
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Royal API Middleware", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // Enable middleware to serve generated Swagger as a JSON endpoint
        app.UseSwagger();

        // Enable middleware to serve Swagger UI
        app.UseSwaggerUI(option =>
        {
            option.SwaggerEndpoint("/swagger/v1/swagger.json", "Royal API Middleware V1");
            option.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}