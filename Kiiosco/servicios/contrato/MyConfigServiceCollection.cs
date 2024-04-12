using Kiosco.servicios.implementacion;

namespace Kiosco.servicios.contrato
{
    public static class MyConfigServiceCollection
    {
        public static IServiceCollection AddMyDependencyGroup(
        this IServiceCollection services)
        {
            services.AddTransient<Ilogin,  LoginService>();

            services.AddTransient<IHorarios, HorariosService>();



            return services;
        }
    }
}
