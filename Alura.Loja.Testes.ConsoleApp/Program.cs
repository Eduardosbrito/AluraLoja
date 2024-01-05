using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsultaUmParaMuitos();
        }

        private static void AtualizarProdutos()
        {
            using (var context = new ProdutoDAOEntity())
            {
                Produto Produtos = (Produto)context.Produtos().FirstOrDefault();

                Produtos.Nome = "Harry Potter e a Ordem da Fênix 2";
                context.Atualizar(Produtos);

            }

            RecuperarProdutos();
        }

        private static void ExcluirProdutos()
        {
            using (var context = new ProdutoDAOEntity())
            {
                IList<Produto> Produtos = context.Produtos();
                foreach (var produto in Produtos)
                {
                    context.Remover(produto);
                }

                Console.WriteLine("Produtos Excluidos com sucesso.");
                Console.ReadKey();
            }
        }

        private static void RecuperarProdutos()
        {
            using (var context = new ProdutoDAOEntity())
            {
                IList<Produto> Produtos = context.Produtos();

                if (Produtos.Count > 0)
                {
                    foreach (var produto in Produtos)
                    {
                        Console.WriteLine(produto.Nome);
                    }

                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Nenhum Produto Encontrado.");
                    Console.ReadKey();
                }
            }
        }

        private static void GravarUsandoEntity()
        {
            Produto p1 = new Produto();
            p1.Nome = "Harry Potter e a Ordem da Fênix";
            p1.Categoria = "Livros";
            p1.PrecoUnitario = 19.89;

            Produto p2 = new Produto();
            p2.Nome = "Senhor dos Anéis 1";
            p2.Categoria = "Livros";
            p2.PrecoUnitario = 19.89;

            Produto p3 = new Produto();
            p3.Nome = "O Monge e o Executivo";
            p3.Categoria = "Livros";
            p3.PrecoUnitario = 19.89;

            using (var context = new ProdutoDAOEntity())
            {
                context.Adicionar(p1, p2, p3);
            }
        }

        private static void MuitosParaMuitos()
        {
            var p1 = new Produto() { Nome = "Suco de Laranja", Categoria = "Bebidas", PrecoUnitario = 8.79, Unidade = "Litros" };
            var p2 = new Produto() { Nome = "Café", Categoria = "Bebidas", PrecoUnitario = 12.45, Unidade = "Gramas" };
            var p3 = new Produto() { Nome = "Macarrão", Categoria = "Alimentos", PrecoUnitario = 4.23, Unidade = "Gramas" };

            var promocaoDePascoa = new Promocao();

            promocaoDePascoa.Descricao = "Páscoa Feliz";
            promocaoDePascoa.DataInicial = DateTime.Now;
            promocaoDePascoa.Datafinal = DateTime.Now.AddMonths(3);

            promocaoDePascoa.IncluirProduto(p1);
            promocaoDePascoa.IncluirProduto(p2);
            promocaoDePascoa.IncluirProduto(p3);

            using (var contexto = new LojaContext())
            {
                contexto.Promocoes.Add(promocaoDePascoa);
                contexto.SaveChanges();
            };
        }

        private static void ConsultaMuitosParaMuitos()
        {
            using (var contexto = new LojaContext())
            {
                var promocao = contexto
                                    .Promocoes
                                    .Include(p => p.Produtos)
                                    .ThenInclude(pp => pp.Produto)
                                    .FirstOrDefault();

                foreach (var promotion in promocao.Produtos)
                {
                    Console.WriteLine(promotion.Produto.Nome);
                    Console.ReadKey();
                }

            }
        }

        private static void UmParaMuitos()
        {
            var fulando = new Cliente();
            fulando.Nome = "Fulando de Tal";
            fulando.EnderecoDeEntrega = new Endereco()
            {
                Numero = 12,
                Logradouro = "Rua dos Inválidos",
                Complemento = "Sobrado",
                Bairo = "Centro",
                Cidade = "Cidade"
            };

            using (var contexto = new LojaContext())
            {
                contexto.Clientes.Add(fulando);
                contexto.SaveChanges();
            }
        }

        private static void ConsultaUmParaMuitos()
        {
            using (var contexto = new LojaContext())
            {
                var cliente = contexto
                .Clientes
                .Include(c => c.EnderecoDeEntrega)
                .FirstOrDefault();

                Console.WriteLine($"Endereço de entrega: {cliente.EnderecoDeEntrega.Logradouro}");

                var produto = contexto
                    .Produtos
                    .Where(p => p.Id == 1004)
                    .FirstOrDefault();

                contexto.Entry(produto)
                    .Collection(p => p.Compras)
                    .Query()
                    .Where(c => c.Quantidade > 5)
                    .Load();

                Console.WriteLine($"Mostrando as compras do produto {produto.Nome}");
                foreach (var item in produto.Compras)
                {
                    Console.WriteLine("\t" + item);
                    
                }

                Console.ReadKey();
            }
        }

        private static void IncluirPromocao()
        {
            using (var contexto = new LojaContext())
            {
                var promocao = new Promocao();
                promocao.Descricao = "Queima Total Janeiro de 2024";
                promocao.DataInicial = DateTime.Now;
                promocao.Datafinal = new DateTime(2024, 1, 31);

                var produto = contexto.Produtos.Where(x => x.Categoria == "Bebidas").ToList();

                foreach (var item in produto)
                {
                    promocao.IncluirProduto(item);
                }

                contexto.Promocoes.Add(promocao);
                contexto.SaveChanges();

            }
        }
    }
}
