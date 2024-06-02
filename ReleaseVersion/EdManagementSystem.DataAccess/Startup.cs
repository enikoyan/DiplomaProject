using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EdManagementSystem.DataAccess
{
    public class Startup
    {
        public Startup()
        {
            var services = new ServiceCollection();

            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISquadService, SquadService>();
            services.AddScoped<ISquadStudentService, SquadStudentService>();
        }
    }
}
