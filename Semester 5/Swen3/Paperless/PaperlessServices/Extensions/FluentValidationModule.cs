using FluentValidation;
using PaperlessServices.Validation;

namespace PaperlessServices.Extensions;

public static class FluentValidationServiceCollectionExtensions
{
    public static void AddFluentValidationRules(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<BlValidation>();
    }
}