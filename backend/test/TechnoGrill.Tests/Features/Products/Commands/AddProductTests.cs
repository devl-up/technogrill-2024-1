using System.Collections;
using FluentValidation.TestHelper;
using TechnoGrill.Features.Products.Commands;

namespace TechnoGrill.Tests.Features.Products.Commands;

public sealed class AddProductTests
{
    private readonly AddProduct.Validator _sut = new();

    [Fact]
    public async Task Validate_Should_HaveNoErrors_When_Valid()
    {
        // Arrange
        var command = new AddProduct.Command(Guid.NewGuid(), "name", "description", 10);

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
        result.ShouldNotHaveValidationErrorFor(c => c.Price);
    }

    [Theory]
    [ClassData(typeof(InvalidTestData))]
    public async Task Validate_Should_HaveErrors_When_Invalid(Guid id, string name, string description, int amount)
    {
        // Arrange
        var command = new AddProduct.Command(id, name, description, amount);

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Description);
        result.ShouldHaveValidationErrorFor(c => c.Price);
    }

    private class InvalidTestData : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [Guid.Empty, "", "", 0];
            yield return [Guid.Empty, new string('*', 51), new string('*', 201), 0];
        }
    }
}