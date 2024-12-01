using ServicoPedido.Models;
using ServicoPedido.Repositorio.Infra;

namespace ServicoPedido.Repositorio
{
    public class PedidoRepositorio
    {
        public DataContext _dataContext { get; set; }
        public PedidoRepositorio()
        {
            _dataContext = GeradorDeServicos.CarregarContexto();
        }
        public void Inserir(Pedido pedido)
        {
            _dataContext.Add(pedido);
            _dataContext.SaveChanges();
        }
        public Pedido FindById(int id)
        {
            return _dataContext.Find<Pedido>(id);
        }
        public List<Pedido> FindAll()
        {
            return _dataContext.Set<Pedido>().ToList();
        }
        public void DeleteById(Pedido pedido)
        {
            _dataContext.Remove(pedido);
            _dataContext.SaveChanges();
        }
        public void UpdateById(Pedido pedido)
        {
            _dataContext.Update(pedido);
            _dataContext.SaveChanges();
        }
    }
}
