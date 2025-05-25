using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.Data;
using ReStudyAPI.Data;
using ReStudyAPI.Interfaces.Operation;
using ReStudyAPI.Services.Operation;

namespace ReStudyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            var app = builder.Build();
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