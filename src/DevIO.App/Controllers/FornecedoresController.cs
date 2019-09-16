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
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        //deve retornar todos...
        public async Task<IActionResult> Index()
        {
            //no exemplo do Eduardo está dando erro. Na View Index usamos FornecedoresViewModel e o método ObterTodos retorna Fornecedor. No meu exemplo nao deu erro
            //precisamos fazer uma conversão

            //return View(await _fornecedorRepository.ObterTodos()); //alterar para o código comentado

            //fazemos a conversão com AutoMapper 
            //A busca no _fornecedorRepository recebemos uma lista de Fornecedores, mas na View Index trabalhamos com uma lista de FornecedorViewModel
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>( await _fornecedorRepository.ObterTodos())); 
        }

        // GET: Fornecedores/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            //var fornecedorViewModel = await _context.FornecedorViewModel
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var fornecedorViewModel = await ObterFornecedorEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        // GET: Fornecedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fornecedores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (ModelState.IsValid)
            {
                var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
                await _fornecedorRepository.Adicionar(fornecedor);
                //await _fornecedorRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedorViewModel);
        }

        // GET: Fornecedores/Edit/5
        //colocar o endereço + lista de produtos
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }
            return View(fornecedorViewModel);
        }

        // POST: Fornecedores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorRepository.Atualizar(fornecedor);
            return RedirectToAction(nameof(Index));
        }

        // GET: Fornecedores/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();
        
            return View(fornecedorViewModel);
        }

        // POST: Fornecedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
                return NotFound();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorRepository.Remover(fornecedor.Id);

            //_context.FornecedorViewModel.Remove(fornecedorViewModel);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /* Métodos extras para facilitar nossa vida */

        //método que retorna o Fornecedor + Endereço já mapeado para viewModel -> para não precisarmos ficar repentido o código de Map.
        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }
    }
}
