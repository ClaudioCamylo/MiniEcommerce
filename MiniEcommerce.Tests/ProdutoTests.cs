
using MiniEcommerce.Dominio;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Fact]
    public void CadastrarRejeitaPrecoNegativo()
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentOutOfRangeException>(() => catalogo.Cadastrar("Caderno", -0.01m, 2));
        Assert.Empty(catalogo.Listar());
    }

    [Fact]
    public void CadastrarRejeitaEstoqueNegativo()
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentOutOfRangeException>(() => catalogo.Cadastrar("Caderno", 10m, -1));
        Assert.Empty(catalogo.Listar());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CadastrarRejeitaNomeEmBranco(string nome)
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentException>(() => catalogo.Cadastrar(nome, 10m, 2));
        Assert.Empty(catalogo.Listar());
    }

    [Fact]
    public void CadastrarAceitaPrecoEEstoqueZeroEPermiteConsultarPorId()
    {
        var catalogo = new Catalogo();
        var amostra = catalogo.Cadastrar(" Amostra ", 0m, 0);
        var caderno = catalogo.Cadastrar("Caderno", 12.50m, 3);

        Assert.Equal("Amostra", amostra.Nome);
        Assert.Equal(0m, amostra.Preco);
        Assert.Equal(0, amostra.Estoque);
        Assert.NotEqual(amostra.Id, caderno.Id);
        Assert.Same(caderno, catalogo.Buscar(caderno.Id));
        Assert.Null(catalogo.Buscar(0));
        Assert.Collection(catalogo.Listar(), item => Assert.Same(amostra, item), item => Assert.Same(caderno, item));
    }
}
