
using System.Globalization;
using System.Text;
using MiniEcommerce.Dominio;

namespace MiniEcommerce;

internal static class Program
{
    private static void Main()
    {
        System.Console.InputEncoding = Encoding.UTF8;
        System.Console.OutputEncoding = Encoding.UTF8;
        new Menu(System.Console.In, System.Console.Out).Executar();
    }
}

public sealed class Menu(TextReader entrada, TextWriter saida)
{
    private const string Opcoes = """
        MiniEcommerce
        1. Cadastrar produto
        2. Listar catálogo
        3. Adicionar ao carrinho
        4. Ver carrinho
        5. Remover do carrinho
        6. Finalizar pedido
        0. Sair
        """;

    private const NumberStyles FormatoPreco = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");
    private readonly Catalogo catalogo = new();
    private readonly CarrinhoDeCompras carrinho = new();
    private bool fimDaEntrada;

    public void Executar()
    {
        while (!fimDaEntrada)
        {
            saida.WriteLine(Opcoes);
            var opcao = LerLinha("Opção: ");

            try
            {
                switch (opcao)
                {
                    case "1":
                        CadastrarProduto();
                        break;
                    case "2":
                        ListarCatalogo();
                        break;
                    case "3":
                        AdicionarAoCarrinho();
                        break;
                    case "4":
                        VerCarrinho();
                        break;
                    case "5":
                        RemoverDoCarrinho();
                        break;
                    case "6":
                        FinalizarPedido();
                        break;
                    case "0":
                    case null:
                        return;
                    default:
                        saida.WriteLine("Opção inválida.");
                        break;
                }
            }
            catch (ArgumentException erro)
            {
                saida.WriteLine(erro.Message);
            }
            catch (InvalidOperationException erro)
            {
                saida.WriteLine(erro.Message);
            }
            catch (OverflowException)
            {
                saida.WriteLine("O valor total excede o limite permitido.");
            }

            saida.WriteLine();
        }
    }

    private void CadastrarProduto()
    {
        var nome = LerLinha("Nome: ");
        if (nome is null)
        {
            return;
        }

        var valor = LerLinha("Preço (ex.: 12,50): ");
        if (valor is null)
        {
            return;
        }

        if (!decimal.TryParse(valor, FormatoPreco, Cultura, out var preco) || decimal.Round(preco, 2) != preco)
        {
            saida.WriteLine("Preço inválido. Use vírgula e até duas casas decimais.");
            return;
        }

        if (LerInteiro("Estoque inicial: ", "Estoque inválido.") is int estoque)
        {
            var produto = catalogo.Cadastrar(nome, preco, estoque);
            saida.WriteLine($"Produto cadastrado com ID {produto.Id}.");
        }
    }

    private void ListarCatalogo()
    {
        var produtos = catalogo.Listar();
        if (produtos.Count == 0)
        {
            saida.WriteLine("Catálogo vazio.");
            return;
        }

        foreach (var produto in produtos)
        {
            saida.WriteLine($"[{produto.Id}] {produto.Nome} | {Moeda(produto.Preco)} | Estoque: {produto.Estoque}");
        }
    }

    private void AdicionarAoCarrinho()
    {
        var produto = LerProduto();
        if (produto is null)
        {
            return;
        }

        if (LerInteiro("Quantidade: ", "Quantidade inválida.") is int quantidade)
        {
            carrinho.Adicionar(produto, quantidade);
            saida.WriteLine("Carrinho atualizado.");
        }
    }

    private void VerCarrinho()
    {
        var itens = carrinho.Itens;
        if (itens.Count == 0)
        {
            saida.WriteLine("Carrinho vazio.");
            return;
        }

        foreach (var item in itens)
        {
            saida.WriteLine($"[{item.Produto.Id}] {item.Quantidade} x {item.Produto.Nome} = {Moeda(item.Subtotal)}");
        }

        saida.WriteLine($"Subtotal: {Moeda(carrinho.Subtotal)}");
    }

    private void RemoverDoCarrinho()
    {
        var produto = LerProduto();
        if (produto is not null)
        {
            saida.WriteLine(carrinho.Remover(produto) ? "Item removido." : "Produto fora do carrinho.");
        }
    }

    private void FinalizarPedido()
    {
        var pedido = Pedido.Finalizar(carrinho);
        saida.WriteLine("Pedido finalizado.");
        foreach (var item in pedido.Itens)
        {
            saida.WriteLine($"{item.Quantidade} x {item.Nome} ({Moeda(item.PrecoUnitario)}) = {Moeda(item.Subtotal)}");
        }

        saida.WriteLine($"Total do pedido: {Moeda(pedido.Total)}");
    }

    private Produto? LerProduto()
    {
        if (LerInteiro("ID do produto: ", "ID inválido.") is not int id)
        {
            return null;
        }

        var produto = catalogo.Buscar(id);
        if (produto is null)
        {
            saida.WriteLine("Produto não encontrado.");
        }

        return produto;
    }

    private int? LerInteiro(string rotulo, string erro)
    {
        var linha = LerLinha(rotulo);
        if (linha is null)
        {
            return null;
        }

        if (int.TryParse(linha, NumberStyles.Integer, Cultura, out var numero))
        {
            return numero;
        }

        saida.WriteLine(erro);
        return null;
    }

    private string? LerLinha(string rotulo)
    {
        saida.Write(rotulo);
        var linha = entrada.ReadLine();
        fimDaEntrada = linha is null;
        return linha?.Trim();
    }

    private static string Moeda(decimal valor) => valor.ToString("C", Cultura);
}
