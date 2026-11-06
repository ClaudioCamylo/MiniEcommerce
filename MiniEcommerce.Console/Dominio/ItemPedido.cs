
namespace MiniEcommerce.Dominio;

public sealed record ItemPedido(int ProdutoId, string Nome, decimal PrecoUnitario, int Quantidade)
{
    public decimal Subtotal => PrecoUnitario * Quantidade;
}
