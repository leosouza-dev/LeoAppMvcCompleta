using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        //além dos métodos do IRepository criaremos alguns especificos para Fornecedor

        Task<Fornecedor> ObterFornecedorEndereco(Guid id); //obter fornecedor + o Endereço em um único objeto
        Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id);
    }
}
