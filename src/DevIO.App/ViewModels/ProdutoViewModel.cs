using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.App.ViewModels
{
    public class ProdutoViewModel
    {
        [Key] //indica que é uma chave, e não um campo
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Fornecedor")]
        public Guid FornecedorId { get; set; } //para gerar o drop down list

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage ="O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; }

        //upload de imagem não pode ser do tipo string
        [DisplayName("Imagem do Produto")]
        public IFormFile ImagemUpload { get; set; }

        //mas não podemos perder o campo Imagem (string)
        public string Imagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }

        public FornecedorViewModel Fornecedor { get; set; }

        //usado para enviarmos uma lista de forncedores para quisermos editar o fornecedor.
        public IEnumerable<FornecedorViewModel> Fornecedores;
    }
}
