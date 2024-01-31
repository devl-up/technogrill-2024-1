using FluentValidation.TestHelper;
using TechnoGrill.Features.Orders.Commands;

namespace TechnoGrill.Tests.Features.Orders.Commands;

public sealed class DeleteOrderItemTests
{
    private readonly DeleteOrderItem.Validator _sut = new();

    [Fact]
    public async Task Validate_Should_HaveNoErrors_When_Valid()
    {
        // Arrange
        var command = new DeleteOrderItem.Command(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
        result.ShouldNotHaveValidationErrorFor(c => c.ItemId);
    }

    [Fact]
    public async Task Validate_Should_HaveErrors_When_Invalid()
    {
        // Arrange
        var command = new DeleteOrderItem.Command(Guid.Empty, Guid.Empty);

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
        result.ShouldHaveValidationErrorFor(c => c.ItemId);
    }
}