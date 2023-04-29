using FluentValidation;
using ProjetoIntegrador.Modulo1.BackEnd.Models;

namespace ProjetoIntegrador.Modulo1.BackEnd.Validators
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório")
                .MaximumLength(100).WithMessage("O nome do produto deve ter no máximo 100 caracteres");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição do produto é obrigatória")
                .MaximumLength(500).WithMessage("A descrição do produto deve ter no máximo 500 caracteres");

            RuleFor(x => x.Preco)
                .NotEmpty().WithMessage("O preço do produto é obrigatório")
                .GreaterThan(0).WithMessage("O preço do produto deve ser maior que zero");

            RuleFor(x => x.Quantidade)
                .NotEmpty().WithMessage("A quantidade de produtos em estoque é obrigatória")
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade do produto deve ser maior ou igual a zero");

            RuleFor(x => x.Categoria)
                .NotEmpty().WithMessage("A categoria do produto é obrigatória");

            RuleFor(x => x.Imagem)
                .NotEmpty().WithMessage("A imagem do produto é obrigatória");
        }
    }
}
