using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.DTO;
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
            services.AddOutputCache();

            #region Authentication

            // Get DB connection and DBContext
            var connectionString = builderConfig.GetConnectionString("DefaultConnection");
            services.AddDbContext<EdSystemDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                dbContextOptions.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, ServiceLifetime.Transient);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44354")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            #endregion

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISquadService, SquadService>();
            services.AddScoped<ISquadStudentService, SquadStudentService>();
            services.AddScoped<IProfileInfoService, ProfileInfoService>();
            services.AddScoped<ISocialMediaService, SocialMediaService>();
            services.AddScoped<ITechSupportService, TechSupportService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IFileManagementService, FileManagementService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IHomeworkService, HomeworkService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IEmailClient, EmailClient>();
            services.AddScoped<IRecoveryService, RecoveryService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowOrigin");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseOutputCache();

            app.MapControllers();

            app.Run();
        }
    }
}
