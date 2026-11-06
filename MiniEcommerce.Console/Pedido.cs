
namespace MiniEcommerce;

public sealed class Pedido
{
    public IReadOnlyList<ItemPedido> Itens => throw new NotImplementedException();

    public decimal Total => throw new NotImplementedException();

    public static Pedido Finalizar(CarrinhoDeCompras carrinho) => throw new NotImplementedException();
}
