using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;

namespace DevIO.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository, IMapper mapper, IFornecedorRepository fornecedorRepository)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepository.ObterProdutosFornecedor(); //retorna produtos com fornecedores
            var produtosViewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);
            return View(produtosViewModel);
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        // GET: Produtos/Create
        public async Task<IActionResult> Create()
        {
            //ViewData["FornecedorId"] = new SelectList(_context.Set<FornecedorViewModel>(), "Id", "Documento");

            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);
            if (!ModelState.IsValid)
                return View(produtoViewModel);

            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoViewModel));
            return RedirectToAction(nameof(Index));
            
            //ViewData["FornecedorId"] = new SelectList(_context.Set<FornecedorViewModel>(), "Id", "Documento", produtoViewModel.FornecedorId);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }
            //ViewData["FornecedorId"] = new SelectList(_context.Set<FornecedorViewModel>(), "Id", "Documento", produtoViewModel.FornecedorId);
            return View(produtoViewModel);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            await _produtoRepository.Atualizar(_mapper.Map<Produto>(produtoViewModel));
            return RedirectToAction(nameof(Index));
            //ViewData["FornecedorId"] = new SelectList(_context.Set<FornecedorViewModel>(), "Id", "Documento", produtoViewModel.FornecedorId);    
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            await _produtoRepository.Remover(id);

            return RedirectToAction(nameof(Index));
        }

        /* Métodos extras */

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            //obtem o produto com o Fornecedor
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));

            //obtem todos os fornecedores do sistema
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produtoViewModel)
        {
            //obtem todos os fornecedores do sistema
            produtoViewModel.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            return produtoViewModel;
        }
    }
}
