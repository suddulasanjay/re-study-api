using AutoMapper;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.Data;
using ReStudyAPI.Data;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Interfaces.Repositories;
using ReStudyAPI.Interfaces.Security;
using ReStudyAPI.Repositories;
using ReStudyAPI.Services.Operation;
using ReStudyAPI.Services.Security;
using ReStudyAPI.Utility.Helpers;

namespace ReStudyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            //DB Connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var DBConnectionSettings = new List<CustomConnectionStringSettings> {
                new CustomConnectionStringSettings { Name = "DefaultConnection", ProviderName = ProviderName.SqlServer, ConnectionString = connectionString! }
            };
            DataConnection.DefaultSettings = new AppLinqToDBSettings(DBConnectionSettings.ToArray());
            builder.Services.AddLinqToDBContext<AppDBContext>((provider, options) =>
            {
                options.UseSqlServer(connectionString!);
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
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();

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

            var app = builder.Build();
            app.UseCors(MyAllowSpecificOrigins);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}