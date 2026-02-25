using movie_service_backend.Interfaces;
using movie_service_backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using movie_service_backend.Data;
using movie_service_backend.Mapping;
using movie_service_backend.Repo;
using movie_service_backend.Services;
using System.Text;


namespace movie_service_backend.Startup
{
    public static class AppServicesSetup
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // AutoMapper
            services.AddAutoMapper(typeof(UserProfile).Assembly);

            // Repository
            services.AddScoped<UserRepo>();
            services.AddScoped<FilmRepo>();
            services.AddScoped<SeriesRepo>();
            services.AddScoped<RatingRepo>();
            services.AddScoped<CommentRepo>();
            services.AddScoped<DebateRepo>();
            services.AddScoped<DebatePostLikeRepo>();
            services.AddScoped<WatchlistRepo>();

            // Service
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<ISeriesService, SeriesService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IDebateService, DebateService>();
            services.AddScoped<IWatchlistService, WatchlistService>();
            services.AddScoped<EmailService>();
            services.AddScoped<PasswordService>();

            

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
                        )
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Diplomski API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Unesite JWT token kao: Bearer {token}"
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
                        new string[] { }
                    }
                });
            });

            return services;

        }

        public static void ConfigureMiddleware(this IServiceCollection services)
        {
            services.AddCors();
            services.AddAuthentication();
            services.AddAuthorization();
        }

        public static void ConfigureMiddlewarePipeline(this WebApplication app)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
