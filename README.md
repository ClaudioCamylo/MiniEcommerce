
# MiniEcommerce · Lista 32

Exercício de Garantia da Qualidade de Software do professor Daniel Henrique Matos de Paiva. A aplicação simula uma compra no console, do cadastro de produtos à baixa de estoque no checkout.

## Requisitos e execução

Instale o SDK do .NET 10. Para inspecionar o histórico, use o Git. Extraia o ZIP inteiro e execute os comandos dentro da pasta `MiniEcommerce`.

```sh
dotnet restore MiniEcommerce.sln
dotnet build MiniEcommerce.sln -c Release --no-restore
dotnet test MiniEcommerce.sln -c Release --no-build --no-restore
dotnet run --project MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

O NuGet precisa acessar os pacotes de teste na primeira restauração, caso ainda estejam fora do cache local.

## Como usar o menu

| Opção | Ação |
| --- | --- |
| `1` | Cadastrar nome, preço e estoque inicial. O catálogo atribui o ID. |
| `2` | Listar IDs, preços e estoques atuais. |
| `3` | Adicionar uma quantidade de um produto identificado pelo ID. |
| `4` | Conferir os itens e o subtotal do carrinho. |
| `5` | Remover todas as unidades de um produto do carrinho. |
| `6` | Finalizar a compra e exibir os valores por item e o total. |
| `0` | Encerrar a sessão. |

Digite os preços com vírgula e até duas casas decimais, como `12,50`. Preço e estoque inicial podem ser zero. A quantidade adicionada precisa ser inteira e positiva. Adições repetidas somam as quantidades do mesmo produto, dentro do estoque disponível.

Para experimentar, cadastre **Caderno** a `12,50` com estoque `5`. Adicione `2` unidades pelo ID exibido, finalize e consulte o catálogo. O pedido totaliza **R$ 25,00** e o estoque passa a **3**.

## Organização do código

- `MiniEcommerce.Console/Program.cs` lê a entrada, formata valores em reais e aciona as operações do domínio.
- `MiniEcommerce.Console/Dominio/` contém as regras de negócio. `Produto` valida nome, preço e estoque. `Catalogo` cadastra, consulta e lista produtos.
- `CarrinhoDeCompras` agrupa itens, valida a quantidade acumulada e calcula o subtotal. A quantidade em estoque permanece disponível até o checkout.
- `Pedido` confere o estoque de todos os itens e monta o resumo financeiro antes de dar baixa. A compra concluída esvazia o carrinho. Uma falha durante a validação preserva o estoque e os itens.
- `MiniEcommerce.Tests/` referencia a aplicação e usa xUnit para testar o domínio e o menu com entrada e saída de texto.

O catálogo e o carrinho ficam em memória durante a sessão do console.

## TDD e histórico Git

```sh
git log --oneline --reverse
```

Cada ciclo começa com testes e os contratos mínimos para compilá-los. No commit de teste, as regras novas ainda lançam `NotImplementedException`. O commit seguinte implementa as regras e faz a suíte passar. A refatoração reorganiza os arquivos do domínio e centraliza a verificação de disponibilidade.

| Ciclo | Red | Green | Casos aprovados após a implementação |
| --- | --- | --- | ---: |
| Produto e catálogo | `affe452` | `71a1401` | 5 |
| Carrinho | `f331e0e` | `2018ab2` | 13 |
| Pedido | `74e7c90` | `11f97bb` | 16 |
| Menu | `e6db3af` | `42972e3` | 21 |

Em uma cópia extraída do ZIP, use `git switch --detach <hash>` para visitar um dos commits e rode `dotnet test MiniEcommerce.sln`. Volte à entrega com `git switch main`.

## Entrega no GitHub

A entrega está na branch `main` do repositório público. Você também pode consultar o histórico na cópia extraída do ZIP. [MiniEcommerce no GitHub](https://github.com/ClaudioCamylo/MiniEcommerce)
