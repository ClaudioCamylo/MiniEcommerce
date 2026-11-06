using MiniEcommerce;
using Xunit;

namespace MiniEcommerce.Tests;

public class PedidoTests
{
    [Fact]
    public void FinalizarCalculaResumoBaixaEstoqueELimpaCarrinho()
    {
        var catalogo = new Catalogo();
        var caderno = catalogo.Cadastrar("Caderno", 12.50m, 5);
        var caneta = catalogo.Cadastrar("Caneta", 2.10m, 3);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(caderno, 2);
        carrinho.Adicionar(caneta, 3);

        var pedido = Pedido.Finalizar(carrinho);

        Assert.Equal(31.30m, pedido.Total);
        Assert.Collection(pedido.Itens.OrderBy(item => item.Nome),
            item =>
            {
                Assert.Equal(caderno.Id, item.ProdutoId);
                Assert.Equal("Caderno", item.Nome);
                Assert.Equal(12.50m, item.PrecoUnitario);
                Assert.Equal(2, item.Quantidade);
                Assert.Equal(25m, item.Subtotal);
            },
            item =>
            {
                Assert.Equal(caneta.Id, item.ProdutoId);
                Assert.Equal("Caneta", item.Nome);
                Assert.Equal(2.10m, item.PrecoUnitario);
                Assert.Equal(3, item.Quantidade);
                Assert.Equal(6.30m, item.Subtotal);
            });
        Assert.Equal(3, caderno.Estoque);
        Assert.Equal(0, caneta.Estoque);
        Assert.Empty(carrinho.Itens);
        Assert.Equal(0m, carrinho.Subtotal);
    }

    [Fact]
    public void FinalizarRejeitaCarrinhoVazio()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(carrinho));
    }

    [Fact]
    public void FinalizarComEstoqueInsuficientePreservaTodosOsEstoquesEOCarrinho()
    {
        var catalogo = new Catalogo();
        var caderno = catalogo.Cadastrar("Caderno", 12.50m, 5);
        var caneta = catalogo.Cadastrar("Caneta", 2m, 3);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(caderno, 2);
        carrinho.Adicionar(caneta, 3);

        var outroCarrinho = new CarrinhoDeCompras();
        outroCarrinho.Adicionar(caneta, 1);
        Pedido.Finalizar(outroCarrinho);

        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(carrinho));

        Assert.Equal(5, caderno.Estoque);
        Assert.Equal(2, caneta.Estoque);
        Assert.Collection(carrinho.Itens.OrderBy(item => item.Produto.Nome),
            item =>
            {
                Assert.Same(caderno, item.Produto);
                Assert.Equal(2, item.Quantidade);
            },
            item =>
            {
                Assert.Same(caneta, item.Produto);
                Assert.Equal(3, item.Quantidade);
            });
        Assert.Equal(31m, carrinho.Subtotal);
    }
}
