
namespace MiniEcommerce;

public sealed record ItemCarrinho(Produto Produto, int Quantidade)
{
    public decimal Subtotal => throw new NotImplementedException();
}
