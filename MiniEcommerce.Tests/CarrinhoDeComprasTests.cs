using MiniEcommerce;
using Xunit;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
    [Fact]
    public void AdicionarMesmoProdutoSomaQuantidadesESubtotalSemBaixarEstoque()
    {
        var produto = new Catalogo().Cadastrar("Caderno", 12.50m, 5);
        var carrinho = new CarrinhoDeCompras();

        carrinho.Adicionar(produto, 2);
        carrinho.Adicionar(produto, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Same(produto, item.Produto);
        Assert.Equal(5, item.Quantidade);
        Assert.Equal(62.50m, item.Subtotal);
        Assert.Equal(62.50m, carrinho.Subtotal);
        Assert.Equal(5, produto.Estoque);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AdicionarRejeitaQuantidadeNaoPositiva(int quantidade)
    {
        var produto = new Catalogo().Cadastrar("Caneta", 2m, 4);
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<ArgumentOutOfRangeException>(() => carrinho.Adicionar(produto, quantidade));

        Assert.Empty(carrinho.Itens);
        Assert.Equal(0m, carrinho.Subtotal);
    }

    [Fact]
    public void AdicionarAcimaDoEstoqueNaoCriaItem()
    {
        var produto = new Catalogo().Cadastrar("Caneta", 2m, 4);
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => carrinho.Adicionar(produto, 5));

        Assert.Empty(carrinho.Itens);
        Assert.Equal(4, produto.Estoque);
    }

    [Theory]
    [InlineData(4, 3, 2)]
    [InlineData(int.MaxValue, int.MaxValue, 1)]
    public void AdicionarVerificaEstoqueAcumulado(int estoque, int quantidadeAtual, int quantidadeAdicional)
    {
        var produto = new Catalogo().Cadastrar("Caneta", 2m, estoque);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(produto, quantidadeAtual);

        Assert.Throws<InvalidOperationException>(() => carrinho.Adicionar(produto, quantidadeAdicional));

        Assert.Equal(quantidadeAtual, Assert.Single(carrinho.Itens).Quantidade);
        Assert.Equal(2m * quantidadeAtual, carrinho.Subtotal);
        Assert.Equal(estoque, produto.Estoque);
    }

    [Fact]
    public void RemoverExcluiSomenteOLancamentoDoProdutoEAtualizaSubtotal()
    {
        var catalogo = new Catalogo();
        var caderno = catalogo.Cadastrar("Caderno", 12.50m, 5);
        var caneta = catalogo.Cadastrar("Caneta", 2m, 4);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(caderno, 2);
        carrinho.Adicionar(caneta, 3);
        Assert.Equal(31m, carrinho.Subtotal);

        Assert.True(carrinho.Remover(caderno));
        Assert.False(carrinho.Remover(caderno));

        Assert.Same(caneta, Assert.Single(carrinho.Itens).Produto);
        Assert.Equal(6m, carrinho.Subtotal);
        Assert.Equal(5, caderno.Estoque);
    }

    [Fact]
    public void AdicionarRejeitaTotalForaDoLimiteDecimalSemAlterarOCarrinho()
    {
        var catalogo = new Catalogo();
        var produto = catalogo.Cadastrar("Produto caro", decimal.MaxValue, 2);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(produto, 1);

        Assert.Throws<OverflowException>(() => carrinho.Adicionar(produto, 1));

        Assert.Equal(1, Assert.Single(carrinho.Itens).Quantidade);
        Assert.Equal(decimal.MaxValue, carrinho.Subtotal);
    }
}
