namespace ServicoPedido.Repositorio.Infra
{
    public class GeradorDeServicos
    {
        public static ServiceProvider serviceProvider;

        public static DataContext CarregarContexto()
        {
            return serviceProvider.GetService<DataContext>();
        }

    }
}
