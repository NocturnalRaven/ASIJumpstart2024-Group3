using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _conditionProperty;
    private readonly object _expectedValue;

    public RequiredIfAttribute(string conditionProperty, object expectedValue)
    {
        _conditionProperty = conditionProperty;
        _expectedValue = expectedValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_conditionProperty);

        if (property == null)
            throw new ArgumentException($"Property with name {_conditionProperty} not found");

        var conditionValue = property.GetValue(validationContext.ObjectInstance);

        if (conditionValue?.Equals(_expectedValue) == true && value == null)
        {
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required");
        }

        return ValidationResult.Success;
    }
}
