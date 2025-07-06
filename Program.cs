using AutoMapper;
using Hangfire;
using Hangfire.PostgreSql;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReStudyAPI.Data;
using ReStudyAPI.Interfaces.Jobs;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Interfaces.Repositories;
using ReStudyAPI.Interfaces.Security;
using ReStudyAPI.Models.Common;
using ReStudyAPI.Repositories;
using ReStudyAPI.Services.Jobs;
using ReStudyAPI.Services.Operation;
using ReStudyAPI.Services.Security;
using ReStudyAPI.Utility.Helpers;
using System.Net;
using System.Net.Mail;

namespace ReStudyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);
            //Configuration
            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.Configure<SSOConfiguration>(builder.Configuration.GetSection("SSOConfiguration"));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.Authority = builder.Configuration["SSOConfiguration:BaseUrl"];
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = builder.Configuration["SSOConfiguration:BaseUrl"],
                                    ValidAudience = builder.Configuration["SSOConfiguration:ClientId"],
                                    ClockSkew = TimeSpan.Zero
                                };
                            });


            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();
            //DB Connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var DBConnectionSettings = new List<CustomConnectionStringSettings> {
                new CustomConnectionStringSettings { Name = "DefaultConnection", ProviderName = ProviderName.PostgreSQL, ConnectionString = connectionString! }
            };
            DataConnection.DefaultSettings = new AppLinqToDBSettings(DBConnectionSettings.ToArray());
            builder.Services.AddLinqToDBContext<AppDBContext>((provider, options) =>
            {
                options.UsePostgreSQL(connectionString!);
                return options;
            });
            builder.Services.AddTransient<ICurrentSessionHelper, CurrentSessionHelper>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IConceptRepository, ConceptRepository>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IConceptService, ConceptService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
            builder.Services.AddScoped<IPopulateNotificationJob, PopulateNotificationJob>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddTransient((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var userName = config.GetValue<string>("EmailConfiguration:SenderEmailAddress");
                var password = config.GetValue<string>("EmailConfiguration:SenderPassword");
                return new SmtpClient()
                {
                    Host = config.GetValue<string>("EmailConfiguration:SMTPAddress")!,
                    Port = config.GetValue<int>("EmailConfiguration:SMTPPort"),
                    EnableSsl = config.GetValue<bool>("EmailConfiguration:EnableSsl"),
                    Credentials = new NetworkCredential(userName, password)
                };
            });
            builder.Services.AddTransient<IEmailUtility, EmailUtility>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    x => x
                        .WithOrigins(builder.Configuration.GetSection("Cors").GetSection("AllowedOrigins").Get<string[]>()!)
                        .AllowAnyMethod()
                        .WithExposedHeaders("Content-Disposition")
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            builder.Services.AddHangfire(config =>
            {
                config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))).WithJobExpirationTimeout(TimeSpan.FromMinutes(1));
            });

            builder.Services.AddHangfireServer();


            var app = builder.Build();
            app.UseCors(MyAllowSpecificOrigins);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate<IPopulateNotificationJob>(
                recurringJobId: "daily-populate-notifications",
                methodCall: job => job.RunAsync(),
                cronExpression: Cron.Daily(0, 30), // 12:30 AM
                options: new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")
                }
            );

            app.Run();
        }
    }
}