
namespace MiniEcommerce.Dominio;

public sealed class Pedido
{
    private Pedido(ItemPedido[] itens)
    {
        Itens = Array.AsReadOnly(itens);
        Total = itens.Sum(item => item.Subtotal);
    }

    public IReadOnlyList<ItemPedido> Itens { get; }

    public decimal Total { get; }

    public static Pedido Finalizar(CarrinhoDeCompras carrinho)
    {
        ArgumentNullException.ThrowIfNull(carrinho);

        var itens = carrinho.Itens;
        if (itens.Count == 0)
        {
            throw new InvalidOperationException("Carrinho vazio.");
        }

        foreach (var item in itens)
        {
            item.Produto.VerificarDisponibilidade(item.Quantidade);
        }

        var resumo = itens.Select(item => new ItemPedido(
            item.Produto.Id, item.Produto.Nome, item.Produto.Preco, item.Quantidade)).ToArray();
        var pedido = new Pedido(resumo);

        // O estoque só muda depois que todos os itens e o total foram conferidos.
        foreach (var item in itens)
        {
            item.Produto.BaixarEstoque(item.Quantidade);
        }

        carrinho.Limpar();
        return pedido;
    }
}
