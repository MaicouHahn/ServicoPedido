namespace ServicoPedido.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string CodPedido { get; set; }
        public int IdFornecedor { get; set; }
        public string DataPedido { get; set; }
        public int IdItem { get; set; }

    }
}
