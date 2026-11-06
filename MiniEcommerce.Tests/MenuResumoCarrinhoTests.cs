using System.Globalization;
using Xunit;

namespace MiniEcommerce.Tests;

public class MenuResumoCarrinhoTests
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");

    private static string Moeda(decimal valor) => valor.ToString("C", Cultura);

    [Fact]
    public void VerCarrinho_DeveMostrarPrecoUnitarioTotalDaLinhaESubtotal()
    {
        var entrada = new StringReader(string.Join(Environment.NewLine,
            "1", "Caderno", "12,50", "5",
            "3", "1", "2",
            "4",
            "0"));
        var saida = new StringWriter();

        new Menu(entrada, saida).Executar();

        var texto = saida.ToString();
        Assert.Contains($"2 x Caderno ({Moeda(12.50m)}) = {Moeda(25.00m)}", texto);
        Assert.Contains($"Subtotal: {Moeda(25.00m)}", texto);
    }
}