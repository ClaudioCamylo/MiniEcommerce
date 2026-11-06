
namespace MiniEcommerce;

public sealed class Produto
{
    internal Produto(int id, string nome, decimal preco, int estoque)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("Informe o nome do produto.", nameof(nome));
        }

        if (preco < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(preco), "O preço deve ser maior ou igual a zero.");
        }

        if (estoque < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(estoque), "O estoque deve ser maior ou igual a zero.");
        }

        Id = id;
        Nome = nome.Trim();
        Preco = preco;
        Estoque = estoque;
    }

    public int Id { get; }
    public string Nome { get; }
    public decimal Preco { get; }
    public int Estoque { get; private set; }
}
