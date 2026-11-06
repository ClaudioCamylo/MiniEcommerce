
namespace MiniEcommerce;

public sealed class CarrinhoDeCompras
{
    private readonly Dictionary<Produto, ItemCarrinho> itens = new();

    public IReadOnlyList<ItemCarrinho> Itens => itens.Values.ToArray();

    public decimal Subtotal => itens.Values.Sum(item => item.Subtotal);

    public void Adicionar(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);
        if (quantidade <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantidade), "Informe uma quantidade maior que zero.");
        }

        var quantidadeAtual = itens.GetValueOrDefault(produto)?.Quantidade ?? 0;
        if (quantidade > produto.Estoque - quantidadeAtual)
        {
            throw new InvalidOperationException($"Estoque insuficiente para {produto.Nome}.");
        }

        var novoItem = new ItemCarrinho(produto, quantidadeAtual + quantidade);
        var subtotalOutrosItens = itens.Values.Where(item => item.Produto != produto).Sum(item => item.Subtotal);

        // Confere se o subtotal cabe em decimal antes de alterar o carrinho.
        _ = subtotalOutrosItens + novoItem.Subtotal;
        itens[produto] = novoItem;
    }

    public bool Remover(Produto produto) => itens.Remove(produto);

    internal void Limpar() => itens.Clear();
}
