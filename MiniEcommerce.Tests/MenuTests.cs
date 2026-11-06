using MiniEcommerce;

namespace MiniEcommerce.Tests;

public sealed class MenuTests
{
    [Fact]
    public void CompraExibeTotalEAtualizaEstoqueNoCatalogo()
    {
        var tela = Executar("1", "Café", "12,34", "5", "2", "3", "1", "2", "6", "2", "0");

        Assert.Contains("Café", tela);
        Assert.Contains("R$", tela);
        Assert.Contains("12,34", tela);
        Assert.Contains("Estoque: 5", tela);
        Assert.Contains("Pedido finalizado", tela);
        Assert.Contains("24,68", tela);
        Assert.Contains("Estoque: 3", tela);
    }

    [Fact]
    public void RemocaoEsvaziaCarrinhoEPreservaEstoque()
    {
        var tela = Executar("1", "Café", "12,34", "5", "3", "1", "2", "5", "1", "4", "2", "0");

        Assert.Contains("Carrinho vazio.", tela);
        Assert.Contains("Estoque: 5", tela);
    }

    [Fact]
    public void OpcaoInvalidaPermiteContinuarNoMenu()
    {
        var tela = Executar("99", "4", "0");

        Assert.Contains("Opção inválida.", tela);
        Assert.Contains("Carrinho vazio.", tela);
    }

    [Fact]
    public void CadastroSinalizaPrecoQueNaoPodeSerLido()
    {
        var tela = Executar("1", "Café", "abc", "0");

        Assert.Contains("Preço inválido.", tela);
    }

    [Fact]
    public void FimDaEntradaDuranteCadastroEncerraMenu()
    {
        Executar("1", "Café");
    }

    private static string Executar(params string[] linhas)
    {
        using var entrada = new StringReader(string.Join(Environment.NewLine, linhas));
        using var saida = new StringWriter();
        new Menu(entrada, saida).Executar();
        return saida.ToString();
    }
}
