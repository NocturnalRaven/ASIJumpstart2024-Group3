using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class CompareDatesAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public CompareDatesAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var currentValue = value as DateTime?;
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            throw new ArgumentException($"Property with name {_comparisonProperty} not found");

        var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateTime?;

        if (currentValue.HasValue && comparisonValue.HasValue && currentValue <= comparisonValue)
        {
            return new ValidationResult(ErrorMessage ?? $"The {validationContext.DisplayName} must be after {_comparisonProperty}");
        }

        return ValidationResult.Success;
    }
}
