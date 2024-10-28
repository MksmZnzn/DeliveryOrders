using DeliveryOrders.Application.CQRS.Queries;
using FluentValidation;

namespace DeliveryOrders.Application.CQRS.Validators
{
    public class GetFilteredOrdersQueryValidator : AbstractValidator<GetFilteredOrdersQuery>
    {
        public GetFilteredOrdersQueryValidator()
        {
            RuleFor(query => query.District)
                .NotEmpty().WithMessage("Параметр 'District' обязателен");

            RuleFor(query => query.Time).LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Параметр 'Time' не может быть из будущего");
            RuleFor(query => query.Time)
                .NotEmpty().WithMessage("Параметр 'Time' обязателен");
        }
    }
}