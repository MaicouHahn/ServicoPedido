using ServicoPedido.Repositorio.Infra;

namespace ServicoPedido.Services.Infra
{
    public static class ServiceExtensionDomain
    {
        public static void ConfigurarDominio(this IServiceCollection services,
                                               IConfiguration configuration)
        {
            services.ConfigurarRepositorio(configuration);
        }
    }
}
