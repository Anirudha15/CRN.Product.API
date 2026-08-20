using FluentValidation;
using CRN.Product.API.DTOs;

namespace CRN.Product.API.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(255)
                .WithMessage("Product name cannot exceed 255 characters.");

            RuleFor(x => x.CreatedBy)
                .NotEmpty()
                .WithMessage("CreatedBy is required.")
                .MaximumLength(100)
                .WithMessage("CreatedBy cannot exceed 100 characters.");
        }
    }
}