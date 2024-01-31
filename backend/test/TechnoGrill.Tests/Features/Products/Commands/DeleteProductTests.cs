using FluentValidation.TestHelper;
using TechnoGrill.Features.Products.Commands;

namespace TechnoGrill.Tests.Features.Products.Commands;

public sealed class DeleteProductTests
{
    private readonly DeleteProduct.Validator _sut = new();

    [Fact]
    public async Task Validate_Should_HaveNoErrors_When_Valid()
    {
        // Arrange
        var command = new DeleteProduct.Command(Guid.NewGuid());

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }

    [Fact]
    public async Task Validate_Should_HaveErrors_When_Invalid()
    {
        // Arrange
        var command = new DeleteProduct.Command(Guid.Empty);

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
}