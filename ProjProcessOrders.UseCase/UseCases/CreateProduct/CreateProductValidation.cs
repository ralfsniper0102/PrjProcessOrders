﻿using FluentValidation;

namespace ProjProcessOrders.UseCase.UseCases.CreateProduct
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.ProductName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.ProductPrice)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0);
        }
    }
}
