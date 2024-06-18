using Microsoft.OpenApi.Models;
using System.Text;

namespace Royal.API.Middleware;

public class Startup
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
        //var jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        //var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

        //services.AddAutoMapper(typeof(Startup).Assembly);

        //services.AddAuthentication(x =>
        //{
        //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //})
        //.AddJwtBearer(x =>
        //{
        //    x.RequireHttpsMetadata = false;
        //    x.SaveToken = true;
        //    x.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(key),
        //        ValidateIssuer = false,
        //        ValidateAudience = false
        //    };
        //});

        // Services

        // Repositories

        //services.AddDbContext<FilmForgeDbContext>(options =>
            //options.UseSqlServer(
                //.GetConnectionString("DefaultConnection")));

        services.AddEndpointsApiExplorer();

        // Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Solar Power Plant API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, FilmForgeDbContext context*/)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            // DONE: Init User
            // TODO: Implement data Seeding
            //DbInitializer.Initialize(context);
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // Enable middleware to serve generated Swagger as a JSON endpoint
        app.UseSwagger();

        // Enable middleware to serve Swagger UI
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Solar Power Plant API V1");
            c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}