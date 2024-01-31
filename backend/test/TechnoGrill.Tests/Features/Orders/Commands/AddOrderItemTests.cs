using FluentValidation.TestHelper;
using TechnoGrill.Features.Orders.Commands;

namespace TechnoGrill.Tests.Features.Orders.Commands;

public sealed class AddOrderItemTests
{
    private readonly AddOrderItem.Validator _sut = new();

    [Fact]
    public async Task Validate_Should_HaveNoErrors_When_Valid()
    {
        // Arrange
        var command = new AddOrderItem.Command(Guid.NewGuid(), Guid.NewGuid(), 1);

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
        result.ShouldNotHaveValidationErrorFor(c => c.ProductId);
        result.ShouldNotHaveValidationErrorFor(c => c.Amount);
    }

    [Fact]
    public async Task Validate_Should_HaveErrors_When_Invalid()
    {
        // Arrange
        var command = new AddOrderItem.Command(Guid.Empty, Guid.Empty, 0);

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
        result.ShouldHaveValidationErrorFor(c => c.ProductId);
        result.ShouldHaveValidationErrorFor(c => c.Amount);
    }
}