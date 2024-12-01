using Microsoft.EntityFrameworkCore;

namespace ServicoPedido.Repositorio.Infra
{
    public static class ServicesExtensionRepository
    {
        public static void ConfigurarRepositorio(this IServiceCollection services,
                                                 IConfiguration configuration)
        {
            var enderecoBanco = configuration.GetConnectionString("Sqlite");
            services.AddDbContext<DataContext>(opt => opt.UseSqlite(enderecoBanco));

            GeradorDeServicos.serviceProvider = services.BuildServiceProvider();
        }
    }
}
