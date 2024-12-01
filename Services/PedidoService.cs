using ServicoPedido.Models;
using ServicoPedido.Repositorio;

namespace ServicoPedido.Services
{
    public class PedidoService
    {
        public PedidoRepositorio _pedidoRepositorio {  get; set; }
        public PedidoService() { 
            _pedidoRepositorio = new PedidoRepositorio();
        }
        public void InserirPedido(Pedido pedido)
        {

            _pedidoRepositorio.Inserir(pedido);
        }
        public Pedido FindById(int id)
        {
            return _pedidoRepositorio.FindById(id);

        }

        public List<Pedido> FindAll()
        {
            return _pedidoRepositorio.FindAll();
        }

        public bool DeleteById(int id)
        {
            var pedido = _pedidoRepositorio.FindById(id);
            if (pedido == null)
            {
                return false;
            }
            _pedidoRepositorio.DeleteById(pedido);
            return true;
        }

        public bool UpdateById(int id, Pedido pedido)
        {

            var pedidoDTO = _pedidoRepositorio.FindById(id);
            if (pedidoDTO == null)
            {
                return false;
            }

            pedidoDTO.IdPedido = id;
            pedidoDTO.IdFornecedor = pedido.IdFornecedor;
            pedidoDTO.CodPedido = pedido.CodPedido;
            pedidoDTO.DataPedido = pedido.DataPedido;
            pedidoDTO.IdItem = pedido.IdItem;

            _pedidoRepositorio.UpdateById(pedidoDTO);
            return true;
        }

    }
}
