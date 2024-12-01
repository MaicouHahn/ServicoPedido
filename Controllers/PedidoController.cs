using Microsoft.AspNetCore.Mvc;
using ServicoPedido.Models;
using ServicoPedido.Services;
using System.Net.Http;
using static System.Net.WebRequestMethods;

#region DTO
public class FornecedorDTO
{
    public string CpfCnpj { get; set; }
    public bool IsFisicaOuJuridica { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }

}


public class ItemDTO
{
    public int IdItem { get; set; }
    public string CodItem { get; set; }
    public string NomeItem { get; set; }
    public string DescricaoItem { get; set; }
    public decimal PrecoItem { get; set; }

}
#endregion

namespace ServicoPedido.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private  PedidoService _pedidoService;
        private  HttpClient _httpClient;
        private string apiItem = "https://localhost:7234/api/ItemControlador/";
        private string apiForncedor = "https://localhost:7217/api/Fornecedor/";
        // Construtor que injeta o serviço de pedido
        public PedidoController()
        {
            _pedidoService = new PedidoService();
            _httpClient = new HttpClient();
            
        }

        [HttpPost]
        public async Task<IActionResult> InserirAsync([FromBody] Pedido pedidoDTO)
        {
            try
            {
                var itemResponse = await _httpClient.GetAsync($"{apiItem}{pedidoDTO.IdItem}");

                if (!itemResponse.IsSuccessStatusCode)
                {
                    return BadRequest(new { Message = $"Item com ID {pedidoDTO.IdItem} não encontrado no sistema de itens." });
                }

                var itemResponseFornecedor = await _httpClient.GetAsync($"{apiForncedor}{pedidoDTO.IdFornecedor}");
                if (!itemResponseFornecedor.IsSuccessStatusCode)
                {
                    return BadRequest(new { Message = $"Fornecedor com ID {pedidoDTO.IdFornecedor} não encontrado no sistema de Fornecedores." });
                }

                var pedido = new Pedido()
                {
                    IdFornecedor = pedidoDTO.IdFornecedor,
                    CodPedido = pedidoDTO.CodPedido,
                    DataPedido = pedidoDTO.DataPedido,
                    IdItem = pedidoDTO.IdItem,
                };
                _pedidoService.InserirPedido(pedido);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }



        [HttpGet("pedidosPorCodigo")]
        public async Task<IActionResult> BuscarTodosAgrupadosAsync()
        {
            try
            {
                var pedidos = _pedidoService.FindAll();

                if (pedidos == null || !pedidos.Any())
                {
                    return NotFound(new { Message = "Nenhum pedido cadastrado ainda." });
                }

                var pedidosAgrupados = pedidos
                    .GroupBy(p => p.CodPedido)
                    .Select(async group =>
                    {
                        var primeiroPedido = group.First();

                        // Obter fornecedor
                        var fornecedorResponse = await _httpClient.GetAsync($"{apiForncedor}{primeiroPedido.IdFornecedor}");
                        var fornecedor = fornecedorResponse.IsSuccessStatusCode
                            ? await fornecedorResponse.Content.ReadFromJsonAsync<FornecedorDTO>()
                            : null;

                        // Obter itens
                        var itens = new List<ItemDTO>();
                        foreach (var pedido in group)
                        {
                            var itemResponse = await _httpClient.GetAsync($"{apiItem}{pedido.IdItem}");
                            if (itemResponse.IsSuccessStatusCode)
                            {
                                var item = await itemResponse.Content.ReadFromJsonAsync<ItemDTO>();
                                if (item != null)
                                {
                                    itens.Add(item);
                                }
                            }
                        }

                        // Estruturar o resultado agrupado
                        return new
                        {
                            CodPedido = group.Key,
                            Fornecedor = fornecedor,
                            DataPedido = primeiroPedido.DataPedido,
                            Itens = itens
                        };
                    })
                    .ToList();

                // Aguarde a resolução de todas as tarefas para os grupos
                var resultadoFinal = await Task.WhenAll(pedidosAgrupados);

                return Ok(resultadoFinal);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpGet("pedidosPorCodigo/{codigo}")]
        public async Task<IActionResult> BuscarPorCodigoAgrupadoAsync([FromRoute] string codigo)
        {
            try
            {
                // Obter os pedidos pelo código informado
                var pedidos = _pedidoService.FindAll().Where(p => p.CodPedido == codigo).ToList();

                if (pedidos == null || !pedidos.Any())
                {
                    return NotFound(new { Message = $"Nenhum pedido com código {codigo} encontrado." });
                }

                // Dados compartilhados para o agrupamento
                FornecedorDTO fornecedor = null;
                string dataPedido = null;
                var itens = new List<ItemDTO>();

                foreach (var pedido in pedidos)
                {
                    // Consultar o micro serviço de item
                    var itemResponse = await _httpClient.GetAsync($"{apiItem}{pedido.IdItem}");
                    if (itemResponse.IsSuccessStatusCode)
                    {
                        var item = await itemResponse.Content.ReadFromJsonAsync<ItemDTO>();
                        if (item != null)
                        {
                            itens.Add(item);
                        }
                    }

                    // Apenas uma chamada para obter o fornecedor, já que ele é o mesmo para todos os pedidos com o mesmo código
                    if (fornecedor == null)
                    {
                        var fornecedorResponse = await _httpClient.GetAsync($"{apiForncedor}{pedido.IdFornecedor}");
                        if (fornecedorResponse.IsSuccessStatusCode)
                        {
                            fornecedor = await fornecedorResponse.Content.ReadFromJsonAsync<FornecedorDTO>();
                        }

                        // Capturar a data do pedido (assumindo que todos os pedidos têm a mesma data para o mesmo código)
                        dataPedido = pedido.DataPedido;
                    }
                }

                // Estruturar a resposta agrupada
                var resposta = new
                {
                    CodPedido = codigo,
                    Fornecedor = fornecedor,
                    DataPedido = dataPedido,
                    Itens = itens
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("/pedidoPorId/{id:int}")]
        public IActionResult BuscarPorId([FromRoute] int id)
        {
            try
            {
                var pedido = _pedidoService.FindById(id);
                if (pedido == null)
                {
                    return NotFound(new { Message = $"Pedido com ID {id} não encontrado." });
                }
                return Ok(pedido);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpGet("/pedidoPorId")]
        public IActionResult BuscarTodos()
        {
            try
            {
                var pedidos= _pedidoService.FindAll();

                if (pedidos == null)
                {
                    return NotFound(new { Message = $"Nenhum pedido cadastrado ainda" });
                }

                return Ok(pedidos);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletarPorId([FromRoute] int id)
        {
            try
            {
                var check = _pedidoService.DeleteById(id);
                if (check == false)
                {
                    return NotFound(new { Message = $"Houve problema ao deletar o pedido" });
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
