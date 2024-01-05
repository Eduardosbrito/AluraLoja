using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class ProdutoDAOEntity : IProdutoDAO, IDisposable
    {
        private LojaContext context;

        public ProdutoDAOEntity()
        {
            this.context = new LojaContext();
        }

        public void Adicionar(Produto p)
        {
            context.Produtos.Add(p);
            context.SaveChanges();
        }

        public void Adicionar(Produto p1, Produto p2, Produto p3)
        {
            context.Produtos.AddRange(p1, p2, p3);
            context.SaveChanges();
        }

        public void Atualizar(Produto p)
        {
            context.Produtos.Update(p);
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IList<Produto> Produtos()
        {
            return context.Produtos.ToList();
        }

        public void Remover(Produto p)
        {
            context.Produtos.Remove(p);
            context.SaveChanges();
        }
    }
}
