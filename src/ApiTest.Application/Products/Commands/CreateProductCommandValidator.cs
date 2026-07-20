using ApiTest.Application.Products.Commands.CreateProduct;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTest.Application.Products.Commands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(v => v.Name).NotEmpty().WithMessage("El nombre es requerido.");
            RuleFor(v => v.Price).GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
        }
    }
}
