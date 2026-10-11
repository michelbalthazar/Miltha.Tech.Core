using FluentValidation;
using Miltha.Tech.Core.Validation;

namespace Miltha.Tech.Core.Validation.Tests;

public class FluentValidatorTests
{
    private sealed class SampleEntity
    {
        public string Name { get; set; }
    }

    private sealed class SampleValidator : FluentValidator<SampleEntity>
    {
        public SampleValidator() => RuleFor(x => x.Name).NotEmpty();
    }

    [Fact]
    public void Validate_ShouldPass_WhenRulesAreSatisfied()
    {
        // Arrange
        var validator = new SampleValidator();

        // Act
        var result = validator.Validate(new SampleEntity { Name = "Miltha" });

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenARuleIsBroken()
    {
        // Arrange
        var validator = new SampleValidator();

        // Act
        var result = validator.Validate(new SampleEntity { Name = string.Empty });

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(SampleEntity.Name));
    }

    [Fact]
    public void Validate_ShouldReportNullEntity_WhenInstanceIsNull()
    {
        // Arrange
        var validator = new SampleValidator();

        // Act
        var result = validator.Validate(new ValidationContext<SampleEntity>(null));

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(SampleEntity));
    }

    [Fact]
    public void NullEntity_ShouldAllowCustomizingMessageAndErrorCode()
    {
        // Arrange
        var validator = new SampleValidator();
        ((INullEntityBuilderOptions)validator)
            .WithMessage("entity is required")
            .WithErrorCode("E_NULL");

        // Act
        var result = validator.Validate(new ValidationContext<SampleEntity>(null));

        // Assert
        var error = result.Errors.Single();
        Assert.Equal("entity is required", error.ErrorMessage);
        Assert.Equal("E_NULL", error.ErrorCode);
    }
}