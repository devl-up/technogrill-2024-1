using FluentValidation.TestHelper;
using TechnoGrill.Features.Orders.Commands;

namespace TechnoGrill.Tests.Features.Orders.Commands;

public sealed class DeclineOrderTests
{
    private readonly DeclineOrder.Validator _sut = new();

    [Fact]
    public async Task Validate_Should_HaveNoErrors_When_Valid()
    {
        // Arrange
        var command = new DeclineOrder.Command(Guid.NewGuid());

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }

    [Fact]
    public async Task Validate_Should_HaveErrors_When_Invalid()
    {
        // Arrange
        var command = new DeclineOrder.Command(Guid.Empty);

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
}