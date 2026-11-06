
namespace MiniEcommerce.Dominio;

public sealed class Catalogo
{
    private readonly List<Produto> produtos = new();

    public Produto Cadastrar(string nome, decimal preco, int estoque)
    {
        var produto = new Produto(produtos.Count + 1, nome, preco, estoque);
        produtos.Add(produto);
        return produto;
    }

    public Produto? Buscar(int id) => produtos.Find(produto => produto.Id == id);

    public IReadOnlyList<Produto> Listar() => produtos.ToArray();
}
