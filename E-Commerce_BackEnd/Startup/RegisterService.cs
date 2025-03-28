using E_Commerce_BackEnd.DAL;
using E_Commerce_BackEnd.Repository;
using E_Commerce_BackEnd.Services;

namespace E_Commerce_BackEnd.Startup
{
    public static partial class ServiceCollectionExtensions
    {
        public static void RegisterAppService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            //others Svc
            services.AddScoped<UserServices>();
            //services.AddScoped<TaskServices>();
        }
    }
}
