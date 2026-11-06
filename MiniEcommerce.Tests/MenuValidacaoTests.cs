
using MiniEcommerce;

namespace MiniEcommerce.Tests;

public sealed class MenuValidacaoTests
{
    [Fact]
    public void AdicionarProdutoComIdInexistente()
    {
        using var entrada = new StringReader(
            string.Join(Environment.NewLine, "3", "999", "4", "0"));

        using var saida = new StringWriter();

        var menu = new Menu(entrada, saida);
        menu.Executar();

        var tela = saida.ToString();

        Assert.Contains("Produto não encontrado.", tela);
        Assert.Contains("Carrinho vazio.", tela);

        // O menu deve aparecer novamente após a tentativa inválida.
        Assert.Equal(3, ContarOcorrencias(tela, "MiniEcommerce"));
    }

    [Fact]
    public void AdicionarProdutoComQuantidadeZero()
    {
        using var entrada = new StringReader(
            string.Join(Environment.NewLine,
                "1",
                "Teclado",
                "50,00",
                "7",
                "3",
                "1",
                "0",
                "4",
                "2",
                "0"));

        using var saida = new StringWriter();

        var menu = new Menu(entrada, saida);
        menu.Executar();

        var tela = saida.ToString();

        Assert.Contains("Informe uma quantidade maior que zero.", tela);
        Assert.Contains("Carrinho vazio.", tela);
        Assert.Contains("Teclado", tela);
        Assert.Contains("Estoque: 7", tela);

        // Confirma que o menu continuou funcionando.
        Assert.Equal(5, ContarOcorrencias(tela, "MiniEcommerce"));
    }

    private static int ContarOcorrencias(string texto, string trecho)
    {
        return texto.Split(trecho).Length - 1;
    }
}
