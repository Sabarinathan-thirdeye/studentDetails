using studentDetails_Api.IRepository;
using studentDetails_Api.Repository;

namespace studentDetails_Api.Extensions
{
    public static class BuilderExtensions
    {
        public static IServiceCollection APIServices(this IServiceCollection services)
        {
            services.AddTransient<ILogInRepo, LogInRepo>();
            services.AddTransient<IStudentRepo, StudentRepo>();
            return services;
        }
    }
}
