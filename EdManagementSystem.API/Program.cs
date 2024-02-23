using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Infrastructure;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Services;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var builderConfig = builder.Configuration;

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            #region Authentication

            // Get DB connection and DBContext
            var connectionString = builderConfig.GetConnectionString("DefaultConnection");
            services.AddDbContext<User004Context>(dbContextOptions => dbContextOptions
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString) ??
            throw new InvalidOperationException("Connection string is not found!")),
            ServiceLifetime.Singleton);

            #endregion

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
